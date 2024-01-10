import { put } from 'redux-saga/effects';

import { StatusCodes } from '@enums';
import {
  setIsAuthenticated,
  showExceptionsModal,
  stopDataFetch
} from '@redux/actions';

export type CatchExceptionsParamsType = {
	status: StatusCodes;
	xCorrId: string | null;
}
export function* catchExceptions({ status, xCorrId }: CatchExceptionsParamsType) {
  const errorDate = new Date().toUTCString();
  switch (status) {
    case StatusCodes.UNAUTHORIZED:
      yield put(setIsAuthenticated(false));
      break;    
    default:
      yield put(showExceptionsModal({ errorDate, xCorrId }));
      yield put(stopDataFetch());
  }
}

