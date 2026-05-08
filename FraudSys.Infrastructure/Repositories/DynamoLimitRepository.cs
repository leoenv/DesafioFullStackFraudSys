using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;

namespace FraudSys.Infrastructure.Repositories;

public class DynamoLimitRepository : ILimitRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private const string TableName = "PixLimits";

    public DynamoLimitRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    private static string BuildPk(string agency, string accountNumber)
        => $"ACCOUNT#{agency}#{accountNumber}";

    public async Task CreateAsync(AccountLimit accountLimit)
    {
        var request = new PutItemRequest
        {
            TableName = TableName,
            Item = new Dictionary<string, AttributeValue>
            {
                ["PK"] = new AttributeValue { S = BuildPk(accountLimit.Agency, accountLimit.AccountNumber) },
                ["Document"] = new AttributeValue { S = accountLimit.Document },
                ["Agency"] = new AttributeValue { S = accountLimit.Agency },
                ["AccountNumber"] = new AttributeValue { S = accountLimit.AccountNumber },
                ["PixLimit"] = new AttributeValue { N = accountLimit.PixLimit.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) }
            }
        };

        await _dynamoDb.PutItemAsync(request);
    }

    public async Task<AccountLimit?> GetByAccountAsync(string agency, string accountNumber)
    {
        var request = new GetItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                ["PK"] = new AttributeValue { S = BuildPk(agency, accountNumber) }
            }
        };

        var response = await _dynamoDb.GetItemAsync(request);

        if (response.Item == null || response.Item.Count == 0) return null;

        return new AccountLimit
        {
            Document = response.Item["Document"].S,
            Agency = response.Item["Agency"].S,
            AccountNumber = response.Item["AccountNumber"].S,
            PixLimit = decimal.Parse(response.Item["PixLimit"].N, System.Globalization.CultureInfo.InvariantCulture)
        };
    }

    public async Task UpdateAsync(AccountLimit accountLimit)
    {
        var request = new UpdateItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                ["PK"] = new AttributeValue { S = BuildPk(accountLimit.Agency, accountLimit.AccountNumber) }
            },
            UpdateExpression = "SET PixLimit = :limit",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                [":limit"] = new AttributeValue { N = accountLimit.PixLimit.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) }
            }
        };

        await _dynamoDb.UpdateItemAsync(request);
    }

    public async Task DeleteAsync(string agency, string accountNumber)
    {
        var request = new DeleteItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                ["PK"] = new AttributeValue { S = BuildPk(agency, accountNumber) }
            }
        };

        await _dynamoDb.DeleteItemAsync(request);
    }
}