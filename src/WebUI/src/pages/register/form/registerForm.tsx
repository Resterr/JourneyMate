import React, { useContext, useEffect, useState } from "react";
import "./registerForm.css";
import { SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { UserContext } from "../../../contexts/userContext";
import axiosInstance from "../../../utils/axiosInstance";

type FormData = {
  email: string;
  userName: string;
  password: string;
  confirmPassword: string;
};

const RegisterForm: React.FC = () => {
  const userContext = useContext(UserContext);
  const currentUser = userContext.currentUser;
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>();
  const [status, setStatus] = useState<string | undefined>();
  const navigate = useNavigate();

  useEffect(() => {
    if (currentUser) {
      navigate("/");
    }
  }, [currentUser, navigate]);

  const onSubmit: SubmitHandler<FormData> = async (data) => {
    await axiosInstance
      .post("/api/users/register", data)
      .then((response) => {
        if (response.status === 200) {
          setStatus("Successfully registered");
          const formId = document.getElementById("form-id") as HTMLInputElement;
          const formLogin = document.getElementById(
            "form-login",
          ) as HTMLInputElement;
          const formPassword = document.getElementById(
            "form-password",
          ) as HTMLInputElement;
          const formPasswordConfirm = document.getElementById(
            "form-password-confirm",
          ) as HTMLInputElement;

          formId.value = "";
          formLogin.value = "";
          formPassword.value = "";
          formPasswordConfirm.value = "";
        } else {
          setStatus("Failed to register");
        }
      })
      .catch((error: any) => {
        console.log(error);
        setStatus("Failed to register");
      });
  };

  return (
    <div className="register_form">
      <div className="register_form__status">
        <h1>{status}</h1>
      </div>
      <form onSubmit={handleSubmit(onSubmit)} id="my-form">
        <div className="register_form__email">
          <label>Email:</label>
          <br />
          <input
            id="form-id"
            {...register("email", {
              required: "This is required",
              minLength: 6,
            })}
          />
          <br />
          <p>{errors.email?.message}</p>
        </div>
        <div className="register_form__username">
          <label>Username:</label>
          <br />
          <input
            id="form-login"
            {...register("userName", {
              required: "This is required",
              minLength: 6,
            })}
          />
          <br />
          <p>{errors.userName?.message}</p>
        </div>
        <div className="register_form__password">
          <label>Password:</label>
          <br />
          <input
            id="form-password"
            type="password"
            {...register("password", {
              required: "This is required",
              minLength: 6,
            })}
          />
          <br />
          <p>{errors.password?.message}</p>
        </div>
        <div className="register_form__password-confirm">
          <label>Confirm password:</label>
          <br />
          <input
            id="form-password-confirm"
            type="password"
            {...register("confirmPassword", {
              required: "This is required",
              minLength: 6,
            })}
          />
          <br />
          <p>{errors.confirmPassword?.message}</p>
        </div>
        <div className="register_form__button">
          <button type="submit">Register</button>
        </div>
      </form>
    </div>
  );
};

export default RegisterForm;
