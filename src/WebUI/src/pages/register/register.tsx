import React from "react";
import RegisterForm from "./form/registerForm";
import './register.css';

export const Register : React.FC = () => {
	return (
		<div className="login__container">
			<RegisterForm></RegisterForm>
		</div>
	)
}

export default Register;