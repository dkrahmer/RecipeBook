import React, {
  useState
} from "react";
import {
  Typography,
  Divider,
  Table
} from "@material-ui/core";
import ScaleEntryModal from "./ScaleEntryModal";

export function RecipeIngredientsSection(props) {
  const [isScaleModalOpen, setIsScaleModalOpen] = useState(false);
  let tableRows = props.ingredientsList.map((item, key) => {
    if (item.isHeading) {
      return <tr key={key}><td colSpan="2" className="rb-recipe-ingredient-list-heading">{item.name}</td></tr>
    }
    else {
      return <tr key={key}><td>{item.amount}</td><td>{item.name}</td></tr>
    }
  });

  let scale = props.scale;

  if (!scale)
    scale = "1";

  let scaleClassName = "scale-label";
  if (scale !== "1")
    scaleClassName += " scale-label-changed";

  let newScale = null;

  function showScaleDialog() {
    setIsScaleModalOpen(true);
  }

  function onScaleEntryModalApply() {
    if (newScale)
      props.setScale(newScale);
    setIsScaleModalOpen(false);
  }

  function onScaleEntryModalCancel() {
    setIsScaleModalOpen(false);
  }

  function onScaleChange(e) {
    newScale = e.target.value;
    return newScale;
  }

  let scaleLabel = <span className={scaleClassName}>(<button className="link-button" onClick={showScaleDialog}>{`scale: x ${scale}`}</button>)</span>

  return (
    <div className="rb-recipe-info">
      <Typography variant="h6" color="primary">
        {props.title} {scaleLabel}
      </Typography>
      <Table className="rb-recipe-info-body rb-recipe-ingredient-list">
        <tbody>
          {tableRows}
        </tbody>
      </Table>
      <Divider style={{ marginTop: 12 }} />
      <ScaleEntryModal
        isOpen={isScaleModalOpen}
        scale={scale}
        onApply={onScaleEntryModalApply}
        onCancel={onScaleEntryModalCancel}
        onScaleChange={onScaleChange} />
    </div>
  );
}
