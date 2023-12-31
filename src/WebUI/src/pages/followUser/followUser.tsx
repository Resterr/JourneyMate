import React, { useContext, useEffect, useState } from "react";
import "./followUser.css";
import { useNavigate } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";
import { SubmitHandler, useForm } from "react-hook-form";
import axios from "axios";
import axiosInstance from "../../utils/axiosInstance";

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
      await axiosInstance.post("/api/users/follow", data, config);
      setStatus("Successfully followed");
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

  return (
    <div className="followUser">
      <form onSubmit={handleSubmit(onSubmit)} className="followUser__form">
        <div className="followUser__status">
          <h1>{status}</h1>
        </div>
        <div className="followUser__form-name">
          <label htmlFor="fname">Name:</label>
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
          <button type="submit">Follow</button>
        </div>
      </form>
    </div>
  );
};

export default FollowUser;
