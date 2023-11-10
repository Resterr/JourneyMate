import axios from "axios";

const axiosInstance = axios.create({
	baseURL: "https://journeymate.azurewebsites.net/",
});

export default axiosInstance;
