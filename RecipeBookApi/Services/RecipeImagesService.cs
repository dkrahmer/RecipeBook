using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBookApi.Services
{
	public class RecipeImagesService : IRecipeImagesService
	{
		private AppOptions _options;
		private const string ORIGINAL = null;

		public RecipeImagesService(IOptionsSnapshot<AppOptions> options)
		{
			_options = options.Value;
		}

		public FileInfo GetImageFileInfo(int recipeId, int imageId, string size)
		{
			string imageFilePath = GetImageFilePath(recipeId, imageId, size ?? ORIGINAL);
			if (imageFilePath == null)
			{
				if (size == null)
					throw new FileNotFoundException();

				var sizes = size.Split('x');
				if (sizes.Length != 2)
					throw new ArgumentException("Invalid width x height.");

				if (!int.TryParse(sizes[0], out int width) || !int.TryParse(sizes[1], out int height))
					throw new ArgumentException("Invalid width x height.");

				string originalImageFilePath = GetImageFilePath(recipeId, imageId, ORIGINAL);
				if (originalImageFilePath == null)
					throw new FileNotFoundException();

				// Resize the image and save it before serving
				using (var image = new MagickImage(originalImageFilePath))
				{
					if (width < image.Width && height < image.Height)
					{
						image.Resize(width, height);

						// Save the result
						string imageFilename = GetImageFileMask(recipeId, size, imageId, Path.GetExtension(originalImageFilePath).Trim('.').ToLower());
						imageFilePath = Path.Combine(Path.GetDirectoryName(originalImageFilePath), imageFilename);
						image.Write(imageFilePath);
					}
					else
					{
						imageFilePath = originalImageFilePath;
					}
				}
			}

			return new FileInfo(imageFilePath);
		}

		public async Task<List<int>> AddImageAsync(int recipeId, List<IFormFile> files)
		{
			int lastImageId = GetImageIds(recipeId).DefaultIfEmpty(0).Max();
			var newImageIds = new List<int>();

			foreach (var file in files)
			{
				if (file.Length == 0)
					continue;

				string extension = Path.GetExtension(file.FileName).Trim('.').ToLower();
				var imageFilePath = Path.Combine(_options.RecipeImagesDirectory, GetImageFileMask(recipeId, ORIGINAL, ++lastImageId, extension: extension));

				using (var stream = File.Create(imageFilePath))
				{
					await file.CopyToAsync(stream);
				}

				// Verify the file contains valid image data
				try
				{
					using (var image = new MagickImage(imageFilePath))
					{
					}
				}
				catch (Exception ex)
				{
					File.Delete(imageFilePath);
					throw new ArgumentException("Invalid image file.", ex);
				}

				newImageIds.Add(lastImageId);
			}

			return newImageIds;
		}

		public void DeleteImage(int recipeId, int imageId)
		{
			string imageFilePath = null;
			int maxTries = 100;

			while (!string.IsNullOrEmpty(imageFilePath = GetImageFilePath(recipeId, imageId, ORIGINAL)))
			{
				File.Delete(imageFilePath);
				if (--maxTries <= 0)
					throw new ApplicationException("Could not delete file(s).");
			}

			while (!string.IsNullOrEmpty(imageFilePath = GetImageFilePath(recipeId, imageId, "*")))
			{
				File.Delete(imageFilePath);
				if (--maxTries <= 0)
					throw new ApplicationException("Could not delete file(s).");
			}
		}

		public IEnumerable<int> GetImageIds(int recipeId)
		{
			return GetImageFilePaths(recipeId)
				.Select(imageFilePath =>
				{
					string imageIdStr = Path.GetFileName(imageFilePath).Substring(10, 4);
					if (!int.TryParse(imageIdStr, out int imageId))
						return -1;

					return imageId;
				})
				.Distinct()
				.Where(i => i > 0)
				.OrderBy(i => i);
		}

		private string GetImageFilePath(int recipeId, int imageId, string size)
		{
			return Directory.EnumerateFiles(_options.RecipeImagesDirectory, GetImageFileMask(recipeId, size, imageId)).FirstOrDefault();
		}

		private IEnumerable<string> GetImageFilePaths(int recipeId, string size = null)
		{
			return Directory.EnumerateFiles(_options.RecipeImagesDirectory, GetImageFileMask(recipeId, size));
		}

		private string GetImageFileMask(int recipeId, string size, int? imageId = null, string extension = null)
		{
			// Image naming format: <recipeId>_<imageId>.<ext> - 000000001_0001.jpg
			// Resized image naming format: <recipeId>_<imageId>.<size>.<ext> - 000000001_0001.200x200.jpg
			string imageIdMask = imageId == null ? "????" : $"{imageId:0000}";
			string dotSize = string.IsNullOrEmpty(size) ? "" : $".{size}";
			string extensionMask = extension ?? "????";
			return $"{recipeId:000000000}_{imageIdMask}{dotSize}.{extensionMask}";
		}
	}
}
