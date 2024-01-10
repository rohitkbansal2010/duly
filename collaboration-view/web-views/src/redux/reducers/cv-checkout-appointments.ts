import { StepsListData } from '@mock-data';
import { UIActions, UIType } from '@redux/actions';
import { CheckoutAppointmentsAction, CVCheckoutAppointmentsType } from '@redux/actions/cv-checkout-appointments';
import {
  FollowUpDetailsType,
  ScheduledFollowUpDetails,
  ChooseProviderDetailsType,
  SaveLabDetailsRequestType,
  ReferralDetailsType,
  ScheduledImagingTestDetails,
  Location,
  DateTimeSlotsType,
  PreferredPharmacyDetailsType,
  StepsList,
  FollowUpOrderType,
  LabOrImagingTestDetailsType
} from '@types';
import { getDoctorDetails } from '@utils';

export type CheckoutAppointmentsStateType = {
  stepsList: StepsList[];
  isFollowUpScheduled: boolean,
  isFollowUpSkipped: boolean,
  isReferralScheduled: boolean,
  isReferralSkipped: boolean,
  isLabTestScheduled: boolean,
  isLabTestSkipped: boolean,
  isImagingTestScheduled: boolean,
  isImagingTestSkipped: boolean,
  isPrescriptionsChecked: boolean,
  followUpOrderDetails: FollowUpOrderType | null;
  scheduledFollowUpDetails: ScheduledFollowUpDetails[] | null;
  referralDetails: ReferralDetailsType[] | null;
  referralProviderDetailsList: ChooseProviderDetailsType[] | null;
  scheduledReferralDetails: ScheduledFollowUpDetails[] | null;
  scheduledImagingTestDetails: ScheduledImagingTestDetails[] | null;
  labTestDetails: LabOrImagingTestDetailsType | null;
  scheduledLabTestDetails: SaveLabDetailsRequestType | null;
  imagingTestDetails: LabOrImagingTestDetailsType | null;
  prescriptionDetails: FollowUpDetailsType[] | null;
  weekDayTimeSlots: DateTimeSlotsType[] | [] | null;
  loading: boolean | null;
  locationLists: Location[] | null;
  isTimeSlotSkeletonShown: boolean;
  scheduledReferralProviderDetails: ChooseProviderDetailsType | null;
  preferredPharmacyDetails: PreferredPharmacyDetailsType | null;
  labsLocation: Location[] | null;
  imagingLocation: Location[] | null;
  selectedFollowUpDate:Date;
}

export const cvCheckoutAppointmentsState: CheckoutAppointmentsStateType = {
  stepsList: StepsListData(false),
  isFollowUpScheduled: false,
  isFollowUpSkipped: false,
  isReferralScheduled: false,
  isReferralSkipped: false,
  isLabTestScheduled: false,
  isLabTestSkipped: false,
  isImagingTestScheduled: false,
  isImagingTestSkipped: false,
  isPrescriptionsChecked: false,
  followUpOrderDetails: null,
  scheduledFollowUpDetails: null,
  referralDetails: null,
  referralProviderDetailsList: null,
  scheduledReferralDetails: null,
  scheduledImagingTestDetails: null,
  labTestDetails: null,
  labsLocation: null,
  imagingLocation: null,
  scheduledLabTestDetails: null,
  imagingTestDetails: null,
  prescriptionDetails: null,
  loading: null,
  locationLists: null,
  weekDayTimeSlots: null,
  scheduledReferralProviderDetails: null,
  isTimeSlotSkeletonShown: false,
  preferredPharmacyDetails: null,
  selectedFollowUpDate:new Date(),
};

const setStepList = (
  state: CheckoutAppointmentsStateType,
  action: any
) =>
  ({
    ...state,
    stepsList: state.stepsList.map((step) => {
      action.payload.data.forEach((dataValue: any) => {
        if(step.alias === dataValue.alias) {
          step.isSkeletonShown = dataValue.isSkeletonShown;
        }
      });
      return step;
    }),
  });

