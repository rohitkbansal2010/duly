import { put, takeLatest } from 'redux-saga/effects';

import { AppointmentModuleWidgetsAlias } from '@enums';
import { getMedications } from '@http/requests';
import {
  GetMedicationsWidgetDataAction,
  OverviewWidgetsType,
  setMedicationsWidgetData,
  setSkeletonShown
} from '@redux/actions';
import { MedicationsWidgetDataType } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerGetMedicationsWidgetData({ payload }: GetMedicationsWidgetDataAction) {
  try {
    const { patientId } = payload;

    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.MEDICATIONS, true));
    const medications: MedicationsWidgetDataType = yield getMedications(patientId);
    yield put(setMedicationsWidgetData(medications));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  } finally {
    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.MEDICATIONS, false));
  }
}

export function* medicationsWidgetSaga() {
  yield takeLatest(OverviewWidgetsType.GET_MEDICATIONS_WIDGET_DATA, workerGetMedicationsWidgetData);
}
