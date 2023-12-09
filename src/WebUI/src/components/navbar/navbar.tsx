import React, {useContext, useEffect, useState} from "react";
import './navbar.css';
import {Link, useLocation} from "react-router-dom";
import {UserContext} from "../../contexts/userContext";

const Navbar : React.FC = () => {
	const userContext = useContext(UserContext);
	const currentUser = userContext.currentUser;
	const location = useLocation();
	const [userOptionsVisible, SetUserOptionsVisible] = useState<boolean>(false);

	useEffect(() => {
		if (!currentUser) {
			return;
		}
		SetUserOptionsVisible(true);
	}, [currentUser, userContext]);

	const logoutHandler = () => {
		SetUserOptionsVisible(false);
		userContext.logout();
	};

	return (
		<div className="navbar">
			<div className="navbar__navbar-wordmark"><Link to="/">JourneyMate</Link></div>
			<div className="navbar__navbar-menu">
				{userOptionsVisible ? (
					<><Link to="/searchPlaces">
						<button className="navbar__navbar-item">Search</button>
					</Link><Link to="/searchHistory">
						<button className="navbar__navbar-item">History</button>
					</Link></>
						) : null}
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