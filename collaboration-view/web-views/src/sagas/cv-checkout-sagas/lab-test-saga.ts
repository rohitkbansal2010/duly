import { 
  call, 
  put, 
  delay, 
  takeLatest 
} from 'redux-saga/effects';
    
import { ErrorData } from '@constants';
import { StepperTitles } from '@enums';
import { getLabsLocation, getLabTestDetails, saveLabDetails } from '@http/requests';
import {  
  stopDataFetch,
  refreshDataAction
} from '@redux/actions';
import { 
  setLabTestDetails,
  GetLabTestDetails,
  CVCheckoutAppointmentsType,
  SaveLabTestDetails,
  setLabsTestScheduled,
  setScheduledLabTestDetails,
  startLoading, 
  stopLoading, 
  setStepsList, 
  setLabsLocation,
  GetLabsLocation,
  setLabsTestNotSkipped,
  setLabsTestSkipped
} from '@redux/actions/cv-checkout-appointments';
import { LabOrImagingTestDetailsType, Location, NearbyLabLocation } from '@types';
import { ApiExceptionParamType, ApiExceptions } from '@utils';

const refreshValue = {
  isRefreshRequired:true,
  refreshLocation:'savedLabs',
};
const noRefresh = {
  isRefreshRequired:false,
  refreshLocation:null,
};


function* workerGetLabTestDetails({ payload }: GetLabTestDetails) {
  try {
    const data: LabOrImagingTestDetailsType = yield call(
      getLabTestDetails,
      payload.patientId,
      payload.appointmentId
    );
    yield put(setLabTestDetails(data));
    yield put(stopDataFetch());
    yield put(setStepsList([ { alias: StepperTitles.LABS, isSkeletonShown: false } ]));
    yield put(stopLoading());
  } catch (err) {
    if(!payload.notInitialLoad) {
      yield ApiExceptions(
        { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
      );
    }
    yield put(setStepsList([ { alias: StepperTitles.LABS, isSkeletonShown: false } ]));
    yield put(stopLoading());
  }
}

function* workerGetLabsLocation({ payload }: GetLabsLocation) {
  try{
    yield put(setStepsList([ { alias: StepperTitles.LAB_LOCATIONS, isSkeletonShown: true } ]));
    yield delay(1000);
    yield put(setLabsLocation(null));
    const data: NearbyLabLocation[] = yield call(getLabsLocation, payload.lat, payload.lng);
    const labLocations: Location[] = data.map(location=>
      (
        {
          id: location.labId.toString(),
          name: location.labName,
          location: {
            address: {
              addressLine: location.llbAddressLn1,
              city: location.llCity,
              state: location.llState,
              zipCode: location.llZip,
            },
            phoneNumber: '',
            workingHours: '',
            distance: parseInt((parseInt(location.distance) * 0.000621).toFixed(2)),
          },
        }
      ));
    yield put(setLabsLocation(labLocations));
    yield put(setStepsList([ { alias: StepperTitles.LAB_LOCATIONS, isSkeletonShown: false } ]));
  }
  catch(err){
    console.log(err);
    yield put(setStepsList([ { alias: StepperTitles.LAB_LOCATIONS, isSkeletonShown: false } ]));
  }
}
export function* checkoutLabTestSaga () {
  yield takeLatest(CVCheckoutAppointmentsType.GET_LAB_TEST_DETAILS, workerGetLabTestDetails);   
}

export function* getLabsLocationSaga () {
  yield takeLatest(CVCheckoutAppointmentsType.GET_LABS_LOCATION, workerGetLabsLocation);
}

function* workerSaveLabTest({ payload }: SaveLabTestDetails) {
  try{
    yield put(startLoading());
    yield call(saveLabDetails, payload.data);
    if(!payload.data.skipped){
      yield put(setLabsTestScheduled());
    }
    yield put(setScheduledLabTestDetails(payload.data));
    yield put(refreshDataAction(noRefresh));
    if(payload.data.skipped){
      yield put(setLabsTestSkipped());
    }
    if(!payload.data.skipped){
      yield put(setLabsTestNotSkipped());
    }
  }catch(err){
    yield put(refreshDataAction(refreshValue));
    if(payload.data.skipped){
      yield put(refreshDataAction({
        isRefreshRequired:true,
        refreshLocation:'saveSkippedLab',
      }));
      yield ApiExceptions(
        { error:err, errorData:ErrorData.SkipErrorData } as ApiExceptionParamType
      );
    }else{
      yield put(refreshDataAction({
        isRefreshRequired:true,
        refreshLocation:'saveLabs',
      }));
      yield ApiExceptions(
        { error:err, errorData:ErrorData.SkipErrorData } as ApiExceptionParamType
      );      
    }
  }finally {
    yield put(stopLoading());
  }
}

export function* saveLabTestSaga() {
  yield takeLatest(
    CVCheckoutAppointmentsType.SAVE_LAB_TEST_DETAILS, 
    workerSaveLabTest
  );
}
