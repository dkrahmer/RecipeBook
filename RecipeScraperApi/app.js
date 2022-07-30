import express from "express";
import http from "http";
import recipeDataScraper from "recipe-data-scraper";
import { decode } from 'html-entities';

async function recipeScraperApi() {
	const app = express();
	const server = http.createServer(app);

	app.get("/", async function (req, res) {
		const recipeUrl = req.query.url;

		try {
			console.log(`Recipe URL: ${recipeUrl}`);

			// https://www.npmjs.com/package/recipe-data-scraper
			const scrapedRecipe = await recipeDataScraper(recipeUrl);

			let notes = `Based on: ${recipeUrl}`

			if (scrapedRecipe.cookTime)
				notes = `Cook time: ${scrapedRecipe.cookTime}\n` + notes;

			if (scrapedRecipe.prepTime)
				notes = `Prep time: ${scrapedRecipe.prepTime}\n` + notes;

			if (scrapedRecipe.totalTime)
				notes = `Total time: ${scrapedRecipe.totalTime}\n` + notes;

			if (scrapedRecipe.recipeYield)
				notes = `Yield: ${scrapedRecipe.recipeYield.replace(/([0-9]+)/g, "<$1>")}\n` + notes;

			const recipe = {
				"Name": scrapedRecipe.name,
				"Ingredients": decode(scrapedRecipe.recipeIngredients.join("\n")),
				"Instructions": decode(scrapedRecipe.recipeInstructions.join("\n")),
				"Tags": scrapedRecipe.recipeCategories,
				"Notes": notes,
				"ImageUrls": [ scrapedRecipe.image ]
			};

			res.json(recipe);
		} catch (ex) {
			console.log(`GET error: ${ex.message}`);
			res.status(500).json({ message: ex.message });
		}
	});

	server.listen(5091, "localhost");
	server.on("listening", function () {
		console.log("Express server started: http://%s:%s", server.address().address, server.address().port);
	});
}

export default recipeScraperApi();
