import { useRecipeService } from "../../Hooks/useRecipeService";
import { EditRecipe } from "../EditRecipe/EditRecipe";
import React, {
	useState
} from "react";

export function CloneRecipe(props) {
	const recipeService = useRecipeService(props.config);
	const [isExecuting, setIsExecuting] = useState(false);

	function createRecipe(newRecipe) {
		setIsExecuting(true);
		newRecipe = { ...newRecipe };
		delete newRecipe.recipeId;
		recipeService.createRecipe(newRecipe, (response) => {
			if (response && response.status === 200 && response.data) {
				props.history.replace(`/recipes/${response.data}`);
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

	return (
		<EditRecipe
			title="Clone"
			saveRecipe={createRecipe}
			isExecuting={isExecuting}
			match={{ params: { recipeId: props.match.params.recipeId } }}
			config={props.config}
		/>
	);
}
