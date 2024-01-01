import React, { useContext, useEffect } from "react";
import "./searchPlaces.css";
import { useNavigate } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";
import SearchTypes from "../../components/searchTypes/searchTypes";
import SearchAddress from "../../components/searchAddress/searchAddress";
import { SubmitHandler, useForm } from "react-hook-form";
import { AxiosResponse } from "axios";
import axiosInstance from "../../utils/axiosInstance";
import { Slider } from "@mui/material";

type FormData = {
  addressId: string;
  types: string[];
  distance: number;
};

const SearchPlaces: React.FC = () => {
  const userContext = useContext(UserContext);
  const currentUser = userContext.currentUser;
  const navigate = useNavigate();
  const [distance, setDistance] = React.useState<number>(10);
  const { handleSubmit, setValue } = useForm<FormData>();

  useEffect(() => {
    if (!currentUser) {
      navigate("/");
    }
  }, [currentUser, navigate]);

  const onSubmit: SubmitHandler<FormData> = async (data: FormData) => {
    try {
      let token: string | null = userContext.accessToken;
      let config = {
        headers: { Authorization: `Bearer ${token}` },
      };
      setValue("distance", distance);

      const response: AxiosResponse<any> = await axiosInstance.post(
        "/api/place/report/generate",
        data,
        config,
      );

      if (response.status === 201) {
        navigate(`/searchDisplay/${response.data}`);
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleSelectedAddressChange = (newSelectedAddress: string) => {
    setValue("addressId", newSelectedAddress);
  };

  const handleSelectedTypesChange = (newSelectedTypes: string[]) => {
    setValue("types", newSelectedTypes);
  };

  const handleDistanceChange = (event: Event, newValue: number | number[]) => {
    setDistance(newValue as number);
    setValue("distance", newValue as number);
  };

  return (
    <div className="searchPlaces">
      <form onSubmit={handleSubmit(onSubmit)}>
        <SearchAddress onSelectedAddressChange={handleSelectedAddressChange} />
        <SearchTypes onSelectedTypesChange={handleSelectedTypesChange} />
        <p className="searchPlaces__text">Distance from address</p>
        <Slider
          step={1}
          min={1}
          max={20}
          valueLabelDisplay="auto"
          aria-label="Default"
          value={distance}
          onChange={handleDistanceChange}
        />

        <div className="searchPlaces-button">
          <button type="submit">Generate</button>
        </div>
      </form>
    </div>
  );
};

export default SearchPlaces;
