import { StepsListDataType } from '@mock-data';
import { 
  ChooseProviderDetailsType,
  FollowUpDetailsType, 
  Location, 
  ProviderDetailsType, 
  ReferralDetailsType, 
  SaveLabDetailsRequestType, 
  ScheduledFollowUpDetails,
  ScheduledImagingTestDetails,
  DateTimeSlotsType,
  PreferredPharmacyDetailsType,
  GetTimeSlotsRequestDataType,
  FollowUpOrderType,
  LabOrImagingTestDetailsType
} from '@types';

import { GetFollowUpActionType } from './types';

export enum CVCheckoutAppointmentsType {
  SET_STEPS_LIST = 'SET_STEPS_LIST',
  SET_FOLLOW_UP_SCHEDULED = 'SET_FOLLOW_UP_SCHEDULED',
  SET_FOLLOW_UP_SKIPPED = 'SET_FOLLOW_UP_SKIPPED',
  SET_REFERRAL_SCHEDULED = 'SET_REFERRAL_SCHEDULED',
  SET_REFERRAL_SKIPPED = 'SET_REFERRAL_SKIPPED',
  SET_LABS_TEST_SCHEDULED = 'SET_LABS_TEST_SCHEDULED',
  SET_LABS_TEST_SKIPPED = 'SET_LABS_TEST_SKIPPED',
  SET_LABS_TEST_NOT_SKIPPED = 'SET_LABS_TEST_NOT_SKIPPED',
  SET_IMAGING_TEST_SCHEDULED = 'SET_IMAGING_TEST_SCHEDULED',
  SET_IMAGING_TEST_SKIPPED = 'SET_IMAGING_TEST_SKIPPED',
  SET_PRESCRIPTIONS_CHECKED = 'SET_PRESCRIPTIONS_CHECKED',
  GET_FOLLOW_UP_DETAILS = 'GET_FOLLOW_UP_DETAILS',
  SET_FOLLOW_UP_DETAILS = 'SET_FOLLOW_UP_DETAILS',
  SET_FOLLOW_UP_ORDER = 'SET_FOLLOW_UP_ORDER',
  SAVE_FOLLOW_UP_DETAILS = 'SAVE_FOLLOW_UP_DETAILS',
  GET_REFERRAL_DETAILS = 'GET_REFERRAL_DETAILS',
  SET_REFERRAL_DETAILS = 'SET_REFERRAL_DETAILS',
  SAVE_REFERRAL_DETAILS = 'SAVE_REFERRAL_DETAILS',
  GET_REFERRAL_PROVIDER_DETAILS = 'GET_REFERRAL_PROVIDER_DETAILS',
  SET_REFERRAL_PROVIDER_DETAILS = 'SET_REFERRAL_PROVIDER_DETAILS',
  SET_SCHEDULED_REFERRAL_DETAILS = 'SET_SCHEDULED_REFERRAL_DETAILS',
  GET_LAB_TEST_DETAILS = 'GET_LAB_TEST_DETAILS',
  SET_LAB_TEST_DETAILS = 'SET_LAB_TEST_DETAILS',
  SAVE_LAB_TEST_DETAILS = 'SAVE_LAB_TEST_DETAILS',
  GET_LABS_LOCATION = 'GET_LABS_LOCATION',
  SET_LABS_LOCATION = 'SET_LABS_LOCATION',
  SET_SCHEDULED_LAB_TEST_DETAILS = 'SET_SCHEDULED_LAB_TEST_DETAILS',
  GET_IMAGING_TEST_DETAILS = 'GET_IMAGING_TEST_DETAILS',
  SET_IMAGING_TEST_DETAILS = 'SET_IMAGING_TEST_DETAILS',
  SET_IMAGING_LOCATION = 'SET_IMAGING_LOCATION',
  GET_IMAGING_LOCATION = 'GET_IMAGING_LOCATION',
  SAVE_IMAGING_TEST_DETAILS = 'SAVE_IMAGING_TEST_DETAILS',
  GET_PRESCRIPTION_DETAILS = 'GET_PRESCRIPTION_DETAILS',
  SET_PRESCRIPTION_DETAILS = 'SET_PRESCRIPTION_DETAILS',
  SAVE_PRESCRIPTION_DETAILS = 'SAVE_PRESCRIPTION_DETAILS',
  SET_SCHEDULED_FOLLOW_UP_DETAILS = 'SET_SCHEDULED_FOLLOW_UP_DETAILS',
  SET_SELECTED_PROVIDER_IN_REFERRAL = 'SET_SELECTED_PROVIDER_IN_REFERRAL',
  SET_SCHEDULED_IMAGING_TEST_DETAILS = 'SET_SCHEDULED_IMAGING_TEST_DETAILS',
  GET_LOCATIONS_LIST = 'GET_LOCATIONS_LIST',
  SET_LOCATIONS_LIST = 'SET_LOCATIONS_LIST',
  START_LOADING = 'START_LOADING',
  STOP_LOADING = 'STOP_LOADING',
  RESET_LOADING = 'RESET_LOADING',
  GET_SCHEDULED_DATA = 'GET_SCHEDULED_DATA',
  SET_ALL_SCHEDULED_FOLLOW_UP_DETAILS = 'SET_ALL_SCHEDULED_FOLLOW_UP_DETAILS',
  SET_ALL_SCHEDULED_REFERRAL_DETAILS = 'SET_ALL_SCHEDULED_REFERRAL_DETAILS',
  SET_SCHEDULED_REFERRAL_PROVIDER_DETAILS = 'SET_SCHEDULED_REFERRAL_PROVIDER_DETAILS',
  GET_WEEK_DAY_TIME_SLOTS = 'GET_WEEK_DAY_TIME_SLOTS',
  SET_WEEK_DAY_TIME_SLOTS = 'SET_WEEK_DAY_TIME_SLOTS',
  SET_IS_TIME_SLOT_SKELETON_SHOWN = 'SET_IS_TIME_SLOT_SKELETON_SHOWN',
  SET_PROVIDER_FOR_ALL_SCHEDULED_REFERRALS = 'SET_PROVIDER_FOR_ALL_SCHEDULED_REFERRALS',
  GET_PREFERRED_PHARMACY_DETAILS = 'GET_PREFERRED_PHARMACY_DETAILS',
  SET_PREFERRED_PHARMACY_DETAILS = 'SET_PREFERRED_PHARMACY_DETAILS',
  SET_FOLLOWUP_DATE = 'SET_FOLLOWUP_DATE',
}

