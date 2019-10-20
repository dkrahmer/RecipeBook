import React, { useState } from "react";
import {
	Typography,
	Divider,
	Table
} from "@material-ui/core";
import ScaleEntryModal from "./ScaleEntryModal";
import Linkify from "react-linkify";

export function RecipeIngredientsSection(props) {
	const [isScaleModalOpen, setIsScaleModalOpen] = useState(false);
	const [checkedRows, setCheckedRows] = useState({});

	function onRowClick(e, key) {
		const newCheckedRows = { ...checkedRows };

		// Reverse the checked state of the selected row
		if (newCheckedRows[key]) {
			delete newCheckedRows[key];
		}
		else {
			newCheckedRows[key] = true;
		}

		setCheckedRows(newCheckedRows);

		e.preventDefault();
		return false;
	}

	let tableRows = props.ingredientsList.map((item, key) => {
		if (item.isHeading) {
			return (<tr key={key}><td colSpan="2" className="rb-recipe-ingredient-list-heading">{item.name}</td></tr>);
		}
		else {
			return (<tr key={key} onClick={(e) => onRowClick(e, key)} className={checkedRows[key] ? "checked-row" : "unchecked-row"}><td>{item.amount}</td><td>{item.name}</td></tr>);
		}
	});

	let scale = props.scale;

	if (!scale)
		scale = "1";

	let scaleClassName = "scale-label";
	if (scale !== "1")
		scaleClassName += " scale-label-changed";

	let newScale = null;

	function showScaleDialog() {
		setIsScaleModalOpen(true);
	}

	function onScaleEntryModalApply() {
		if (newScale)
			props.setScale(newScale);
		setIsScaleModalOpen(false);
	}

	function onScaleEntryModalCancel() {
		setIsScaleModalOpen(false);
	}

	function onScaleChange(value) {
		newScale = value;
		return newScale;
	}

	let scaleLabel = (<span className={scaleClassName}>(<button className="link-button" onClick={showScaleDialog}>{`scale: x ${scale}`}</button>)</span>);

	return (
		<div className="rb-recipe-info">
			<Typography variant="h6" color="primary">
				{props.title} {scaleLabel}
			</Typography>
			<Linkify properties={{ target: "_blank" }}>
				<Table className={"rb-recipe-info-body rb-recipe-ingredient-list" + (scale === "1" ? "" : " rb-scale-enabled")}>
					<tbody>
						{tableRows}
					</tbody>
				</Table>
			</Linkify>
			<Divider style={{ marginTop: 12 }} />
			<ScaleEntryModal
				isOpen={isScaleModalOpen}
				scale={scale}
				onApply={onScaleEntryModalApply}
				onCancel={onScaleEntryModalCancel}
				onScaleChange={onScaleChange} />
		</div>
	);
}
