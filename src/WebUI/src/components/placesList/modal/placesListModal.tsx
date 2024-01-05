import * as React from "react";
import { Box, Button, Modal, Typography } from "@mui/material";
import { Place } from "../../../models/Place";
import config from "../../../utils/config";

type PlacesListModalProps = {
    placeDetails: Place;
};

const PlacesListModal: React.FC<PlacesListModalProps> = (props) => {
    const place: Place = props.placeDetails;
    const [open, setOpen] = React.useState(false);
    const apiKey = config.googleMapsApiKey;
    const mapSrc = `https://www.google.com/maps/embed/v1/place?q=${place.location.latitude},${place.location.longitude}&key=${apiKey}`;

    const handleOpen = () => {
        setOpen(true);
    };
    const handleClose = () => setOpen(false);

    const style = {
        position: "absolute",
        top: "50%",
        left: "50%",
        transform: "translate(-50%, -50%)",
        width: 1000,
        bgcolor: "#352961",
        border: "2px solid #b17a41",
        color: "#ffffff",
        boxShadow: 24,
        display: "flex",
        textAlign: "center",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        p: 4,
    };

    return (
        <div>
            <Button sx={{ color: "#FFFFFF" }} onClick={handleOpen}>
                Szczegóły
            </Button>
            <Modal
                open={open}
                onClose={handleClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <Typography
                        id="modal-modal-title"
                        variant="h6"
                        component="h2"
                    >
                        <div>Ocena: {place.rating}</div>
                        <div>Liczba ocen: {place.userRatingsTotal}</div>
                        <div>Adres miejsca: {place.vicinity}</div>
                        <div>
                            Rodzaje miejsc:{" "}
                            {place.types.map((x) => x.name).join(", ")}
                        </div>
                        <div>
                            <iframe
                                width="600"
                                height="450"
                                style={{ border: 0 }}
                                src={mapSrc}
                                allowFullScreen
                                title="Google Map"
                            ></iframe>
                        </div>
                    </Typography>
                </Box>
            </Modal>
        </div>
    );
};

export default PlacesListModal;
