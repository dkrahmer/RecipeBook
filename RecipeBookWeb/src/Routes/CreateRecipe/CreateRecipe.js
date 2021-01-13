import { useRecipeService } from "../../Hooks/useRecipeService";
import { LoadingWrapper } from "../../Shared/LoadingWrapper";
import { RecipeSavedSnackbar } from "./Components/RecipeSavedSnackbar";
import { RecipeForm } from "./Components/RecipeForm";
import React, { useState, useEffect } from "react";

export function CreateRecipe(props) {
	const recipeService = useRecipeService(props.config);
	const [recipe, setRecipe] = useState(setInitialRecipe());
	const [toastOpen, setToastOpen] = useState(false);
	const [isExecuting, setIsExecuting] = useState(false);
	const [newRecipeId, setNewRecipeId] = useState("");
	const [isLoading, setIsLoading] = useState(true);
	const [tags, setTags] = useState([]);

	useEffect(() => {
		setIsLoading(true);
		recipeService.getTags((response) => {
			setTags(response.data);
			setIsLoading(false);
		}, (error) => {
			console.error(error);
			alert("Error getting list of tags.");
		});
	}, []); // eslint-disable-line react-hooks/exhaustive-deps

	function onToastClose() {
		setToastOpen(false);
		setNewRecipeId("");
	}

	function createRecipe(newRecipe) {
		setIsExecuting(true);
		recipeService.createRecipe(newRecipe, (response) => {
			if (response && response.status === 200 && response.data) {
				setNewRecipeId(response.data);
				setToastOpen(true);
				setRecipe(setInitialRecipe());
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

	function cancelCreateRecipe() {
		props.history.replace("/recipes");
	}

	return (
		<React.Fragment>
			<LoadingWrapper isLoading={isLoading}>
				<RecipeForm
					config={props.config}
					pageTitle="Create a new Recipe"
					recipe={recipe}
					tagOptions={tags}
					onSaveClick={createRecipe}
					onCancel={cancelCreateRecipe}
					isSaveExecuting={isExecuting} />
				<RecipeSavedSnackbar
					toastOpen={toastOpen}
					onToastClose={onToastClose}
					recipeId={newRecipeId} />
			</LoadingWrapper>
		</React.Fragment>
	);
}

function setInitialRecipe() {
	return {
		name: "",
		ingredients: "",
		instructions: "",
		notes: ""
	};
}