export type SetStepsList = {
  type: CVCheckoutAppointmentsType.SET_STEPS_LIST,
  payload: { data: StepsListDataType[]},
}

export type StartLoading = {
  type: CVCheckoutAppointmentsType.START_LOADING,
}

export type ResetLoading = {
  type: CVCheckoutAppointmentsType.RESET_LOADING,
}

export type StopLoading = {
  type: CVCheckoutAppointmentsType.STOP_LOADING,
}

export type GetFollowUpDetails = {
  type: CVCheckoutAppointmentsType.GET_FOLLOW_UP_DETAILS,
  payload: { data: GetFollowUpActionType },
}

export type SetFollowUpDetails = {
  type: CVCheckoutAppointmentsType.SET_FOLLOW_UP_DETAILS,
  payload: { data: FollowUpDetailsType[] },
}

export type SetFollowUpOrder = {
  type: CVCheckoutAppointmentsType.SET_FOLLOW_UP_ORDER,
  payload: { data: FollowUpOrderType },
}

export type SetScheduledFollowUpDetails = {
  type: CVCheckoutAppointmentsType.SET_SCHEDULED_FOLLOW_UP_DETAILS,
  payload: { data: ScheduledFollowUpDetails },
}

export type SaveFollowUpDetails = {
  type: CVCheckoutAppointmentsType.SAVE_FOLLOW_UP_DETAILS,
  payload: { data: ScheduledFollowUpDetails },
}

export type GetReferralDetails = {
  type: CVCheckoutAppointmentsType.GET_REFERRAL_DETAILS,
  payload: { patientId: string, notInitialLoad?: boolean },
}

export type SetReferralDetails = {
  type: CVCheckoutAppointmentsType.SET_REFERRAL_DETAILS,
  payload: { data: ReferralDetailsType[] },
}

