import React, { useContext, useEffect, useState } from "react";
import "./planDisplay.css";
import PlacesList from "../../components/placesList/placesList";
import axiosInstance from "../../utils/axiosInstance";
import { AxiosResponse } from "axios";
import { useNavigate, useParams } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";
import { Pagination } from "@mui/material";
import { PaginatedPlaces } from "../../models/PaginatedPlaces";
import SearchTypes from "../../components/searchTypes/searchTypes";

const PlanDisplay: React.FC = () => {
  const { id } = useParams();
  const userContext = useContext(UserContext);
  const currentUser = userContext.currentUser;
  const navigate = useNavigate();
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [pageSize] = useState<number>(10);
  const [paginatedPlaces, setPaginatedPlaces] =
    useState<PaginatedPlaces | null>(null);
  const [tagsString, setTagsString] = useState<string | null>(null);

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
      .get<PaginatedPlaces>(
        `/api/plan/${id}/places?PageNumber=${pageNumber}&PageSize=${pageSize}${
          tagsString ? `&TagsString=${tagsString}` : ""
        }`,
        config,
      )
      .then((response: AxiosResponse<PaginatedPlaces>) => {
        if (response.status === 200) {
          setPaginatedPlaces(response.data);
          console.log(response.data);
        } else {
          navigate(`/searchPlaces`);
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }, [id, navigate, pageNumber, pageSize, userContext.accessToken, tagsString]);

  const handlePaginationChange = (
    _event: React.ChangeEvent<unknown>,
    value: number,
  ) => {
    setPageNumber(value);
  };

  const handleSelectedTypesChange = (newSelectedTypes: string[]) => {
    setTagsString(newSelectedTypes.join("|"));
  };

  return (
    <div className="planDisplay">
      {paginatedPlaces !== null ? (
        <>
          <SearchTypes onSelectedTypesChange={handleSelectedTypesChange} />
          <PlacesList places={paginatedPlaces!.items} />
          <div className="planDisplay__pagination">
            <Pagination
              className="planDisplay__pagination-items"
              variant="outlined"
              count={paginatedPlaces.totalPages}
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

export default PlanDisplay;
