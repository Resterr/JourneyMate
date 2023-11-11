import React from "react";
import './searchForm.css';
import {Link} from "react-router-dom";

const SearchForm : React.FC = () => {
	return (
		<div className="home_form">
			<form>
				<div className="home_form__button">
					<Link to="Result">
						<button type="submit">Generate</button>
					</Link>
				</div>
			</form>
		</div>
	);
};

export default SearchForm;