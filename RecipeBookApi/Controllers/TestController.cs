using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Common.Dynamo;
using Common.Dynamo.Models;
using Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;

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

                var repo = new DynamoStorageRepository<Recipe>(context);

                var recipes = repo.ReadAll().Result;

                return recipes.ToJson();

                //var userId = Guid.NewGuid().ToString();
                //var now = DateTime.Now;

                //var newRecipe = new Recipe
                //{
                //    Name = "Recipe name",
                //    Description = "Recipe description",
                //    Ingredients = "Recipe ingredients",
                //    Instructions = "Recipe instructions",
                //    CreateDate = now,
                //    UpdateDate = now,
                //    CreatedById = userId,
                //    UpdatedById = userId,
                //    Id = Guid.NewGuid().ToString()
                //};

                //context.SaveAsync(newRecipe);

                //var data = Task.Run(() => context.LoadAsync<Recipe>(newRecipe.Id)).Result;
                //return JsonConvert.SerializeObject(data);

                //return "disabled";
            }
            catch (Exception ex)
            {
                return $"DB connection error: {ex.Message}";
            }
        }
    }
}