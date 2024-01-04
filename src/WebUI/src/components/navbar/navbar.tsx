import React, { useContext, useEffect, useState } from "react";
import "./navbar.css";
import { Link, useLocation } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";

const Navbar: React.FC = () => {
    const userContext = useContext(UserContext);
    const currentUser = userContext.currentUser;
    const location = useLocation();
    const [userOptionsVisible, SetUserOptionsVisible] =
        useState<boolean>(false);

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
            <div className="navbar__navbar-wordmark">
                <Link to="/">JourneyMate</Link>
            </div>
            <div className="navbar__navbar-menu">
                {userOptionsVisible ? (
                    <>
                        <Link to="/searchPlaces">
                            <button className="navbar__navbar-item">
                                Wyszukaj miejsca
                            </button>
                        </Link>
                        <Link to="/searchHistory">
                            <button className="navbar__navbar-item">
                                Historia
                            </button>
                        </Link>
                        <Link to="/plans">
                            <button className="navbar__navbar-item">
                                Plany podróży
                            </button>
                        </Link>
                        <Link to="/schedules">
                            <button className="navbar__navbar-item">
                                Harmonogramy
                            </button>
                        </Link>
                        <Link to="/sharedPlans">
                            <button className="navbar__navbar-item">
                                Udostępnione plany
                            </button>
                        </Link>
                        <Link to="/follow">
                            <button className="navbar__navbar-item">
                                Obserwuj
                            </button>
                        </Link>
                    </>
                ) : null}
                {currentUser ? (
                    <button
                        className="navbar__navbar-item"
                        onClick={logoutHandler}
                    >
                        Wyloguj{" "}
                        {userContext.currentUser
                            ? `${userContext.currentUser}`
                            : ""}
                    </button>
                ) : location.pathname === "/" ? (
                    <Link to="/login">
                        <button className="navbar__navbar-item">Zaloguj</button>
                    </Link>
                ) : location.pathname === "/login" ? (
                    <Link to="/register">
                        <button className="navbar__navbar-item">
                            Zarejestruj
                        </button>
                    </Link>
                ) : (
                    <Link to="/login">
                        <button className="navbar__navbar-item">Zaloguj</button>
                    </Link>
                )}
            </div>
        </div>
    );
};

export default Navbar;
