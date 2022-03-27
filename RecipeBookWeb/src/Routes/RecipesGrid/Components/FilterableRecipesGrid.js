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
	const [tagQuery, setTagQuery] = useState(queryStringValues.tagQuery);
	const [isNameQuery, setIsNameQuery] = useState(!!nameQuery);
	const [matchingRecipes, setMatchingRecipes] = useState([]);
	const [tags, setTags] = useState([]);
	const [selectedTags, setSelectedTags] = useState([]);
	const [isLoading, setIsLoading] = useState(false);
	const [areTagsRequested, setAreTagsRequested] = useState(false);

	useEffect(() => {
		if (!areTagsRequested) {
			if (tagQuery) {
				const selectedTags = tagQuery.split(",");
				setSelectedTags(selectedTags);
			}
			props.recipeService.getTags((response) => {
				setTags(response.data);
			}, (error) => {
				console.error(error);
				alert("Error getting tags!");
			});
			setAreTagsRequested(true);
		}

		setIsNameQuery(false);
		const qs = queryString.stringify(_.pickBy({ nameQuery, tagQuery }));
		props.history.replace({ search: qs }); // update the URL QS

		if (!nameQuery && !tagQuery) {
			setMatchingRecipes([]);
			return;
		}

		setIsLoading(true);
		props.recipeService.getRecipes(nameQuery, tagQuery, (response) => {
			setIsNameQuery(true);
			const recipes = response.data;
			if (recipes.length === 1 && queryStringValues["auto"]) {
				props.history.replace(`/recipes/${recipes[0].recipeId}`);
				return;
			}
			sortRecipesByUpdateDateTime(recipes);
			setMatchingRecipes(recipes);
			setIsLoading(false);
		}, (error) => {
			console.error(error);
			setIsNameQuery(true);
			alert("Error getting recipes!");
		});
	}, [nameQuery, tagQuery]); // eslint-disable-line react-hooks/exhaustive-deps

	function onSelectedTagsChange(e) {
		setSelectedTags(e.target.value);
		setTagQuery(e.target.value.join(","));
	}

	return (
		<Grid container spacing={24}>
			<Grid item xs={12}>
				<Paper style={{ padding: 12 }}>
					<RecipesFilterForm
						nameQuery={nameQuery}
						setNameQuery={setNameQuery}
						tags={tags}
						selectedTags={selectedTags}
						onSelectedTagsChange={onSelectedTagsChange}
					/>
				</Paper>
			</Grid>
			<LoadingWrapper isLoading={isLoading}>
				<PageableRecipesGrid
					recipes={matchingRecipes}
					setNameQuery={setNameQuery}
					isNameQuery={isNameQuery}
					history={props.history} />
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
