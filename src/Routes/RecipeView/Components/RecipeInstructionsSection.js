import React from "react";
import {
  Typography,
  Divider,
  Table
} from "@material-ui/core";

export function RecipeInstructionsSection(props) {
  let instructionNumber = 1;
  let tableRows = props.instructions.split('\n').map((item, key) => {
    if (item.trim() === "")
      return null;

    if (item.startsWith("[") && item.endsWith("]"))
    {
      instructionNumber = 1; // Restart numbering for the next section
      return <tr key={key}><td colSpan="2" className="rb-recipe-instruction-list-heading">{item.substr(1, item.length - 2)}</td></tr>
    }
    else
    {
      return <tr key={key}><td>{instructionNumber++}.</td><td>{item}</td></tr>
    }
  });

  return (
    <div className="rb-recipe-info rb-recipe-instruction-list">
      <Typography variant="h6" color="primary">
        {props.title}
      </Typography>
      <Table className="rb-recipe-info-body rb-recipe-instruction-list">
        <tbody>
          {tableRows}
        </tbody>
      </Table>
      <Divider style={{ marginTop: 12 }} />
    </div>
  );
}
