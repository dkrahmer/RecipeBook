using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeBookApi.Controllers
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("get")]
        public string Get()
        {
            return "AndyTestAPI returned GET";
        }

        [HttpGet]
        [Route("external")]
        public string External()
        {
            try
            {
                var client = new HttpClient();

                var requestTask = client.GetStringAsync("http://www.google.com");
                requestTask.Wait();

                return "External connectivity PASS";
            }
            catch (Exception ex)
            {
                return $"External connectivity FAIL: {ex.Message}";
            }
        }

        [HttpGet]
        [Route("database")]
        public string Database()
        {
            try
            {
                var context = new DynamoDBContext(new AmazonDynamoDBClient("", "", RegionEndpoint.USEast1));

                var data = Task.Run(() => context.LoadAsync<Recipe>("")).Result;

                return data.CreateDate;
            }
            catch (Exception ex)
            {
                return $"DB connection error: {ex.Message}";
            }
        }
    }

    [DynamoDBTable("Recipe")]
    public class Recipe
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty("CreateDate")]
        public string CreateDate { get; set; }
    }
}