import React, { useContext, useEffect, useState } from "react";
import "./addSchedule.css";
import { useNavigate, useParams } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";
import { DateTimePicker, LocalizationProvider } from "@mui/x-date-pickers";
import dayjs, { Dayjs } from "dayjs";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import axiosInstance from "../../utils/axiosInstance";
import axios from "axios";
import { SubmitHandler, useForm } from "react-hook-form";
import PlaceNames from "../../components/placesNames/placeNames";

type FormData = {
    planId: string;
    placeId: string;
    startingDate: Date;
    endingDate: Date;
};

const AddSchedule: React.FC = () => {
    const { id } = useParams();
    const userContext = useContext(UserContext);
    const currentUser = userContext.currentUser;
    const navigate = useNavigate();
    const [startingDate, setStartingDate] = useState<Dayjs>(dayjs(new Date()));
    const [endingDate, setEndingDate] = useState<Dayjs | null>(null);
    const {
        register,
        handleSubmit,
        formState: { errors },
        setValue,
    } = useForm<FormData>();
    const [status, setStatus] = useState<string | null>(null);

    useEffect(() => {
        if (!currentUser) {
            navigate("/");
        }
    }, [currentUser, navigate]);

    const onSubmit: SubmitHandler<FormData> = async (data: FormData) => {
        try {
            if (id !== undefined) setValue("planId", id);
            let token: string | null = userContext.accessToken;
            let config = {
                headers: { Authorization: `Bearer ${token}` },
            };
            setValue("startingDate", startingDate.toDate());

            await axiosInstance.put(`/api/schedule`, data, config);
            setStatus("Zaktualizowano z sukcesem");
        } catch (error) {
            if (axios.isAxiosError(error)) {
                if (error.response) {
                    setStatus(error.response.data.Detail);
                }
            } else {
                setStatus("Błąd przy aktualizacji");
            }
        }
    };

    const handlePlaceNameChange = (placeId: string) => {
        setValue("placeId", placeId);
    };

    const handleStartingDateChange = (startingDate: Dayjs | null) => {
        if (startingDate != null) {
            setStartingDate(startingDate);
            setValue("startingDate", startingDate.toDate());
        }
    };

    const handleEndingDateChange = (endingDate: Dayjs | null) => {
        if (endingDate != null) {
            setEndingDate(endingDate);
            setValue("endingDate", endingDate.toDate());
        }
    };

    return (
        <div className="addSchedule">
            <div className="addSchedule__status">
                <h1>{status}</h1>
            </div>
            <form
                onSubmit={handleSubmit(onSubmit)}
                className="addSchedule__form"
            >
                <PlaceNames
                    planId={id ? id : ""}
                    onSelectedPlaceNameChange={handlePlaceNameChange}
                />
                <div className="addSchedule__form-calendar">
                    <div className="addSchedule__form-calendar-item">
                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DateTimePicker
                                className="addSchedule__form-calendar-item-picker"
                                value={startingDate}
                                onChange={(newValue) =>
                                    handleStartingDateChange(newValue)
                                }
                                ampm={false}
                                label="Data początkowa"
                                format="DD-MM-YYYY HH:mm"
                            />
                        </LocalizationProvider>
                    </div>
                    <div className="addSchedule__form-calendar-item">
                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DateTimePicker
                                className="addSchedule__form-calendar-item-picker"
                                value={endingDate}
                                onChange={(newValue) =>
                                    handleEndingDateChange(newValue)
                                }
                                ampm={false}
                                label="Data końcowa"
                                format="DD-MM-YYYY HH:mm"
                            />
                        </LocalizationProvider>
                    </div>
                </div>
                <button className="addSchedule__form-button" type="submit">
                    Dodaj
                </button>
            </form>
        </div>
    );
};

export default AddSchedule;
