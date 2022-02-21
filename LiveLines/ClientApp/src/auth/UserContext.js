import { createContext } from 'react';

export const UserContext = createContext({ name: '', auth: null, streak: '', profile: null });