import {
  call, delay, put, takeLatest 
} from 'redux-saga/effects';

import { getLocationsList } from '@http/requests';
import { 
  hideSpinner,
  showSpinner, 
  stopDataFetch 
} from '@redux/actions';
import { CVCheckoutAppointmentsType, GetLocationsList, setLocationsList } from '@redux/actions/cv-checkout-appointments';
import { Location } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerGetLocationsList({ payload }: GetLocationsList) {
  try {
    yield put(showSpinner());
    const data: Location[] = yield call(
      getLocationsList,
      payload.orderType, 
      payload.patientAddress,
      payload.testsList
    );
    yield delay(1000);
    yield put(setLocationsList(data));
    yield put(stopDataFetch());
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  } finally {
    yield put(hideSpinner());
  }
}

export function* checkoutLocationsSaga () {
  yield takeLatest(CVCheckoutAppointmentsType.GET_LOCATIONS_LIST, workerGetLocationsList);   
}
