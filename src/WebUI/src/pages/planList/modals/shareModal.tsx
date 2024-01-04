import * as React from "react";
import { useContext } from "react";
import { Box, Button, Modal, Typography } from "@mui/material";
import { SubmitHandler, useForm } from "react-hook-form";
import axiosInstance from "../../../utils/axiosInstance";
import axios from "axios";
import { UserContext } from "../../../contexts/userContext";

type PlacesListModalProps = {
    setStatus: (status: string) => void;
    planId: string;
};

type FormData = {
    planId: string;
    userName: string;
};

const ShareModal: React.FC<PlacesListModalProps> = (props) => {
    const userContext = useContext(UserContext);
    const [open, setOpen] = React.useState(false);
    const {
        register,
        handleSubmit,
        formState: { errors },
        setValue,
    } = useForm<FormData>();

    const onSubmit: SubmitHandler<FormData> = async (data: FormData) => {
        try {
            let token: string | null = userContext.accessToken;
            let config = {
                headers: { Authorization: `Bearer ${token}` },
            };
            setValue("planId", props.planId);
            await axiosInstance.put("/api/plan/share", data, config);
            props.setStatus("Successfully shared");
            setOpen(false);
        } catch (error) {
            if (axios.isAxiosError(error)) {
                if (error.response) {
                    props.setStatus(error.response.data.Detail);
                }
            } else {
                props.setStatus("Failed to share");
            }
            setOpen(false);
        }
    };

    const handleOpen = () => {
        setOpen(true);
    };
    const handleClose = () => setOpen(false);

    const style = {
        position: "absolute",
        top: "50%",
        left: "50%",
        transform: "translate(-50%, -50%)",
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
        <div className="shareModal">
            <Button
                sx={{ color: "#FFFFFF", paddingRight: "5px" }}
                onClick={handleOpen}
            >
                Udostępnij
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
                        <form
                            onSubmit={handleSubmit(onSubmit)}
                            className="shareModal__form"
                        >
                            <div
                                className="shareModal__form-name"
                                style={{ textAlign: "center" }}
                            >
                                Username to share:
                                <br />
                                <input
                                    style={{ width: 300 }}
                                    id="shareModel__form-name-input"
                                    {...register("userName", {
                                        required: "This is required",
                                        minLength: 3,
                                    })}
                                />
                                <br />
                                <p>{errors.userName?.message}</p>
                            </div>
                            <div
                                className="shareModal__form-button"
                                style={{ textAlign: "center" }}
                            >
                                <button type="submit">Share</button>
                            </div>
                        </form>
                    </Typography>
                </Box>
            </Modal>
        </div>
    );
};

export default ShareModal;