const setScheduledFollowUpDetails = (
  state: CheckoutAppointmentsStateType,
  action: any
) =>
  ({
    ...state,
    scheduledFollowUpDetails: state.scheduledFollowUpDetails
      ? [
        ...state.scheduledFollowUpDetails.filter(detail =>
          action.payload.data.appointment_Id !== detail.appointment_Id),
        action.payload.data,
      ]
      : [ action.payload.data ],
  });

const setScheduledReferralDetails = (
  state: CheckoutAppointmentsStateType,
  action: any
) =>
  ({
    ...state,
    scheduledReferralDetails: state.scheduledReferralDetails
      ? [
        ...state.scheduledReferralDetails.filter(detail =>
          action.payload.data.refVisitType !== detail.refVisitType),
        action.payload.data,
      ]
      : [ action.payload.data ],
  });

const setScheduledImagingTestDetails = (
  state: CheckoutAppointmentsStateType,
  action: any
) =>
  ({
    ...state,
    scheduledImagingTestDetails: state.scheduledImagingTestDetails
      ? [
        ...state.scheduledImagingTestDetails.filter(detail =>
          action.payload.data.imagingType !== detail.imagingType),
        action.payload.data,
      ]
      : [ action.payload.data ],
  });

const setProviderForAllScheduledReferral = (
  state: CheckoutAppointmentsStateType,
  action: any
) =>
  ({
    ...state,
    referralDetails: state.referralDetails
      ? state.referralDetails.map((referral) => {
        action.payload.provider.forEach((data: any) => {
          if (!referral.providerDetails &&
            (referral.providerType.toLowerCase() == data.providerSpecialty.toLowerCase())) {
            referral.providerDetails = getDoctorDetails(data);
          }
        });
        return referral;
      })
      : null,
  });

const setSelectedProviderInReferral = (
  state: CheckoutAppointmentsStateType,
  action: any
) =>
  ({
    ...state,
    referralDetails: state.referralDetails
      ? state.referralDetails.map((referral) => {
        if(referral.providerType === action.payload.data.type){
          return {
            providerType: referral.providerType,
            providerDetails: action.payload.data.doctor,
          };
        }
        return referral;
      })
      : null,
  });
