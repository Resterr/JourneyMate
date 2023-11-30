import React, {useContext, useEffect, useState} from 'react';
import {BrowserRouter, Navigate, Route, Routes} from 'react-router-dom';
import './App.css';
import Home from "./pages/home/home";
import Navbar from "./components/navbar/navbar";
import Login from "./pages/login/login";
import Register from "./pages/register/register";
import SearchPlaces from "./pages/searchPlaces/searchPlaces";
import {AxiosResponse} from 'axios';
import axiosInstance from './utils/axiosInstance';
import {UserContext} from './contexts/userContext';
import SearchDisplay from './pages/searchDisplay/searchDisplay';

function App() {
	const userContext = useContext(UserContext);

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

				try {
					await axiosInstance
						.post("/api/users/token/refresh", data)
						.then((response : AxiosResponse<any, any>) => {
							if (response.status === 200) {
								console.log("Refreshed");

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
				<Route path="searchPlaces" element={<SearchPlaces/>}/>
				<Route path="searchDisplay/:id" element={<SearchDisplay/>} />
				<Route path="*" element={<Navigate to="/" />} />
			</Routes>
		</BrowserRouter>
	);
}

export default App;
