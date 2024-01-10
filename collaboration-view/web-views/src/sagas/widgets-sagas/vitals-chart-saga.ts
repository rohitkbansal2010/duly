import { put, takeLatest } from '@redux-saga/core/effects';

import { StatusCodes } from '@enums';
import { getChartByVitalsCardType } from '@http/requests';
import {
  GetChartDataAction,
  hideChartSkeleton,
  OverviewWidgetsType,
  setChartData,
  showChartSkeleton
} from '@redux/actions';
import { DateTimeDoubleChartResponse } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerGetVitalsChartData({ payload }: GetChartDataAction) {
  try {
    const { patientId, vitalCardType } = payload;

    yield put(showChartSkeleton());
    const { chart }: DateTimeDoubleChartResponse =
      yield getChartByVitalsCardType(patientId, vitalCardType);
    yield put(setChartData(chart));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    if (status === StatusCodes.NOT_FOUND) {
      yield put(setChartData(null));
    } else {
      yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
    }
  } finally {
    yield put(hideChartSkeleton());
  }
}

export function* vitalChartSaga() {
  yield takeLatest(OverviewWidgetsType.GET_CHART_DATA, workerGetVitalsChartData);
}
