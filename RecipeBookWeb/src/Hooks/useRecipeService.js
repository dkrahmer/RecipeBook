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
	const api = createAxiosApi("Recipes", user, config);

	function getAllRecipes(handleResponse, handleError) {
		api.get("/")
			.then(handleResponse)
			.catch(handleError);
	}

	function getRecipeById(recipeId, queryString, handleResponse, handleError) {
		queryString = queryString ? `?${queryString}` : "";

		api.get(`/${recipeId}${queryString}`)
			.then(handleResponse)
			.catch(handleError);
	}

	function createRecipe(recipe, handleResponse, handleError) {
		api.post("/", recipe)
			.then(handleResponse)
			.catch(handleError);
	}

	function updateRecipe(id, recipe, handleResponse, handleError) {
		api.put(`/${id}`, recipe)
			.then(handleResponse)
			.catch(handleError);
	}

	function deleteRecipe(id, handleResponse, handleError) {
		api.delete(`/${id}`)
			.then(handleResponse)
			.catch(handleError);
	}

	return {
		getAllRecipes,
		getRecipeById,
		createRecipe,
		updateRecipe,
		deleteRecipe
	};
}
