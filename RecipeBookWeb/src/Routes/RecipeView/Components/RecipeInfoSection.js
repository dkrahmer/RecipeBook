import React from "react";
import {
	Typography,
	Divider
} from "@material-ui/core";
import Linkify from "react-linkify";

export function RecipeInfoSection(props) {
	return (
		<div className="rb-recipe-info">
			<Typography variant="h6" color="primary">
				{props.title}
			</Typography>
			<Typography variant="body1" className="rb-recipe-info-body">
				<Linkify properties={{ target: "_blank" }}>
					{props.body}
				</Linkify>
			</Typography>
			<Divider style={{ marginTop: 12 }} />
		</div>
	);
}
