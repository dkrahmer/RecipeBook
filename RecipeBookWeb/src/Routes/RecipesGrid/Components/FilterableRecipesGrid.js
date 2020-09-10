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
import queryString from "query-string";
import _ from "lodash";

export function FilterableRecipesGrid(props) {
	const queryStringValues = queryString.parse(props.location.search);
	const [nameQuery, setNameQuery] = useState(queryStringValues.nameQuery);
	const [isNameQuery, setIsNameQuery] = useState(!!nameQuery);
	const [matchingRecipes, setMatchingRecipes] = useState([]);
	const [isLoading, setIsLoading] = useState(false);

	useEffect(() => {
		setIsNameQuery(false);
		const qs = queryString.stringify(_.pickBy({ nameQuery }));
		props.history.replace({ search: qs }); // update the URL QS

		if (!nameQuery) {
			setMatchingRecipes([]);
			return;
		}

		setIsLoading(true);
		props.recipeService.getRecipes(nameQuery, (response) => {
			setIsNameQuery(true);
			const recipes = response.data;
			sortRecipesByUpdateDateTime(recipes);
			setMatchingRecipes(recipes);
			setIsLoading(false);
		}, (response) => {
			setIsNameQuery(true);
			alert("Error getting recipes!");
		});
	}, [nameQuery, props.recipeService]); // eslint-disable-line react-hooks/exhaustive-deps

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
				<PageableRecipesGrid recipes={matchingRecipes} setNameQuery={setNameQuery} isNameQuery={isNameQuery} history={props.history} />
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
