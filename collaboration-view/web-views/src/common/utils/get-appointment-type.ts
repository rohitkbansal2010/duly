import { appointmentType } from '@components/start-checkout/checkout-details-bar';

import { ChooseProviderDetailsType, ProviderDetailsType } from '../types/cv-checkout-appointments';

export const getAppointmentType = (value: number): string => {
  let appointmentTypeValue = '';
  appointmentType.forEach((appointment) => {
    if(appointment.val === value){
      appointmentTypeValue = appointment.name;
    }
  });
  return appointmentTypeValue;
};

export const getDoctorDetails = (doctor: ChooseProviderDetailsType): ProviderDetailsType => 
  ({
    id: doctor.id.toString(),
    providerId: doctor.providerId,
    humanName: {
      familyName: doctor.providerDisplayName,
      givenNames: doctor.providerName.split(', '),
    },
    photo:{
      contentType: '',
      url: doctor.providerPhotoURL,
    },
    specialty: doctor.providerSpecialty,
    location: {
      id: doctor.locationId,
      address: {
        addressLine: doctor.locationAdd_1,
        addressLine2: '',
        city: doctor.city,
        state: doctor.locationState,
        zipCode: doctor.locationZip,
      },
      geographicCoordinates: {
        latitude: parseFloat(doctor.locationLatitudeCoordinate),
        longitude: parseFloat(doctor.locationLongitudeCoordinate),
      },
      phoneNumber: '',
      distance: doctor.distance,
    },
  });
