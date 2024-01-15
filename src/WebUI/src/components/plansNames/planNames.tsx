import React, { useContext, useEffect, useState } from "react";
import "./planNames.css";
import { UserContext } from "../../contexts/userContext";
import axiosInstance from "../../utils/axiosInstance";
import { AxiosResponse } from "axios";
import {
    FormControl,
    InputLabel,
    ListItemText,
    MenuItem,
    OutlinedInput,
    Select,
    SelectChangeEvent,
} from "@mui/material";
import { PlanName } from "../../models/PlanName";

const ITEM_HEIGHT = 80;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    PaperProps: {
        style: {
            maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
            width: 250,
        },
    },
};

type PlanNamesProps = {
    onSelectedPlanNameChange: (selectedPlanName: string) => void;
};

const PlanNames: React.FC<PlanNamesProps> = (props) => {
    const userContext = useContext(UserContext);
    const [planNames, setPlanNames] = useState<PlanName[]>([]);
    const [selectedPlanName, setSelectedPlanName] = useState<string>("");

    useEffect(() => {
        let token: string | null = userContext.accessToken;
        let config = {
            headers: { Authorization: `Bearer ${token}` },
        };

        axiosInstance
            .get<PlanName[]>("/api/plan/names", config)
            .then((response: AxiosResponse<PlanName[]>) => {
                if (response.data.length !== 0) {
                    setPlanNames(response.data);
                }
            })
            .catch((error) => {
                console.log(error);
            });
    }, []);

    const handleChange = (event: SelectChangeEvent) => {
        console.log(event.target.value as string);
        setSelectedPlanName(event.target.value as string);
        props.onSelectedPlanNameChange(event.target.value as string);
    };

    return (
        <div className="planNames">
            <FormControl className="planNames__form">
                <InputLabel
                    sx={{ color: "#00AAFF" }}
                    className="planNames__form-label"
                >
                    Plan
                </InputLabel>
                <Select
                    sx={{ color: "#00AAFF" }}
                    className="planNames__form-select"
                    id="planNames__form-select"
                    value={selectedPlanName}
                    onChange={handleChange}
                    input={<OutlinedInput label="Plan" />}
                    MenuProps={MenuProps}
                >
                    {planNames.map((x) => (
                        <MenuItem key={x.id} value={x.id}>
                            <ListItemText primary={x.name} />
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
        </div>
    );
};

export default PlanNames;
