using Azure.Data.Tables;
using ProductReviewApplication.Infrastructure.AzureTables;
using ProductReviewApplication.Repositories;
using ProductReviewApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var connectionString =
    builder.Configuration["AzureTables:ConnectionString"];
builder.Services.AddSingleton(_ =>
    new TableServiceClient(connectionString));

builder.Services.AddScoped<IProductReviewService, ProductReviewService>();
builder.Services.AddSingleton<IProductReviewRepository, AzureTableProductReviewRepository>();
builder.Services.AddSingleton<IUserRepository, AzureTableUserRepository>();
builder.Services.AddSingleton<TableClientFactory>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
