import { RecipeIngredientsSection } from "./RecipeIngredientsSection";
import { RecipeInstructionsSection } from "./RecipeInstructionsSection";
import { RecipeInfoSection } from "./RecipeInfoSection";
import React from "react";
import moment from 'moment'

export function RecipeInfo({ recipe, scale, ...props }) {
  props.setOwnerBlurb(generateOwnerBlurb(recipe.ownerName, recipe.updateDateTime));
  
  return (
    <React.Fragment>
      <RecipeIngredientsSection
        title="Ingredients"
        scale={scale}
        ingredientsList={recipe.ingredientsList}
        updateRecipe={props.updateRecipe} />
      <RecipeInstructionsSection
        title="Instructions"
        instructions={recipe.instructions} />
      <RecipeInfoSection
        title="Notes"
        body={recipe.notes} />
    </React.Fragment>
  );
}

function generateOwnerBlurb(ownerName, updateDateTime) {
  const dateTimeString = moment(updateDateTime).format("MMM D, YYYY h:mm:ss a");
  return `Last update: ${dateTimeString}`;
}
