import React, {useContext, useEffect} from "react";
import './searchPlaces.css';
import {useNavigate} from "react-router-dom";
import {UserContext} from "../../contexts/userContext";
import SearchTypes from "../../components/searchTypes/searchTypes";
import SearchAddress from "../../components/searchAddress/searchAddress";
import {SubmitHandler, useForm} from "react-hook-form";
import {AxiosResponse} from "axios";
import axiosInstance from "../../utils/axiosInstance";

type FormData = {
	addressId : string;
	types : string[];
};

const SearchPlaces : React.FC = () => {
	const userContext = useContext(UserContext);
	const currentUser = userContext.currentUser;
	const navigate = useNavigate();
	const {handleSubmit, setValue} = useForm<FormData>();

	useEffect(() => {
		if (!currentUser) {
			navigate("/");
		}
	}, [currentUser, navigate]);

	const onSubmit : SubmitHandler<FormData> = async (data : FormData) => {
		try {
			console.log("SEND");
			let token: string | null = userContext.accessToken;
			let config = {
				headers: { Authorization: `Bearer ${token}` },
			};

			const response : AxiosResponse<any> = await axiosInstance.post(
				"/api/place/report/generate",
				data, config
			);

			if (response.status === 201) {
/*				navigate(`/searchDisplay/${response.data}`);*/
			}
		} catch (error) {
			console.error(error);
		}
	};

	const handleSelectedAddressChange = (newSelectedAddress: string) => {
		setValue('addressId', newSelectedAddress);
	};

	const handleSelectedTypesChange = (newSelectedTypes: string[]) => {
		setValue('types', newSelectedTypes);
	};

	return (
		<div className="searchPlaces">
			<form onSubmit={handleSubmit(onSubmit)}>
				<SearchAddress onSelectedAddressChange={handleSelectedAddressChange}/>
				<SearchTypes onSelectedTypesChange={handleSelectedTypesChange} />

				<div className="searchPlaces-button">
					<button type="submit">Generate</button>
				</div>
			</form>
		</div>
	)
}

export default SearchPlaces;