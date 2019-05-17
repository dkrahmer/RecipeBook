using Amazon.DynamoDBv2.DataModel;
using System;

namespace Common.Dynamo.Models
{
    [DynamoDBTable("AppUser")]
    public class AppUser : DynamoDocument
    {
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? LastLoggedInDate { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [DynamoDBIgnore]
        public string FullName => $"{FirstName} {LastName}";
    }
}
