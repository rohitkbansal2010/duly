import { call, put, takeLatest } from 'redux-saga/effects';

import { SITE_ID } from '@constants';
import { StatusCodes } from '@enums';
import { getSiteById } from '@http/requests';
import {
  GetCurrentSite, setCurrentSite, showLocationPickerModal, SiteStateType
} from '@redux/actions';
import { Site } from '@types';
import { catchExceptions, removeLocalStorageItem, CatchExceptionsParamsType } from '@utils';

function* WorkerCurrentSite({ payload }: GetCurrentSite) {
  try {
    const { siteId } = payload;
    const site: Site = yield call(getSiteById, { siteId });
    yield put(setCurrentSite(site));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    if (status === StatusCodes.NOT_FOUND) {
      removeLocalStorageItem(SITE_ID);
      yield put(showLocationPickerModal());
    } else {
      yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
    }
  }
}

export function* currentSiteSaga() {
  yield takeLatest(SiteStateType.GET_CURRENT_SITE, WorkerCurrentSite);
}
