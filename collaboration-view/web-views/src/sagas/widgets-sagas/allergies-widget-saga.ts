import { put, takeLatest } from 'redux-saga/effects';

import { AppointmentModuleWidgetsAlias } from '@enums';
import { getAllergies } from '@http/requests';
import {
  GetAllergiesWidgetDataAction,
  OverviewWidgetsType,
  setAllergiesWidgetData,
  setSkeletonShown
} from '@redux/actions';
import { AllergiesData } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerGetAllergiesWidgetData({ payload }: GetAllergiesWidgetDataAction) {
  try {
    const { patientId } = payload;

    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.ALLERGIES, true));
    const allergies: AllergiesData[] = yield getAllergies(patientId);
    yield put(setAllergiesWidgetData(allergies));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  } finally {
    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.ALLERGIES, false));
  }
}

export function* allergiesWidgetSaga() {
  yield takeLatest(OverviewWidgetsType.GET_ALLERGIES_WIDGET_DATA, workerGetAllergiesWidgetData);
}
