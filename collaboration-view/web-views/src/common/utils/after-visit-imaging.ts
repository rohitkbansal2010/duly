import { FollowUpOrderType, ProviderDetailsType } from '../types/cv-checkout-appointments';
import { ImagingDetailsType } from '../types/labs-test';

import { writeHumanName } from './person';

export const getPractitionersDetails = (details: FollowUpOrderType) : ProviderDetailsType => 
  ({
    id: details.practitioner.id,
    providerId: details.practitioner.id,
    humanName: details.practitioner.humanName,
    location: {
      id: details.location.id,
      address: {
        addressLine: details.location.address?.line ? details.location.address?.line : '',
        addressLine2: '',
        city: details.location.address?.city ? details.location.address?.city : '',
        state: details.location.address?.state ? details.location.address?.state : '',
        zipCode: details.location.address?.postalCode ? details.location.address?.postalCode : '',
      },
      geographicCoordinates: {
        latitude: 0,
        longitude: 0,
      },
      phoneNumber: '',
      distance: 0,
    },
    photo: {
      contentType: details.practitioner.photo?.contentType ? details.practitioner.photo?.contentType : '',
      url: details.practitioner.photo?.url,
    },
    specialty: 'PHYSICIAN',
  });

export const getImagingTestDate = (date: Date | undefined): string => 
  date ? date?.toDateString() : '';

export const getImagingPractitionerName = (
  imagingData: ImagingDetailsType[], 
  step: number,
  allTests?: string[] | undefined
): string => {
  let givenNames: string[] | undefined = undefined;
  let familyName = '';

  imagingData.forEach((data) => {
    data.tests.forEach((test) => {
      if(allTests && test === allTests[step]){
        givenNames = data.providerDetails.humanName.givenNames;
        familyName = data.providerDetails.humanName.familyName;
      }
    });
  });
  return writeHumanName(givenNames, familyName);
};

export const getImagingTestByStep = (step: number, allTests: string[] | undefined): string => 
  allTests ? allTests[step] : '';