export type SaveReferralDetails = {
  type: CVCheckoutAppointmentsType.SAVE_REFERRAL_DETAILS,
  payload: { data: ScheduledFollowUpDetails },
}

export type SetSelectedProviderInReferral = {
  type: CVCheckoutAppointmentsType.SET_SELECTED_PROVIDER_IN_REFERRAL,
  payload: { data: {doctor: ProviderDetailsType, type: string} },
}

export type GetLabTestDetails = {
  type: CVCheckoutAppointmentsType.GET_LAB_TEST_DETAILS,
  payload: { patientId: string, notInitialLoad?: boolean, appointmentId: string },
}

export type GetLabsLocation = {
  type: CVCheckoutAppointmentsType.GET_LABS_LOCATION,
  payload: { lat: string, lng: string },
}

export type SetLabsLocation = {
  type: CVCheckoutAppointmentsType.SET_LABS_LOCATION,
  payload: { data: Location[] | null }
}


export type GetImagingLocation = {
  type: CVCheckoutAppointmentsType.GET_IMAGING_LOCATION,
  payload: { patientId: string },
}

export type SetImagingLocation = {
  type: CVCheckoutAppointmentsType.SET_IMAGING_LOCATION,
  payload: { data: Location[] | null }
}


export type SetLabTestDetails = {
  type: CVCheckoutAppointmentsType.SET_LAB_TEST_DETAILS,
  payload: { data: LabOrImagingTestDetailsType },
}

export type SaveLabTestDetails = {
  type: CVCheckoutAppointmentsType.SAVE_LAB_TEST_DETAILS,
  payload: { data: SaveLabDetailsRequestType },
}

export type SetScheduledLabTestDetails = {
  type: CVCheckoutAppointmentsType.SET_SCHEDULED_LAB_TEST_DETAILS,
  payload: { data: SaveLabDetailsRequestType },
}

export type GetImagingTestDetails = {
  type: CVCheckoutAppointmentsType.GET_IMAGING_TEST_DETAILS,
  payload: { patientId: string, appointmentId: string, notInitialLoad?: boolean },
}

export type SetImagingTestDetails = {
  type: CVCheckoutAppointmentsType.SET_IMAGING_TEST_DETAILS,
  payload: { data: LabOrImagingTestDetailsType },
}

export type SaveImagingTestDetails = {
  type: CVCheckoutAppointmentsType.SAVE_IMAGING_TEST_DETAILS,
  payload: { data: ScheduledImagingTestDetails },
}

export type GetPrescriptionDetails = {
  type: CVCheckoutAppointmentsType.GET_PRESCRIPTION_DETAILS,
  payload: { patientId: string, appointmentId: string, notInitialLoad?: boolean },
}

export type SetPrescriptionDetails = {
  type: CVCheckoutAppointmentsType.SET_PRESCRIPTION_DETAILS,
  payload: { data: FollowUpDetailsType[] },
}

export type SavePrescriptionDetails = {
  type: CVCheckoutAppointmentsType.SAVE_PRESCRIPTION_DETAILS,
  payload: { data: FollowUpDetailsType },
}

export type SetFollowUpScheduled = {
  type: CVCheckoutAppointmentsType.SET_FOLLOW_UP_SCHEDULED,
}

export type SetFollowUpSkipped = {
  type: CVCheckoutAppointmentsType.SET_FOLLOW_UP_SKIPPED,
}

export type SetReferralScheduled = {
  type: CVCheckoutAppointmentsType.SET_REFERRAL_SCHEDULED,
}

export type SetReferralSkipped = {
  type: CVCheckoutAppointmentsType.SET_REFERRAL_SKIPPED,
}

export type SetLabsTestScheduled = {
  type: CVCheckoutAppointmentsType.SET_LABS_TEST_SCHEDULED,
}

export type SetLabsTestSkipped = {
  type: CVCheckoutAppointmentsType.SET_LABS_TEST_SKIPPED,
}

export type SetLabsTestNotSkipped = {
  type: CVCheckoutAppointmentsType.SET_LABS_TEST_NOT_SKIPPED,
}

export type SetImagingTestScheduled = {
  type: CVCheckoutAppointmentsType.SET_IMAGING_TEST_SCHEDULED,
}

