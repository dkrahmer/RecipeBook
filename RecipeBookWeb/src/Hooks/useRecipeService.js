import { useUserContext } from "./useUserContext";
import { createAxiosApi } from "../Helpers/axiosApiHelper";
import {
	useState,
	useEffect
} from "react";

export function useRecipeService(config) {
	const user = useUserContext(config);
	const [recipeService, setRecipeService] = useState(() => {
		return createRecipeService(user, config);
	});

	useEffect(() => {
		setRecipeService(createRecipeService(user, config));
	}, [user.authToken]); // eslint-disable-line react-hooks/exhaustive-deps

	return recipeService;
}

function createRecipeService(user, config) {
	const api = createAxiosApi("", user, config);

	function getAllRecipes(handleResponse, handleError) {
		api.get("Recipes/")
			.then(handleResponse)
			.catch(handleError);
	}

	function getRecipes(nameSearch, tagQuery, handleResponse, handleError) {
		nameSearch = nameSearch || "";
		tagQuery = tagQuery || "";

		api.get(`Recipes/?nameSearch=${encodeURIComponent(nameSearch)}&tags=${encodeURIComponent(tagQuery)}`)
			.then(handleResponse)
			.catch(handleError);
	}

	function getRecipeById(recipeId, queryString, handleResponse, handleError) {
		queryString = queryString ? `?${queryString}` : "";

		api.get(`Recipes/${recipeId}${queryString}`)
			.then(handleResponse)
			.catch(handleError);
	}

	function createRecipe(recipe, handleResponse, handleError) {
		api.post("Recipes/", recipe)
			.then(handleResponse)
			.catch(handleError);
	}

	function updateRecipe(id, recipe, handleResponse, handleError) {
		api.put(`Recipes/${id}`, recipe)
			.then(handleResponse)
			.catch(handleError);
	}

	function deleteRecipe(id, handleResponse, handleError) {
		api.delete(`Recipes/${id}`)
			.then(handleResponse)
			.catch(handleError);
	}

	function getImageIdsByRecipeId(id, handleResponse, handleError) {
		api.get(`Recipes/${id}/Images`)
			.then(handleResponse)
			.catch(handleError);
	}
	
	function getTags(handleResponse, handleError) {
		api.get("Tags")
			.then(handleResponse)
			.catch(handleError);
	}

	function sendTo(target, url, handleResponse, handleError) {
		api.post(`SendTo/${target}`, { url: url })
			.then(handleResponse)
			.catch(handleError);
	}

	return {
		getAllRecipes,
		getRecipes,
		getRecipeById,
		createRecipe,
		updateRecipe,
		deleteRecipe,
		getImageIdsByRecipeId,
		getTags,
		sendTo
	};
}
