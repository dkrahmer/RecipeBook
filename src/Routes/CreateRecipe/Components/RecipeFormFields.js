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
        helperText="Separate ingredients with a newline"
        placeholder=""
        margin="normal"
        variant="outlined" />
      <TextField
        fullWidth
        multiline
        value={props.recipe.instructions}
        onChange={props.onInstructionsChange}
        label="Instructions"
        helperText="Separate instructions with a newline"
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