export type SetImagingTestSkipped = {
  type: CVCheckoutAppointmentsType.SET_IMAGING_TEST_SKIPPED,
}

export type SetPrescriptionsChecked = {
  type: CVCheckoutAppointmentsType.SET_PRESCRIPTIONS_CHECKED,
}

export type GetReferralProviderDetails = {
  type: CVCheckoutAppointmentsType.GET_REFERRAL_PROVIDER_DETAILS,
  payload: { lat: string, lng:string, referralVisit: string },
}

export type SetReferralProviderDetails = {
  type: CVCheckoutAppointmentsType.SET_REFERRAL_PROVIDER_DETAILS,
  payload: { data: ChooseProviderDetailsType[] },
}

export type SetScheduledReferralDetails = {
  type: CVCheckoutAppointmentsType.SET_SCHEDULED_REFERRAL_DETAILS,
  payload: { data: ScheduledFollowUpDetails },
}

export type GetScheduledData = {
  type: CVCheckoutAppointmentsType.GET_SCHEDULED_DATA,
  payload: { appointmentId: string },
}

export type SetAllScheduledFollowUpAppointments = {
  type: CVCheckoutAppointmentsType.SET_ALL_SCHEDULED_FOLLOW_UP_DETAILS,
  payload: { data: ScheduledFollowUpDetails[] },
}

export type SetAllScheduledReferralAppointments = {
  type: CVCheckoutAppointmentsType.SET_ALL_SCHEDULED_REFERRAL_DETAILS,
  payload: { data: ScheduledFollowUpDetails[] },
}

export type SetScheduledReferralProviderDetails = {
  type: CVCheckoutAppointmentsType.SET_SCHEDULED_REFERRAL_PROVIDER_DETAILS,
  payload: { data: ChooseProviderDetailsType },
}

export type SetScheduledImagingTestDetails = {
  type: CVCheckoutAppointmentsType.SET_SCHEDULED_IMAGING_TEST_DETAILS,
  payload: { data: ScheduledImagingTestDetails },
}

export type GetLocationsList = {
  type : CVCheckoutAppointmentsType.GET_LOCATIONS_LIST,
  payload: { orderType: string, patientAddress: string, testsList: string[] },
}

export type SetLocationsList = {
  type : CVCheckoutAppointmentsType.SET_LOCATIONS_LIST,
  payload: { data: Location[] },
}

export type GetWeekDayTimeSlots = {
  type: CVCheckoutAppointmentsType.GET_WEEK_DAY_TIME_SLOTS,
  payload: { data: GetTimeSlotsRequestDataType},
}

export type SetWeekDayTimeSlots = {
  type: CVCheckoutAppointmentsType.SET_WEEK_DAY_TIME_SLOTS,
  payload: { data: DateTimeSlotsType[] | null },
}

export type SetIsTimeSlotSkeletonShown = {
  type: CVCheckoutAppointmentsType.SET_IS_TIME_SLOT_SKELETON_SHOWN,
  payload: { isShown: boolean },
}

export type SetProviderForAllScheduledReferrals = {
  type: CVCheckoutAppointmentsType.SET_PROVIDER_FOR_ALL_SCHEDULED_REFERRALS,
  payload: { provider: ChooseProviderDetailsType[] },
}

export type GetPreferredPharmacyDetails = {
  type: CVCheckoutAppointmentsType.GET_PREFERRED_PHARMACY_DETAILS,
  payload: { patientId: string, notInitialLoad?: boolean },
}

export type SetPreferrdPharmacyDetails = {
  type: CVCheckoutAppointmentsType.SET_PREFERRED_PHARMACY_DETAILS,
  payload: { data: PreferredPharmacyDetailsType },
}

export type SetFollowUpDate = {
  type: CVCheckoutAppointmentsType.SET_FOLLOWUP_DATE,
  payload: { data: Date },
}


