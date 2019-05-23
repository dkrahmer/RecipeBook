import { RouterLink } from "../../../Shared/RouterLink";
import React from "react";
import {
  Card,
  CardHeader
} from "@material-ui/core";
import 'intl';
import moment from 'moment'

export function RecipeGridCard({ recipe }) {
  const dateTimeString = moment(recipe.updateDateTime).format("MMM D, YYYY h:mm:ss a");

  return (
    <React.Fragment>
      <Card>
        <RouterLink to={`/recipes/${recipe.recipeId}`}>
		  <CardHeader
            title={recipe.name}
            subheader={`${dateTimeString}`} />
        </RouterLink>
      </Card>
    </React.Fragment>
  );
}
