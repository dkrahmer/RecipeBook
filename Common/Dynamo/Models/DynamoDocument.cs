using Amazon.DynamoDBv2.DataModel;
using System;

namespace Common.Dynamo.Models
{
    public abstract class DynamoDocument
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string CreatedById { get; set; }

        [DynamoDBProperty]
        public DateTime CreateDate { get; set; }

        [DynamoDBProperty]
        public string UpdatedById { get; set; }

        [DynamoDBProperty]
        public DateTime UpdateDate { get; set; }
    }
}