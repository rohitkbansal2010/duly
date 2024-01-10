import { UserData } from '@types';

import {
  UserAction,
  UserType
} from '../actions';

export type UserStateType = {
  isAuthenticated: boolean;
  isAuthenticationInitiated: boolean;
  isLogoutModalShown: boolean;
  userRole: string | null;
  userData: UserData | null;
};

const initialState: UserStateType = {
  isAuthenticated: false,
  isAuthenticationInitiated: false,
  isLogoutModalShown: false,
  userRole: null,
  userData: null,
};

export const userAuthenticationReducer = (
  state = initialState,
  { type, payload }: UserAction
): UserStateType => {
  switch (type) {
    case UserType.AUTHENTICATE_USER:
    case UserType.INITIALIZE_AUTHENTICATION:
    case UserType.SET_USER_ROLE:
    case UserType.SET_USER_DATA:
    case UserType.SHOW_LOGOUT_MODAL:
    case UserType.HIDE_LOGOUT_MODAL:
    case UserType.LOGOUT:
      return {
        ...state,
        ...payload, // TODO: Remove payload wrapping in DPGECLOF-41
      };

    default:
      return state;
  }
};
