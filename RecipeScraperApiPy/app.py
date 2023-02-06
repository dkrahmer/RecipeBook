import json
from recipe_scrapers import scrape_me # https://github.com/hhursev/recipe-scrapers
from flask import Flask, request, Response
app = Flask(__name__)
 
@app.route('/', methods=["GET"]) 
def index():
	recipeUrl = request.args.get("url")

	print("Getting recipe from: " + recipeUrl)
	scrapedRecipe = scrape_me(recipeUrl, wild_mode=True)

	notes = "Based on: " + recipeUrl

	if scrapedRecipe.total_time() != None and scrapedRecipe.total_time() > 0:
		notes = "Total time: " + str(scrapedRecipe.total_time()) + " minutes\n" + notes;

	if scrapedRecipe.yields() != None and scrapedRecipe.yields() != "":
		notes = "Yield: " + scrapedRecipe.yields() + "\n" + notes;


	recipe = {
		"Name": scrapedRecipe.title(),
		"Ingredients": "\n".join(scrapedRecipe.ingredients()),
		"Instructions": scrapedRecipe.instructions(),
	#	"Tags": scrapedRecipe.____,
		"Notes": notes,
		"ImageUrls": [ scrapedRecipe.image() ]
	};

	print("Scraped recipe: " + str(recipe))

	return Response(response=json.dumps(recipe),
                    status=200,
                    mimetype="application/json")
	
#app.run(debug=True, port=5092)  # debug=True uses a lot of idle CPU
app.run(port=5092)
