import React, {useEffect} from "react";
import './searchPlaces.css';
import SearchForm from "./form/searchForm";
import {useNavigate} from "react-router-dom";

export const SearchPlaces : React.FC = () => {
	let navigate = useNavigate();
	const currentUser : string | null | undefined = localStorage.getItem("currentUser");
    
    useEffect(() => {
        if (!currentUser) {
            navigate("/");
        }
    }, [currentUser, navigate]);
    
	return (
		<div className="searchPlaces">
            <SearchForm></SearchForm>
		</div>
	)
}

export default SearchPlaces;