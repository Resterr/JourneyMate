import React from "react";
import './searchForm.css';

const SearchForm : React.FC = () => {
	return (
		<div className="searchPlaces_form">
			<form>
				<div className="searchPlaces_form__button">
					    <button type="submit">Generate</button>
				</div>
			</form>
		</div>
	);
};

export default SearchForm;