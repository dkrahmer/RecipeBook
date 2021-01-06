import React, { Fragment } from "react";
import clsx from "clsx";
import Select from "react-select";
import makeAnimated from "react-select/animated";
import "./ComboBoxWithCreate.scss";

export default class ComboBoxWithCreate extends React.Component {
	constructor(props) {
		super(props);

		this.state = {
			selectedOption: null
		};

		this.animatedComponents = makeAnimated();

		this.onChangeClosure = this.onChangeClosure.bind(this);
	}

	render() {
		let { errorMessage, value, previousValue, showDirty, className, disabled, name, onChange, ...rest } = this.props;

		value = value || [];
		previousValue = previousValue || [];

		let isDirty = false;
		if (value.length === previousValue.length) {
			for (let i = 0; i < value.length; i++) {
				if (value[i].value !== previousValue[i].value) {
					isDirty = true;
					break;
				}
			}
		}
		else {
			isDirty = true;
		}

		className = clsx(className, "combo-box", this.props.isMulti ? "combo-box-multi" : "combo-box-single");

		if (isDirty && showDirty && !this.props.disabled)
			className = clsx(className, "is-dirty");

		return (
			<Fragment>
				<Select
					name={name}
					value={value}
					className={className}
					isDisabled={disabled}
					onChange={this.onChangeClosure(onChange, name)}
					components={this.animatedComponents}
					menuPosition="fixed" // force the options to float
					{...rest}
				/>
				{!errorMessage ? (<Fragment />) : (
					<span className="error-message">{errorMessage}</span>
				)}
			</Fragment>
		);
	}

	onChangeClosure(onChange, name) {
		if (!onChange)
			return undefined;

		return (selectedOption) => {
			const e = {
				target: {
					name,
					type: "select",
					value: (selectedOption || []).map((val) => val.value),
					data: selectedOption
				}
			};
			this.setState({ selectedOption });
			onChange(e);
		}
	};

	getStringValue(value) {
		if (value === undefined || value === null)
			value = "";
		else
			value = `${value}`; // Cast to a string

		return value;
	}
}