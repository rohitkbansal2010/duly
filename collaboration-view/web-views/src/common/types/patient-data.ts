import { PatientGeneralInfo, Photo } from '@types';

export type PatientAddressType = {
  city:string;
  country:string;
  district:string;
  line:string[];
  postalCode:string;
  state:string;
  use:string;
}

export type PatientData = {
  generalInfo: PatientGeneralInfo;
  birthDate: string;
  gender: string;
  photo: Photo;
  patientAddress: PatientAddressType[];
}
