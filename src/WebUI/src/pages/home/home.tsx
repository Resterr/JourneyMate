import React from "react";
import './home.css';
import SearchForm from "./form/searchForm";

export const Home : React.FC = () => {
	return (
		<div className="home__container">
			<SearchForm></SearchForm>
		</div>
	)
}

export default Home;