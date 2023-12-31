import React, { useContext, useEffect, useState } from "react";
import "./placeNames.css";
import { UserContext } from "../../contexts/userContext";
import axiosInstance from "../../utils/axiosInstance";
import { AxiosResponse } from "axios";
import {
  FormControl,
  InputLabel,
  ListItemText,
  MenuItem,
  OutlinedInput,
  Select,
  SelectChangeEvent,
} from "@mui/material";
import { PlaceName } from "../../models/PlaceName";

const ITEM_HEIGHT = 80;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
  PaperProps: {
    style: {
      maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
      width: 250,
    },
  },
};

type PlaceNamesProps = {
  planId: string;
  onSelectedPlaceNameChange: (placeId: string) => void;
};

const PlaceNames: React.FC<PlaceNamesProps> = (props) => {
  const userContext = useContext(UserContext);
  const [placeNames, setPlaceNames] = useState<PlaceName[]>([]);
  const [selectedPlaceName, setSelectedPlaceName] = useState<string>("");

  useEffect(() => {
    let token: string | null = userContext.accessToken;
    let config = {
      headers: { Authorization: `Bearer ${token}` },
    };

    axiosInstance
      .get<PlaceName[]>(`/api/plan/${props.planId}/names`, config)
      .then((response: AxiosResponse<PlaceName[]>) => {
        if (response.data.length !== 0) {
          setPlaceNames(response.data);
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  const handleChange = (event: SelectChangeEvent) => {
    console.log(event.target.value as string);
    setSelectedPlaceName(event.target.value as string);
    props.onSelectedPlaceNameChange(event.target.value as string);
  };

  return (
    <div className="placeNames">
      <FormControl className="placeNames__form">
        <InputLabel
          sx={{ color: "#00AAFF" }}
          className="placeNames__form-label"
        >
          Place
        </InputLabel>
        <Select
          sx={{ color: "#00AAFF" }}
          className="placeNames__form-select"
          id="placeNames__form-select"
          value={selectedPlaceName}
          onChange={handleChange}
          input={<OutlinedInput label="Place" />}
          MenuProps={MenuProps}
        >
          {placeNames.map((x) => (
            <MenuItem key={x.id} value={x.id}>
              <ListItemText primary={x.name} />
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </div>
  );
};

export default PlaceNames;
