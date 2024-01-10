import { call, put, takeLatest } from 'redux-saga/effects';

import { AppointmentModuleWidgetsAlias } from '@enums';
import { getPatientAppointments } from '@http/requests';
import {
  GetPatientAppointmentsAction,
  OverviewWidgetsType,
  setPatientAppointments,
  setSkeletonShown
} from '@redux/actions';
import { PatientAppointments } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerGetPatientAppointments({ payload }: GetPatientAppointmentsAction) {
  try {
    const { appointmentId } = payload;

    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.TIMELINE, true));
    const patientAppointments: PatientAppointments =
      yield call(getPatientAppointments, appointmentId);
    yield put(setPatientAppointments(patientAppointments));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  } finally {
    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.TIMELINE, false));
  }
}

export function* patientAppointmentsWidgetSaga() {
  yield takeLatest(OverviewWidgetsType.GET_PATIENT_APPOINTMENTS, workerGetPatientAppointments);
}
