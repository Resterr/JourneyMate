import React, { useContext, useEffect, useState } from "react";
import "./planList.css";
import axiosInstance from "../../utils/axiosInstance";
import { AxiosResponse } from "axios";
import { Link, useNavigate, useParams } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";
import { Button, Pagination, Typography } from "@mui/material";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import { PaginatedPlans } from "../../models/PaginatedPlans";
import IconButton from "@mui/material/IconButton";
import ShareModal from "./modals/shareModal";

const PlanList: React.FC = () => {
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
  const [loading, setLoading] = useState<boolean>(true);
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
        `/api/plan?PageNumber=${pageNumber}&PageSize=${pageSize}`,
        config,
      )
      .then((response: AxiosResponse<PaginatedPlans>) => {
        if (response.status === 200) {
          setPaginatedPlans(response.data);
          setLoading(false);
        } else {
          navigate(`/searchPlaces`);
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }, [
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

  const deletePlan = async (id: string) => {
    try {
      let token: string | null = userContext.accessToken;
      let config = {
        headers: { Authorization: `Bearer ${token}` },
        data: {
          id: id,
        },
      };

      await axiosInstance.delete("/api/plan", config);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="planList">
      {loading || paginatedPlans == null ? (
        <p>Loading...</p>
      ) : (
        <>
          <div className="planList__content">
            <div className="planList_share-status">
              <h1>{status}</h1>
            </div>
            <List sx={{ width: "100%", maxWidth: 1300 }}>
              {paginatedPlans!.items.map((plan) => {
                const labelId = `checkbox-list-label-${plan.id}`;
                return (
                  <ListItem
                    key={plan.id}
                    secondaryAction={
                      <div>
                        <IconButton edge="end" aria-label="comments">
                          <ShareModal
                            setStatus={setStatus}
                            planId={plan.id}
                          ></ShareModal>
                        </IconButton>
                        <Button
                          variant="contained"
                          color="error"
                          onClick={() => deletePlan(plan.id)}
                        >
                          DELETE
                        </Button>
                      </div>
                    }
                    disablePadding
                  >
                    <ListItemButton
                      component={Link}
                      to={`/planDisplay/${plan.id}`}
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
              count={paginatedPlans!.totalPages}
              page={pageNumber}
              onChange={handlePaginationChange}
            />
          </div>
        </>
      )}
    </div>
  );
};

export default PlanList;
