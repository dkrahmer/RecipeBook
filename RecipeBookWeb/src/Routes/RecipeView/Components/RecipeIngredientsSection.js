import React, { useState } from "react";
import {
	Typography,
	Divider,
	Table
} from "@material-ui/core";
import ScaleEntryModal from "./ScaleEntryModal";
import Linkify from "react-linkify";
import { Slide } from "react-slideshow-image";
import "./RecipeIngredientsSection.scss";
import "react-slideshow-image/dist/styles.css"
import OutsideClickHandler from 'react-outside-click-handler';

export function RecipeIngredientsSection(props) {
	const defaultImageSize = 300;
	const [isScaleModalOpen, setIsScaleModalOpen] = useState(false);
	const [checkedRows, setCheckedRows] = useState({});
	const [imageSize, setImageSize] = useState(defaultImageSize);

	function onRowClick(e, key) {
		const newCheckedRows = { ...checkedRows };

		// Reverse the checked state of the selected row
		if (newCheckedRows[key]) {
			delete newCheckedRows[key];
		}
		else {
			newCheckedRows[key] = true;
		}

		setCheckedRows(newCheckedRows);

		e.preventDefault();
		return false;
	}
	const ingredientsList = props.ingredientsList || [];
	let tableRows = ingredientsList.map((item, key) => {
		if (item.isHeading) {
			return (<tr key={key}><td colSpan="2" className="rb-recipe-ingredient-list-heading">{item.name}</td></tr>);
		}
		else {
			return (<tr key={key} onClick={(e) => onRowClick(e, key)} className={checkedRows[key] ? "checked-row" : "unchecked-row"}><td>{item.amount}</td><td>{item.name}</td></tr>);
		}
	});

	let scale = props.scale;

	if (!scale)
		scale = "1";

	let scaleClassName = "scale-label";
	if (scale !== "1")
		scaleClassName += " scale-label-changed";

	let systemClassName = "scale-label";
	if (props.system)
		systemClassName += " scale-label-changed";

	let convertToMassClassName = "scale-label";
	if (props.convertToMass)
		convertToMassClassName += " scale-label-changed";

	let newScale = null;

	function showScaleDialog() {
		setIsScaleModalOpen(true);
	}

	function toggleSystem() {
		if (props.system !== "metric") {
			props.setSystem("metric");
		}
		else {
			props.setSystem(null);
		}
	}

	function toggleConvertToMass() {
		if (props.convertToMass !== "1") {
			props.setConvertToMass("1");
		}
		else {
			props.setConvertToMass(null);
		}
	}

	function onScaleEntryModalApply() {
		if (newScale)
			props.setScale(newScale);

		setIsScaleModalOpen(false);
	}

	function onScaleEntryModalCancel() {
		setIsScaleModalOpen(false);
	}

	function onScaleChange(value) {
		newScale = value;
		return newScale;
	}

	function handleImageClick(event) {
		toggleImageSize();
		event.stopPropagation();
	}

	function handleOutsideImageClick(event) {
		if (event && event.target.localName === "path") // previous and next buttons are paths
			return;

		setImageSize(defaultImageSize);
	}

	function toggleImageSize() {
		setImageSize(imageSize === defaultImageSize ? 700 : defaultImageSize);
	}

	let scaleLabel = (<span className={scaleClassName}>(<button className="link-button" onClick={showScaleDialog}>{`scale: x ${scale}`}</button>)</span>);
	let systemLabel = (<span className={systemClassName}>(<button className="link-button" onClick={toggleSystem}>{`metric: ${props.system === "metric" ? "on" : "off"}`}</button>)</span>);
	let convertToMassLabel = (<span className={convertToMassClassName}>(<button className="link-button" onClick={toggleConvertToMass}>{`mass: ${props.convertToMass ? "on" : "off"}`}</button>)</span>);
	const multipleImages = props.imageUrls.length > 1;
	const imageStyle = { width: imageSize, height: imageSize };

	return (
		<div className="rb-recipe-info">
			{props.imageUrls && props.imageUrls.length > 0 ?
				<div style={{ float: "right" }}>
					<div>
						<OutsideClickHandler
							onOutsideClick={handleOutsideImageClick}
						>
							<Slide
								className={`slides ${imageSize === defaultImageSize ? "" : "fixed-slides"}`}
								style={imageStyle}
								easing="ease"
								arrows={multipleImages}
								autoplay={multipleImages}
								canSwipe={multipleImages}
								onClick={handleOutsideImageClick}
							>
								{props.imageUrls.map(imageUrl => (
									<div key={imageUrl} className="each-slide">
										<div style={imageStyle}><img alt="" src={`${imageUrl}/${imageSize}x${imageSize}`} onClick={handleImageClick} /></div>
									</div>
								))}
							</Slide>
						</OutsideClickHandler>
					</div>
				</div>
				: <React.Fragment />
			}
			<div style={{ float: "left" }}>
				<Typography variant="h6" color="primary">
					{props.title} {scaleLabel} {systemLabel} {convertToMassLabel}
				</Typography>
				<Linkify properties={{ target: "_blank" }}>
					<Table className={"rb-recipe-info-body rb-recipe-ingredient-list" + (scale === "1" ? "" : " rb-scale-enabled")}>
						<tbody>
							{tableRows}
						</tbody>
					</Table>
				</Linkify>
			</div>
			<div style={{ clear: "both" }} />
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
