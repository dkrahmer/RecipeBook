import React, {
	useState
} from "react";
import Cookies from "js-cookie";
import jwt from "jsonwebtoken";

export const UserContext = React.createContext([{}, () => { }]);

export function UserContextProvider(props) {
	const [user, setUser] = useState(() => getUserFromToken(props.config));

	function triggerResetUserFromToken() {
		setUser(getUserFromToken(props.config));
	}

	return (
		<UserContext.Provider value={[user, triggerResetUserFromToken]}>
			{props.children}
		</UserContext.Provider>
	);
}

function getUserFromToken(config) {
	let userInfo;

	if (config.debugMode) {
		userInfo = {
			isLoggedIn: true,
			id: 1,
			firstName: "Developer",
			lastName: "User",
			canViewRecipe: true,
			canEditRecipe: true,
			isAdmin: true
		};
	}
	else {
		const token = Cookies.get(config.authTokenKey);
		userInfo = {
			isLoggedIn: !!token,
			canViewRecipe: false,
			canEditRecipe: false,
			isAdmin: false
		};

		if (userInfo.isLoggedIn) {
			const decodedToken = jwt.decode(token);

			userInfo = {
				appUserId: decodedToken.AppUserId,
				username: decodedToken.Username,
				isLoggedIn: userInfo.isLoggedIn,
				firstName: decodedToken.FirstName,
				lastName: decodedToken.LastName,
				canViewRecipe: decodedToken.CanViewRecipe.toLowerCase() === "true",
				canEditRecipe: decodedToken.CanEditRecipe.toLowerCase() === "true",
				isAdmin: decodedToken.IsAdmin.toLowerCase() === "true",
				authToken: token
			};
		}
	}

	return userInfo;
}
