import { RecipesFilterForm } from "./RecipesFilterForm";
import { PageableRecipesGrid } from "./PageableRecipesGrid";
import { LoadingWrapper } from "../../../Shared/LoadingWrapper";
import React, {
	useState,
	useEffect
} from "react";
import {
	Grid,
	Paper
} from "@material-ui/core";

export function FilterableRecipesGrid(props) {
	const [nameQuery, setNameQuery] = useState("");
	const [matchingRecipes, setMatchingRecipes] = useState([]);
	const [isLoading, setIsLoading] = useState(false);

	useEffect(() => {
		if (!nameQuery) {
			setMatchingRecipes([]);
			return;
		}

		setIsLoading(true);
		props.recipeService.getRecipes(nameQuery, (response) => {
			const recipes = response.data;
			sortRecipesByUpdateDateTime(recipes);
			setMatchingRecipes(recipes);
			setIsLoading(false);
		}, (response) => {
			alert("Error getting recipes!");
		});
	}, [nameQuery, props.recipeService]);

	return (
		<Grid container spacing={24}>
			<Grid item xs={12}>
				<Paper style={{ padding: 12 }}>
					<RecipesFilterForm
						nameQuery={nameQuery}
						setNameQuery={setNameQuery} />
				</Paper>
			</Grid>
			<LoadingWrapper isLoading={isLoading}>
				<PageableRecipesGrid recipes={matchingRecipes} nameQuery={nameQuery} />
			</LoadingWrapper>
		</Grid>
	);
}

/*
function sortRecipesByName(recipes) {
  recipes.sort((a, b) => {
	var nameA = a.name.toLowerCase();
	var nameB = b.name.toLowerCase();

	return (nameA < nameB ? -1 : (nameA > nameB ? 1 : 0));
  });
}
*/

function sortRecipesByUpdateDateTime(recipes) {
	// Descending order
	recipes.sort((a, b) => {
		return (a.updateDateTime > b.updateDateTime ? -1 : (a.updateDateTime < b.updateDateTime ? 1 : 0));
	});
}
