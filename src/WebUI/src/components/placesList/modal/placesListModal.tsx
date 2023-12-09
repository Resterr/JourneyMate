import * as React from "react";
import {useContext, useEffect, useState} from "react";
import {Box, Button, Modal, Typography} from "@mui/material";
import {Place} from "../../../models/Place";
import {UserContext} from "../../../contexts/userContext";
import axiosInstance from "../../../utils/axiosInstance";

type PlacesListModalProps = {
	placeDetails: Place;
}

const PlacesListModal : React.FC<PlacesListModalProps> = (props) => {
	const place : Place = props.placeDetails;
	const [open, setOpen] = React.useState(false);
	const userContext = useContext(UserContext);
	const [maxHeight] = useState<number>(500);
	const [maxWidth] = useState<number>(500);
	const [imageSrc, setImageSrc] = useState<string | null>(null);
	const [loading, setLoading] = useState<boolean>(true);
	
	useEffect(() => {
		const fetchImage = () => {
			let token : string | null = userContext.accessToken;
			let config = {
				headers: {Authorization: `Bearer ${token}`},
				responseType: 'blob' as 'blob',
			};
			axiosInstance
				.get(`/api/place/photo/${place.id}?MaxHeight=${maxHeight}&MaxWidth=${maxWidth}`, config)
				.then((response) => {
					const imageUrl = URL.createObjectURL(response.data);
					setImageSrc(imageUrl);
				})
				.catch((error) => {
					console.log(error);
				})
				.finally(() => {
					setLoading(false); // Set loading to false regardless of success or failure
				});
		};
		
		fetchImage();
		
		// Cleanup function to revoke the object URL when the component is unmounted
		return () => {
			if (imageSrc) {
				URL.revokeObjectURL(imageSrc);
			}
		};
	}, [imageSrc, maxHeight, maxWidth, place.id, userContext.accessToken]);
		
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
						<div>
							{loading ? (
								<p>Loading...</p>
							) : (
								imageSrc && (
									<img
										src={imageSrc}
										alt={`Photo of ${place.name}`}
										style={{ maxWidth: "100%", maxHeight: "100%", objectFit: "contain" }}
									/>
								)
							)}
						</div>
					</Typography>
				</Box>
			</Modal>
		</div>
	);
}

export default PlacesListModal;