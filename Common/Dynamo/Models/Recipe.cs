using Amazon.DynamoDBv2.DataModel;

namespace Common.Dynamo.Models
{
    [DynamoDBTable("Recipe")]
    public class Recipe : DynamoDocument
    {
        [DynamoDBProperty]
        public string Name { set; get; }

        [DynamoDBProperty]
        public string Description { set; get; }

        [DynamoDBProperty]
        public string Ingredients { set; get; }

        [DynamoDBProperty]
        public string Instructions { set; get; }
    }
}