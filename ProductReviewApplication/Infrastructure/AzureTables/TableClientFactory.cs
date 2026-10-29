using Azure.Data.Tables;
using System.Net.NetworkInformation;

namespace ProductReviewApplication.Infrastructure.AzureTables
{
    public class TableClientFactory(TableServiceClient serviceClient)
    {
        public TableClient ProductReviewsTable =>
            serviceClient.GetTableClient("ProductReviews");

        public TableClient UsersTable =>
            serviceClient.GetTableClient("Users");
    
    }
}
