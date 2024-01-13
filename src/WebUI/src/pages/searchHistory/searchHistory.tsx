import React, { useContext, useEffect, useState } from "react";
import "./searchHistory.css";
import axiosInstance from "../../utils/axiosInstance";
import { AxiosResponse } from "axios";
import { Link, useNavigate, useParams } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";
import { Pagination, Rating } from "@mui/material";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import { PaginatedReports } from "../../models/PaginatedReports";
import { formatDate } from "../../utils/helpers";

const SearchDisplay: React.FC = () => {
    const { id } = useParams();
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
        let token: string | null = userContext.accessToken;
        let config = {
            headers: { Authorization: `Bearer ${token}` },
        };

        axiosInstance
            .get<PaginatedReports>(
                `/api/place/report?PageNumber=${pageNumber}&PageSize=${pageSize}`,
                config,
            )
            .then((response: AxiosResponse<PaginatedReports>) => {
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

    const handlePaginationChange = (
        _event: React.ChangeEvent<unknown>,
        value: number,
    ) => {
        setPageNumber(value);
    };

    const updateRating = async (id: string, newValue: number | null) => {
        try {
            let token: string | null = userContext.accessToken;
            let config = {
                headers: { Authorization: `Bearer ${token}` },
            };

            let formData = {
                reportId: id,
                rate: newValue,
            };

            await axiosInstance.patch(
                `/api/place/report/rating`,
                formData,
                config,
            );
        } catch (error) {
            console.error(error);
        }
        window.location.reload();
    };

    return (
        <div className="searchHistory">
            {paginatedReports !== null ? (
                <>
                    <div className="searchHistory__reportList">
                        <List sx={{ width: "100%", maxWidth: 1400 }}>
                            {paginatedReports.items.map((report) => {
                                const labelId = `checkbox-list-label-${report.id}`;
                                return (
                                    <ListItem
                                        key={report.id}
                                        secondaryAction={
                                            <div>
                                                <Rating
                                                    onChange={(
                                                        event,
                                                        newValue,
                                                    ) =>
                                                        updateRating(
                                                            report.id,
                                                            newValue,
                                                        )
                                                    }
                                                    value={report.rating}
                                                />
                                            </div>
                                        }
                                        disablePadding
                                    >
                                        <ListItemButton role={undefined} dense>
                                            <Link
                                                to={`/searchDisplay/${report.id}`}
                                            >
                                                <ListItemText
                                                    id={labelId}
                                                    primary={`${
                                                        report.name
                                                    } - ${formatDate(
                                                        report.created,
                                                        1,
                                                    )}`}
                                                />
                                            </Link>
                                        </ListItemButton>
                                    </ListItem>
                                );
                            })}
                        </List>
                    </div>
                    <div className="searchHistory__pagination">
                        <Pagination
                            className="searchHistory__pagination-items"
                            variant="outlined"
                            count={paginatedReports.totalPages}
                            page={pageNumber}
                            onChange={handlePaginationChange}
                        />
                    </div>
                </>
            ) : (
                <p>Loading...</p>
            )}
        </div>
    );
};

export default SearchDisplay;
