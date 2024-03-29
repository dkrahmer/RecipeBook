import { useUserContext } from "../../../Hooks/useUserContext";
import { PaperActions } from "../../../Shared/PaperActions";
import React from "react";
import { Button } from "@material-ui/core";
import SaveIcon from "@material-ui/icons/Save";
import CancelIcon from "@material-ui/icons/Cancel";
import ImportIcon from "@material-ui/icons/GetApp";

export function RecipeFormActions(props) {
	const user = useUserContext(props.config);

	return (
		<PaperActions
			className="no-print"
			style={{ marginTop: 10 }}
			left={
				<React.Fragment>
					<Button
						style={{ marginRight: 20 }}
						variant="contained"
						color="primary"
						disabled={!user.canEditRecipe}
						onClick={props.onSaveClick}>
						<SaveIcon style={{ marginRight: 10 }} /> Save
					</Button>
					<Button
						style={{ marginRight: 20 }}
						variant="contained"
						onClick={props.onImportClick}>
						<ImportIcon style={{ marginRight: 10 }} /> Import
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
