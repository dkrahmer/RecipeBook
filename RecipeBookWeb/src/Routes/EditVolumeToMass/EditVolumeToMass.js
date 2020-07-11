import { useSettingsService } from "../../Hooks/useSettingsService";
import { LoadingWrapper } from "../../Shared/LoadingWrapper";
import { VolumeToMassForm } from "./VolumeToMassForm";
import { RecipeSavedSnackbar } from "../CreateRecipe/Components/RecipeSavedSnackbar";
import React, { useState, useEffect } from "react";

export function EditRecipe(props) {
	const settingsService = useSettingsService(props.config);
	const [isLoading, setIsLoading] = useState(true);
	const [toastOpen, setToastOpen] = useState(false);
	const [isExecuting, setIsExecuting] = useState(false);
	const [volumeToMassList, setVolumeToMassList] = useState([]);

	useEffect(() => {
		setIsLoading(true);
		settingsService.getVolumeToMass((response) => {
			setVolumeToMassList(response.data);
			setIsLoading(false);
		}, (error) => {
			if (error.response.status === 404) {
				props.history.push("/notfound");
			}
		});
	}, []); // eslint-disable-line react-hooks/exhaustive-deps

	function onToastClose() {
		setToastOpen(false);
	}

	function saveVolumeToMassList(updatedVolumeToMassList) {
		setIsExecuting(true);
		settingsService.updateVolumeToMass(updatedVolumeToMassList, (response) => {
			if (response && response.status === 200) {
				setToastOpen(true);
				setVolumeToMassList(updatedVolumeToMassList);
				props.history.replace(`/recipes/${recipe.recipeId}`);
			} else {
				console.log(response);
			}

			setIsExecuting(false);
		}, (error) => {
			console.log(error);
			if (error.response) {
				console.log(error.response);
			}

			setIsExecuting(false);
		});
	}

	function cancelRecipe(updatedRecipe) {
		props.history.replace(`/recipes/${recipe.recipeId}`);
	}

	return (
		<React.Fragment>
			<LoadingWrapper isLoading={isLoading}>
				<VolumeToMassForm
					config={props.config}
					pageTitle="Volume to Mass Conversions"
					volumeToMassList={volumeToMassList}
					onSaveClick={saveVolumeToMassList}
					onCancel={cancelRecipe}
					isSaveExecuting={isExecuting} />
				<RecipeSavedSnackbar
					toastOpen={toastOpen}
					onToastClose={onToastClose} />
			</LoadingWrapper>
		</React.Fragment>
	);
}
