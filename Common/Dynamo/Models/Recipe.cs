using Amazon.DynamoDBv2.DataModel;

namespace Common.Dynamo.Models
{
    [DynamoDBTable("Recipe")]
    public class Recipe : DynamoDocument
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        public string Instructions { get; set; }
    }
}