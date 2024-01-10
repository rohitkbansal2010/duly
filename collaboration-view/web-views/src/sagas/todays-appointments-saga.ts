import { call, put, takeLatest } from 'redux-saga/effects';

import { SITE_ID } from '@constants';
import { getAppointments } from '@http/requests';
import {
  TodaysAppointmentType,
  setAppointments,
  setDailyStatistics,
  setPractitioners,
  showSpinner,
  hideSpinner,
  startDataFetch
} from '@redux/actions';
import {
  AppointmentData,
  Practitioner
} from '@types';
import {
  catchExceptions,
  CatchExceptionsParamsType,
  getDatesForAppointments,
  getLocalStorageItem,
  getPractitionersFromAppointments
} from '@utils';

// TODO: send to utils/appointments after 724
const calcDailyStatistics = (appointments: AppointmentData[]) =>
  ({
    newPatients: appointments.filter(({ patient: { isNewPatient } }: AppointmentData) =>
      isNewPatient).length,
  });

function* workerTodaysAppointments() {
  try {
    yield put(startDataFetch());
    yield put(showSpinner());

    const { startDate, endDate } = getDatesForAppointments();
    const siteId = getLocalStorageItem(SITE_ID) || '';
		
    const appointments: AppointmentData[] = 
			yield call(getAppointments, { siteId, startDate, endDate });

    const practitioners: Practitioner[] = getPractitionersFromAppointments(appointments);

    yield put(setDailyStatistics(calcDailyStatistics(appointments)));
    yield put(setAppointments(appointments));
    yield put(setPractitioners(practitioners));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  } finally {
    yield put(hideSpinner());
  }
}

export function* todaysAppointmentsSaga() {
  yield takeLatest(TodaysAppointmentType.GET_TODAYS_APPOINTMENTS_SAGA, workerTodaysAppointments);
}
