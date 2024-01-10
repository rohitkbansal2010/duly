import { formatAddressBottomLine } from '@utils';

import { LabOrImagingTestDetailsType } from '../types/labs-test';
import { PatientData } from '../types/patient-data';

export const getPatientAddressForLabs = (patientData: PatientData | null): string =>{
  let patientAddressValue = '400 S.Eagle Street, NaperVille, IL 60302';
  if(patientData?.patientAddress){    
    patientData.patientAddress.forEach((address)=>{
      if(address.use === 'Home'){
        patientAddressValue = `${address.line[0]}, ${formatAddressBottomLine({ 
          city:address.city, 
          postalCode: address.postalCode, 
          state: address.state, 
        })}`;
      }
    });
  }
  return patientAddressValue;
};

export const showSkeletonForLabs = (skeleton: boolean, loading: boolean | null): boolean => 
  skeleton || !!loading;

export const isLabOrderCountZero = (labTestDetails: LabOrImagingTestDetailsType): boolean => 
  labTestDetails && labTestDetails.orderCount === 0;

export const getMilesForLocation = (distance: number):string => {
  if(distance === 0) {
    return 'Less than 1';
  }
  return distance.toString();
};

export const getPatientLineAddressForLabs = (patientData: PatientData | null) => 
  patientData?.patientAddress 
  && patientData?.patientAddress[0].line && patientData?.patientAddress[0].line[0];
