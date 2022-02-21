import React, { useContext, useEffect, useState } from 'react';
import { FiLogOut } from 'react-icons/fi';
import { getData, postData } from "../Api";
import { UserContext } from '../auth/UserContext';
import { PRIVACY_LEVEL } from "../components/Enum";

export function Profile() {
  
  const { user } = useContext(UserContext);

  const [username, setUsername] = useState("");
  const [linePrivacy, setLinePrivacy] = useState("");

  const logout = 'api/logout';
  
  const getUserProfile = async () => {
    const resp = await getData("api/user/profile");
    const profileResp = await resp.json();
    setUsername(profileResp.username);
    setLinePrivacy(profileResp.linePrivacy);
  }
  
  useEffect(() => {
    if (user.authenticated) {
        getUserProfile()
    }
  }, [user.authenticated]);
  
  useEffect(() => {
    if (user.authenticated) {
        updateProfile();
    }
  }, [linePrivacy]);

  const updateProfile = async () => {
    const updatedProfileResp = await postData("api/user/profile", { 
      linePrivacy: linePrivacy 
    });
    const updatedProfile = await updatedProfileResp.json();
    setUsername(updatedProfile.username);
    setLinePrivacy(updatedProfile.linePrivacy);
  }
  
  return (
    <div className="h-max mx-auto border border-slate-300 bg-white w-max mt-10">
      <div className='flex flex-col items-center px-20 py-10'>
        <div className='text-3xl pb-5'>{username}</div>
        <div className="flex space-x-4 py-4">
          <div>
            <label>
              Default Line Privacy:
              <select 
                  value={linePrivacy} 
                  onChange={e => setLinePrivacy(e.target.value)}
              >
                <option>{PRIVACY_LEVEL.PRIVATE}</option>
                <option>{PRIVACY_LEVEL.UNLISTED}</option>
                <option>{PRIVACY_LEVEL.PUBLIC}</option>
              </select>
            </label>
          </div>
        </div>
        <div>
          <span>Logout</span>
          <a href={logout}><FiLogOut className="ml-2 mb-1 inline-block"/></a>
        </div>
      </div>
    </div>
  );
}
