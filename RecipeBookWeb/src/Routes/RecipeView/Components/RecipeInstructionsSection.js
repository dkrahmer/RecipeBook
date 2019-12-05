import React, { useState } from "react";
import {
	Typography,
	Divider,
	Table
} from "@material-ui/core";
import Linkify from "react-linkify";

export function RecipeInstructionsSection(props) {
	const [checkedRows, setCheckedRows] = useState({});

	const headerRowKeys = {}; // keep track of the head rows for row check logic
	let instructionNumber = 1;
	let tableRows = props.instructions.split("\n").map((item, key) => {
		if (item.trim() === "")
			return null;

		if (item.startsWith("[") && item.endsWith("]")) {
			instructionNumber = 1; // Restart numbering for the next section
			headerRowKeys[key] = true;
			return (<tr key={key}><td colSpan="2" className="rb-recipe-instruction-list-heading">{item.substr(1, item.length - 2)}</td></tr>);
		}
		else {
			return (<tr key={key} onClick={(e) => onRowClick(e, key)} className={checkedRows[key] ? "checked-row" : "unchecked-row"}><td>{instructionNumber++}.</td><td>{item}</td></tr>);
		}
	});

	function onRowClick(e, key) {
		const newCheckedRows = { ...checkedRows };

		if (newCheckedRows[key]) {
			delete newCheckedRows[key];
			// uncheck the seleted row and subsequent rows in the same section
			// for (let i = key; i < tableRows.length; i++) {
			// 	if (headerRowKeys[i])
			// 		break; // stop if we hit a header row
			// 
			// 	if (i >= key)
			// 		delete newCheckedRows[i];
			// }
		}
		else {
			newCheckedRows[key] = true;

			// Check the selected row and previous rows in the same section
			// for (let i = key; i >= 0; i--) {
			// 	if (headerRowKeys[i])
			// 		break; // stop if we hit a header row
			// 
			// 	newCheckedRows[i] = true;
			// }
		}

		setCheckedRows(newCheckedRows);

		e.preventDefault();
		return false;
	}

	return (
		<div className="rb-recipe-info rb-recipe-instruction-list">
			<Typography variant="h6" color="primary">
				{props.title}
			</Typography>
			<Linkify properties={{ target: "_blank" }}>
				<Table className="rb-recipe-info-body rb-recipe-instruction-list">
					<tbody>
						{tableRows}
					</tbody>
				</Table>
			</Linkify>
			<Divider style={{ marginTop: 12 }} />
		</div>
	);
}
