import React from "react";
import {
	TextField
} from "@material-ui/core";
import _ from "lodash";
import ComboBox from "../../../Shared/ComboBox";

export function RecipesFilterForm(props) {
	let setNameQueryDebounced =
		_.debounce((e) => {
			props.setNameQuery(e.target.value);
		}, 500);

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
			<ComboBox
				value={(props.selectedTags || []).map((x) => { return { label: x, value: x }; })}
				options={(props.tags || []).map((x) => { return { label: x, value: x }; })}
				name="tags"
				onChange={props.onSelectedTagsChange}
				label="Tags"
				isClearable={true}
				isMulti={true}
				isSearchable={true}
				closeMenuOnSelect={true}
				backspaceRemovesValue={true}
				placeholder="Select tags..."
			/>
		</React.Fragment>
	);
}
