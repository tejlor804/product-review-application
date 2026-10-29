# ProductReviewApplication

## Overview

ProductReviewApplication is an ASP.NET Core Web API for managing product reviews
using Azure Table Storage. It supports retrieving reviews for a product and
submitting new reviews.

A business rule requires a user to have read the latest review for a product
before submitting another review.

## Architecture

The application uses a single ASP.NET Core project, with folders organized by
responsibility. This keeps the project simple while separating HTTP handling,
business logic, and data access.

- **Controllers** -- handle HTTP requests and responses.
- **Services** -- implement application logic and enforce the review-submission
  rule.
- **Repositories** -- define data-access interfaces used by the service layer.
- **Infrastructure/AzureTables** -- contains Azure Table Storage implementations
  and `TableClientFactory` for providing clients for the repositories.
- **Infrastructure/AzureTables/Entities** -- contains Azure-specific storage
  models.
- **DTOs** -- define API request and response models.
- **Mapping** -- converts between storage models and application models.
- **Exceptions** -- contains custom business exceptions.
- **ErrorHandling** -- maps exceptions to HTTP responses using `ProblemDetails`.

This separation keeps Azure-specific code out of the service layer and makes it
easier to replace or test persistence components.

## Business rule

1. A user retrieves a product's reviews through `GET
   /Product/{name}/reviews?userId={userId}`.
2. If reviews exist, the application records the newest review as the latest one
   read by that user for that product.
3. When the user submits a review, the service compares the latest stored review
   with the user's recorded read state.
4. If the user has not read the newest review, the API returns `428 Precondition
   Required`.
5. Otherwise, the new review is saved.

## API

### Get reviews

`GET /Product/{name}/reviews?userId={userId}`

Example:

``` http
GET http://localhost:5287/Product/Keyboard/reviews?userId=user-123
Accept: application/json
```

Returns the reviews for the specified product and records the newest review as
read for the given user when reviews exist. If there are no reviews of the
product, an empty list is returned.

### Submit a review

`POST /Product/{name}/reviews`

Example:

``` http
POST http://localhost:5287/product/baba/reviews
Content-Type: application/json

{
  "userId": "user-123",
  "review": "It is a very good toy!"
}
```

The request uses `ProductReviewRequest`. The user ID and review text are
required, and the review text is limited to 500 characters. A successful request
returns `201 Created`.

### Error responses

| Scenario                         | Status                          |
|----------------------------------|---------------------------------|
| Invalid input (`ArgumentException`) | `400 Bad Request`            |
| Newest review has not been read  | `428 Precondition Required`     |
| Unexpected exception             | `500 Internal Server Error`     |

Errors are returned using the `ProblemDetails` format.

## Configuration and running

### Requirements

- .NET 10 SDK
- An Azure Storage account with access to the `ProductReviews` and `Users`
  tables
- An Azure Table Storage connection string

The application reads the connection string from `AzureTables:ConnectionString`.
Configure it with .NET User Secrets during development:

``` bash
dotnet user-secrets init
dotnet user-secrets set "AzureTables:ConnectionString" "<AZURE_STORAGE_CONNECTION_STRING>"
```

Run the application from the project directory:

``` bash
dotnet restore
dotnet run
```

The endpoints are exposed at `http://localhost:5287`.

## Limitations and production considerations

The following issues should be addressed before production use:

- **Concurrency:** reading the latest review, recording read state, and
  submitting a new review are separate operations. Concurrent requests can
  change state between the check and save. Concurrency must be handled if strict
  consistency is required.
- **Authentication:** `userId` is supplied by the client, and
  authentication/authorization is not implemented. In production, derive the
  user identity from a trusted authentication context.
- **Pagination:** consider pagination if a product can have many reviews.
- **Observability:** add structured logging, metrics, health checks, tracing,
  and alerts.
- **Testing:** add unit tests for the business rule and integration tests for
  Azure Table Storage operations.
- **Secrets and permissions:** use secure configuration or a secret manager.