export const cvCheckoutAppointmentsReducer = (
  state = cvCheckoutAppointmentsState,
  action: CheckoutAppointmentsAction | UIActions
): CheckoutAppointmentsStateType => {
  switch (action.type) {
    case CVCheckoutAppointmentsType.SET_STEPS_LIST:
      return setStepList(state, action);
    case CVCheckoutAppointmentsType.SET_FOLLOW_UP_SCHEDULED:
      return {
        ...state,
        isFollowUpScheduled: true,
      };
    case CVCheckoutAppointmentsType.SET_FOLLOW_UP_SKIPPED:
      return {
        ...state,
        isFollowUpSkipped: true,
      };
    case CVCheckoutAppointmentsType.SET_REFERRAL_SCHEDULED:
      return {
        ...state,
        isReferralScheduled: true,
      };
    case CVCheckoutAppointmentsType.SET_REFERRAL_SKIPPED:
      return {
        ...state,
        isReferralSkipped: true,
      };
    case CVCheckoutAppointmentsType.SET_LABS_TEST_SCHEDULED:
      return {
        ...state,
        isLabTestScheduled: true,
      };
    case CVCheckoutAppointmentsType.SET_LABS_TEST_SKIPPED:
      return {
        ...state,
        isLabTestSkipped: true,
      };
    case CVCheckoutAppointmentsType.SET_LABS_TEST_NOT_SKIPPED:
      return {
        ...state,  
        isLabTestSkipped: false,
      };
    case CVCheckoutAppointmentsType.SET_IMAGING_TEST_SCHEDULED:
      return {
        ...state,
        isImagingTestScheduled: true,
      };
    case CVCheckoutAppointmentsType.SET_IMAGING_TEST_SKIPPED:
      return {
        ...state,
        isImagingTestSkipped: true,
      };
    case CVCheckoutAppointmentsType.SET_PRESCRIPTIONS_CHECKED:
      return {
        ...state,
        isPrescriptionsChecked: true,
      };
    case CVCheckoutAppointmentsType.SET_FOLLOW_UP_ORDER:
      return {
        ...state,
        followUpOrderDetails: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_SCHEDULED_FOLLOW_UP_DETAILS:
      return setScheduledFollowUpDetails(state, action);

    case CVCheckoutAppointmentsType.SET_REFERRAL_DETAILS:
      return {
        ...state,
        referralDetails: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_LAB_TEST_DETAILS:
      return {
        ...state,
        labTestDetails: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_LABS_LOCATION:
      return{
        ...state,
        labsLocation: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_SCHEDULED_LAB_TEST_DETAILS:
      return {
        ...state,
        scheduledLabTestDetails: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_IMAGING_TEST_DETAILS:
      return {
        ...state,
        imagingTestDetails: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_PRESCRIPTION_DETAILS:
      return {
        ...state,
        prescriptionDetails: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_IMAGING_LOCATION:
      return{
        ...state,
        imagingLocation: action.payload.data,
      };
    case UIType.START_DATA_FETCH:
      return {
        ...state,
        followUpOrderDetails: null,
        referralDetails: null,
        prescriptionDetails: null,
        imagingTestDetails: null,
        labTestDetails: null,
      };
    case CVCheckoutAppointmentsType.SET_REFERRAL_PROVIDER_DETAILS:
      return {
        ...state,
        referralProviderDetailsList: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_SCHEDULED_REFERRAL_DETAILS:
      return setScheduledReferralDetails(state, action);
    case CVCheckoutAppointmentsType.SET_SCHEDULED_IMAGING_TEST_DETAILS:
      return setScheduledImagingTestDetails(state, action);

    case CVCheckoutAppointmentsType.SET_SELECTED_PROVIDER_IN_REFERRAL:
      return setSelectedProviderInReferral(state, action);

    case CVCheckoutAppointmentsType.START_LOADING:
      return {
        ...state,
        loading: true,
      };
    case CVCheckoutAppointmentsType.STOP_LOADING:
      return {
        ...state,
        loading: false,
      };
    case CVCheckoutAppointmentsType.RESET_LOADING:
      return {
        ...state,
        loading: null,
      };
    case CVCheckoutAppointmentsType.SET_ALL_SCHEDULED_FOLLOW_UP_DETAILS:
      return {
        ...state,
        scheduledFollowUpDetails: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_ALL_SCHEDULED_REFERRAL_DETAILS:
      return {
        ...state,
        scheduledReferralDetails: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_SCHEDULED_REFERRAL_PROVIDER_DETAILS:
      return {
        ...state,
        scheduledReferralProviderDetails: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_LOCATIONS_LIST:
      return {
        ...state,
        locationLists: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_WEEK_DAY_TIME_SLOTS:
      return {
        ...state,
        weekDayTimeSlots: action.payload.data,
      };
    case CVCheckoutAppointmentsType.SET_IS_TIME_SLOT_SKELETON_SHOWN:
      return {
        ...state,
        isTimeSlotSkeletonShown: action.payload.isShown,
      };
    case CVCheckoutAppointmentsType.SET_PROVIDER_FOR_ALL_SCHEDULED_REFERRALS:
      return setProviderForAllScheduledReferral(state, action);

    case CVCheckoutAppointmentsType.SET_PREFERRED_PHARMACY_DETAILS:
      return {
        ...state,
        preferredPharmacyDetails: action.payload.data,
      };

    case CVCheckoutAppointmentsType.SET_FOLLOWUP_DATE:
      return {
        ...state,
        selectedFollowUpDate: action.payload.data,
      };  
    default:
      return state;
  }
};
