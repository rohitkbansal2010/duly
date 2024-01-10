import { call, put, takeLatest } from 'redux-saga/effects';

import { getAvailableSlotsOfProvider, getReferralSlotsOfProvider, getWeekDayTimeSlots } from '@http/requests';
import { hideSpinner, showSpinner, stopDataFetch } from '@redux/actions';
import {
  CVCheckoutAppointmentsType, GetWeekDayTimeSlots, setWeekDayTimeSlots, setIsTimeSlotSkeonShown 
} from '@redux/actions/cv-checkout-appointments';
import { DateTimeSlotsType, GetTimeSlotsAPIResponseType } from '@types';
import { setTimeSlots } from '@utils';

function* workerGetWeekDayTimeSlots({ payload }: GetWeekDayTimeSlots) {
  try {
    if(!payload.data.date){
      return;
    }
    yield put(setIsTimeSlotSkeonShown(true));
    yield put(showSpinner());
    yield put(setWeekDayTimeSlots(null));
    if(payload.data.stepType === 'F') {
      let visitTypeId = '2990';
      if (payload.data.meetingType === 'In-Person') {
        visitTypeId = '8001';
      }
      const data: GetTimeSlotsAPIResponseType[] = yield call(
        getAvailableSlotsOfProvider,
        payload.data.date.toISOString().split('T')[0],
        visitTypeId,
        payload.data.appointmentId
      );
      yield put(setWeekDayTimeSlots(setTimeSlots(data)));
    } else if(payload.data.stepType === 'R') {
      const data: GetTimeSlotsAPIResponseType[] = yield call(
        getReferralSlotsOfProvider,
        { 
          date: payload.data.date.toISOString().split('T')[0],
          providerId: payload.data.providerId,
          visitTypeId: '8001',
          departmentId: payload.data.departmentId ? payload.data.departmentId : '',
        }
      );
      yield put(setWeekDayTimeSlots(setTimeSlots(data)));
    } else {
      const data: DateTimeSlotsType[] = yield call(
        getWeekDayTimeSlots,
        payload.data.date
      );
      yield put(setWeekDayTimeSlots(data));
    }
    
    yield put(stopDataFetch());
    yield put(setIsTimeSlotSkeonShown(false));
  }catch (err) {
    console.log(err);
    yield put(setIsTimeSlotSkeonShown(false));
  } 
  finally {
    yield put(hideSpinner());
  }
}

export function* checkoutTimeSlotSaga () {
  yield takeLatest(CVCheckoutAppointmentsType.GET_WEEK_DAY_TIME_SLOTS, workerGetWeekDayTimeSlots);
}
