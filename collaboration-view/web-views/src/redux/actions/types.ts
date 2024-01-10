import { ExceptionDataType } from '@redux/actions';
import { Practitioner, Site, UserData } from '@types';

export enum LocaleType {
  SWITCH_LOCALE = 'SWITCH_LOCALE',
}

export enum UserType {
  AUTHENTICATE_USER = 'AUTHENTICATE_USER',
  INITIALIZE_AUTHENTICATION = 'INITIALIZE_AUTHENTICATION',
  SET_USER_ROLE = 'SET_USER_ROLE',
  SET_USER_DATA = 'SET_USER_DATA',
  SHOW_LOGOUT_MODAL = 'SHOW_LOGOUT_MODAL',
  HIDE_LOGOUT_MODAL = 'HIDE_LOGOUT_MODAL',
  LOGOUT = 'LOGOUT',
	GET_ACTIVE_USER = 'GET_ACTIVE_USER'
}

export enum AppStateType {
  SHOW_SPINNER = 'SHOW_SPINNER',
  HIDE_SPINNER = 'HIDE_SPINNER',
  SHOW_EXCEPTIONS_MODAL = 'SHOW_EXCEPTIONS_MODAL',
  HIDE_EXCEPTIONS_MODAL = 'HIDE_EXCEPTIONS_MODAL',
  SHOW_SCHEDULE_APPOINTMENT_ERROR = 'SHOW_SCHEDULE_APPOINTMENT_ERROR',
  HIDE_SCHEDULE_APPOINTMENT_ERROR = 'HIDE_SCHEDULE_APPOINTMENT_ERROR',
  REFRESH_DATA = 'REFRESH_DATA',
  SET_FONT_SIZE = 'SET_FONT_SIZE',

}

export enum SiteStateType {
  SHOW_LOCATION_PICKER_MODAL = 'SHOW_LOCATION_PICKER_MODAL',
  HIDE_LOCATION_PICKER_MODAL = 'HIDE_LOCATION_PICKER_MODAL',
  SET_SITES = 'SET_SITES',
  GET_SITES = 'GET_SITES',
  GET_CURRENT_SITE = 'GET_CURRENT_SITE',
  SET_CURRENT_SITE = 'SET_CURRENT_SITE',
}
export type ScheduleAppointmentErrorDataType = {
  icon: string;
  isScheduleAppointmentErrorShown : boolean;
  errorHeader: string;
  errorTitle : string;
  errorMessage : string;
  errorButtonText : string;
}
export type LocaleAction = {
  type: typeof LocaleType.SWITCH_LOCALE;
  payload: { locale: string };
};

export type UserAuthenticationAction = {
  type: typeof UserType.AUTHENTICATE_USER;
  payload: { isAuthenticated: boolean };
};

export type UserAuthenticationInitAction = {
  type: typeof UserType.INITIALIZE_AUTHENTICATION;
  payload: { isAuthenticationInitiated: boolean };
};

export type UserRoleAction = {
  type: typeof UserType.SET_USER_ROLE;
  payload: { userRole: string | null };
};

export type UserDataAction = {
  type: typeof UserType.SET_USER_DATA;
  payload: { userData: Practitioner | null };
};

type GetActiveUserAction = {
  type: typeof UserType.GET_ACTIVE_USER;
}

type ShowLogoutModalAction = {
  type: typeof UserType.SHOW_LOGOUT_MODAL;
  payload: { isLogoutModalShown: boolean };
};

type HideLogoutModalAction = {
  type: typeof UserType.HIDE_LOGOUT_MODAL;
  payload: { isLogoutModalShown: boolean };
};

type LogoutAction = {
  type: typeof UserType.LOGOUT;
  payload: {
    isAuthenticated: boolean;
    userRole: string | null;
    userData: UserData | null;
  };
}
export type UserAction = UserAuthenticationAction |
  UserAuthenticationInitAction |
  UserRoleAction |
  UserDataAction |
  ShowLogoutModalAction |
  HideLogoutModalAction |
  LogoutAction | 
	GetActiveUserAction;

type ShowSpinner = {
  type: typeof AppStateType.SHOW_SPINNER;
};

type HideSpinner = {
  type: typeof AppStateType.HIDE_SPINNER;
};

type ShowExceptionsModalAction = {
  type: typeof AppStateType.SHOW_EXCEPTIONS_MODAL;
  payload: ExceptionDataType;
};

type HideExceptionsModalAction = {
  type: typeof AppStateType.HIDE_EXCEPTIONS_MODAL;
};
export type HideScheduleAppointmentError = {
  type: typeof AppStateType.HIDE_SCHEDULE_APPOINTMENT_ERROR;
};

export type RefreshSlotsType = {
  date: Date,
  providerId: string,
  meetingType: string | undefined,
  appointmentId: string,
  stepType: string,
  departmentId?: string,
}

export type ErrorDataType = {
  icon: string;
  isScheduleAppointmentErrorShown : boolean;
  errorHeader: string;
  errorTitle : string;
  errorMessage : string;
  errorButtonText : string;
  refreshSlots?: RefreshSlotsType;
  onAction:()=>HideScheduleAppointmentError;
}

export type RefreshDataType = {
  isRefreshRequired:boolean;
  refreshLocation:string|null;
}

type ShowScheduleAppointmentError = {
  type: typeof AppStateType.SHOW_SCHEDULE_APPOINTMENT_ERROR;
  payload: ErrorDataType;
};
type RefreshDataAction = {
  type: typeof AppStateType.REFRESH_DATA;
  payload: RefreshDataType;
};

type ShowLocationPickerModal = {
  type: typeof SiteStateType.SHOW_LOCATION_PICKER_MODAL;
};

type HideLocationPickerModal = {
  type: typeof SiteStateType.HIDE_LOCATION_PICKER_MODAL;
};

type SetSites = {
  type: typeof SiteStateType.SET_SITES;
  payload: { sites: Site[] };
};

type GetSites = {
  type: typeof SiteStateType.GET_SITES;
}

type SetFontSize = {
  type: typeof AppStateType.SET_FONT_SIZE;
  payload: string;
}
export type GetCurrentSite = {
  type: typeof SiteStateType.GET_CURRENT_SITE;
  payload: {
    siteId: string;
  };
}

export type SetCurrentSite = {
  type: typeof SiteStateType.SET_CURRENT_SITE;
  payload: { currentSite: Site };
}

export type AppStateAction = ShowSpinner |
  HideSpinner |
  ShowExceptionsModalAction |
  HideExceptionsModalAction |
  ShowScheduleAppointmentError |
  HideScheduleAppointmentError |
  RefreshDataAction | 
  SetFontSize;

export type SiteStateAction = ShowLocationPickerModal |
  HideLocationPickerModal |
  SetSites |
  GetSites |
  GetCurrentSite |
  SetCurrentSite;

export type GetFollowUpActionType = {
  patientId: string,
  practitionerId: string,
  siteId: string,
  notInitialLoad?: boolean,
}
