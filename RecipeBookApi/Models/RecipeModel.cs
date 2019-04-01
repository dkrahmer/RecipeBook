using Common.Dynamo.Models;
using System;

namespace RecipeBookApi.Models
{
    public class RecipeModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Ingredients { get; set; }

        public string Instructions { get; set; }

        public string Description { get; set; }

        public string OwnerName { get; set; }

        public DateTime UpdateDate { get; set; }

        public RecipeModel()
        { }

        public RecipeModel(Recipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Ingredients = recipe.Ingredients;
            Instructions = recipe.Instructions;
            Description = recipe.Description;
            OwnerName = "Owner Name Here";
            UpdateDate = recipe.UpdateDate;
        }
    }
}