import React from "react";
import {
  TextField
} from "@material-ui/core";

export function RecipesFilterForm(props) {
  function handleSearchQueryChange(e) {
    props.handleSearchQueryChange(e.target.value);
  }

  return (
    <React.Fragment>
      <TextField
        fullWidth
        value={props.nameQuery}
        onChange={handleSearchQueryChange}
        label="Find Recipe"
        placeholder="Recipe Name..."
        margin="normal"
        variant="outlined" />
    </React.Fragment>
  );
}
