using Common.Dynamo.Contracts;
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
        private readonly IDynamoStorageRepository<Recipe> _recipeStorage;

        public TestController(IDynamoStorageRepository<Recipe> recipeStorage)
        {
            _recipeStorage = recipeStorage;
        }

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
                var recipes = _recipeStorage.ReadAll().Result;

                return recipes.ToJson();
            }
            catch (Exception ex)
            {
                return $"DB connection error: {ex.Message}";
            }
        }
    }
}