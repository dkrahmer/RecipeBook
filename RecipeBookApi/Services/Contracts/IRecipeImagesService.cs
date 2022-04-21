using Common.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RecipeBookApi.Services.Contracts
{
	public interface IRecipeImagesService
	{
		IEnumerable<int> GetImageIds(int recipeId);
		FileInfo GetImageFileInfo(int recipeId, int imageId, string size);
		Task<List<int>> AddImageAsync(int recipeId, List<IFormFile> files);
		void DeleteImage(int recipeId, int imageId);
	}
}
