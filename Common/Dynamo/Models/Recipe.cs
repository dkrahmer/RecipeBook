using Amazon.DynamoDBv2.DataModel;

namespace Common.Dynamo.Models
{
    [DynamoDBTable("Recipe")]
    public class Recipe : DynamoDocument
    {
        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public string Ingredients { get; set; }

        [DynamoDBProperty]
        public string Instructions { get; set; }
    }
}