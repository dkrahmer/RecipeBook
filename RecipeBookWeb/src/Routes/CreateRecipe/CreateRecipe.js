import { useRecipeService } from "../../Hooks/useRecipeService";
import { LoadingWrapper } from "../../Shared/LoadingWrapper";
import { RecipeSavedSnackbar } from "./Components/RecipeSavedSnackbar";
import { RecipeForm } from "./Components/RecipeForm";
import React, { useState, useEffect } from "react";
import queryString from "query-string";
import UrlEntryModal from "./Components/UrlEntryModal";

export function CreateRecipe(props) {
	const recipeService = useRecipeService(props.config);
	const queryStringValues = queryString.parse(props.location.search);
	const [recipe, setRecipe] = useState(setInitialRecipe());
	const [toastOpen, setToastOpen] = useState(false);
	const [isExecuting, setIsExecuting] = useState(false);
	const [isImporting, setIsImporting] = useState(false);
	const [newRecipeId, setNewRecipeId] = useState("");
	const [isLoading, setIsLoading] = useState(true);
	const [tags, setTags] = useState([]);
	const [isImport, setIsImport] = useState(queryStringValues.import);
	const [importUrl, setImportUrl] = useState("");

	useEffect(() => {
		document.title = `New Recipe - ${props.config.appName}`;
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

	function cancelImportRecipe() {
		setIsImport(false);
	}

	function onImportUrlChange(value) {
		setImportUrl(value);
		return value;
	}

	function onImportUrlEntryModalSubmit() {
		setIsImport(false);
		setIsImporting(true);
		recipeService.importRecipe(importUrl, (response) => {
			if (response && response.status === 200 && response.data) {
				setRecipe(response.data);
			} else {
				console.log(response);
			}

			setIsImporting(false);
		}, (error) => {
			console.log(error);
			if (error.response) {
				console.log(error.response);
			}

			setIsImporting(false);
		});
	}

	function onImportClick() {
		setImportUrl("");
		setIsImport(true);
	}

	return (
		<React.Fragment>
			<LoadingWrapper isLoading={isLoading || isImporting}>
				<RecipeForm
					config={props.config}
					pageTitle="Create a new Recipe"
					recipe={recipe}
					tagOptions={tags}
					onSaveClick={createRecipe}
					onCancel={cancelCreateRecipe}
					onImportClick={onImportClick}
					isSaveExecuting={isExecuting || isImporting} />
				<RecipeSavedSnackbar
					toastOpen={toastOpen}
					onToastClose={onToastClose}
					recipeId={newRecipeId} />
			</LoadingWrapper>
			<UrlEntryModal
				isOpen={isImport}
				url={importUrl}
				onSubmit={onImportUrlEntryModalSubmit}
				onCancel={cancelImportRecipe}
				onUrlChange={onImportUrlChange} />
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
