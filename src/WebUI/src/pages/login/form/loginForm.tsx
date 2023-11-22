import React, {useContext, useEffect, useState} from "react";
import "./loginForm.css";
import {SubmitHandler, useForm} from "react-hook-form";
import {AxiosResponse} from "axios";
import {UserContext} from "../../../contexts/userContext";
import {useNavigate} from "react-router-dom";
import axiosInstance from "../../../utils/axiosInstance";

type FormData = {
	userName : string;
	password : string;
};

const LoginForm : React.FC = () => {
	const userContext = useContext(UserContext);
	const {
		register,
		handleSubmit,
		formState: {errors},
	} = useForm<FormData>();
	const [status, setStatus] = useState<string | null>(null);
	let navigate = useNavigate();
	const currentUser : string | null | undefined = userContext.currentUser;

	useEffect(() => {
		if (currentUser) {
			navigate("/");
		}
	}, [currentUser, navigate]);

	const onSubmit : SubmitHandler<FormData> = async (data : FormData) => {
		try {
			const response : AxiosResponse<any> = await axiosInstance.post(
				"/api/users/login",
				data,
			);

			if (response.status === 200) {
				userContext.setTokens(response.data.accessToken, response.data.refreshToken, Date.now().toString())
				userContext.setUserName(data.userName);

				setStatus(`Welcome ${data.userName}!`);

				navigate("/");
			} else {
				setStatus("Failed to login");
			}
		} catch (error) {
			console.error(error);
			setStatus("Failed to login");
		}
	};

	return (
		<div className="login_form">
			<div className="login_form__status">
				<h1>{status}</h1>
			</div>
			<form onSubmit={handleSubmit(onSubmit)}>
				<div className="login_form__login">
					<label htmlFor="fname">Username:</label>
					<br/>
					<input
						id="form-login"
						{...register("userName", {
							required: "This is required",
							minLength: 6,
						})}
					/>
					<br/>
					<p>{errors.userName?.message}</p>
				</div>
				<div className="login_form__password">
					<label htmlFor="lname">Password:</label>
					<br/>
					<input
						id="form-password"
						type="password"
						{...register("password", {
							required: "This is required",
							minLength: 6,
						})}
					/>
					<br/>
					<p>{errors.userName?.message}</p>
				</div>
				<div className="login_form__button">
					<button type="submit">Sign in</button>
				</div>
			</form>
		</div>
	);
};

export default LoginForm;