import { AuthTokenKey } from "../config";
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
  const debugMode = false;

  let userInfo;

  if (debugMode) {
    userInfo = {
      isLoggedIn: true,
      id: 1,
      firstName: "Test",
      lastName: "User",
      isAdmin: true,
    };
  }
  else {
    const token = Cookies.get(AuthTokenKey);
    userInfo = {
      isLoggedIn: !!token
    };

    if (userInfo.isLoggedIn) {
      const decodedToken = jwt.decode(token);

      userInfo.id = decodedToken.Id;
      userInfo.email = decodedToken.EmailAddress;
      userInfo.firstName = decodedToken.FirstName;
      userInfo.lastName = decodedToken.LastName;
      userInfo.isAdmin = decodedToken.IsAdmin;
      userInfo.authToken = token;
    }
  }

  return userInfo;
}
