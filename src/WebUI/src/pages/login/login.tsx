import React from "react";
import LoginForm from "./form/loginForm";
import './login.css';

export const Login : React.FC = () => {
	return (
		<div className="login__container">
			<LoginForm></LoginForm>
		</div>
	)
}

export default Login;