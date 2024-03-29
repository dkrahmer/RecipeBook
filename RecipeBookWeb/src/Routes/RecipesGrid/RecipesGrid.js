import { useUserContext } from "../../Hooks/useUserContext";
import { useRecipeService } from "../../Hooks/useRecipeService";
import { FilterableRecipesGrid } from "./Components/FilterableRecipesGrid";
import React, {
	useEffect
} from "react";
import {
	Grid,
	Typography
} from "@material-ui/core";
import { RouterLink } from "../../Shared/RouterLink";
import MoodBadIcon from "@material-ui/icons/MoodBad";

export function RecipesGrid(props) {
	const user = useUserContext(props.config);
	const recipeService = useRecipeService(props.config);

	useEffect(() => {
		document.title = props.config.appName;
		if (!user || !user.canViewRecipe)
			return;
	}, []); // eslint-disable-line react-hooks/exhaustive-deps

	if (!user || !user.canViewRecipe)
		return (
			<Grid container spacing={8}>
				<Grid item xs={12}>
					<MoodBadIcon fontSize="large" />
					<Typography variant="subtitle1">
						Sorry, you must {user.isLoggedIn ? "" : "be logged in and "}have the "canViewRecipe" permission to view recipes!
					</Typography>
					<Typography style={{ paddingLeft: 40 }} variant="subtitle1">
						Logged in: {user.isLoggedIn ? "True" : "False"}
					</Typography>
					<Typography style={{ paddingLeft: 40 }} variant="subtitle1">
						canViewRecipe: {user.canViewRecipe ? "True" : "False"}
					</Typography>
					<Typography style={{ paddingTop: 20 }} variant="subtitle1">
						<RouterLink to="/login" >
							{user.isLoggedIn ? "" : "Login Now"}
						</RouterLink>
					</Typography>
				</Grid>
			</Grid>
		);

	return (
		<React.Fragment>
			<FilterableRecipesGrid recipeService={recipeService} location={props.location} history={props.history} />
		</React.Fragment>
	);
}
