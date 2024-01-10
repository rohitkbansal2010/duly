import {
  AppStateAction, AppStateType, HideScheduleAppointmentError, ErrorDataType, RefreshDataType 
} from '@redux/actions';

export const showSpinner = (): AppStateAction =>
  ({ type: AppStateType.SHOW_SPINNER });

export const hideSpinner = (): AppStateAction =>
  ({ type: AppStateType.HIDE_SPINNER });

export type ExceptionDataType = {
	errorDate: string;
	xCorrId: string | null;
}
export const showExceptionsModal = ({ errorDate, xCorrId }: ExceptionDataType): AppStateAction =>
  ({
    type: AppStateType.SHOW_EXCEPTIONS_MODAL,
    payload: { errorDate, xCorrId },
  });

export const hideExceptionsModal = (): AppStateAction =>
  ({ type: AppStateType.HIDE_EXCEPTIONS_MODAL });

export const showScheduleAppointmentError = 
(data : ErrorDataType): AppStateAction =>
  ({
    type: AppStateType.SHOW_SCHEDULE_APPOINTMENT_ERROR,
    payload: data,
  });

export const hideScheduleAppointmentError = (): HideScheduleAppointmentError =>
  ({ type: AppStateType.HIDE_SCHEDULE_APPOINTMENT_ERROR });

export const refreshDataAction = (data:RefreshDataType):AppStateAction=>
  ({
    type:AppStateType.REFRESH_DATA,
    payload:data, 
  });


export const setFontSize = (fontSize:string):AppStateAction=>
  ({
    type:AppStateType.SET_FONT_SIZE,
    payload:fontSize, 
  });
