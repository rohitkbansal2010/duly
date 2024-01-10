import { call, put, takeLatest } from 'redux-saga/effects';

import { AppointmentModuleWidgetsAlias, CardType, VitalType } from '@enums';
import { getTodaysVitals } from '@http/requests';
import {
  GetTodaysVitalsAction,
  OverviewWidgetsType,
  showChartSkeleton,
  setSkeletonShown,
  hideChartSkeleton,
  getChartData,
  setCurrentCardType,
  setTodaysVitals
} from '@redux/actions';
import { TodayVitalCard } from '@types';
import {
  catchExceptions, CatchExceptionsParamsType, convertHeightToFeet, convertWeightToLBS 
} from '@utils';

const modifyTodaysVitals = (todaysVitals: TodayVitalCard[]):TodayVitalCard[] => 
  todaysVitals.map((vitals) => {
    if(vitals.cardType === CardType.WEIGHT_HEIGHT){
      return {
        ...vitals,
        vitals: vitals.vitals.map((vital)=>{
          if(vital.vitalType === VitalType.WEIGHT){
            return {
              ...vital,
              measurements: vital.measurements.map(measurement=>
                ({ 
                  ...measurement, 
                  convertedValue: convertWeightToLBS(measurement.value),
                  convertedUnit: 'lbs', 
                })),
            };
          }
          return {
            ...vital,
            measurements: vital.measurements.map(measurement=>
              ({ 
                ...measurement, 
                convertedValue: convertHeightToFeet(measurement.value),
                convertedUnit: 'inches', 
              })),
          };
        }),
      };
    }
    return vitals;
  });

function* workerGetTodaysVitals({ payload }: GetTodaysVitalsAction) {
  try {
    const { patientId } = payload;

    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.VITALS, true));
    yield put(showChartSkeleton());
    const todaysVitals: TodayVitalCard[] = yield call(getTodaysVitals, patientId);
    yield put(setTodaysVitals(modifyTodaysVitals(todaysVitals)));
    console.log(todaysVitals);

    if (todaysVitals.length) {
      const { cardType, vitals } = todaysVitals[0];

      yield put(setCurrentCardType(cardType));
      yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.VITALS, false));

      yield vitals?.length
        ? put(getChartData(patientId, cardType))
        : put(hideChartSkeleton());
    } else {
      yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.VITALS, false));
      yield put(hideChartSkeleton());
    }
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
    yield put(setSkeletonShown(AppointmentModuleWidgetsAlias.VITALS, false));
    yield put(hideChartSkeleton());
  }
}

export function* todaysVitalsWidgetSaga() {
  yield takeLatest(OverviewWidgetsType.GET_TODAYS_VITALS, workerGetTodaysVitals);
}
