import { FormActions } from "../../Shared/FormActions";
import { VolumeToMassFields } from "./VolumeToMassFields";
import YesNoModal from "../../Shared/YesNoModal";
import React, { useState, useEffect } from "react";
import { Divider, LinearProgress } from "@material-ui/core";

export function VolumeToMassForm(props) {
	const [isCancelModalOpen, setIsCancelModalOpen] = useState(false);

	function onCancelModalYes() {
		setIsCancelModalOpen(false);
		recipeForm.reset();
		props.onCancel();
	}

	function onItemChange() {
	}

	function onCancelModalNo() {
		setIsCancelModalOpen(false);
	}

	function onCancelClick() {
		setIsCancelModalOpen(true);
	}

	function onSaveClick() {
		//if (recipeForm.isValid()) {
			props.onSaveClick(recipeForm.recipe);
		//}
	}

	return (
		<React.Fragment>
			{props.volumeToMassList.map((item, index) => {
				return (
					<VolumeToMassFields
						fields={item}
						onChange={onItemChange}
						itemIndex={index}
					/>)})
			}
			<Divider className="rb-divider" />
			{props.isSaveExecuting ? (<LinearProgress />) : (null)}
			<FormActions
				config={props.config}
				onSaveClick={onSaveClick}
				onCancelClick={onCancelClick} />
			<YesNoModal
				isOpen={isCancelModalOpen}
				title="Cancel Changes"
				question="Are you sure you want to cancel all changes?"
				onYes={onCancelModalYes}
				onNo={onCancelModalNo} />
		</React.Fragment>
	);
}
