import { call, put, takeLatest } from 'redux-saga/effects';
  
import { ErrorData, SITE_ID } from '@constants';
import { StepperTitles } from '@enums';
import {
  getAppointments, 
  getSiteById, 
  saveFollowUpAppointment 
} from '@http/requests';
import { 
  stopDataFetch,
  refreshDataAction
} from '@redux/actions';
import { 
  CVCheckoutAppointmentsType, 
  GetFollowUpDetails, 
  SaveFollowUpDetails,
  setFollowUpOrder, 
  setFollowUpScheduled,
  setScheduledFollowUpDetails,
  setStepsList,
  startLoading,
  stopLoading
} from '@redux/actions/cv-checkout-appointments';
import { 
  AppointmentData, 
  Practitioner, 
  Site 
} from '@types';
import { 
  ApiExceptionParamType, 
  ApiExceptions,
  errorDataForSchedulingFail, 
  getDatesForAppointments, 
  getLocalStorageItem 
} from '@utils';

const getCurrentAppointmentPractitionerDetail = 
(Appointments: AppointmentData[], patientId: string, practitionerId: string): 
  Practitioner => {
  let practitioner: Practitioner = {
    id: '',
    humanName: {
      familyName: '',
      givenNames: [],
      prefixes: [],
    },
  };
  Appointments.forEach((appointment) => {
    if(
      appointment.patient.patientGeneralInfo.id === patientId 
        && appointment.practitioner.id === practitionerId
    ) {
      practitioner = appointment.practitioner;
    }
  });
  
  return practitioner;
};

function* workerGetFollowUpDetail({ payload }: GetFollowUpDetails) {
  try {
    const siteLocation: Site = yield call(getSiteById, { siteId: payload.data.siteId });
    const siteId = getLocalStorageItem(SITE_ID) || '';
    const { startDate, endDate } = getDatesForAppointments();
    const Appointments: AppointmentData[] = 
      yield call(getAppointments, { siteId, startDate, endDate });
    const practitionerData = getCurrentAppointmentPractitionerDetail(
      Appointments, 
      payload.data.patientId, 
      payload.data.practitionerId
    );
    yield put(setFollowUpOrder({
      location: siteLocation,
      practitioner: practitionerData,
    }));
    yield put(stopDataFetch());
    yield put(setStepsList([ { alias: StepperTitles.FOLLOW_UP, isSkeletonShown: false } ]));
  } catch (err) {
    if(!payload.data.notInitialLoad){
      yield ApiExceptions(
        { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
      );
    }
    yield put(setStepsList([ { alias: StepperTitles.FOLLOW_UP, isSkeletonShown: false } ]));
  } 
}

const noRefresh = {
  isRefreshRequired:false,
  refreshLocation:null,
};
function* workerScheduleFollowUpAppointment({ payload }: SaveFollowUpDetails) {
  try{
    yield put(startLoading());
    yield call(saveFollowUpAppointment, payload.data);
    yield put(setScheduledFollowUpDetails(payload.data));
    if(!payload.data.skipped) {
      yield put(setFollowUpScheduled());
    }
    yield put(refreshDataAction(noRefresh));
  } catch (err) {
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
            meetingType: payload.data.aptType,
            appointmentId: payload.data.appointment_Id,
            stepType: 'F',
          }),
        } as ApiExceptionParamType
      );
      yield put(refreshDataAction({
        isRefreshRequired:true,
        refreshLocation:'saveSchedule',
      }));
    }    
  } finally {
    yield put(stopLoading());
  }
}

export function* checkoutFollowUpSaga () {
  yield takeLatest(CVCheckoutAppointmentsType.GET_FOLLOW_UP_DETAILS, workerGetFollowUpDetail);  
}

export function* saveFollowUpAppointmentSaga() {
  yield takeLatest(
    CVCheckoutAppointmentsType.SAVE_FOLLOW_UP_DETAILS, 
    workerScheduleFollowUpAppointment
  );
}
