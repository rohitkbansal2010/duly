import {
  CareTeamMemberRole, Gender, PractitionerRole, UserRoles 
} from '@enums';
import { PatientHumanName, PractitionerHumanName } from '@interfaces';

import { Photo, Practitioner } from './member';

export type PersonalData = {
  id: number;
  firstName: string;
  secondName?: string;
  lastName: string;
  avatarSmall: string;
  avatarLarge?: string;
  birthDate: string;
  gender: Gender;
  userRole: UserRoles;
};

export type PatientGeneralInfo = {
  id: string;
  humanName: PatientHumanName;
  photo?: Photo;
}

export type PatientPersonalData = {
  patientGeneralInfo: PatientGeneralInfo;
  isNewPatient: boolean;
};

export type PractitionerPersonalData = {
  id: string;
  humanName: PractitionerHumanName;
  role: PractitionerRole | CareTeamMemberRole;
};

export type UserData = Practitioner;
