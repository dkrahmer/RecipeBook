import React from "react";
import { TextField } from "@material-ui/core";

export function RecipeFormFields(props) {
  return (
    <React.Fragment>
      <TextField
        fullWidth
        required
        error={!props.errors.name.isValid}
        value={props.recipe.name}
        onChange={props.onNameChange}
        label="Name"
        placeholder=""
        margin="normal"
        variant="outlined" />
      <TextField
        fullWidth
        multiline
        value={props.recipe.ingredients}
        onChange={props.onIngredientsChange}
        label="Ingredients"
        helperText="One ingredient per line. Use brackets for Ingredient Groups. Example: [For Crust] and [For Filling]"
        placeholder=""
        margin="normal"
        variant="outlined" />
      <TextField
        fullWidth
        multiline
        value={props.recipe.instructions}
        onChange={props.onInstructionsChange}
        label="Instructions"
        helperText="Use brackets for Instruction Groups. Example: [For Crust] and [For Filling]"
        placeholder=""
        margin="normal"
        variant="outlined" />
      <TextField
        fullWidth
        multiline
        value={props.recipe.notes || ""}
        onChange={props.onNotesChange}
        label="Notes"
        placeholder=""
        margin="normal"
        variant="outlined" />
    </React.Fragment>
  );
}
