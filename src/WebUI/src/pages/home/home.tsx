import React from "react";
import './home.css';
import SearchForm from "./form/searchForm";

const Home : React.FC = () => {
	return (
		<div className="home">
			<SearchForm></SearchForm>
		</div>
	)
}

export default Home;