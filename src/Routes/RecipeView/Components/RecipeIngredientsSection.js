import React from "react";
import {
  Typography,
  Divider,
  Table
} from "@material-ui/core";

export function RecipeIngredientsSection(props) {
  let tableRows = props.ingredientsList.map((item, key) => {
    if (item.isHeading)
    {
      return <tr><td colSpan="2" className="rb-recipe-ingredient-list-heading">{item.name}</td></tr>
    }
    else
    {
      return <tr><td>{item.amount}</td><td>{item.name}</td></tr>
    }
  });

  return (
    <div className="rb-recipe-info">
      <Typography variant="h6" color="primary">
        {props.title}
      </Typography>
      <Table className="rb-recipe-info-body rb-recipe-ingredient-list">
        <tbody>
          {tableRows}
        </tbody>
      </Table>
      <Divider style={{ marginTop: 12 }} />
    </div>
  );
}
