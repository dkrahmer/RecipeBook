import React from "react";
import { TextField } from "@material-ui/core";

export function VolumeToMassFields(props) {
	function onChange(e) {
		const newFields = { ...props.fields, [e.target.name]: e.target.value };

		props.onChange(props.index, newFields);
	}

	return (
		<React.Fragment>
			<TextField
				fullWidth
				required
				//error={!props.errors.name.isValid}
				value={props.fields.Name}
				onChange={onChange}
				label="Name"
				placeholder=""
				margin="normal"
				variant="outlined" />
			<TextField
				fullWidth
				required
				//error={!props.errors.name.isValid}
				value={props.fields.AlternateNames}
				onChange={onChange}
				label="Alternate Names"
				placeholder=""
				margin="normal"
				variant="outlined" />
			<TextField
				fullWidth
				required
				//error={!props.errors.name.isValid}
				value={props.fields.Density}
				onChange={onChange}
				label="Density"
				placeholder=""
				margin="normal"
				variant="outlined" />
			<TextField
				fullWidth
				required
				//error={!props.errors.name.isValid}
				value={props.fields.Notes}
				onChange={onChange}
				label="Notes"
				placeholder=""
				margin="normal"
				variant="outlined" />
		</React.Fragment>
	);
}
