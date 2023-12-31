import { StyledEngineProvider } from "@mui/material/styles";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { UserProvider } from "./contexts/userContext";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement,
);
root.render(
  <React.StrictMode>
    <UserProvider>
      <StyledEngineProvider injectFirst>
        <App />
      </StyledEngineProvider>
    </UserProvider>
  </React.StrictMode>,
);
