import { UserContextProvider } from "./Contexts/UserContext";
import { Menu } from "./Layout/Menu";
import { Footer } from "./Layout/Footer";
import { RecipesGrid } from "./Routes/RecipesGrid/RecipesGrid";
import { RecipeView } from "./Routes/RecipeView/RecipeView";
import { CreateRecipe } from "./Routes/CreateRecipe/CreateRecipe";
import { EditRecipe } from "./Routes/EditRecipe/EditRecipe";
import { Login } from "./Routes/Login/Login";
import { Logout } from "./Routes/Logout/Logout";
import { RouteNotFound } from "./Shared/RouteNotFound";
import { Redirect } from 'react-router';
import "typeface-roboto";
import React from "react";
import {
  BrowserRouter,
  Switch,
  Route
} from "react-router-dom";
import CssBaseline from "@material-ui/core/CssBaseline";
import "./Content/site.scss";

export default function App() {
  if ((navigator.userAgent.indexOf("iPad") > -1 || navigator.userAgent.indexOf("iPhone") > -1) 
      && navigator.userAgent.indexOf(" OS 9") > -1) {
    document.body.classList.add("disableAnimation"); // Animations make some old devices very slow
  }

  return (
    <BrowserRouter>
      <CssBaseline />
      <UserContextProvider>
        <Menu />
        <main id="main-content">
          <Switch> 
            <Route exact path="/" render={() => (<Redirect to="/recipes"/>)} />
            <Route exact path="/recipes" component={RecipesGrid} />
            <Route exact path="/recipes/create" component={CreateRecipe} />
            <Route exact path="/recipes/:recipeId" component={RecipeView} />
            <Route exact path="/recipes/:recipeId/edit" component={EditRecipe} />
            <Route exact path="/login" component={Login} />
            <Route exact path="/logout" component={Logout} />
            <Route component={RouteNotFound} />
          </Switch>
        </main>
        <Footer />
      </UserContextProvider>
    </BrowserRouter>
  );
}
