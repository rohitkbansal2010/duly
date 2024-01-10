import { put, takeLatest } from 'redux-saga/effects';

import { AppointmentModuleWidgetsAlias } from '@enums';
import { getHealthConditions } from '@http/requests';
import {
  GetConditionsWidgetDataAction,
  OverviewWidgetsType,
  setConditionsWidgetData,
  setSkeletonShown
} from '@redux/actions';
import { HealthConditionsWidgetDataType } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerGetConditionsWidgetData({ payload }: GetConditionsWidgetDataAction) {
  try {
    const { patientId } = payload;

    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.CONDITIONS, true));
    const conditions: HealthConditionsWidgetDataType = yield getHealthConditions(patientId);
    yield put(setConditionsWidgetData(conditions));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  } finally {
    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.CONDITIONS, false));
  }
}

export function* conditionsWidgetSaga() {
  yield takeLatest(OverviewWidgetsType.GET_CONDITIONS_WIDGET_DATA, workerGetConditionsWidgetData);
}
