import { put } from 'redux-saga/effects';

import { ErrorData } from '@constants';
import {
  ErrorDataType,
  showScheduleAppointmentError
} from '@redux/actions';

export type ApiExceptionParamType = {
	error:any;
  errorData:ErrorDataType;
}
export function* ApiExceptions({ error, errorData }: ApiExceptionParamType) {
  if(error === undefined){ 
    yield put(showScheduleAppointmentError(ErrorData.BadNetwork));
  }
  else if(error.status === 409){
    yield put(showScheduleAppointmentError(ErrorData.sameSlotBooked));
  }
  else{
    yield put(showScheduleAppointmentError(errorData));
  }
}


