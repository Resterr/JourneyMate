import React, {useContext, useEffect, useState} from "react";
import './searchAddress.css';
import {
	FormControl,
	InputLabel,
	ListItemText,
	MenuItem,
	OutlinedInput,
	Select,
	SelectChangeEvent
} from "@mui/material";
import {UserContext} from "../../contexts/userContext";
import axiosInstance from "../../utils/axiosInstance";
import {AxiosResponse} from "axios";

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
}

type Address = {
	id: string;
	placeId: string;
	location: {
		latitude: number;
		longitude: number;
	};
	locality: {
		shortName: string;
		longName: string;
	};
	administrativeAreaLevel2: {
		shortName: string;
		longName: string;
	};
	administrativeAreaLevel1: {
		shortName: string;
		longName: string;
	};
	country: {
		shortName: string;
		longName: string;
	};
	postalCode: {
		shortName: string;
		longName: string;
	};
};

const SearchAddress : React.FC<SearchAddressProps> = ({ onSelectedAddressChange }) => {
	const userContext = useContext(UserContext);
	const [addresses, SetAddresses] = useState<Address[]>([]);
	const [selectedAddress, setSelectedAddress] = useState<string>('');

	useEffect(() => {
		let token: string | null = userContext.accessToken;
		let config = {
			headers: { Authorization: `Bearer ${token}` },
		};

		axiosInstance
			.get<Address[]>("/api/address", config)
			.then((response: AxiosResponse<Address[]>) => {
				if (response.data.length !== 0) {
					SetAddresses(response.data);
				}
			})
			.catch((error) => {
				console.log(error);
			});
	}, [userContext.accessToken]);

	const handleChange = (event: SelectChangeEvent) => {
		setSelectedAddress(event.target.value as string);
		onSelectedAddressChange(event.target.value as string);
	};

	return (
		<div className="searchPlace__address">
			<FormControl className="searchPlace__address-form">
				<InputLabel style={{ color: '#00AAFF' }} className="searchPlace__address-form-label">Address</InputLabel>
				<Select style={{ color: '#00AAFF' }} className="searchPlace__address-form-select"
					id="searchPlace__address-form-select"
					value={selectedAddress}
					onChange={handleChange}
					input={<OutlinedInput label="Address" />}
					MenuProps={MenuProps}
				>
					{addresses.map((x) => (
						<MenuItem key={x.id} value={x.id}>
							<ListItemText primary={x.locality.shortName} />
						</MenuItem>
					))}
				</Select>
			</FormControl>
		</div>
	);
}

export default SearchAddress;