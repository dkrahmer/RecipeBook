import {
	useState,
	useEffect,
	useCallback
} from "react";

export function useRecipeForm(initialRecipe) {
	const [recipe, setRecipe] = useState(initialRecipe);
	const [errors, setErrors] = useState(() => getBlankErrors());

	const validateCallback = useCallback((markAllDirty) => {
		const stringifiedErrors = JSON.stringify(errors);
		const foundErrors = JSON.parse(stringifiedErrors); // copy with no object references
		Object.keys(foundErrors).forEach(k => {
			if (markAllDirty) {
				foundErrors[k].isDirty = true;
			}

			if (foundErrors[k].isDirty) {
				foundErrors[k].isValid = recipe[k].trim() !== "";
			}
		});

		if (stringifiedErrors !== JSON.stringify(foundErrors)) {
			setErrors(foundErrors);
		}

		return Object.keys(foundErrors).every(k => foundErrors[k].isValid);
	}, [recipe, errors]);

	const resetCallback = useCallback(() => {
		setRecipe(initialRecipe);
		setErrors(getBlankErrors());
	}, [initialRecipe]);

	useEffect(() => {
		validateCallback(false);
	}, [validateCallback]);

	useEffect(() => {
		resetCallback();
	}, [resetCallback]);

	function markFieldDirty(field) {
		const updatingErrors = JSON.parse(JSON.stringify(errors)); // copy with no object references
		updatingErrors[field].isDirty = true;

		setErrors(updatingErrors);
	}

	function isValid() {
		return validateCallback(true);
	}

	function handleNameChange(value) {
		setRecipe({ ...recipe, name: value });
		markFieldDirty("name");
	}

	function handleTagsChange(value) {
		setRecipe({ ...recipe, tags: value });
	}

	function handleIngredientsChange(value) {
		setRecipe({ ...recipe, ingredients: value });
	}

	function handleInstructionsChange(value) {
		setRecipe({ ...recipe, instructions: value });
	}

	return {
		recipe,
		errors,
		isValid,
		reset: resetCallback,
		handleNameChange,
		handleTagsChange,
		handleIngredientsChange,
		handleInstructionsChange
	};
}

function getBlankErrors() {
	return {
		name: {
			isDirty: false,
			isValid: true,
			message: "Name is required"
		}
	};
}
