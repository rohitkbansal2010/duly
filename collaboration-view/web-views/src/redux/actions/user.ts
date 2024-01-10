import { UserData } from '@types';

import { UserAction, UserType } from './types';

export const setIsAuthenticated = (isAuthenticated: boolean): UserAction =>
  ({
    type: UserType.AUTHENTICATE_USER,
    payload: { isAuthenticated },
  });

export const initiateAuthentication =
  (isAuthenticationInitiated: boolean): UserAction =>
    ({
      type: UserType.INITIALIZE_AUTHENTICATION,
      payload: { isAuthenticationInitiated },
    });

export const setUserRole = (userRole: string | null): UserAction =>
  ({
    type: UserType.SET_USER_ROLE,
    payload: { userRole },
  });

export const setUserData = (userData: UserData | null): UserAction =>
  ({
    type: UserType.SET_USER_DATA,
    payload: { userData },
  });

export const showLogoutModal = (): UserAction =>
  ({
    type: UserType.SHOW_LOGOUT_MODAL,
    payload: { isLogoutModalShown: true },
  });

export const hideLogoutModal = (): UserAction =>
  ({
    type: UserType.HIDE_LOGOUT_MODAL,
    payload: { isLogoutModalShown: false },
  });

export const logout = (): UserAction =>
  ({
    type: UserType.LOGOUT,
    payload: {
      userRole: null,
      isAuthenticated: false,
      userData: null,
    },
  });

export const getActiveUser = (): UserAction =>
  ({ type: UserType.GET_ACTIVE_USER });
