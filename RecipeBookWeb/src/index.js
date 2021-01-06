import React from "react";
import ReactDOM from "react-dom";
import App from "./App";

window.RenderApp = (config) => {
	ReactDOM.render(<App config={config} />, document.getElementById("root"));
};
