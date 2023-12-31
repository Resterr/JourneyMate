import React, { useContext, useEffect, useState } from "react";
import "./schedulesList.css";
import axiosInstance from "../../utils/axiosInstance";
import { AxiosResponse } from "axios";
import { Link, useNavigate } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";
import { Pagination } from "@mui/material";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import { PaginatedSchedules } from "../../models/PaginatedSchedules";
import PlanNames from "../../components/plansNames/planNames";
import { DateTimePicker, LocalizationProvider } from "@mui/x-date-pickers";
import dayjs, { Dayjs } from "dayjs";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import IconButton from "@mui/material/IconButton";
import ScheduleDetailModal from "./modal/scheduleDetailModal";

const SchedulesList: React.FC = () => {
  const userContext = useContext(UserContext);
  const currentUser = userContext.currentUser;
  const navigate = useNavigate();
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [pageSize] = useState<number>(10);
  const [selectedPlan, setSelectedPlan] = useState<string | null>(null);
  const [selectedDate, setSelectedDate] = useState<Dayjs | null>(
    dayjs(new Date()),
  );
  const [paginatedSchedules, setPaginatedSchedules] =
    useState<PaginatedSchedules | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [status, setStatus] = useState<string | null>(null);

  useEffect(() => {
    if (!currentUser) {
      navigate("/");
    }
  }, [currentUser, navigate]);

  useEffect(() => {
    if (selectedPlan != null && selectedDate != null) {
      loadSchedules(selectedPlan, selectedDate);
    }
  }, [
    navigate,
    pageNumber,
    pageSize,
    userContext.accessToken,
    selectedPlan,
    selectedDate,
  ]);

  const loadSchedules = (planId: string, date: Dayjs) => {
    let token: string | null = userContext.accessToken;
    let config = {
      headers: { Authorization: `Bearer ${token}` },
    };

    let data = {
      planId: planId,
      date: date,
      pageNumber: pageNumber,
      pageSize: pageSize,
    };

    axiosInstance
      .post<PaginatedSchedules>(`/api/schedule/plan`, data, config)
      .then((response: AxiosResponse<PaginatedSchedules>) => {
        if (response.status === 200) {
          console.log(response.data);
          setPaginatedSchedules(response.data);
          setLoading(false);
        } else {
          navigate(`/searchPlaces`);
        }
      })
      .catch((error) => {
        console.log(error);
      });
  };

  const onSelectedPlanNameChange = (selectedPlanName: string) => {
    setSelectedPlan(selectedPlanName);
  };

  const handlePaginationChange = (
    _event: React.ChangeEvent<unknown>,
    value: number,
  ) => {
    setPageNumber(value);
  };

  return (
    <div className="schedulesList">
      <div className="schedulesList__search">
        <PlanNames onSelectedPlanNameChange={onSelectedPlanNameChange} />
        <div className="schedulesList__calendar">
          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DateTimePicker
              value={selectedDate}
              onChange={(newValue) => setSelectedDate(newValue)}
              ampm={false}
            />
          </LocalizationProvider>
        </div>
        {selectedPlan ? (
          <Link to={`/schedule/${selectedPlan}`}>
            <button className="schedulesList__search-add-button">
              Add schedule
            </button>
          </Link>
        ) : null}
      </div>
      {loading || paginatedSchedules == null ? (
        <p></p>
      ) : (
        <>
          <div className="schedulesList__content">
            <div className="schedulesList__status">
              <h1>{status}</h1>
            </div>
            <List sx={{ width: "100%", maxWidth: 1400 }}>
              {paginatedSchedules!.items.map((schedule) => {
                const labelId = `checkbox-list-label-${schedule.id}`;
                return (
                  <ListItem
                    key={schedule.id}
                    secondaryAction={
                      <IconButton edge="end" aria-label="comments">
                        <ScheduleDetailModal
                          scheduleDetail={schedule}
                        ></ScheduleDetailModal>
                      </IconButton>
                    }
                    disablePadding
                  >
                    <ListItemButton role={undefined} dense>
                      <ListItemText
                        id={labelId}
                        primary={`${schedule.placeName}`}
                      />
                    </ListItemButton>
                  </ListItem>
                );
              })}
            </List>
          </div>
          <div className="schedulesList__pagination">
            <Pagination
              className="schedulesList__pagination-items"
              variant="outlined"
              count={paginatedSchedules!.totalPages}
              page={pageNumber}
              onChange={handlePaginationChange}
            />
          </div>
        </>
      )}
    </div>
  );
};

export default SchedulesList;
