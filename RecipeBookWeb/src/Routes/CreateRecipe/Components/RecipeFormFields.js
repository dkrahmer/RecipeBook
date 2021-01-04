import React from "react";
import { TextField } from "@material-ui/core";
import FancyComboBoxWithCreate from "../../../Shared/FancyComboBoxWithCreate";

export function RecipeFormFields(props) {
	const scalableHelper = "Enter scalable numbers in triangle brackets. Example: <2> = auto -- <2:/> = fraction -- <2:.3> = decimal with 3 decimal places.";

	return (
		<React.Fragment>
			<TextField
				fullWidth
				required
				error={!props.errors.name.isValid}
				value={props.recipe.name}
				onChange={props.onNameChange}
				label="Name"
				placeholder=""
				margin="normal"
				variant="outlined" />
			<FancyComboBoxWithCreate
				value={props.recipe.tags}
				options={props.tagOptions}
				name="tags"
				onChange={props.onTagsChange}
				label="Tags"/>
			<TextField
				fullWidth
				multiline
				value={props.recipe.ingredients}
				onChange={props.onIngredientsChange}
				label="Ingredients"
				helperText={<span>One ingredient per line. Use brackets for Ingredient Groups. Example: [For Crust] and [For Filling]<br />{scalableHelper}</span>}
				placeholder=""
				margin="normal"
				variant="outlined" />
			<TextField
				fullWidth
				multiline
				value={props.recipe.instructions}
				onChange={props.onInstructionsChange}
				label="Instructions"
				helperText={<span>Use brackets for Instruction Groups. Example: [For Crust] and [For Filling]<br />{scalableHelper}</span>}
				placeholder=""
				margin="normal"
				variant="outlined" />
			<TextField
				fullWidth
				multiline
				value={props.recipe.notes || ""}
				onChange={props.onNotesChange}
				label="Notes"
				helperText={scalableHelper}
				placeholder=""
				margin="normal"
				variant="outlined" />
		</React.Fragment>
	);
}
