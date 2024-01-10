import {
  call, delay, put, takeEvery, takeLatest 
} from 'redux-saga/effects';

import { ErrorData } from '@constants';
import { StepperTitles } from '@enums';
import { getImagingDetails, getLabsLocation, saveImagingTestDetails } from '@http/requests';
import {
  hideSpinner, refreshDataAction, showSpinner, stopDataFetch 
} from '@redux/actions';
import {
  CVCheckoutAppointmentsType,
  GetImagingLocation,
  GetImagingTestDetails,
  SaveImagingTestDetails, 
  setImagingLocation, 
  setImagingTestDetails, 
  setImagingTestScheduled, 
  setScheduledImagingTestDetails, 
  setStepsList
} from '@redux/actions/cv-checkout-appointments';
import { LabOrImagingTestDetailsType, Location } from '@types';
import { ApiExceptions, ApiExceptionParamType, errorDataForSchedulingFail } from '@utils';


const refreshValue = {
  isRefreshRequired:true,
  refreshLocation:'savedImaging',
};


function* workerGetImagingDetails({ payload }: GetImagingTestDetails) {
  try {
    yield delay(1000);
    yield put(showSpinner());
    const data: LabOrImagingTestDetailsType = yield call(
      getImagingDetails,
      payload.patientId, 
      payload.appointmentId
    );
    yield put(setImagingTestDetails(data));
    yield put(stopDataFetch());
    yield put(setStepsList([ { alias: StepperTitles.IMAGING, isSkeletonShown: false } ]));
  } catch (err) {
    if (!payload.notInitialLoad) {
      yield ApiExceptions(
      { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
      );
    }
    yield put(setStepsList([ { alias: StepperTitles.IMAGING, isSkeletonShown: false } ]));
  } finally {
    yield put(hideSpinner()); 
  }
}

function* workerScheduleImagingAppointment({ payload }: SaveImagingTestDetails) {
  try{
    yield call(saveImagingTestDetails, payload.data);
    yield put(setScheduledImagingTestDetails(payload.data));
    if(!payload.data.skipped) {
      yield put(setImagingTestScheduled());
    }
  }catch(err){
    if(payload.data.skipped){
      yield ApiExceptions(
        { error:err, errorData:ErrorData.SkipErrorData } as ApiExceptionParamType
      );
    }else{
      yield ApiExceptions(
        {
          error:err,
          errorData: errorDataForSchedulingFail({
            date: payload.data.aptScheduleDate ?
              new Date(payload.data.aptScheduleDate) : new Date(),
            providerId: '',
            meetingType: '',
            appointmentId: payload.data.appointment_ID!,
            stepType: 'I',
          }),
        } as ApiExceptionParamType
      );
      yield put(refreshDataAction({
        isRefreshRequired:true,
        refreshLocation:'saveSchedule',
      }));
    }   
  }
}

function* workerGetImagingLocation({ payload }: GetImagingLocation) {
  try{
    yield put(setStepsList([ { alias: StepperTitles.IMAGING_LOCATION, isSkeletonShown: true } ]));
    yield delay(1000);
    yield put(setImagingLocation(null));
    const data: Location[] = yield call(getLabsLocation, payload.patientId); 
    yield put(setImagingLocation(data));
  }
  catch(err){
    yield ApiExceptions(
      { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
    );
    yield put(refreshDataAction(refreshValue));
  }finally{
    yield put(setStepsList([ { alias: StepperTitles.LAB_LOCATIONS, isSkeletonShown: false } ]));
  }
}

export function* getImagingLocationSaga () {
  yield takeLatest(CVCheckoutAppointmentsType.GET_IMAGING_LOCATION, workerGetImagingLocation);
}

export function* checkoutImagingSaga () {
  yield takeLatest(CVCheckoutAppointmentsType.GET_IMAGING_TEST_DETAILS, workerGetImagingDetails);   
}

export function* saveImagingAppointmentSaga() {
  yield takeEvery(
    CVCheckoutAppointmentsType.SAVE_IMAGING_TEST_DETAILS, 
    workerScheduleImagingAppointment
  );
}
