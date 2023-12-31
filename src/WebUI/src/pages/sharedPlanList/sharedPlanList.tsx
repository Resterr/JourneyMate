import React, { useContext, useEffect, useState } from "react";
import "./sharedPlanList.css";
import axiosInstance from "../../utils/axiosInstance";
import { AxiosResponse } from "axios";
import { Link, useNavigate, useParams } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";
import { Pagination } from "@mui/material";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import { PaginatedPlans } from "../../models/PaginatedPlans";

const SharedPlanList: React.FC = () => {
  const { id } = useParams();
  const userContext = useContext(UserContext);
  const currentUser = userContext.currentUser;
  const navigate = useNavigate();
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [pageSize] = useState<number>(10);
  const [paginatedPlans, setPaginatedPlans] = useState<PaginatedPlans | null>(
    null,
  );
  const [maxHeight] = useState<number>(500);
  const [maxWidth] = useState<number>(500);
  const [status, setStatus] = useState<string | null>(null);

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
      .get<PaginatedPlans>(
        `/api/plan/shared?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        config,
      )
      .then((response: AxiosResponse<PaginatedPlans>) => {
        if (response.status === 200) {
          setPaginatedPlans(response.data);
        } else {
          navigate(`/searchPlaces`);
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }, [
    id,
    navigate,
    pageNumber,
    pageSize,
    userContext.accessToken,
    maxHeight,
    maxWidth,
  ]);

  const handlePaginationChange = (
    _event: React.ChangeEvent<unknown>,
    value: number,
  ) => {
    setPageNumber(value);
  };

  return (
    <div className="planList">
      <div className="planList_share-status">
        <h1>{status}</h1>
      </div>
      {paginatedPlans !== null ? (
        <>
          <div className="planList__content">
            <List sx={{ width: "100%", maxWidth: 1400 }}>
              {paginatedPlans.items.map((plan) => {
                const labelId = `checkbox-list-label-${plan.id}`;
                return (
                  <ListItem key={plan.id} disablePadding>
                    <ListItemButton
                      component={Link}
                      to={`/sharedPlanDisplay/${plan.id}`}
                      role={undefined}
                      dense
                    >
                      <ListItemText
                        id={labelId}
                        primary={`${plan.name} - ${plan.created}`}
                      />
                    </ListItemButton>
                  </ListItem>
                );
              })}
            </List>
          </div>
          <div className="planList__pagination">
            <Pagination
              className="planList__pagination-items"
              variant="outlined"
              count={paginatedPlans.totalPages}
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

export default SharedPlanList;
