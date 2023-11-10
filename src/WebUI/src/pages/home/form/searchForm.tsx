import React from "react";
import './searchForm.css';
import {Link} from "react-router-dom";

export const SearchForm : React.FC = () => {
	return (
		<div className="home__form">
			<form>
				<div className="home__form--button">
					<Link to="Result">
						<button type="submit">Generate</button>
					</Link>
				</div>
			</form>
		</div>
	);
};

export default SearchForm;