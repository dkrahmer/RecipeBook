import { UserContextProvider } from "./Contexts/UserContext";
import { Menu } from "./Layout/Menu";
import { Footer } from "./Layout/Footer";
import { RecipesGrid } from "./Routes/RecipesGrid/RecipesGrid";
import { RecipeView } from "./Routes/RecipeView/RecipeView";
import { CreateRecipe } from "./Routes/CreateRecipe/CreateRecipe";
import { EditRecipe } from "./Routes/EditRecipe/EditRecipe";
import { CloneRecipe } from "./Routes/CloneRecipe/CloneRecipe";
import { Login } from "./Routes/Login/Login";
import { Logout } from "./Routes/Logout/Logout";
import { RouteNotFound } from "./Shared/RouteNotFound";
import { Redirect } from "react-router";
import "typeface-roboto";
import React, { useState, useEffect } from "react";
import {
	BrowserRouter,
	Switch,
	Route
} from "react-router-dom";
import CssBaseline from "@material-ui/core/CssBaseline";
import "./Content/site.scss";

export default function App(props) {
	const [config, setConfig] = useState(props.config);

	useEffect(() => {
		setConfig(props.config);
	}, [config]); // eslint-disable-line react-hooks/exhaustive-deps

	const moreProps = { config: config };

	return (
		<BrowserRouter>
			<CssBaseline />
			<UserContextProvider config={config}>
				<Menu />
				<main id="main-content">
					<Switch>
						<Route exact path="/" render={() => (<Redirect to="/recipes" />)} />
						<Route exact path="/recipes" render={(props) => <RecipesGrid {...{ ...props, ...moreProps }} />} />
						<Route exact path="/recipes/create" render={(props) => <CreateRecipe {...{ ...props, ...moreProps }} />} />
						<Route exact path="/recipes/:recipeId" render={(props) => <RecipeView {...{ ...props, ...moreProps }} />} />
						<Route exact path="/recipes/:recipeId/edit" render={(props) => <EditRecipe {...{ ...props, ...moreProps }} />} />
						<Route exact path="/recipes/:recipeId/clone" render={(props) => <CloneRecipe {...{ ...props, ...moreProps }} />} />
						<Route exact path="/login" render={(props) => <Login {...{ ...props, ...moreProps }} />} />
						<Route exact path="/logout" render={(props) => <Logout {...{ ...props, ...moreProps }} />} />
						<Route component={RouteNotFound} />
					</Switch>
				</main>
				<Footer />
			</UserContextProvider>
		</BrowserRouter >
	);
}