export type CheckoutAppointmentsAction =
  SetStepsList |
  GetFollowUpDetails |
  SetFollowUpDetails |
  SetFollowUpOrder |
  SetScheduledFollowUpDetails |
  SaveFollowUpDetails |
  GetReferralDetails |
  SetReferralDetails |
  SaveReferralDetails |
  GetLabTestDetails |
  SetLabTestDetails |
  GetLabsLocation |
  SetLabsLocation |
  SaveLabTestDetails |
  SetScheduledLabTestDetails |
  GetImagingTestDetails |
  SetImagingTestDetails |
  GetImagingLocation |
  SetImagingLocation |
  SaveImagingTestDetails |
  GetPrescriptionDetails |
  SetPrescriptionDetails |
  SavePrescriptionDetails |
  SetFollowUpScheduled |
  SetFollowUpSkipped |
  SetReferralScheduled |
  SetReferralSkipped |
  SetLabsTestScheduled |
  SetLabsTestSkipped |
  SetLabsTestNotSkipped |
  SetImagingTestScheduled |
  SetImagingTestSkipped |
  SetPrescriptionsChecked |
  GetReferralProviderDetails |
  SetReferralProviderDetails |
  SetScheduledReferralDetails |
  SetSelectedProviderInReferral |
  SetScheduledImagingTestDetails |
  GetLocationsList |
  SetLocationsList |
  StartLoading |
  StopLoading |
  ResetLoading |
  GetScheduledData |
  SetAllScheduledFollowUpAppointments |
  SetAllScheduledReferralAppointments |
  SetScheduledReferralProviderDetails |
  GetWeekDayTimeSlots |
  SetWeekDayTimeSlots |
  SetIsTimeSlotSkeletonShown |
  SetProviderForAllScheduledReferrals |
  GetPreferredPharmacyDetails | 
  SetPreferrdPharmacyDetails |
  SetFollowUpDate ;

export const setStepsList = 
(data: StepsListDataType[]): CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.SET_STEPS_LIST,
    payload: { data },
  });

export const startLoading = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.START_LOADING });

export const stopLoading = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.STOP_LOADING });

export const resetLoading = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.RESET_LOADING });
  
export const setFollowUpScheduled = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_FOLLOW_UP_SCHEDULED });

export const setFollowUpSkipped = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_FOLLOW_UP_SKIPPED });

export const setReferralScheduled = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_REFERRAL_SCHEDULED });
  
export const setReferralSkipped = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_REFERRAL_SKIPPED });

export const setLabsTestScheduled = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_LABS_TEST_SCHEDULED });

export const setLabsTestSkipped = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_LABS_TEST_SKIPPED });

export const setLabsTestNotSkipped = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_LABS_TEST_NOT_SKIPPED });

export const setImagingTestScheduled = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_IMAGING_TEST_SCHEDULED });

export const setImagingTestSkipped = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_IMAGING_TEST_SKIPPED });
  
export const setPrescriptionsChecked = (): CheckoutAppointmentsAction =>
  ({ type: CVCheckoutAppointmentsType.SET_PRESCRIPTIONS_CHECKED });

export const getFollowUpDetails = (data: GetFollowUpActionType): CheckoutAppointmentsAction =>
  ({ 
    type: CVCheckoutAppointmentsType.GET_FOLLOW_UP_DETAILS,
    payload: { data },
  });

export const setFollowUpDetails = (data: FollowUpDetailsType[]): CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.SET_FOLLOW_UP_DETAILS,
    payload: { data },
  });
export const setFollowUpOrder = (data: FollowUpOrderType): CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.SET_FOLLOW_UP_ORDER,
    payload: { data },
  });

export const setScheduledFollowUpDetails = 
  (data: ScheduledFollowUpDetails): CheckoutAppointmentsAction =>
    ({
      type: CVCheckoutAppointmentsType.SET_SCHEDULED_FOLLOW_UP_DETAILS,
      payload: { data },
    });

export const saveFollowUpDetails = (data: ScheduledFollowUpDetails): CheckoutAppointmentsAction => 
  ({ 
    type: CVCheckoutAppointmentsType.SAVE_FOLLOW_UP_DETAILS,
    payload: { data },
  });

export const getReferralDetails = 
(patientId: string, notInitialLoad?: boolean): CheckoutAppointmentsAction =>
  ({ 
    type: CVCheckoutAppointmentsType.GET_REFERRAL_DETAILS,
    payload: { patientId, notInitialLoad },
  });

export const setReferralDetails = (data: ReferralDetailsType[]): CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.SET_REFERRAL_DETAILS,
    payload: { data },
  });

