import { createAxiosApi } from "../Helpers/axiosApiHelper";
import { UserContext } from "../Contexts/UserContext";
import { useContext } from "react";
import { DateTime } from "luxon";
import Cookies from "js-cookie";
import jwt from "jsonwebtoken";

export function useUserContext(config) {
	const [user, triggerResetUserFromToken] = useContext(UserContext);

	function login(newAuthToken, handleResponse) {
		const body = {
			token: newAuthToken
		};

		createAxiosApi("Auth", user, config)
			.post("/login", body)
			.then((response) => {
				if (response && response.status === 200 && response.data.token) {
					setUserToken(response.data.token, config.authTokenKey);
					triggerResetUserFromToken();
					handleResponse(true);
				}
				else {
					logout();
					handleResponse(false);
				}
			})
			.catch((error) => {
				console.log(error.response);
				logout();
				handleResponse(false);
			});
	}

	function logout() {
		removeUserToken(config.authTokenKey);
		triggerResetUserFromToken();
	}

	return {
		login,
		logout,
		triggerResetUserFromToken,
		...user
	};
}

function setUserToken(token, authTokenKey) {
	const decodedToken = jwt.decode(token);
	const expireDate = DateTime.fromMillis(decodedToken.exp * 1000);

	Cookies.set(authTokenKey, token, { expires: expireDate.toJSDate() });
}

function removeUserToken(authTokenKey) {
	Cookies.remove(authTokenKey);
}
