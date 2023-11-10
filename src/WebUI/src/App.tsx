import React, {useContext, useEffect, useState} from 'react';
import {BrowserRouter, Route, Routes} from 'react-router-dom';
import './App.css';
import Home from "./pages/home/home";
import Navbar from "./components/navbar/navbar";
import Login from "./pages/login/login";
import Register from "./pages/register/register";
import Result from "./pages/result/result";
import {AxiosResponse} from 'axios';
import axiosInstance from './utils/axiosInstance';
import {UserContext} from './contexts/userContext';

function App() {
	const userContext = useContext(UserContext);

	useEffect(() => {
		userContext.setTokens(localStorage.getItem("accessToken"), localStorage.getItem("refreshToken"),
			localStorage.getItem("refreshToken"))
		userContext.setUserName(localStorage.getItem("userName"));
	}, [userContext]);

	let currentUser = userContext.currentUser;
	const [, setIntervalToken] = useState<number | null | NodeJS.Timer>(null);

	useEffect(() => {
		const refreshTokens = async () => {
			if (currentUser !== undefined && currentUser !== null && currentUser !== "") {
				const tokenDateStr = userContext.refreshTokenExpireDate;
				const tokenDate = parseInt(tokenDateStr as string, 10);
				const TWENTY_THREE_HOURS = 23 * 60 * 60 * 1000; // 23 hours in milliseconds

				if (Date.now() - tokenDate > TWENTY_THREE_HOURS) {
					userContext.logout();
				}

				let data = {
					accessToken: userContext.accessToken,
					refreshToken: userContext.refreshToken,
				};

				let requestConfig : {
					headers : { Authorization : string };
				} = {
					headers: {Authorization: `Bearer ${userContext.accessToken}`},
				};

				try {
					console.log(data);
					await axiosInstance
						.post("/api/users/token/refresh", data, requestConfig)
						.then((response : AxiosResponse<any, any>) => {
							if (response.status === 200) {
								console.log(response);

								userContext.setTokens(response.data.accessToken, response.data.refreshToken, Date.now().toString());
							} else {
								console.log("Bad token data");
							}
						});
				} catch (error) {
					console.log(error);
				}
			}
		};

		const intervalId = setInterval(refreshTokens, 60000);
		setIntervalToken(intervalId);
		return () => clearInterval(intervalId);
	}, [currentUser, userContext]);

	return (
		<BrowserRouter>
			<Navbar></Navbar>
			<Routes>
				<Route index element={<Home/>}/>
				<Route path="login" element={<Login/>}/>
				<Route path="register" element={<Register/>}/>
				<Route path="result" element={<Result/>}/>
			</Routes>
		</BrowserRouter>
	);
}

export default App;
