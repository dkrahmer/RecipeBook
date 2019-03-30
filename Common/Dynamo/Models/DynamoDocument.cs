using Amazon.DynamoDBv2.DataModel;
using System;

namespace Common.Dynamo.Models
{
    public abstract class DynamoDocument
    {
        [DynamoDBHashKey]
        public string Id { set; get; }

        [DynamoDBProperty]
        public string CreatedById { set; get; }

        [DynamoDBProperty]
        public DateTime CreateDate { set; get; }

        [DynamoDBProperty]
        public string UpdatedById { set; get; }

        [DynamoDBProperty]
        public DateTime UpdateDate { set; get; }
    }
}