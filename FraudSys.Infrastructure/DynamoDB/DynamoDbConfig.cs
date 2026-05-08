using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace FraudSys.Infrastructure.DynamoDB;

public static class DynamoDbConfig
{
    public static AmazonDynamoDBClient CreateClient(bool useLocal = true)
    {
        if (useLocal)
        {
            var credentials = new BasicAWSCredentials("fakeKey", "fakeSecret");
            var config = new AmazonDynamoDBConfig
            {
                ServiceURL = "http://localhost:8000"
            };
            return new AmazonDynamoDBClient(credentials, config);
        }

        return new AmazonDynamoDBClient();
    }
}