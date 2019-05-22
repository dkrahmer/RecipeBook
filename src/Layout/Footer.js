import React from "react";
import {
  Typography,
  Link
} from "@material-ui/core";

export function Footer() {
  const today = new Date();

  return (
    <footer className="footer-container">
      <Typography
        variant="subtitle1"
        align="center"
        color="inherit"
        component="p">
        <span>
          {`© ${today.getFullYear()} Doug Krahmer `}
        </span>
      </Typography>
    </footer>
  );
}
