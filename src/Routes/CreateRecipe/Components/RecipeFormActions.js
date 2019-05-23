import { useUserContext } from "../../../Hooks/useUserContext";
import { PaperActions } from "../../../Shared/PaperActions";
import React from "react";
import { Button } from "@material-ui/core";
import SaveIcon from "@material-ui/icons/Save";
import CancelIcon from "@material-ui/icons/Cancel";

export function RecipeFormActions(props) {
  const user = useUserContext();

  return (
    <PaperActions
      style={{ marginTop: 10 }}
      left={
        <React.Fragment>
          <Button
            style={{ marginRight: 20 }}
            variant="contained"
            color="primary"
            disabled={!user.isLoggedIn}
            onClick={props.onSaveClick}>
            <SaveIcon style={{ marginRight: 10 }} /> Save
          </Button>
          <Button
            variant="contained"
            onClick={props.onCancelClick}>
            <CancelIcon style={{ marginRight: 10 }} /> Cancel
          </Button>
        </React.Fragment>
      }
      right={null} />
  );
}
