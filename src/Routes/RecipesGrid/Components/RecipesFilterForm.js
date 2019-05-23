import React from "react";
import {
  TextField
} from "@material-ui/core";
import { Debounce } from 'react-throttle';

export function RecipesFilterForm(props) {
  return (
    <React.Fragment>
      <Debounce time="200" handler="onChange">
        <TextField
          fullWidth
          defaultValue={props.nameQuery}
          onChange={(e) => { props.setNameQuery(e.target.value); }}
          label="Find Recipe"
          placeholder="Recipe Name..."
          margin="normal"
          variant="outlined" />
      </Debounce>
    </React.Fragment>
  );
}
