import { useUserContext } from "../../../Hooks/useUserContext";
import { PaperActions } from "../../../Shared/PaperActions";
import React from "react";
import {
	Button
} from "@material-ui/core";
import EditIcon from "@material-ui/icons/Edit";
import CloneIcon from "@material-ui/icons/FileCopy";
import DeleteIcon from "@material-ui/icons/Delete";

export function RecipeViewActions(props) {
	const user = useUserContext(props.config);

	return (
		<PaperActions
			left={
				<React.Fragment>
					<Button
						style={{ marginRight: 20 }}
						variant="contained"
						color="primary"
						disabled={!user.canEditRecipe}
						onClick={props.editRecipe}>
						<EditIcon style={{ marginRight: 10 }} />Edit
					</Button>
					<Button
						style={{ marginRight: 20 }}
						variant="contained"
						color="primary"
						disabled={!user.canEditRecipe}
						onClick={props.cloneRecipe}>
						<CloneIcon style={{ marginRight: 10 }} />Clone
					</Button>
					<Button
						variant="contained"
						disabled={!user.canEditRecipe}
						onClick={props.deleteRecipe}>
						<DeleteIcon style={{ marginRight: 10 }} /> Delete
					</Button>
				</React.Fragment>
			} />
	);
}
