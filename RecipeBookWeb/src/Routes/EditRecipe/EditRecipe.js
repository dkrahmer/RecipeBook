import { useRecipeService } from "../../Hooks/useRecipeService";
import { LoadingWrapper } from "../../Shared/LoadingWrapper";
import { RecipeForm } from "../CreateRecipe/Components/RecipeForm";
import {
	RecipeSavedSnackbar
} from "../CreateRecipe/Components/RecipeSavedSnackbar";
import React, {
	useState,
	useEffect
} from "react";

export function EditRecipe(props) {
	const recipeService = useRecipeService();
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

	useEffect(() => {
		setIsLoading(true);
		recipeService.getRecipeById(props.match.params.recipeId, null, null, (response) => {
			setRecipe(response.data);
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
					pageTitle={`${props.title || "Edit"} ${recipe.name}`}
					recipe={recipe}
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
