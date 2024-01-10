import { Practitioner } from './member';
import { Site } from './site';

export type FollowUpDetailsType = {
  id: string;
  humanName: {
    familyName: string;
    givenNames?: string[];
    suffixes?: string[];
  };
  photo:{
    contentType: string;
    url?: string;
  };
  specialty: string;
  location: {
    id: string,
    address: {
      addressLine: string;
      addressLine2: string;
      city: string;
      state: string;
      zipCode: string;
    };
    geographicCoordinates: { 
      latitude: number;
      longitude: number; 
    };
    phoneNumber: string;
    distance: number;
  };
  scheduledSlot?:{
    appointmentType: string;
    appointmentFormat: string;
    date: Date;
    timeSlot: string;
  }
}

export type ScheduledFollowUpDetails = {
  id?: number,
  provider_ID: string,
  patient_ID: string,
  aptType?: string,
  aptFormat?: string,
  location_ID: number,
  aptScheduleDate: string,
  bookingSlot: string,
  refVisitType?: string,
  created_Date: string,
  type: string,
  appointment_Id: string,
  skipped?: boolean,
  department_Id?: string,
  visitTypeId?: string,
}

export type ScheduledImagingTestDetails = {
  provider_ID?: string,
  patient_ID?: string,
  location_ID?: string,
  appointment_ID?: string,
  imagingType?: string,
  imagingLocation?: string,
  bookingSlot?: string,
  aptScheduleDate?: string,
  skipped?: boolean,
}

export type ScheduleFollowUpAppointmentData = {
  date: Date;
  time: string;
  location: string;
  pincode: string;
  appointmentType: number;
  meetingType: string;
};

export type ScheduledReferralDetailsType = {
  id: string;
  humanName: {
    familyName: string;
    givenNames?: string[];
    suffixes?: string[];
  };
  photo:{
    contentType: string;
    url?: string;
  };
  specialty: string;
  location: {
    id: string,
    address: {
      addressLine: string;
      addressLine2: string;
      city: string;
      state: string;
      zipCode: string;
    };
    geographicCoordinates: { 
      latitude: number;
      longitude: number; 
    };
    phoneNumber: string;
    distance: number;
  };
  referralVisit: string;
  scheduledSlot?: {
    date: Date;
    timeSlot: string;
    duration: string;
  }
};

export type ProviderDetailsType = {
  id: string;
  providerId: string;
  humanName: {
    familyName: string;
    givenNames?: string[];
    suffixes?: string[];
  };
  photo:{
    contentType: string;
    url?: string;
  };
  specialty: string;
  location: {
    id: string,
    address: {
      addressLine: string;
      addressLine2: string;
      city: string;
      state: string;
      zipCode: string;
    };
    geographicCoordinates: {
      latitude: number;
      longitude: number;
    };
    phoneNumber: string;
    distance: number;
  };
  department_Id?: string;
};

export type ChooseProviderDetailsType = {
  id: number;
  providerId: string;
  providerName:string;
  providerDisplayName:string;
  providerPhotoURL:string;
  locationLatitudeCoordinate:string;
  locationLongitudeCoordinate:string;
  city:string;
  providerSpecialty:string;
  distance:number;
  locationId: string;
  locationName: string;
  locationAdd_1: string;
  locationState: string;
  locationZip: string;
  department_Id: string;
};

export type ReferralDetailsType = {
  providerType: string,
  providerDetails?: ProviderDetailsType,
};

export type SaveLabDetailsResponseType = {
  recordID: number,
  creationDate: string,
  errorMessage: string,
  statusCode: string
}

export type SaveLabDetailsRequestType = {
  id?: number,
  type: string,
  lab_ID: string,
  lab_Location: string,
  lab_Name: string,
  createdDate: string,
  appointment_ID: string,
  patient_ID: string,
  provider_ID?: string,
  location_ID?: string,
  imagingType?: string,
  imagingLocation?: string,
  bookingSlot?: string,
  aptScheduleDate?: string,
  skipped?: boolean,
}

export type SaveFollowUpDetailsResponseType = {
  recordID: number,
  creationDate: string,
  message: string,
  errorMessage: string,
  statusCode: string
}

export type GetScheduledAppointmentsType = {
  labDetailsList: SaveLabDetailsRequestType[] | [],
  scheduleFollowUpList: ScheduledFollowUpDetails[] | [],
}

export type PreferredPharmacyDetailsType = {
  city: string | undefined,
  pharmacyName: string,
  addressLine1: string,
  addressLine2: string,
  state: string,
  phoneNumber: string,
  closingTime: string,
  zipCode: string,
  pharmacyID: number,
}

export type StepsList = {
  isSkeletonShown: boolean,
  alias: string,
}

export type FollowUpOrderType = {
  location: Site,
  practitioner: Practitioner,
} 
