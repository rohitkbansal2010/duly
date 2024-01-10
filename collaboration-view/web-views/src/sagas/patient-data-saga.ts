import { call, put, takeLatest } from 'redux-saga/effects';

import { ErrorData } from '@constants';
import { getPatientData } from '@http/requests';
import {
  CurrentAppointmentType,
  GetPatientData,
  setPatientData
} from '@redux/actions';
import { PatientData } from '@types';
import { ApiExceptionParamType, ApiExceptions } from '@utils';

function* workerPatientData({ payload }: GetPatientData) {
  try {
    const { patientId } = payload;

    const patientData: PatientData = yield call(getPatientData, patientId);

    yield put(setPatientData(patientData));
  } catch (err) {
    yield ApiExceptions(
      { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
    );
  }
}

export function* patientDataSaga() {
  yield takeLatest(CurrentAppointmentType.GET_PATIENT_DATA, workerPatientData);
}
