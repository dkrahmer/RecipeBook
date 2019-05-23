import { RouterLink } from "../../../Shared/RouterLink";
import React from "react";
import {
  Grid,
  Card,
  CardHeader
} from "@material-ui/core";
//import moment from 'moment'

export function RecipeGridCard({ recipe }) {
  //const dateTimeString = moment(recipe.updateDateTime).format("MMM D, YYYY h:mm:ss a");

  return (
    <React.Fragment>
      <RouterLink to={`/recipes/${recipe.recipeId}`}>
        <Grid item>
          <Card>
            <CardHeader
              titleTypographyProps={{ variant: 'body2' }}
              title={recipe.name} />
          </Card>
        </Grid>
      </RouterLink>
    </React.Fragment>
  );
}
