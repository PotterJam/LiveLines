import React, { useContext, useEffect, useState } from 'react';
import { FiLogOut } from 'react-icons/fi';
import { getData, postData } from "../Api";
import { UserContext } from '../auth/UserContext';

export function Profile() {
  
  const { user } = useContext(UserContext);

  const [userInfo, setUserInfo] = useState({});

  const logout = 'api/logout';
  
  const getUserProfile = async () => {
    const resp = await getData("api/user/profile");
    const profileResp = await resp.json();
    setUserInfo({ username: profileResp.username });
  }
  
  useEffect(() => {
    if (user.authenticated) {
        getUserProfile()
    }
  }, [user.authenticated]);

  return (
    <div className="h-full flex mt-4 p-0.5 sm:p-2 flex-col items-center">
      <div>{userInfo.username}</div>
      <div>
        <span>Logout</span>
        <a href={logout}><FiLogOut className="mb-1 inline-block"/></a>
      </div>
    </div>
  );
}
