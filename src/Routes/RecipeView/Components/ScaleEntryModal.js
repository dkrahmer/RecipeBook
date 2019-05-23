import React from "react";
import {
  Button,
  Dialog,
  DialogContent,
  DialogActions,
  DialogTitle,
  DialogContentText,
  TextField
} from "@material-ui/core";

function ScaleEntryModal({ isOpen, onApply, onCancel, scale, onScaleChange, ...props}) {
  const setDefaultFocus = el => {
    if (el)
    {
      el.focus();
      el.select();
    }
  };

  function onScaleKeyPress(e) {
    if(e.key === 'Enter'){
      onApply();
    }
    if(e.key === 'Esc'){
      onCancel();
    }
  }

  return (
    <Dialog open={isOpen} onClose={onCancel} {...props}>
      <DialogTitle>
        Recipe scale factor
      </DialogTitle>
      <DialogContent>
        <DialogContentText>
          Enter a number to scale the recipe (decimal or fraction)
        </DialogContentText>
        <TextField
          defaultValue={scale}
          type="number"
          label="Scale factor"
          margin="normal"
          variant="outlined"
          onChange={onScaleChange}
          inputRef={setDefaultFocus}
          onKeyPress={onScaleKeyPress} />
      </DialogContent>
      <DialogActions>
        <Button size="small" color="primary" onClick={onApply}>
          Apply
        </Button>
        <Button size="small" color="secondary" onClick={onCancel}>
          Cancel
        </Button>
      </DialogActions>
    </Dialog>
  );
}

export default ScaleEntryModal;
