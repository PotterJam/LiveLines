import React, { useContext, useEffect, useState } from 'react';
import { FiLogOut } from 'react-icons/fi';
import { getData, postData } from "../Api";
import { UserContext } from '../auth/UserContext';
import { BsSpotify } from 'react-icons/bs';

export function Profile() {
  
  const { user } = useContext(UserContext);

  const [userInfo, setUserInfo] = useState({});

  const loginSpotify = 'api/spotify/login';
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
    <div className="h-max mx-auto border border-slate-300 bg-white w-max mt-10">
      <div className='flex flex-col items-center px-20 py-10'>
        <div className='text-3xl pb-5'>{userInfo.username}</div>
        <a className='pb-3' href={loginSpotify}>Login to Spotify<BsSpotify className="ml-2 mb-1 inline-block"/></a>
        <div>
          <span>Logout</span>
          <a href={logout}><FiLogOut className="ml-2 mb-1 inline-block"/></a>
        </div>
      </div>
    </div>
  );
}
