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
  Typography
} from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import FastFoodIcon from "@material-ui/icons/Fastfood";
//import SettingsIcon from "@material-ui/icons/Settings";
import ArrowForwardIcon from "@material-ui/icons/ArrowForward";
import ArrowBackIcon from "@material-ui/icons/ArrowBack";
import CreateIcon from "@material-ui/icons/Create";

export function Menu() {
  const [isOpenDrawer, setIsOpenDrawer] = useState(false);
  const user = useUserContext();

  const alwaysActions = [{
    text: "Recipes",
    url: "/recipes",
    icon: <FastFoodIcon />
  },
];

  const signedOutActions = [{
    text: "Login",
    url: "/login",
    icon: <ArrowForwardIcon />
  }];

  const signedInActions = [/*{
    text: "Settings",
    url: "/settings",
    icon: <SettingsIcon />
  }, */ {
    text: "Create Recipe",
    url: "/recipes/create",
    icon: <CreateIcon />
  },
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
    </React.Fragment>
  );
}
