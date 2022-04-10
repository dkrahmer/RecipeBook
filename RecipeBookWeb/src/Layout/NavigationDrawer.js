import { RouterLink } from "../Shared/RouterLink";
import React from "react";
import {
	List,
	ListItem,
	ListItemIcon,
	ListItemText,
	Drawer,
	Divider,
	Typography
} from "@material-ui/core";

export function NavigationDrawer(props) {
	const actions = [...props.primaryActions, ...props.otherActions];

	return (
		<Drawer open={props.isOpen} onClose={props.toggleOpen}>
			<Typography variant="h6" style={{ padding: 12 }}>
				{props.header}
			</Typography>
			<Divider />
			<NavigationDrawerList actions={actions} {...props} />
		</Drawer>
	);
}

function NavigationDrawerList(props) {
	let dividerCount = 0;
	return (
		<List>
			{props.actions.map(a => a === "divider" ?
				<Divider key={`divider${dividerCount++}`} style={{ "margin": 8 }} />
				: (
					<RouterLink
						key={a.text}
						to={a.url}
						onClick={() => {
							props.toggleOpen();
							if (a.onClick)
								return a.onClick();
						}}>
						<ListItem button>
							<ListItemIcon>
								{a.icon}
							</ListItemIcon>
							<ListItemText primary={a.text} />
						</ListItem>
					</RouterLink>
				))}
		</List>
	);
}
