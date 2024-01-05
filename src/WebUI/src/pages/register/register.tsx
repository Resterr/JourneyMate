import React from "react";
import RegisterForm from "./form/registerForm";
import "./register.css";

const Register: React.FC = () => {
    return (
        <div className="register">
            <RegisterForm></RegisterForm>
        </div>
    );
};

export default Register;
