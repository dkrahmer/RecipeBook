import { RouterLink } from "../../../Shared/RouterLink";
import React from "react";
import {
  Card,
  CardHeader,
  CardContent,
  CardActions,
  Button,
  Typography
} from "@material-ui/core";

export function RecipeGridCard({ recipe }) {
  const dateString = new Intl.DateTimeFormat("en-US", {
    timeZone: "UTC",
    year: "numeric",
    month: "long",
    day: "numeric",
    hour: "numeric",
    minute: "numeric",
    hour12: "true"
  }).format(new Date(recipe.updateDateTime));

  return (
    <React.Fragment>
      <Card>
        <RouterLink to={`/recipes/${recipe.recipeId}`}>
		  <CardHeader
            title={recipe.name}
            subheader={`${dateString}`} />
        </RouterLink>
      </Card>
    </React.Fragment>
  );
}
