import React from "react";
import {
	Button,
	Dialog,
	DialogContent,
	DialogActions,
	DialogTitle,
	DialogContentText,
	TextField
} from "@material-ui/core";

function ScaleEntryModal({ isOpen, onApply, onCancel, scale, onScaleChange, ...props }) {
	function onScaleKeyPress(e) {
		if (e.key === "Enter") {
			onApply();
		}
	}

	function onScaleChangeLocal(e) {
		let newScale = e.target.value;
		onScaleChange(newScale);
		return newScale;
	}

	function presetScale(newScale) {
		onScaleChange(newScale);
		onApply();
	}

	const scalePresents = ["1/4", "1/3", "1/2", "1", "2", "3", "4"];

	return (
		<Dialog open={isOpen} onClose={onCancel} {...props}>
			<DialogTitle>
				Recipe scale factor
			</DialogTitle>
			<DialogContent>
				<DialogContentText className="rb-scale-presets">
					{scalePresents.map((s, key) => {
						return (
							<button key={key} className="link-button" style={{ marginRight: 15 }} onClick={() => { presetScale(s); }}>{s}</button>);
					})}
				</DialogContentText>
				<TextField
					autoFocus
					defaultValue={scale}
					//inputProps={{inputMode:"decimal", pattern:"^(?<Amount>(?<Fraction>(([0-9]+\\s+)?[0-9]+\\/[1-9]+[0-9]*))|(?<Decimal>([0-9]+\\.[0-9]*)|([0-9]*\\.[0-9]+)|[0-9]+))$"}}
					label="Scale factor"
					margin="normal"
					variant="outlined"
					onChange={onScaleChangeLocal}
					//inputRef={setDefaultFocus}
					onKeyPress={onScaleKeyPress} />
			</DialogContent>
			<DialogActions>
				<Button size="small" color="primary" onClick={onApply}>
					Apply
				</Button>
				<Button size="small" color="secondary" onClick={onCancel}>
					Cancel
				</Button>
			</DialogActions>
		</Dialog>
	);
}

export default ScaleEntryModal;
