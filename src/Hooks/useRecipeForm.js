import {
  useState,
  useEffect,
  useCallback
} from "react";

export function useRecipeForm(initialRecipe) {
  const [recipe, setRecipe] = useState(initialRecipe);
  const [errors, setErrors] = useState(() => getBlankErrors());

  const validateCallback = useCallback((markAllDirty) => {
    const foundErrors = Object.assign({}, errors);
    Object.keys(foundErrors).forEach(k => {
      if (markAllDirty) {
        foundErrors[k].isDirty = true;
      }

      if (foundErrors[k].isDirty) {
        foundErrors[k].isValid = recipe[k].trim() !== "";
      }
    });

    setErrors(foundErrors);
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
    const updatingErrors = Object.assign({}, errors);
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

  function handleIngredientsChange(value) {
    setRecipe({ ...recipe, ingredients: value });
  }

  function handleInstructionsChange(value) {
    setRecipe({ ...recipe, instructions: value });
  }

  function handleNotesChange(value) {
    setRecipe({ ...recipe, notes: value });
  }

  return {
    recipe,
    errors,
    isValid,
    reset: resetCallback,
    handleNameChange,
    handleIngredientsChange,
    handleInstructionsChange,
    handleNotesChange
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
