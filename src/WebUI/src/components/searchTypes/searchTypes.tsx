import React, { useContext, useEffect, useState } from "react";
import "./searchTypes.css";
import {
    Checkbox,
    FormControl,
    InputLabel,
    ListItemText,
    MenuItem,
    OutlinedInput,
    Select,
    SelectChangeEvent,
} from "@mui/material";
import { UserContext } from "../../contexts/userContext";
import axiosInstance from "../../utils/axiosInstance";
import { AxiosResponse } from "axios";
import { Type } from "../../models/Type";

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

type SearchTypesProps = {
    onSelectedTypesChange: (selectedTypes: string[]) => void;
};

const SearchTypes: React.FC<SearchTypesProps> = ({ onSelectedTypesChange }) => {
    const userContext = useContext(UserContext);
    const [types, setTypes] = useState<Type[]>([]);
    const [selectedTypes, setSelectedTypes] = useState<string[]>([]);

    useEffect(() => {
        let token: string | null = userContext.accessToken;
        let config = {
            headers: { Authorization: `Bearer ${token}` },
        };
        axiosInstance
            .get<Type[]>("/api/address/type", config)
            .then((response: AxiosResponse<Type[]>) => {
                if (response.data.length !== 0) {
                    setTypes(response.data);
                }
            })
            .catch((error) => {
                console.log(error);
            });
    }, [userContext.accessToken]);

    const handleChange = (event: SelectChangeEvent<typeof selectedTypes>) => {
        const {
            target: { value },
        } = event;
        setSelectedTypes(typeof value === "string" ? value.split(",") : value);
        onSelectedTypesChange(event.target.value as string[]);
    };

    return (
        <div className="searchPlace__types">
            <FormControl className="searchPlace__types-form">
                <InputLabel
                    style={{ color: "#00AAFF" }}
                    className="searchPlace__types-form-label"
                >
                    Rodzaje miejsc
                </InputLabel>
                <Select
                    style={{ color: "#00AAFF" }}
                    className="searchPlace__types-form-select"
                    id="searchPlace__types-form-select"
                    multiple
                    value={selectedTypes}
                    onChange={handleChange}
                    input={<OutlinedInput label="Type" />}
                    renderValue={(selected) => selected.join(", ")}
                    MenuProps={MenuProps}
                >
                    {types.map((x) => (
                        <MenuItem key={x.name} value={x.name}>
                            <Checkbox
                                checked={selectedTypes.indexOf(x.name) > -1}
                            />
                            <ListItemText primary={x.name} />
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
        </div>
    );
};

export default SearchTypes;
