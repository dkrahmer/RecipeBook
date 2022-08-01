import { useRecipeService } from "../../Hooks/useRecipeService";
import { PageHeader } from "../../Shared/PageHeader";
import PageOverlay from "../../Shared/PageOverlay";
import { RecipeInfo } from "./Components/RecipeInfo";
import { RecipeViewActions } from "./Components/RecipeViewActions";
import YesNoModal from "../../Shared/YesNoModal";
import React, { useState, useEffect, Fragment } from "react";
import { Paper, Typography } from "@material-ui/core";
import queryString from "query-string";
import _ from "lodash";

export function RecipeView(props) {
	const recipeService = useRecipeService(props.config);
	const [isLoading, setIsLoading] = useState(true);
	const [recipe, setRecipe] = useState({ name: "Loading Recipe...", placeholder: true });
	const [ownerBlurb, setOwnerBlurb] = useState("");
	const [isModalOpen, setIsModalOpen] = useState(false);

	const queryStringValues = queryString.parse(props.location.search);
	var [scale, setScale] = useState(queryStringValues.scale);
	var [system, setSystem] = useState(queryStringValues.system);
	var [convertToMass, setConvertToMass] = useState(queryStringValues.convertToMass);

	useEffect(() => {
		const title = `View Recipe - ${props.config.appName}`;
		document.title = title;
		if (scale === "1") {
			setScale(null);
			return;
		}

		const qs = queryString.stringify(_.pickBy({ scale, system, convertToMass }));
		props.history.replace({ search: qs }); // update the URL QS

		setIsLoading(true);
		recipeService.getRecipeById(props.match.params.recipeId, qs, (response) => {
			document.title = `${response.data.name} - ${props.config.appName}`;
			setRecipe(response.data);
			setIsLoading(false);
		}, (error) => {
			document.title = title;
			if (error.response.status === 404) {
				props.history.push("/notfound");
			}
		});
	}, [scale, system, convertToMass]); // eslint-disable-line react-hooks/exhaustive-deps

	function confirmDeleteRequest() {
		setIsModalOpen(true);
	}

	function onNoModal() {
		setIsModalOpen(false);
	}

	function editRecipe() {
		props.history.push(`/recipes/${recipe.recipeId}/edit`);
	}

	function cloneRecipe() {
		props.history.push(`/recipes/${recipe.recipeId}/clone`);
	}

	function onDeleteConfirmed() {
		setIsModalOpen(false);
		recipeService.deleteRecipe(recipe.recipeId, (response) => {
			if (response && response.status === 200) {
				props.history.push("/");
			} else {
				console.log(response);
			}
		}, (error) => {
			console.log(error);
			if (error.response) {
				console.log(error.response);
				if (error.response.status === 404) {
					props.history.push("/notfound");
				}
			}
		});
	}

	return (
		<React.Fragment>
			<PageHeader text={recipe.name} config={props.config} />
			<PageOverlay showOverlay={isLoading} />
			{!recipe.placeholder ?
				<Paper className="print-no-padding" style={{ padding: 12 }}>
					<RecipeInfo
						recipe={recipe}
						scale={scale}
						setScale={setScale}
						system={system}
						setSystem={setSystem}
						convertToMass={convertToMass}
						setConvertToMass={setConvertToMass}
						setOwnerBlurb={setOwnerBlurb} />

					<RecipeViewActions
						config={props.config}
						editRecipe={editRecipe}
						cloneRecipe={cloneRecipe}
						deleteRecipe={confirmDeleteRequest} />
				</Paper>
				: <Fragment />
			}
			<YesNoModal
				isOpen={isModalOpen}
				title="Delete Recipe"
				question={`Are you sure you want to delete ${recipe.name}?`}
				onYes={onDeleteConfirmed}
				onNo={onNoModal} />
			<Typography variant="subtitle2" color="textSecondary">
				{ownerBlurb}
			</Typography>
		</React.Fragment >
	);
}
