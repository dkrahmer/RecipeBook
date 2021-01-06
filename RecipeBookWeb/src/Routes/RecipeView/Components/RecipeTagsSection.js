import React, { Fragment } from "react";
import {
	Typography,
	Divider
} from "@material-ui/core";

export function RecipeTagsSection(props) {
	const tags = props.tags || [];

	return (
		<div className="rb-recipe-info tag-list">
			{tags.map(t => <Fragment><Typography component="span">{t}</Typography> </Fragment>)}
			<Divider style={{ marginTop: 5 }} />
		</div>
	);
}
