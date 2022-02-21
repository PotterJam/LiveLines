import { useState } from "react";
import { UserContext } from "./UserContext";

export const UserProvider = ({ children }) => {
    // User is the name of the "data" that gets stored in context
    const [user, setUser] = useState({ username: '', authenticated: null, streak: '', defaultPrivacy: null });

    const loginAttempted = user.authenticated !== null;
  
    const login = (name, privacy) => {
      setUser(_ => ({
        username: name,
        authenticated: true,
        streak: '',
        defaultPrivacy : privacy
      }));
    };
  
    const logout = () => {
      setUser(_ => ({
        username: '',
        authenticated: false,
        streak: '',
        defaultPrivacy: null
      }));
    };
    
    const setStreak = newStreak => setUser({
        ...user,
        streak: newStreak
    });
  
    return (
      <UserContext.Provider value={{ user, loginAttempted, login, logout, setStreak }}>
        {children}
      </UserContext.Provider>
    );
  }
  