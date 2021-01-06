import { RecipeTagsSection } from "./RecipeTagsSection";
import { RecipeIngredientsSection } from "./RecipeIngredientsSection";
import { RecipeInstructionsSection } from "./RecipeInstructionsSection";
import { RecipeInfoSection } from "./RecipeInfoSection";
import React from "react";
import moment from "moment";

export function RecipeInfo({ recipe, scale, setScale, system, setSystem, convertToMass, setConvertToMass, ...props }) {
	props.setOwnerBlurb(generateOwnerBlurb(recipe.ownerName, recipe.updateDateTime));

	return (
		<React.Fragment>
			{(!recipe.tags || recipe.tags.length === 0) ? "" : (
				<RecipeTagsSection tags={recipe.tags} />
			)}
			{!recipe.ingredientsList ? "" : (
				<RecipeIngredientsSection
					title="Ingredients"
					scale={scale}
					setScale={setScale}
					system={system}
					setSystem={setSystem}
					convertToMass={convertToMass}
					setConvertToMass={setConvertToMass}
					ingredientsList={recipe.ingredientsList} />
			)}
			{!recipe.instructions ? "" : (
				<RecipeInstructionsSection
					title="Instructions"
					instructions={recipe.instructions} />
			)}
			{!recipe.notes ? "" : (
				<RecipeInfoSection
					title="Notes"
					body={recipe.notes} />
			)}
		</React.Fragment>
	);
}

function generateOwnerBlurb(ownerName, updateDateTime) {
	const dateTimeString = moment(updateDateTime).format("MMM D, YYYY h:mm:ss a");
	return `Last update: ${dateTimeString}`;
}
