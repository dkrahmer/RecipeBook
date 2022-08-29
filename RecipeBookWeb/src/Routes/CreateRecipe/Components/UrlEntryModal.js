import React from "react";
import {
	Button,
	Dialog,
	DialogContent,
	DialogActions,
	DialogTitle,
	TextField
} from "@material-ui/core";

function UrlEntryModal({ isOpen, onSubmit, onCancel, url, onUrlChange, ...props }) {
	function onUrlKeyPress(e) {
		if (e.key === "Enter") {
			onSubmit();
		}
	}

	function onUrlChangeLocal(e) {
		let newUrl = e.target.value;
		onUrlChange(newUrl);
		return newUrl;
	}

	return (
		<Dialog fullWidth open={isOpen} onClose={onCancel} {...props}>
			<DialogTitle>
				Import Recipe
			</DialogTitle>
			<DialogContent>
				<TextField
					autoFocus
					fullWidth
					value={url}
					label="External recipe URL"
					margin="normal"
					variant="outlined"
					onChange={onUrlChangeLocal}
					onKeyPress={onUrlKeyPress} />
			</DialogContent>
			<DialogActions>
				<Button size="small" color="primary" onClick={onSubmit}>
					Submit
				</Button>
				<Button size="small" color="secondary" onClick={onCancel}>
					Cancel
				</Button>
			</DialogActions>
		</Dialog>
	);
}

export default UrlEntryModal;
