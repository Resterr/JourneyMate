import React, { useContext, useEffect, useState } from "react";
import "./searchAddress.css";
import {
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
import { Address } from "../../models/Address";

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

type SearchAddressProps = {
    onSelectedAddressChange: (selectedAddress: string) => void;
};

const SearchAddress: React.FC<SearchAddressProps> = ({
    onSelectedAddressChange,
}) => {
    const userContext = useContext(UserContext);
    const [addresses, SetAddresses] = useState<Address[]>([]);
    const [selectedAddress, setSelectedAddress] = useState<string>("");

    useEffect(() => {
        let token: string | null = userContext.accessToken;
        let config = {
            headers: { Authorization: `Bearer ${token}` },
        };

        axiosInstance
            .get<Address[]>("/api/address", config)
            .then((response: AxiosResponse<Address[]>) => {
                console.log(response.data);
                if (response.data.length !== 0) {
                    SetAddresses(response.data);
                }
            })
            .catch((error) => {
                console.log(error);
            });
    }, []);

    const handleChange = (event: SelectChangeEvent) => {
        setSelectedAddress(event.target.value as string);
        onSelectedAddressChange(event.target.value as string);
    };

    return (
        <div className="searchPlace__address">
            <FormControl className="searchPlace__address-form">
                <InputLabel
                    sx={{ color: "#00AAFF" }}
                    className="searchPlace__address-form-label"
                >
                    Adres
                </InputLabel>
                <Select
                    sx={{ color: "#00AAFF" }}
                    className="searchPlace__address-form-select"
                    id="searchPlace__address-form-select"
                    value={selectedAddress}
                    onChange={handleChange}
                    input={<OutlinedInput label="Address" />}
                    MenuProps={MenuProps}
                >
                    {addresses.map((x) => (
                        <MenuItem key={x.id} value={x.id}>
                            <ListItemText
                                primary={
                                    x.locality +
                                    ", " +
                                    x.administrativeAreaLevel2 +
                                    ", " +
                                    x.administrativeAreaLevel1
                                }
                            />
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
        </div>
    );
};

export default SearchAddress;
