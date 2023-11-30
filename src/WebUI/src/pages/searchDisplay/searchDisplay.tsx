import React, {useContext, useEffect, useState} from "react";
import "./searchDisplay.css";
import PlacesList from "../../components/placesList/placesList";
import axiosInstance from "../../utils/axiosInstance";
import {AxiosResponse} from "axios";
import {useNavigate, useParams} from "react-router-dom";
import {UserContext} from "../../contexts/userContext";
import {useForm} from "react-hook-form";
import {Pagination} from "@mui/material";
import {PaginatedPlaces} from "../../models/PaginatedPlaces";

type FormData = {
	name: string;
	places: string[];
};

const SearchDisplay : React.FC = () => {
	const {id} = useParams();
	const userContext = useContext(UserContext);
	const currentUser = userContext.currentUser;
	const navigate = useNavigate();
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [pageSize] = useState<number>(10);
	const [paginatedPlaces, setPaginatedPlaces] =
		useState<PaginatedPlaces | null>(null);
	const { handleSubmit, setValue } = useForm<FormData>();

	useEffect(() => {
		if (!currentUser) {
			navigate("/");
		}
	}, [currentUser, navigate]);
	
	useEffect(() => {
		let token : string | null = userContext.accessToken;
		let config = {
			headers: {Authorization: `Bearer ${token}`},
		};

		axiosInstance
			.get<PaginatedPlaces>(`/api/place/report/${id}/place?PageNumber=${pageNumber}&PageSize=${pageSize}`, config)
			.then((response : AxiosResponse<PaginatedPlaces>) => {
				if (response.status === 200) {
					setPaginatedPlaces(response.data);
				} else {
					navigate(`/searchPlaces`);
				}
			})
			.catch((error) => {
				console.log(error);
			});
	}, [id, navigate, pageNumber, pageSize, userContext.accessToken]);
	
	const handleSelectedPlacesChange = (selectedPlaces: string[]) => {
		setValue("places", selectedPlaces);
	};
	
	const handlePaginationChange = (_event: React.ChangeEvent<unknown>, value: number) => {
		setPageNumber(value);
	};
	
	return (
		<div className="searchDisplay">
			{paginatedPlaces !== null ? (
				<><PlacesList places={paginatedPlaces!.items} onSelectedPlacesChange={handleSelectedPlacesChange}/>
					<div className="searchDisplay__pagination">
						<Pagination className="searchDisplay__pagination-items" variant="outlined" count={paginatedPlaces.totalPages} page={pageNumber} onChange={handlePaginationChange}/>
					</div>
				</>
			) : (
				<p>Loading...</p>
				)}
		</div>
	);
};

export default SearchDisplay;
