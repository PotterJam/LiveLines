import { useState } from "react";
import { UserContext } from "./UserContext";

export const UserProvider = ({ children }) => {
    // User is the name of the "data" that gets stored in context
    const [user, setUser] = useState({ username: '', authenticated: null, streak: '' });

    const loginAttempted = user.authenticated !== null;
  
    const login = name => {
      setUser(_ => ({
        username: name,
        authenticated: true,
        streak: ''
      }));
    };
  
    const logout = () => {
      setUser(_ => ({
        username: '',
        authenticated: false,
        streak: ''
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
  