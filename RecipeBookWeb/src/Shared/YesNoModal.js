import React from "react";
import {
	Button,
	Dialog,
	DialogContent,
	DialogActions,
	DialogTitle,
	DialogContentText
} from "@material-ui/core";

function YesNoModal({ isOpen, onYes, onNo, title, question, ...props }) {
	return (
		<Dialog open={isOpen} onClose={onNo} {...props}>
			<DialogTitle>
				{title}
			</DialogTitle>
			<DialogContent>
				<DialogContentText>
					{question}
				</DialogContentText>
			</DialogContent>
			<DialogActions>
				<Button size="small" color="secondary" onClick={onYes}>
					Yes
				</Button>
				<Button size="small" color="primary" onClick={onNo}>
					No
				</Button>
			</DialogActions>
		</Dialog>
	);
}

export default YesNoModal;
