import React, {useContext, useEffect} from "react";
import './navbar.css';
import {Link, useLocation} from "react-router-dom";
import {UserContext} from "../../contexts/userContext";

const Navbar : React.FC = () => {
	const userContext = useContext(UserContext);
	const currentUser : string | null | undefined = localStorage.getItem("currentUser");
	const location = useLocation();

	useEffect(() => {
		if (!currentUser) {
			return;
		}
		userContext.setUserName(currentUser);
	}, [currentUser, userContext]);

	const logoutHandler = () => {
		userContext.logout();
	};

	return (
		<div className="navbar">
			<div className="navbar__navbar-wordmark"><Link to="/">JourneyMate</Link></div>
			<div className="navbar__navbar-menu">
				<Link to="/searchPlaces">
					<button className="navbar__navbar-item">Search</button>
				</Link>
				{currentUser ? (
					<button className="navbar__logout-button" onClick={logoutHandler}>
						Log out{" "}
						{userContext.currentUser ? `${userContext.currentUser}` : ""}
					</button>
				) : location.pathname === "/" ? (
					<Link to="/login">
						<button className="navbar__navbar-item">Sign in</button>
					</Link>
				) : location.pathname === "/login" ? (
					<Link to="/register">
						<button className="navbar__navbar-item">Sign up</button>
					</Link>
				) : (
					<Link to="/login">
						<button className="navbar__navbar-item">Sign in</button>
					</Link>
				)}
			</div>
		</div>
	)
}

export default Navbar;