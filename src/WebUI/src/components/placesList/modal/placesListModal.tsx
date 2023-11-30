import * as React from "react";
import {Box, Button, Modal, Typography} from "@mui/material";
import {Place} from "../../../models/Place";

type PlacesListModalProps = {
	placeDetails: Place;
}

const PlacesListModal : React.FC<PlacesListModalProps> = (props) => {
	const place: Place = props.placeDetails;
	const [open, setOpen] = React.useState(false);
	const handleOpen = () => {
		setOpen(true);
	}
	const handleClose = () => setOpen(false);

	const style = {
		position: "absolute",
		top: "50%",
		left: "50%",
		transform: "translate(-50%, -50%)",
		width: 800,
		bgcolor: "#352961",
		border: "2px solid #b17a41",
		color: "#ffffff",
		boxShadow: 24,
		display: "flex",
		flexDirection: "column",
		justifyContent: "center",
		alignItems: "center",
		p: 4,
	};
	
	return (
		<div>
			<Button sx={{color: "#FFFFFF"}} onClick={handleOpen}>Details</Button>
			<Modal
				open={open}
				onClose={handleClose}
				aria-labelledby="modal-modal-title"
				aria-describedby="modal-modal-description"
			>
				<Box sx={style}>
					<Typography id="modal-modal-title" variant="h6" component="h2">
						<div>Rating: {place.rating}</div>
						<div>Total ratings: {place.userRatingsTotal}</div>
						<div>Distance from address: {place.distanceFromAddress}</div>
						<div>Business status: {place.businessStatus}</div>
						<div>Types: {place.types.map(x => x.name + ", ")}</div>
					</Typography>
				</Box>
			</Modal>
		</div>
	);
}

export default PlacesListModal;