import { RecipeIngredientsSection } from "./RecipeIngredientsSection";
import { RecipeInstructionsSection } from "./RecipeInstructionsSection";
import { RecipeInfoSection } from "./RecipeInfoSection";
import React from "react";

export function RecipeInfo({ recipe, scale, ...props }) {
  props.setOwnerBlurb(generateOwnerBlurb(recipe.ownerName, recipe.updateDateTime));
  
  return (
    <React.Fragment>
      <RecipeIngredientsSection
        title="Ingredients"
        scale={scale}
        ingredientsList={recipe.ingredientsList} />
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
  /*
  const dateString = new Intl.DateTimeFormat("en-US", {
    timeZone: "UTC",
    year: "numeric",
    month: "long",
    day: "numeric",
    weekday: "long"
  }).format(new Date(updateDateTime));

  const timeString = new Intl.DateTimeFormat("en-US", {
    timeZone: "UTC",
    hour: "numeric",
    minute: "numeric",
    hour12: "true"
  }).format(new Date(updateDateTime));
*/
  const dateString = "---";
  const timeString = "---";

  const dateTimeString = `${dateString} at ${timeString}`;

  return `Last update: ${dateTimeString}`;
}
