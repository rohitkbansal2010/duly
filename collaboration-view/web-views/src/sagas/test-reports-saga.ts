import { put, call, takeLatest } from 'redux-saga/effects';

import { getTestReportsData } from '@http/requests';
import {
  CurrentAppointmentType,
  GetTestReports,
  setTestReports,
  setTestReportsSkeleton,
  setTestResultsMounted,
  setTestResultsNextPage
} from '@redux/actions';
import { TestReport } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerTestReports({ payload }: GetTestReports) {
  try {
    const testReports: TestReport[] = yield call(getTestReportsData, payload);
    if (testReports?.length) {
      yield put(setTestReports(testReports));
      yield put(setTestReportsSkeleton(false));
      yield put(setTestResultsMounted(true));
    }
    yield put(setTestResultsNextPage());
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
    yield put(setTestReportsSkeleton(false));
    yield put(setTestResultsMounted(true));
  }
}

export function* testReportsSaga() {
  yield takeLatest(CurrentAppointmentType.GET_TEST_REPORTS, workerTestReports);
}
