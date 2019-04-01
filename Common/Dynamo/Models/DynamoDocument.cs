using Amazon.DynamoDBv2.DataModel;
using System;

namespace Common.Dynamo.Models
{
    public abstract class DynamoDocument
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string CreatedById { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdatedById { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}