import { put, call, takeLatest } from 'redux-saga/effects';

import { getTestReportsResultsData } from '@http/requests';
import {
  CurrentAppointmentType,
  GetTestReportsResults,
  setTestReportsResults,
  setTestReportsResultsSkeleton
} from '@redux/actions';
import { TestReportsResult } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerTestReportsResults({ payload }: GetTestReportsResults) {
  try {
    const { testReportId } = payload;
    
    yield put(setTestReportsResultsSkeleton(true));
    const testReportsResults: TestReportsResult =
      yield call(getTestReportsResultsData, testReportId);
    yield put(setTestReportsResults(testReportsResults));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  } finally {
    yield put(setTestReportsResultsSkeleton(false));
  }
}

export function* testReportsResultsSaga() {
  yield takeLatest(CurrentAppointmentType.GET_TEST_REPORTS_RESULTS, workerTestReportsResults);
}
