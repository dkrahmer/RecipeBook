import { useRecipeService } from "../../Hooks/useRecipeService";
import { PageHeader } from "../../Shared/PageHeader";
import { LoadingWrapper } from "../../Shared/LoadingWrapper";
import { RecipeInfo } from "./Components/RecipeInfo";
import { RecipeViewActions } from "./Components/RecipeViewActions";
import YesNoModal from "../../Shared/YesNoModal";
import React, {
  useState,
  useEffect
} from "react";
import { Paper } from "@material-ui/core";
import queryString from 'query-string'

export function RecipeView(props) {
  const recipeService = useRecipeService();
  const [isLoading, setIsLoading] = useState(true);
  const [recipe, setRecipe] = useState({ name: "" });
  const [ownerBlurb, setOwnerBlurb] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const queryStringValues = queryString.parse(props.location.search);
  let scale = queryStringValues.scale;

  function getRecipe(newScale) {
    if (newScale) {
      scale = newScale;
      props.history.replace({
        search: `scale=${newScale}`
      })  
    }

    setIsLoading(true);
    recipeService.getRecipeById(props.match.params.recipeId, scale, (response) => {
      setRecipe(response.data);
      setIsLoading(false);
    }, (error) => {
      if (error.response.status === 404) {
        props.history.push("/notfound");
      }
    });
  }

  useEffect(getRecipe, []); // eslint-disable-line react-hooks/exhaustive-deps

  function confirmDeleteRequest() {
    setIsModalOpen(true);
  }

  function onNoModal() {
    setIsModalOpen(false);
  }

  function editRecipe() {
    props.history.push(`/recipes/${recipe.recipeId}/edit`);
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
      <PageHeader text={recipe.name} subText={ownerBlurb} />
      <LoadingWrapper isLoading={isLoading}>
        <Paper style={{ padding: 12 }}>
          <RecipeInfo recipe={recipe} scale={scale} updateRecipe={getRecipe} setOwnerBlurb={setOwnerBlurb} />
          <RecipeViewActions
            editRecipe={editRecipe}
            deleteRecipe={confirmDeleteRequest} />
        </Paper>
        <YesNoModal
          isOpen={isModalOpen}
          title="Delete Recipe"
          question={`Are you sure you want to delete ${recipe.name}?`}
          onYes={onDeleteConfirmed}
          onNo={onNoModal} />
      </LoadingWrapper>
    </React.Fragment>
  );
}
