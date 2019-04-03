namespace RecipeBookApi.Models
{
    public class RecipePostModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        public string Instructions { get; set; }

        public string CreatedById { get; set; }
    }
}