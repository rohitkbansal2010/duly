import { call, put, takeLatest } from 'redux-saga/effects';

import { getSites } from '@http/requests';
import { setSites, SiteStateType } from '@redux/actions';
import { Site } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* WorkerSites() {
  try {
    const sites: Site[] = yield call(getSites);
    yield put(setSites(sites));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  }
}

export function* sitesSaga() {
  yield takeLatest(SiteStateType.GET_SITES, WorkerSites);
}
