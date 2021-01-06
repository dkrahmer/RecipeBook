import React from "react";
import CircularProgress from "@material-ui/core/CircularProgress";

export default function PageOverlay(props) {
	return (
		<React.Fragment>
			{props.showOverlay ? (
				<div className="overlay" style={props.style}>
					<div className="overlay-opacity" />
					<CircularProgress style={{ margin: 15 }} />
					{props.children}
				</div>)
				: (<React.Fragment></React.Fragment>)}
		</React.Fragment>
	);
}
