import { 
  call, put, takeEvery, takeLatest 
} from 'redux-saga/effects';
  
import { ErrorData } from '@constants';
import { StepperTitles } from '@enums';
import { getProviderDetails, getReferralDetails, saveReferralDetails } from '@http/requests';
import { 
  hideSpinner, 
  refreshDataAction, 
  showSpinner, 
  stopDataFetch 
} from '@redux/actions';
import { 
  CVCheckoutAppointmentsType,
  GetReferralDetails,
  GetReferralProviderDetails,
  SaveReferralDetails,
  setReferralDetails,
  setReferralProviderDetails,
  setReferralScheduled,
  setScheduledReferralDetails,
  setStepsList,
  startLoading,
  stopLoading
} from '@redux/actions/cv-checkout-appointments';
import { ChooseProviderDetailsType, ReferralDetailsType } from '@types';
import { ApiExceptionParamType, ApiExceptions, errorDataForSchedulingFail } from '@utils';

const noRefresh = {
  isRefreshRequired:false,
  refreshLocation:null,
};

function* workerGetReferralDetails({ payload }: GetReferralDetails) {
  try {
    yield put(showSpinner());
    yield put(setStepsList([ { alias: StepperTitles.REFERRAL, isSkeletonShown: true } ]));
    const data: ReferralDetailsType[] = yield call(getReferralDetails, payload.patientId);
    yield put(setReferralDetails(data));
    yield put(stopDataFetch());
    yield put(setStepsList([ { alias: StepperTitles.REFERRAL, isSkeletonShown: false } ]));
  } catch (err) {
    if (!payload.notInitialLoad) {
      yield ApiExceptions(
        { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
      );
    }
    yield put(setStepsList([ { alias: StepperTitles.REFERRAL, isSkeletonShown: false } ]));
  } finally {
    yield put(hideSpinner());
  }
}

function* workerScheduleReferralAppointment({ payload }: SaveReferralDetails) {
  try{
    yield call(saveReferralDetails, payload.data);
    yield put(setScheduledReferralDetails(payload.data));
    if(!payload.data.skipped) {
      yield put(setReferralScheduled());
    }
    yield put(refreshDataAction(noRefresh));
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
            date: new Date(payload.data.aptScheduleDate),
            providerId: payload.data.provider_ID,
            meetingType: '',
            appointmentId: payload.data.appointment_Id,
            stepType: 'R',
            departmentId: payload.data.department_Id,
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
function* workerGetReferralProviderDetails({ payload }: GetReferralProviderDetails) {
  try{
    yield put(startLoading());
    const data: ChooseProviderDetailsType[] = yield call(getProviderDetails, payload);
    if(!data) {
      yield put(setReferralProviderDetails([]));
    }
    yield put(setReferralProviderDetails(data));
    yield put(refreshDataAction(noRefresh));
  } catch(err){
    yield put(refreshDataAction({
      isRefreshRequired:true,
      refreshLocation:'chooseProvider',
    }));

    yield ApiExceptions(
      { error:err, errorData:ErrorData.UnableToFetchDataChooseProvider } as ApiExceptionParamType
    );
  } finally {
    yield put(stopLoading());
  }
}

export function* checkoutReferralSaga () {
  yield takeLatest(CVCheckoutAppointmentsType.GET_REFERRAL_DETAILS, workerGetReferralDetails);   
}

export function* saveReferralAppointmentSaga() {
  yield takeEvery(
    CVCheckoutAppointmentsType.SAVE_REFERRAL_DETAILS, 
    workerScheduleReferralAppointment
  );
}

export function* getReferralProviderDetails() {
  yield takeLatest(
    CVCheckoutAppointmentsType.GET_REFERRAL_PROVIDER_DETAILS,
    workerGetReferralProviderDetails
  );
}
