import { RecipesFilterForm } from "./RecipesFilterForm";
import { PageableRecipesGrid } from "./PageableRecipesGrid";
import React, {
  useState,
  useEffect
} from "react";
import {
  Grid,
  Paper
} from "@material-ui/core";

export function FilterableRecipesGrid(props) {
  const [nameQuery, setNameQuery] = useState("");
  const [matchingRecipes, setMatchingRecipes] = useState(() => {
    return props.allRecipes;
  });

  useEffect(() => {
    let workingRecipes = [...props.allRecipes];
    if (nameQuery) {
      workingRecipes = workingRecipes.filter(r => {
        return r.name.toLowerCase().includes(nameQuery.toLowerCase());
      });
    }

    sortRecipesByUpdateDateTime(workingRecipes);

    setMatchingRecipes(workingRecipes);
  }, [nameQuery, props.allRecipes]);

  return (
    <Grid container spacing={24}>
      <Grid item xs={12}>
        <Paper style={{ padding: 12 }}>
          <RecipesFilterForm
            nameQuery={nameQuery}
            setNameQuery={setNameQuery} />
        </Paper>
      </Grid>
      <PageableRecipesGrid recipes={matchingRecipes} />
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
