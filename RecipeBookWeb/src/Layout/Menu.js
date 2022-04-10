import { useRecipeService } from "../Hooks/useRecipeService";
import { useUserContext } from "../Hooks/useUserContext";
import { NavigationDrawer } from "./NavigationDrawer";
import { RouterLink } from "../Shared/RouterLink";
import React, {
	useState
} from "react";
import {
	AppBar,
	Toolbar,
	IconButton,
	Typography,
	Snackbar,
	SnackbarContent,
	LinearProgress
} from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import FastFoodIcon from "@material-ui/icons/Fastfood";
//import SettingsIcon from "@material-ui/icons/Settings";
import ArrowForwardIcon from "@material-ui/icons/ArrowForward";
import ArrowBackIcon from "@material-ui/icons/ArrowBack";
import CreateIcon from "@material-ui/icons/Create";
import LaunchIcon from "@material-ui/icons/Launch";
import CheckCircleIcon from "@material-ui/icons/CheckCircle";
import CloseIcon from "@material-ui/icons/Close";
import green from "@material-ui/core/colors/green";

export function Menu(props) {
	const recipeService = useRecipeService(props.config);
	const [isOpenDrawer, setIsOpenDrawer] = useState(false);
	const [isSendToKitchenOpen, setIsSendToKitchenOpen,] = useState(false);
	const [isExecutingSendToKitchen, setIsExecutingSendToKitchen] = useState(false);
	const user = useUserContext(props.config);

	const alwaysActions = [{
		text: "Recipes",
		url: "/recipes",
		icon: <FastFoodIcon />
	}];

	const signedOutActions = [{
		text: "Login",
		url: "/login",
		icon: <ArrowForwardIcon />
	}];

	const sendToKitchen = () => {
		setIsExecutingSendToKitchen(true);
		setIsSendToKitchenOpen(true);
		recipeService.sendTo("kitchen", window.location.href, (response) => {
			if (response && response.status === 200 && response.data) {
				setTimeout(() => setIsSendToKitchenOpen(false), 3000);
			} else {
				setIsSendToKitchenOpen(false);
				console.log(response);
				alert(response.status);
			}

			setIsExecutingSendToKitchen(false);
		}, (error) => {
			setIsSendToKitchenOpen(false);
			console.log(error);
			if (error.response) {
				console.log(error.response);
				alert(error.response.status);
			}
			else {
				alert(error);
			}

			setIsExecutingSendToKitchen(false);
		});

		return false;
	};

	const signedInActions = [
		/*{
			text: "Settings",
			url: "/settings",
			icon: <SettingsIcon />
		}, */
		{
			text: "Create Recipe",
			url: "/recipes/create",
			icon: <CreateIcon />
		},
		"divider",
		{
			text: "Send to Kitchen",
			url: "#",
			onClick: sendToKitchen,
			icon: <LaunchIcon />
		},
		"divider",
		{
			text: "Logout",
			url: "/logout",
			icon: <ArrowBackIcon />
		}];

	function toggleDrawer() {
		setIsOpenDrawer(!isOpenDrawer);
	}

	return (
		<React.Fragment>
			<NavigationDrawer
				header="Recipe Book"
				isOpen={isOpenDrawer}
				toggleOpen={toggleDrawer}
				primaryActions={alwaysActions}
				otherActions={user.isLoggedIn ? signedInActions : signedOutActions} />
			<AppBar position="static" color="primary">
				<Toolbar>
					<IconButton color="inherit" onClick={toggleDrawer}>
						<MenuIcon />
					</IconButton>
					<Typography variant="h6" color="inherit" style={{ flexGrow: 1 }}>
						<RouterLink to="/" className="inherit-style" >
							Recipe Book
						</RouterLink>
					</Typography>
					{!user.isLoggedIn ? null : (
						<Typography variant="h6" color="inherit">
							Hello, {user.firstName}!
						</Typography>
					)}
				</Toolbar>
			</AppBar>
			<SendToKitchenSnackBar
				open={isSendToKitchenOpen}
				setIsSendToKitchenOpen={setIsSendToKitchenOpen}
				isExecuting={isExecutingSendToKitchen}
			/>
		</React.Fragment>
	);
}

function SendToKitchenSnackBar(props) {
	return (
		<Snackbar
			anchorOrigin={{ vertical: "top", horizontal: "center" }}
			open={props.open}
			autoHideDuration={8000}
			onClose={props.onClose}
			style={{ marginTop: 20 }}>
			<SnackbarContent
				style={{ backgroundColor: green[600] }}
				message={<div className="rb-snackbar-message">
					<CheckCircleIcon />
					<span style={{ paddingLeft: 10 }}>{props.isExecuting ? "Sending to kitchen..." : "This page has been sent to kitchen!"}</span>
					{props.isExecuting ? (<LinearProgress />) : (null)}
				</div>}
				action={<IconButton onClick={() => props.setIsSendToKitchenOpen(false)}>
					<CloseIcon />
				</IconButton>}
			/>
		</Snackbar>
	);
}
