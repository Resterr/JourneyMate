import React, {useContext, useEffect, useState} from "react";
import "./searchHistory.css";
import axiosInstance from "../../utils/axiosInstance";
import {AxiosResponse} from "axios";
import {Link, useNavigate, useParams} from "react-router-dom";
import {UserContext} from "../../contexts/userContext";
import {Pagination} from "@mui/material";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import {PaginatedReports} from "../../models/PaginatedReports";

const SearchDisplay : React.FC = () => {
	const {id} = useParams();
	const userContext = useContext(UserContext);
	const currentUser = userContext.currentUser;
	const navigate = useNavigate();
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [pageSize] = useState<number>(10);
	const [paginatedReports, setPaginatedReports] =
		useState<PaginatedReports | null>(null);
	
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
			.get<PaginatedReports>(`/api/place/report?PageNumber=${pageNumber}&PageSize=${pageSize}`, config)
			.then((response : AxiosResponse<PaginatedReports>) => {
				if (response.status === 200) {
					setPaginatedReports(response.data);
				} else {
					navigate(`/searchPlaces`);
				}
			})
			.catch((error) => {
				console.log(error);
			});
	}, [id, navigate, pageNumber, pageSize, userContext.accessToken]);
	
	const handlePaginationChange = (_event: React.ChangeEvent<unknown>, value: number) => {
		setPageNumber(value);
	};
	
	return (
		<div className="searchHistory">
			{paginatedReports !== null ? (
				<><div className="searchHistory__reportList">
					<List sx={{width: '100%', maxWidth: 1400}}>
						{paginatedReports.items.map((report) => {
							const labelId = `checkbox-list-label-${report.id}`;
							return (
								<Link to={`/searchDisplay/${report.id}`}>
									<ListItem
										key={report.id}
										disablePadding
									>
										<ListItemButton role={undefined} dense>
											<ListItemText id={labelId} primary={`${report.id} - ${report.created}`}/>
										</ListItemButton>
									</ListItem>
								</Link>
							);
						})}
					</List>
				</div>
					<div className="searchHistory__pagination">
						<Pagination className="searchHistory__pagination-items" variant="outlined" count={paginatedReports.totalPages} page={pageNumber} onChange={handlePaginationChange}/>
					</div>
				</>
			) : (
				<p>Loading...</p>
			)}
		</div>
	);
};

export default SearchDisplay;
