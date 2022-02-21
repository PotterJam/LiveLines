import React, { useContext, useEffect, useState } from 'react';
import { FiLogOut } from 'react-icons/fi';
import { getData, postData } from "../Api";
import { UserContext } from '../auth/UserContext';
import {PRIVACY_LEVELS} from "../components/Enum";

export function Profile() {
  
  const { user } = useContext(UserContext);

  const [username, setUsername] = useState("");
  const [defaultPrivacy, setDefaultPrivacy] = useState("");

  const logout = 'api/logout';
  
  const getUserProfile = async () => {
    const resp = await getData("api/user/profile");
    const profileResp = await resp.json();
    setUsername(profileResp.username);
    setDefaultPrivacy(profileResp.defaultPrivacy);
  }
  
  useEffect(() => {
    if (user.authenticated) {
        getUserProfile()
    }
  }, [user.authenticated]);

  const updateProfile = async () => {
    const updatedProfileResp = await postData("api/user/profile", { DefaultPrivacy: defaultPrivacy });
  
    const updatedProfile = await updatedProfileResp.json();
    setUsername(updatedProfile.username);
    setDefaultPrivacy(updatedProfile.defaultPrivacy);
  }

  const setPrivacySelection = e => {
    setDefaultPrivacy(e.target.value);
  }
  
  return (
    <div className="h-max mx-auto border border-slate-300 bg-white w-max mt-10">
      <div className='flex flex-col items-center px-20 py-10'>
        <div className='text-3xl pb-5'>{username}</div>
          <div>
            <label className="pr-2">
              Default Line Privacy:
              <select value={defaultPrivacy} onChange={setPrivacySelection}>
                <option value={PRIVACY_LEVELS.PRIVATE}>Private</option>
                <option value={PRIVACY_LEVELS.UNLISTED}>Unlisted</option>
                <option value={PRIVACY_LEVELS.PUBLIC}>Public</option>
              </select>
            </label>
          </div>
          <button onClick={updateProfile}>Update</button>
        <div>
          <span>Logout</span>
          <a href={logout}><FiLogOut className="ml-2 mb-1 inline-block"/></a>
        </div>
      </div>
    </div>
  );
}
