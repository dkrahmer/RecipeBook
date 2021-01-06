import { useUserContext } from "../../Hooks/useUserContext";
import { LoadingWrapper } from "../../Shared/LoadingWrapper";
import React, {
	useState,
	useEffect
} from "react";
import { Redirect } from "react-router-dom";

export function Logout(props) {
	const [isLoading, setIsLoading] = useState(true);
	const user = useUserContext(props.config);

	useEffect(() => {
		setIsLoading(true);
		user.logout();
		setIsLoading(false);
	}, []); // eslint-disable-line react-hooks/exhaustive-deps

	return (
		<LoadingWrapper isLoading={isLoading}>
			<Redirect to="/" />
		</LoadingWrapper>
	);
}
