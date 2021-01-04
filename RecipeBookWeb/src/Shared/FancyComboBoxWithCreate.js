import React from "react";
import { TextField } from "@material-ui/core";
import ComboBoxWithCreate from "./ComboBoxWithCreate";

export default function FancyComboBoxWithCreate({ children, label, name, value, options, onChange }) {
	value = value || [];

	const InputComponent = ({ inputRef, ...other }) => (
		<ComboBoxWithCreate ref={inputRef}
			name={name}
			onChange={onChange}
			value={value.map((x) => { return { label: x, value: x }; })}
			options={(options || []).map((x) => { return { label: x, value: x }; })}
			isClearable={false}
			isMulti={true}
			isSearchable={true}
			closeMenuOnSelect={false}
			backspaceRemovesValue={true}
		/>
	);

	return (
		<TextField
			fullWidth
			multiline
			label={label}
			InputLabelProps={{ shrink: true }}
			InputProps={{
				inputComponent: InputComponent
			}}
			margin="normal"
			variant="outlined" />
	);
};
