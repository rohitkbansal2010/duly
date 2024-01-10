import { FONT_MEDIUM } from '@components/font-size-setting/helper';
import {
  AppStateAction, AppStateType, ErrorDataType, hideScheduleAppointmentError 
} from '@redux/actions';

import { RefreshDataType } from './../actions/types';


type AppState = {
  isSpinnerShown: boolean;
  isExceptionsModalShown: boolean;
  ScheduleAppointmentErrorData: ErrorDataType;
  errorDate: string | null;
	xCorrId: string | null;
  refreshData: RefreshDataType;
  fontSize:string;
};
const ScheduleAppointmentNoErrorData = {
  icon:'',
  isScheduleAppointmentErrorShown: false,
  errorHeader: '',
  errorTitle : '',
  errorMessage : '',
  errorButtonText : '',
  onAction : hideScheduleAppointmentError,
};

const appState: AppState = {
  isSpinnerShown: false,
  isExceptionsModalShown: false,
  ScheduleAppointmentErrorData: ScheduleAppointmentNoErrorData,
  errorDate: null,
  xCorrId: null,
  refreshData:{
    isRefreshRequired:false,
    refreshLocation:null,
  },
  fontSize:FONT_MEDIUM,
};


export const appStateReducer = (
  state = appState,
  action: AppStateAction
) => {
  switch (action.type) {
    case AppStateType.SHOW_SPINNER:
      return {
        ...state,
        isSpinnerShown: true,
      };
    case AppStateType.HIDE_SPINNER:
      return {
        ...state,
        isSpinnerShown: false,
      };
    case AppStateType.SHOW_EXCEPTIONS_MODAL:
      return {
        ...state,
        ...action.payload,
        isExceptionsModalShown: true,
      };
    case AppStateType.HIDE_EXCEPTIONS_MODAL:
      return {
        ...state,
        isExceptionsModalShown: false,
      };
    case AppStateType.SHOW_SCHEDULE_APPOINTMENT_ERROR:
      return {
        ...state,
        ScheduleAppointmentErrorData:action.payload,
      };
    case AppStateType.HIDE_SCHEDULE_APPOINTMENT_ERROR:
      return {
        ...state,
        ScheduleAppointmentErrorData: ScheduleAppointmentNoErrorData,
      };
    case AppStateType.REFRESH_DATA:
      return {
        ...state,
        refreshData:action.payload,
      };
    case AppStateType.SET_FONT_SIZE:
      return {
        ...state,
        fontSize:action.payload,
      };
    default:
      return state;
  }
};