export const saveReferralDetails = 
  (data: ScheduledFollowUpDetails): CheckoutAppointmentsAction => 
    ({ 
      type: CVCheckoutAppointmentsType.SAVE_REFERRAL_DETAILS,
      payload: { data },
    });

export const setSelectedProviderInReferral = 
  (data: {doctor: ProviderDetailsType, type: string}): CheckoutAppointmentsAction => 
    ({
      type: CVCheckoutAppointmentsType.SET_SELECTED_PROVIDER_IN_REFERRAL,
      payload: { data },
    });

export const getReferralProviderDetails = 
  (lat: string, lng:string, referralVisit: string): CheckoutAppointmentsAction => 
    ({
      type:CVCheckoutAppointmentsType.GET_REFERRAL_PROVIDER_DETAILS,
      payload: { lat, lng, referralVisit },
    });

export const setReferralProviderDetails =
  (data: ChooseProviderDetailsType[]): CheckoutAppointmentsAction => 
    ({
      type: CVCheckoutAppointmentsType.SET_REFERRAL_PROVIDER_DETAILS,
      payload: { data },
    });

export const setScheduledReferralDetails =
  (data: ScheduledFollowUpDetails): CheckoutAppointmentsAction => 
    ({
      type: CVCheckoutAppointmentsType.SET_SCHEDULED_REFERRAL_DETAILS,
      payload: { data },
    });

export const setScheduledImagingTestDetails =
  (data: ScheduledImagingTestDetails): CheckoutAppointmentsAction => 
  
    ({
      type: CVCheckoutAppointmentsType.SET_SCHEDULED_IMAGING_TEST_DETAILS,
      payload: { data },
    });
export const getLabTestDetails = 
(patientId: string, appointmentId: string, notInitialLoad?: boolean): CheckoutAppointmentsAction =>
  ({ 
    type: CVCheckoutAppointmentsType.GET_LAB_TEST_DETAILS,
    payload: { patientId, notInitialLoad, appointmentId },
  });

export const setLabTestDetails = (data: LabOrImagingTestDetailsType): CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.SET_LAB_TEST_DETAILS,
    payload: { data },
  });

export const getLabsLocation = (lat: string, lng: string): CheckoutAppointmentsAction => 
  ({
    type: CVCheckoutAppointmentsType.GET_LABS_LOCATION,
    payload: { lat, lng },
  });

export const setLabsLocation = (data: Location[] | null): CheckoutAppointmentsAction => 
  ({
    type: CVCheckoutAppointmentsType.SET_LABS_LOCATION,
    payload: { data },
  });

export const saveLabTestDetails = (data: SaveLabDetailsRequestType): CheckoutAppointmentsAction => 
  ({ 
    type: CVCheckoutAppointmentsType.SAVE_LAB_TEST_DETAILS,
    payload: { data },
  });

export const setScheduledLabTestDetails = 
  (data: SaveLabDetailsRequestType): CheckoutAppointmentsAction => 
    ({ 
      type: CVCheckoutAppointmentsType.SET_SCHEDULED_LAB_TEST_DETAILS,
      payload: { data },
    });

export const getImagingTestDetails = 
(patientId: string, appointmentId:string, notInitialLoad?: boolean): CheckoutAppointmentsAction =>
  ({ 
    type: CVCheckoutAppointmentsType.GET_IMAGING_TEST_DETAILS,
    payload: { patientId, appointmentId, notInitialLoad },
  });

export const setImagingTestDetails = (data: LabOrImagingTestDetailsType): 
CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.SET_IMAGING_TEST_DETAILS,
    payload: { data },
  });

export const saveImagingTestDetails = 
(data: ScheduledImagingTestDetails): CheckoutAppointmentsAction => 
  ({ 
    type: CVCheckoutAppointmentsType.SAVE_IMAGING_TEST_DETAILS,
    payload: { data },
  });

export const getImagingLocation = (patientId: string): CheckoutAppointmentsAction => 
  ({
    type: CVCheckoutAppointmentsType.GET_IMAGING_LOCATION,
    payload: { patientId },
  });

export const setImagingLocation = (data: Location[] | null): CheckoutAppointmentsAction => 
  ({
    type: CVCheckoutAppointmentsType.SET_IMAGING_LOCATION,
    payload: { data },
  });

