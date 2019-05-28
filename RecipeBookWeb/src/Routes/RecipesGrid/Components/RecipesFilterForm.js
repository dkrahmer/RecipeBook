import React from "react";
import {
	TextField
} from "@material-ui/core";
import _ from "lodash";

export function RecipesFilterForm(props) {
	let setNameQueryDebounced =
		_.debounce((e) => {
			props.setNameQuery(e.target.value);
		}, 200);

	return (
		<React.Fragment>
			<TextField
				fullWidth
				defaultValue={props.nameQuery}
				onChange={(e) => { e.persist(); setNameQueryDebounced(e); }}
				label="Find Recipe"
				placeholder="Recipe Name..."
				margin="normal"
				variant="outlined" />
		</React.Fragment>
	);
}
