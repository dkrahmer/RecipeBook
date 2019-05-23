import { useRecipeService } from "../../Hooks/useRecipeService";
import { LoadingWrapper } from "../../Shared/LoadingWrapper";
import { PageHeader } from "../../Shared/PageHeader";
import { FilterableRecipesGrid } from "./Components/FilterableRecipesGrid";
import React, {
  useState,
  useEffect
} from "react";

export function RecipesGrid(props) {
  const recipeService = useRecipeService();
  const [isLoading, setIsLoading] = useState(true);
  const [allRecipes, setAllRecipes] = useState([]);

  useEffect(() => {
    setIsLoading(true);
    recipeService.getAllRecipes((response) => {
      setAllRecipes(response.data);
      setIsLoading(false);
    });
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  return (
    <React.Fragment>
      <LoadingWrapper isLoading={isLoading}>
        <FilterableRecipesGrid allRecipes={allRecipes} />
      </LoadingWrapper>
    </React.Fragment>
  );
}
