import * as React from "react";
import { useContext, useEffect, useState } from "react";
import "./placesList.css";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import Checkbox from "@mui/material/Checkbox";
import IconButton from "@mui/material/IconButton";
import PlacesListModal from "./modal/placesListModal";
import { Place } from "../../models/Place";
import axiosInstance from "../../utils/axiosInstance";
import { UserContext } from "../../contexts/userContext";
import PhotoModal from "./modal/photoModal";
import { SubmitHandler, useForm } from "react-hook-form";
import { AxiosResponse } from "axios";

type PlacesListProps = {
  places: Place[];
};

type FormData = {
  name: string;
  placesId: string[];
};

const PlacesList: React.FC<PlacesListProps> = (props) => {
  const places = props.places;
  const [checked, setChecked] = React.useState<string[]>([]);
  const userContext = useContext(UserContext);
  const [maxHeight] = useState<number>(500);
  const [maxWidth] = useState<number>(500);
  const [loading, setLoading] = useState<boolean>(true);
  const {
    register,
    handleSubmit,
    formState: { errors },
    setValue,
  } = useForm<FormData>();

  useEffect(() => {
    setLoading(true);
    places.forEach((place) => {
      let token: string | null = userContext.accessToken;
      let config = {
        headers: { Authorization: `Bearer ${token}` },
        responseType: "blob" as "blob",
      };
      axiosInstance
        .get(
          `/api/place/photo/${place.id}?Height=${maxHeight}&Width=${maxWidth}`,
          config,
        )
        .then((response) => {
          if (response.status === 200) {
            place.photo = URL.createObjectURL(response.data);
          }
        })
        .catch(() => {
          console.log(`Image not found for place ID: ${place.id}`);
        });
    });
    setLoading(false);
  }, [places, maxHeight, maxWidth, userContext.accessToken]);

  const handleToggle = (value: string) => () => {
    const currentIndex = checked.indexOf(value);
    const newChecked = [...checked];

    if (currentIndex === -1) {
      newChecked.push(value);
    } else {
      newChecked.splice(currentIndex, 1);
    }
    setChecked(newChecked);
    setValue("placesId", newChecked);
  };

  const onSubmit: SubmitHandler<FormData> = async (data: FormData) => {
    try {
      console.log("SEND");
      let token: string | null = userContext.accessToken;
      let config = {
        headers: { Authorization: `Bearer ${token}` },
      };

      const response: AxiosResponse<any> = await axiosInstance.put(
        "/api/plan",
        data,
        config,
      );

      if (response.status === 201) {
        console.log(response.data);
        //navigate(`/searchDisplay/${response.data}`);
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="searchPlace__placesList">
      {loading ? (
        <p>Loading...</p>
      ) : (
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="searchPlace_form__name">
            <label htmlFor="fname">Name:</label>
            <br />
            <input
              id="form-plan"
              {...register("name", {
                required: "This is required",
                minLength: 3,
              })}
            />
            <br />
            <p>{errors.name?.message}</p>
          </div>
          <div className="searchPlace_form__button">
            <button type="submit">Create</button>
          </div>
          <List sx={{ width: "100%", maxWidth: 1300 }}>
            {places.map((place) => {
              const labelId = `checkbox-list-label-${place.id}`;
              return (
                <ListItem
                  key={place.id}
                  secondaryAction={
                    <IconButton edge="end" aria-label="comments">
                      <PhotoModal placeDetails={place}></PhotoModal>
                      <PlacesListModal placeDetails={place}></PlacesListModal>
                    </IconButton>
                  }
                  disablePadding
                >
                  <ListItemButton
                    role={undefined}
                    onClick={handleToggle(place.id)}
                    dense
                  >
                    <ListItemIcon>
                      <Checkbox
                        edge="start"
                        checked={checked.indexOf(place.id) !== -1}
                        tabIndex={-1}
                        disableRipple
                        inputProps={{ "aria-labelledby": labelId }}
                      />
                    </ListItemIcon>
                    <ListItemText id={labelId} primary={`${place.name}`} />
                  </ListItemButton>
                </ListItem>
              );
            })}
          </List>
        </form>
      )}
    </div>
  );
};

export default PlacesList;
