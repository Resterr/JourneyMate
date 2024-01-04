import React, { useContext, useEffect, useState } from "react";
import "./followUser.css";
import { useNavigate } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";
import { SubmitHandler, useForm } from "react-hook-form";
import axios, { AxiosResponse } from "axios";
import axiosInstance from "../../utils/axiosInstance";
import { PaginatedFollow } from "../../models/PaginatedFollow";
import ListItem from "@mui/material/ListItem";
import { Button, Pagination } from "@mui/material";
import ListItemText from "@mui/material/ListItemText";
import List from "@mui/material/List";

type FormData = {
    userName: string;
};

const FollowUser: React.FC = () => {
    const userContext = useContext(UserContext);
    const currentUser = userContext.currentUser;
    const navigate = useNavigate();
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<FormData>();
    const [status, setStatus] = useState<string | null>(null);
    const [pageNumberFollowed, setPageNumberFollowed] = useState<number>(1);
    const [pageNumberFollowers, setPageNumberFollowers] = useState<number>(1);
    const [pageSize] = useState<number>(5);
    const [paginatedFollowed, setPaginatedFollowed] =
        useState<PaginatedFollow | null>(null);
    const [paginatedFollowers, setPaginatedFollowers] =
        useState<PaginatedFollow | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

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
            .get<PaginatedFollow>(
                `/api/users/followers?PageNumber=${pageNumberFollowers}&PageSize=${pageSize}`,
                config,
            )
            .then((response: AxiosResponse<PaginatedFollow>) => {
                if (response.status === 200) {
                    setPaginatedFollowers(response.data);
                }
            })
            .catch((error) => {
                console.log(error);
            });

        axiosInstance
            .get<PaginatedFollow>(
                `/api/users/followed?PageNumber=${pageNumberFollowed}&PageSize=${pageSize}`,
                config,
            )
            .then((response: AxiosResponse<PaginatedFollow>) => {
                if (response.status === 200) {
                    setPaginatedFollowed(response.data);
                    setLoading(false);
                }
            })
            .catch((error) => {
                console.log(error);
            });
    }, [
        pageNumberFollowed,
        pageNumberFollowers,
        pageSize,
        userContext.accessToken,
    ]);

    const onSubmit: SubmitHandler<FormData> = async (data: FormData) => {
        try {
            let token: string | null = userContext.accessToken;
            let config = {
                headers: { Authorization: `Bearer ${token}` },
            };
            await axiosInstance.post("/api/users/follow", data, config);
            setStatus("Successfully followed");
            window.location.reload();
        } catch (error) {
            if (axios.isAxiosError(error)) {
                if (error.response) {
                    setStatus(error.response.data.Detail);
                }
            } else {
                setStatus("Failed to follow");
            }
        }
    };

    const handleFollowersPaginationChange = (
        _event: React.ChangeEvent<unknown>,
        value: number,
    ) => {
        setPageNumberFollowers(value);
    };

    const handleFollowedPaginationChange = (
        _event: React.ChangeEvent<unknown>,
        value: number,
    ) => {
        setPageNumberFollowed(value);
    };

    const deleteFollowed = async (name: string) => {
        try {
            let token: string | null = userContext.accessToken;
            let config = {
                headers: { Authorization: `Bearer ${token}` },
                data: {
                    userName: name,
                },
            };

            await axiosInstance.delete("/api/users/unfollow", config);
        } catch (error) {
            console.error(error);
        }

        window.location.reload();
    };

    return (
        <div className="followUser">
            <form
                onSubmit={handleSubmit(onSubmit)}
                className="followUser__form"
            >
                <div className="followUser__status">
                    <h1>{status}</h1>
                </div>
                <div className="followUser__form-name">
                    <label htmlFor="fname">Nazwa:</label>
                    <br />
                    <input
                        id="form-plan"
                        {...register("userName", {
                            required: "This is required",
                            minLength: 3,
                        })}
                    />
                    <br />
                    <p>{errors.userName?.message}</p>
                </div>
                <div className="followUser__form-button">
                    <button type="submit">Obserwuj</button>
                </div>
            </form>
            <br />
            {loading ? (
                <p>Loading...</p>
            ) : (
                <div className="followUser__content">
                    <div className="followUser__content-list">
                        <p>Followers</p>
                        <List sx={{ width: "100%", maxWidth: 1300 }}>
                            {paginatedFollowers!.items.map((follow) => {
                                const labelId = `checkbox-list-label-${follow.userName}`;
                                return (
                                    <ListItem
                                        key={follow.userName}
                                        disablePadding
                                    >
                                        <ListItemText
                                            id={labelId}
                                            primary={`${follow.userName}`}
                                        />
                                    </ListItem>
                                );
                            })}
                        </List>
                        <div className="followUser__content-list-pagination">
                            <Pagination
                                className="followUser__content-list-pagination-item"
                                variant="outlined"
                                count={paginatedFollowers!.totalPages}
                                page={pageNumberFollowers}
                                onChange={handleFollowersPaginationChange}
                            />
                        </div>
                    </div>
                    <div className="followUser__content-list">
                        <p>Followed</p>
                        <List sx={{ width: "100%", maxWidth: 1300 }}>
                            {paginatedFollowed!.items.map((follow) => {
                                const labelId = `checkbox-list-label-${follow.userName}`;
                                return (
                                    <ListItem
                                        key={follow.userName}
                                        secondaryAction={
                                            <div>
                                                <Button
                                                    variant="contained"
                                                    color="error"
                                                    onClick={() =>
                                                        deleteFollowed(
                                                            follow.userName,
                                                        )
                                                    }
                                                >
                                                    USUŃ
                                                </Button>
                                            </div>
                                        }
                                        disablePadding
                                    >
                                        <ListItemText
                                            id={labelId}
                                            primary={`${follow.userName}`}
                                        />
                                    </ListItem>
                                );
                            })}
                        </List>
                        <div className="followUser__content-list-pagination">
                            <Pagination
                                className="followUser__content-list-pagination-item"
                                variant="outlined"
                                count={paginatedFollowed!.totalPages}
                                page={pageNumberFollowed}
                                onChange={handleFollowedPaginationChange}
                            />
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default FollowUser;
