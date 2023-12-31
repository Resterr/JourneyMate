import React, { createContext, useState } from "react";

type UserContextType = {
  accessToken: string | null;
  refreshToken: string | null;
  refreshTokenExpireDate: string | null;
  currentUser: string | null;
  setTokens: (
    accessToken: string | null,
    refreshToken: string | null,
    refreshTokenExpireDate: string | null,
  ) => void;
  updateTokens: (
    accessToken: string | null,
    refreshToken: string | null,
  ) => void;
  setUserName: (userName: string | null) => void;
  logout: () => void;
};

export const UserContext = createContext<UserContextType>({
  accessToken: null,
  refreshToken: null,
  refreshTokenExpireDate: null,
  currentUser: null,
  setTokens: () => {},
  updateTokens: () => {},
  setUserName: () => {},
  logout: () => {},
});

export const UserProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [accessToken, setAccessToken] = useState<string | null>(
    localStorage.getItem("accessToken"),
  );
  const [refreshToken, setRefreshToken] = useState<string | null>(
    localStorage.getItem("refreshToken"),
  );
  const [refreshTokenExpireDate, setRefreshTokenExpireDate] = useState<
    string | null
  >(localStorage.getItem("refreshTokenExpireDate"));
  const [currentUser, setCurrentUserName] = useState<string | null>(
    localStorage.getItem("currentUser"),
  );

  const setTokens = (
    newAccessToken: string | null,
    newRefreshToken: string | null,
    newRefreshTokenExpireDate: string | null,
  ) => {
    setAccessToken(newAccessToken);
    setRefreshToken(newRefreshToken);
    setRefreshTokenExpireDate(newRefreshTokenExpireDate);
    if (
      newAccessToken !== null &&
      newRefreshToken !== null &&
      newRefreshTokenExpireDate !== null
    ) {
      localStorage.setItem("accessToken", newAccessToken);
      localStorage.setItem("refreshToken", newRefreshToken);
      localStorage.setItem("refreshTokenExpireDate", newRefreshTokenExpireDate);
    }
  };

  const updateTokens = (
    newAccessToken: string | null,
    newRefreshToken: string | null,
  ) => {
    setAccessToken(newAccessToken);
    setRefreshToken(newRefreshToken);

    if (newAccessToken !== null && newRefreshToken !== null) {
      localStorage.setItem("accessToken", newAccessToken);
      localStorage.setItem("refreshToken", newRefreshToken);
    }
  };

  const setUserName = (userName: string | null) => {
    setCurrentUserName(userName);

    if (userName !== null) {
      localStorage.setItem("currentUser", userName);
    }
  };

  const logout = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("refreshTokenExpireDate");
    localStorage.removeItem("currentUser");

    setAccessToken(null);
    setRefreshToken(null);
    setRefreshTokenExpireDate(null);
    setCurrentUserName(null);
  };

  return (
    <UserContext.Provider
      value={{
        accessToken,
        refreshToken,
        refreshTokenExpireDate,
        currentUser,
        setTokens,
        updateTokens,
        setUserName,
        logout,
      }}
    >
      {children}
    </UserContext.Provider>
  );
};

export class UsersContext {}
