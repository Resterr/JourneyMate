import React from "react";
import LoginForm from "./form/loginForm";
import "./login.css";

const Login: React.FC = () => {
    return (
        <div className="login">
            <LoginForm></LoginForm>
        </div>
    );
};

export default Login;
