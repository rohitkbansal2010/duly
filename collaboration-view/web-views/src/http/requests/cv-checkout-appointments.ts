import { HttpMethod } from '@enums';
import { CheckoutAppointmentService, EncouterServices } from '@http/api';
import { LocationData } from '@mock-data';
import {
  ChooseProviderDetailsType, 
  FollowUpDetailsType, 
  GetScheduledAppointmentsType, 
  SaveFollowUpDetailsResponseType, 
  SaveLabDetailsRequestType, 
  SaveLabDetailsResponseType, 
  ScheduledFollowUpDetails,
  DateTimeSlotsType,
  ScheduledImagingTestDetails,
  PreferredPharmacyDetailsType,
  GetTimeSlotsAPIResponseType,
  LabOrImagingTestDetailsType,
  NearbyLabLocation,
  MedicationsWidgetDataType
} from '@types';
import { httpRequest } from '@utils';

export const getFollowUpDetails = (patientId: string):
  Promise<FollowUpDetailsType[] | undefined> =>
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/v1/appointment/${patientId}/follow-up-details`,
    method: HttpMethod.GET,
  });

export const getReferralDetails = (patientId: string):
  Promise<{providerType: string}[] | undefined> =>
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/v1/appointment/${patientId}/referral-details`,
    method: HttpMethod.GET,
  });

export const getProviderDetails = 
  ({ lat, lng, referralVisit }: { lat: string, lng: string, referralVisit: string})
    : Promise<ChooseProviderDetailsType[] | undefined> => 
    httpRequest({
      url: `${EncouterServices.CHECKOUT_APPOINTMENT}/api/${CheckoutAppointmentService.PROVIDER_BY_LAT_LNG}/${lat}/${lng}/${referralVisit}`,
      method: HttpMethod.GET,
    });

export const getLabTestDetails = (patientId: string, appointmentId:string): 
Promise<LabOrImagingTestDetailsType | undefined> =>
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/ServiceRequest/${patientId}/appointment/${appointmentId}/type/Labs`,
    method: HttpMethod.GET,
  });

export const getLabsLocation = (lat: string, lng: string): 
Promise<NearbyLabLocation[] | undefined> => 
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/labdetail/LabDetail/ByLatLng/${lat}/${lng}`,
    method: HttpMethod.GET,
  });

export const saveLabDetails = 
  (data: SaveLabDetailsRequestType): Promise<SaveLabDetailsResponseType | undefined> =>
    httpRequest({
      url: `${EncouterServices.CHECKOUT_APPOINTMENT}/${CheckoutAppointmentService.LABS}`,
      method: HttpMethod.POST,
      data,
    });

export const saveFollowUpAppointment = 
  (data: ScheduledFollowUpDetails): Promise<SaveFollowUpDetailsResponseType | undefined> => 
    httpRequest({
      url:`${EncouterServices.CHECKOUT_APPOINTMENT}/${CheckoutAppointmentService.FOLLOW_UP}`,
      method: HttpMethod.POST,
      data,
    });

export const saveReferralDetails = (data: ScheduledFollowUpDetails) => 
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/${CheckoutAppointmentService.REFERRAL}`,
    method: HttpMethod.POST,
    data,
  });

export const saveImagingTestDetails = (data: ScheduledImagingTestDetails) => 
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/${CheckoutAppointmentService.IMAGING}`,
    method: HttpMethod.POST,
    data,
  });

export const getScheduledDetails = (appointmentId: string): 
  Promise<GetScheduledAppointmentsType | undefined> => 
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/${appointmentId}/CheckOutDetails`,
    method: HttpMethod.GET,
  });

export const getImagingDetails = (patientId: string, appointmentId: string): 
Promise<LabOrImagingTestDetailsType | undefined> =>
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/ServiceRequest/${patientId}/appointment/${appointmentId}/type/Imaging`,
    method: HttpMethod.GET,
  });

export const getLocationsList = (orderType: string, patientAddress:string, testsList: string[]) => {
  console.log(orderType, testsList, patientAddress);
  return LocationData;
};

export const getWeekDayTimeSlots = (date: Date):
  Promise<DateTimeSlotsType[] | undefined> =>
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/v1/practitioner/${date.getDate()}/slots`,
    method: HttpMethod.GET,
  });

export const getAvailableSlotsOfProvider = (
  date: string,
  visitTypeId: string, 
  appointmentId: string
):
  Promise<GetTimeSlotsAPIResponseType[] | undefined> =>
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/${visitTypeId}/appointmentId/${appointmentId}/TimeSlotsOpen?startDate=${date}&endDate=${date}`,
    method: HttpMethod.GET,
  });

export const getReferralSlotsOfProvider = ({
  date, providerId, visitTypeId, departmentId, 
}:
  { date: string,
  providerId: string,
  visitTypeId: string,
  departmentId: string }):
  Promise<GetTimeSlotsAPIResponseType[] | undefined> => 
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/${visitTypeId}/departmentId/${departmentId}/providerId/${providerId}/TimeSlotsOpen?startDate=${date}&endDate=${date}`,
    method: HttpMethod.GET,
  });

export const getScheduledReferralProviderDetails = (providerIds: number[]):
  Promise<ChooseProviderDetailsType[] | undefined> =>
  httpRequest({
    url: `${EncouterServices.CHECKOUT_APPOINTMENT}/api/Providers?providerIds=${providerIds.toString()}`,
    method: HttpMethod.GET,
  });

export const getPreferredPharmacy = 
  (patientId: string): Promise<PreferredPharmacyDetailsType | undefined> => 
    httpRequest({
      url: `${EncouterServices.CHECKOUT_APPOINTMENT}/Pharmacy/${patientId}`,
      method: HttpMethod.GET,
    });

export const getPrescribedMedicineDetails = 
  (patientId: string, appointmentId: string): Promise<MedicationsWidgetDataType | undefined> =>
    httpRequest({
      url: `${EncouterServices.CHECKOUT_APPOINTMENT}/patientId/${patientId}/appointmentId/${appointmentId}`,
      method: HttpMethod.GET,
    }); 
