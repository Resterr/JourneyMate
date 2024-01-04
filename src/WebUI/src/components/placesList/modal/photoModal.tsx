import * as React from "react";
import { Box, Button, Modal, Typography } from "@mui/material";
import { Place } from "../../../models/Place";

type PlacesListModalProps = {
    placeDetails: Place;
};

const PhotoModal: React.FC<PlacesListModalProps> = (props) => {
    const place: Place = props.placeDetails;
    const [open, setOpen] = React.useState(false);

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
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        p: 4,
    };

    return (
        <div>
            <Button sx={{ color: "#FFFFFF" }} onClick={handleOpen}>
                Zdjęcie
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
                        {place.photo !== null ? (
                            <div>
                                <img
                                    src={place.photo}
                                    alt={`${place.name}`}
                                    style={{
                                        maxWidth: "100%",
                                        maxHeight: "100%",
                                        objectFit: "contain",
                                    }}
                                />
                            </div>
                        ) : (
                            <p>No photo data</p>
                        )}
                    </Typography>
                </Box>
            </Modal>
        </div>
    );
};

export default PhotoModal;