export const getPrescriptionDetails = 
(patientId: string, appointmentId: string, notInitialLoad?: boolean): CheckoutAppointmentsAction =>
  ({ 
    type: CVCheckoutAppointmentsType.GET_PRESCRIPTION_DETAILS,
    payload: { patientId, appointmentId, notInitialLoad },
  });

export const setPrescriptionDetails = (data: FollowUpDetailsType[]): CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.SET_PRESCRIPTION_DETAILS,
    payload: { data },
  });

export const savePrescriptionDetails = (data: FollowUpDetailsType): CheckoutAppointmentsAction => 
  ({ 
    type: CVCheckoutAppointmentsType.SAVE_PRESCRIPTION_DETAILS,
    payload: { data },
  });

export const getScheduledData = (appointmentId: string) : CheckoutAppointmentsAction => 
  ({
    type: CVCheckoutAppointmentsType.GET_SCHEDULED_DATA,
    payload: { appointmentId },
  });

export const setAllScheduledFollowUpAppointments = 
  (data: ScheduledFollowUpDetails[]) : CheckoutAppointmentsAction => 
    ({
      type: CVCheckoutAppointmentsType.SET_ALL_SCHEDULED_FOLLOW_UP_DETAILS,
      payload: { data },
    });

export const setAllScheduledReferralAppointments = 
  (data: ScheduledFollowUpDetails[]) : CheckoutAppointmentsAction => 
    ({
      type: CVCheckoutAppointmentsType.SET_ALL_SCHEDULED_REFERRAL_DETAILS,
      payload: { data },
    });

export const setScheduledReferralProviderDetails = 
  (data: ChooseProviderDetailsType) : CheckoutAppointmentsAction => 
    ({
      type: CVCheckoutAppointmentsType.SET_SCHEDULED_REFERRAL_PROVIDER_DETAILS,
      payload: { data },
    });

export const getLocationsList = (orderType: string, patientAddress: string, testsList: string[]): 
  CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.GET_LOCATIONS_LIST,
    payload: { orderType, patientAddress, testsList },
  });

export const setLocationsList = (data: Location[]): CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.SET_LOCATIONS_LIST,
    payload: { data },
  });
export const getWeekDayTimeSlots = 
  (data: GetTimeSlotsRequestDataType): CheckoutAppointmentsAction => 
    ({
      type: CVCheckoutAppointmentsType.GET_WEEK_DAY_TIME_SLOTS,
      payload: { data },
    });
  
export const setWeekDayTimeSlots = 
  (data: DateTimeSlotsType[] | [] | null): CheckoutAppointmentsAction => 
    ({
      type: CVCheckoutAppointmentsType.SET_WEEK_DAY_TIME_SLOTS,
      payload: { data },
    });

export const setIsTimeSlotSkeonShown = (isShown: boolean): CheckoutAppointmentsAction => 
  ({
    type: CVCheckoutAppointmentsType.SET_IS_TIME_SLOT_SKELETON_SHOWN,
    payload: { isShown },
  });
export const setProviderForAllScheduledReferrals = 
  (provider: ChooseProviderDetailsType[]): CheckoutAppointmentsAction =>
    ({
      type: CVCheckoutAppointmentsType.SET_PROVIDER_FOR_ALL_SCHEDULED_REFERRALS,
      payload: { provider },
    });

export const getPreferredPharmacyDetails = (patientId: string, notInitialLoad?: boolean):
 CheckoutAppointmentsAction =>
  ({ 
    type: CVCheckoutAppointmentsType.GET_PREFERRED_PHARMACY_DETAILS,
    payload: { patientId, notInitialLoad },
  });
  
export const setPreferredPharmacyDetails = 
  (data: PreferredPharmacyDetailsType): CheckoutAppointmentsAction =>
    ({
      type: CVCheckoutAppointmentsType.SET_PREFERRED_PHARMACY_DETAILS,
      payload: { data },
    });

export const setFollowUpDate = (data:Date): CheckoutAppointmentsAction =>
  ({
    type: CVCheckoutAppointmentsType.SET_FOLLOWUP_DATE,
    payload: { data },
  });  
