import { useRecipeService } from "../../Hooks/useRecipeService";
import { LoadingWrapper } from "../../Shared/LoadingWrapper";
import { RecipeForm } from "../CreateRecipe/Components/RecipeForm";
import {
	RecipeSavedSnackbar
} from "../CreateRecipe/Components/RecipeSavedSnackbar";
import React, { useState, useEffect } from "react";

export function EditRecipe(props) {
	const recipeService = useRecipeService(props.config);
	const [isLoading, setIsLoading] = useState(true);
	const [toastOpen, setToastOpen] = useState(false);
	const [isExecuting, setIsExecuting] = useState(false);
	const [recipe, setRecipe] = useState({
		id: "",
		name: "",
		ingredients: "",
		instructions: "",
		notes: ""
	});
	const [tags, setTags] = useState([]);

	useEffect(() => {
		const operation = props.title || "Edit";
		const title = `${operation} Recipe - ${props.config.appName}`;
		document.title = title;
		setIsLoading(true);
		recipeService.getRecipeById(props.match.params.recipeId, "editing=1", (response) => {
			document.title = `${response.data.name} (${operation}) - ${props.config.appName}`;
			setRecipe(response.data);
			recipeService.getTags((response) => {
				setTags(response.data);
				setIsLoading(false);
			}, (error) => {
				console.error(error);
				alert("Error getting list of tags.");
			});
		}, (error) => {
			document.title = title;
			if (error.response && error.response.status === 404) {
				props.history.push("/notfound");
			}
			else {
				console.error(error);
			}
		});
	}, []); // eslint-disable-line react-hooks/exhaustive-deps

	function onToastClose() {
		setToastOpen(false);
	}

	function saveRecipe(updatedRecipe) {
		setIsExecuting(true);
		recipeService.updateRecipe(recipe.recipeId, updatedRecipe, (response) => {
			if (response && response.status === 200) {
				setToastOpen(true);
				setRecipe(updatedRecipe);
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
				<RecipeForm
					config={props.config}
					pageTitle={`${props.title || "Edit"} ${recipe.name}`}
					recipe={recipe}
					tagOptions={tags}
					onSaveClick={props.saveRecipe || saveRecipe}
					onCancel={cancelRecipe}
					isSaveExecuting={props.isExecuting || isExecuting} />
				<RecipeSavedSnackbar
					toastOpen={toastOpen}
					onToastClose={onToastClose}
					recipeId={recipe.recipeId} />
			</LoadingWrapper>
		</React.Fragment>
	);
}
