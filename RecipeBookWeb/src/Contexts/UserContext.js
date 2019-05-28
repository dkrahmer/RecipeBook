import { AuthTokenKey } from "../config";
import { DebugMode } from "../config";
import React, {
	useState
} from "react";
import Cookies from "js-cookie";
import jwt from "jsonwebtoken";

export const UserContext = React.createContext([{}, () => { }]);

export function UserContextProvider(props) {
	const [user, setUser] = useState(() => getUserFromToken());

	function triggerResetUserFromToken() {
		setUser(getUserFromToken());
	}

	return (
		<UserContext.Provider value={[user, triggerResetUserFromToken]}>
			{props.children}
		</UserContext.Provider>
	);
}

function getUserFromToken() {
	let userInfo;

	if (DebugMode) {
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
		const token = Cookies.get(AuthTokenKey);
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
