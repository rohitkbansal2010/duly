import { put, takeLatest, call } from 'redux-saga/effects';

import { AppointmentModuleWidgetsAlias } from '@enums';
import { getImmunizations } from '@http/requests';
import {
  GetImmunizationsWidgetDataAction,
  OverviewWidgetsType,
  setImmunizationsWidgetData,
  setSkeletonShown
} from '@redux/actions';
import { ImmunizationsWidgetDataType } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerGetImmunizationsWidgetData({ payload }: GetImmunizationsWidgetDataAction) {
  try {
    const { patientId } = payload;

    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.IMMUNIZATIONS, true));
    const immunizations: ImmunizationsWidgetDataType = yield call(getImmunizations, patientId);
    yield put(setImmunizationsWidgetData(immunizations));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  } finally {
    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.IMMUNIZATIONS, false));
  }
}

export function* immunizationsWidgetSaga() {
  yield takeLatest(
    OverviewWidgetsType.GET_IMMUNIZATIONS_WIDGET_DATA,
    workerGetImmunizationsWidgetData
  );
}
