import axios from "axios";

export function createAxiosApi(resource, user, config) {
	const api = axios.create({
		baseURL: `${config.apiUrl}/${resource}`
	});

	if (user.isLoggedIn) {
		const bearerAuthToken = `Bearer ${user.authToken}`;

		api.defaults.withCredentials = true;
		api.defaults.headers.common["Authorization"] = bearerAuthToken;
	}
	else {
		api.defaults.withCredentials = false;
		api.defaults.headers.common["Authorization"] = null;
	}

	return api;
}
