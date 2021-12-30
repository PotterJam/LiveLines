import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import  { UserContext } from '../auth/UserContext';
import { FaGithub } from 'react-icons/fa';

export function NavMenu(props) {
  const { user, loginAttempted } = useContext(UserContext);

  const loginWithGithub = `api/login?returnUrl=${window.location.origin}/`;

  const loginOrProfile = () => {
    return user.authenticated
      ? <span className="font-medium mr-3 text-lg text-slate-800">{user.username}</span>
      : <a
          href={loginWithGithub}
          className="bg-transparent hover:bg-blue-500 text-slate-700 hover:text-white py-2 px-4 border border-slate-500 hover:border-transparent rounded"
          role="button"
        >
          <div className="inline-block">
            <FaGithub className="mb-1 inline-block"/>
            <span className="font-medium">&nbsp; Login</span>
          </div>
        </a>
  }

  return (
      <div className="min-w- sticky top-0 z-40 w-full flex bg-white border-b border-gray-200 inset-x-0 z-100 h-16 items-center">
        <div className="w-full max-w-screen-xl relative mx-auto px-6">
          <div className="flex items-center place-content-between">
            <Link className="flex-none text-slate-800 text-2xl sm:text-3xl" tag={Link} to="/">LiveLines</Link>
            <div className="relative flex">
              <div>
                <Link tag={Link} className="font-medium mr-3 text-lg text-slate-800" to="/">Home</Link>
              </div>
              <div className="mx-2">
                {loginAttempted && loginOrProfile()}
              </div>
            </div>
          </div>
      </div>
    </div>
  );
}