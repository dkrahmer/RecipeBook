using Amazon.DynamoDBv2.DataModel;

namespace Common.Dynamo.Models
{
    public abstract class DynamoDocument
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
    }
}
