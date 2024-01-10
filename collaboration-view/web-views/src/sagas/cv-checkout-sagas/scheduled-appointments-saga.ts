import {
  call, delay, put, takeLatest 
} from 'redux-saga/effects';

import { ErrorData } from '@constants';
import { getScheduledDetails, getScheduledReferralProviderDetails } from '@http/requests';
import { 
  CVCheckoutAppointmentsType, 
  GetScheduledData, 
  setAllScheduledFollowUpAppointments, 
  setAllScheduledReferralAppointments, 
  setFollowUpScheduled, 
  setImagingTestScheduled, 
  setLabsTestScheduled, 
  setProviderForAllScheduledReferrals, 
  setReferralScheduled, 
  setScheduledImagingTestDetails, 
  setScheduledLabTestDetails,
  stopLoading
} from '@redux/actions/cv-checkout-appointments';
import { 
  ChooseProviderDetailsType, 
  GetScheduledAppointmentsType, 
  SaveLabDetailsRequestType, 
  ScheduledFollowUpDetails 
} from '@types';
import { ApiExceptionParamType, ApiExceptions } from '@utils';

const isFollowUpOrImagingScheduled = 
  (followUpDetails: ScheduledFollowUpDetails[] | SaveLabDetailsRequestType[]): boolean => {
    let isScheduled = false;
    followUpDetails.forEach((detail)=>{
      if(!detail.skipped){
        isScheduled = true;
      }
    });
    return isScheduled;
  };

const getScheduledReferralIds = (referralDetails: ScheduledFollowUpDetails[]): number[] | [] => {
  const referralIds: number[] = [];
  referralDetails.forEach((referral) =>{ 
    if(!referral.skipped) referralIds.push(parseInt(referral.provider_ID));
  });
  return referralIds;
};

const isLabTestScheduled = (labDetails: SaveLabDetailsRequestType): boolean => 
  !labDetails.skipped;

function* workerScheduledAppointments({ payload }: GetScheduledData) {
  try {
    const scheduledDetails: GetScheduledAppointmentsType =
      yield call(getScheduledDetails, payload.appointmentId);
    const labDetails = scheduledDetails.labDetailsList.filter(detail => 
      detail.type === 'L');
    const imagingDetails = scheduledDetails.labDetailsList.filter(detail => 
      detail.type === 'I');
    const followUpDetails = scheduledDetails.scheduleFollowUpList.filter(detail => 
      detail.type === 'F');
    const referralDetails = scheduledDetails.scheduleFollowUpList.filter(detail => 
      detail.type === 'R');
    let isLabScheduled = false;
    if (labDetails.length) {
      yield put(setScheduledLabTestDetails(labDetails[labDetails.length - 1]));
      isLabScheduled = isLabTestScheduled(labDetails[labDetails.length - 1]);
    }
    if(isLabScheduled) {
      yield put(setLabsTestScheduled());
    }
    let isImagingScheduled = false;
    if (imagingDetails.length) {
      for(const imagingData of imagingDetails){
        yield put(setScheduledImagingTestDetails(imagingData));
      }
      isImagingScheduled = isFollowUpOrImagingScheduled(imagingDetails);
    }
    if(isImagingScheduled) { 
      yield put(setImagingTestScheduled());
    }
    if (followUpDetails.length) {
      yield put(setAllScheduledFollowUpAppointments(followUpDetails));
      if(isFollowUpOrImagingScheduled(followUpDetails)){
        yield put(setFollowUpScheduled());
      }
    }
    if (referralDetails.length) {
      yield put(setAllScheduledReferralAppointments(referralDetails));
      const referralIds = getScheduledReferralIds(referralDetails);
      if(referralIds.length) {
        yield put(setReferralScheduled());
        const providerDetails: ChooseProviderDetailsType[] = 
          yield call(getScheduledReferralProviderDetails, referralIds);
        yield referralDetails && put(setProviderForAllScheduledReferrals(providerDetails));
      }
    }
    yield delay(1000);
  } catch(err) {
    yield ApiExceptions(
      { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
    );
  }finally {
    yield put(stopLoading());
  }
}

export function* getScheduledAppointmentsSaga() {
  yield takeLatest(CVCheckoutAppointmentsType.GET_SCHEDULED_DATA, workerScheduledAppointments);
}
