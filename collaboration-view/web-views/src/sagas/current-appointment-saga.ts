import { call, put, takeLatest } from 'redux-saga/effects';

import { ErrorData, SITE_ID } from '@constants';
import { AppPart } from '@enums';
import { getConfigurations } from '@http/requests';
import {
  CurrentAppointmentType,
  GetConfigurationsAction,
  setCurrentAppointmentModules,
  showModuleMenu
} from '@redux/actions';
import { ModulesDataType } from '@types';
import {
  ApiExceptionParamType,
  ApiExceptions,
  getLocalStorageItem
} from '@utils';

function* workerCurrentAppointment({ payload }: GetConfigurationsAction) {
  try {
    const { patientId } = payload;

    yield put(showModuleMenu(false));

    const siteId = getLocalStorageItem(SITE_ID) || '';

    const configurations: ModulesDataType[] = yield call(getConfigurations, {
      appPart: AppPart.CURRENT_APPOINTMENT,
      siteId,
      patientId,
    });

    yield put(setCurrentAppointmentModules(configurations));
  } catch (err) {
    yield ApiExceptions(
      { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
    );
  } finally {
    yield put(showModuleMenu(true));
  }
}

export function* currentAppointmentSaga() {
  yield takeLatest(CurrentAppointmentType.GET_CONFIGURATIONS_REQUEST, workerCurrentAppointment);
}
