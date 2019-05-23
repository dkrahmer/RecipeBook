import { useUserContext } from "../../../Hooks/useUserContext";
import { RouterLink } from "../../../Shared/RouterLink";
import { PaperActions } from "../../../Shared/PaperActions";
import React from "react";
import {
  Button,
  Fab,
  Tooltip
} from "@material-ui/core";
import EditIcon from "@material-ui/icons/Edit";
import DeleteIcon from "@material-ui/icons/Delete";

export function RecipeViewActions(props) {
  const user = useUserContext();

  return (
    <PaperActions
      left={
        <React.Fragment>
          <Button
            style={{ marginRight: 20 }}
            variant="contained"
            color="primary"
            disabled={!user.isLoggedIn}
            onClick={props.editRecipe}>
            <EditIcon style={{ marginRight: 10 }} />Edit
          </Button>
          <Button
            variant="contained"
            onClick={props.deleteRecipe}>
            <DeleteIcon style={{ marginRight: 10 }} /> Delete
          </Button>
        </React.Fragment>
      }
      right={
        <RouterLink to="/">
          <Button size="small" color="primary">
            To All Recipes
          </Button>
        </RouterLink>
      } />
  );
}
