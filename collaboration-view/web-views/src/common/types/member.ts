import { CareTeamMemberRole, PractitionerRole } from '@enums';

export type HumanName = {
  familyName: string;
  givenNames: string[];
  prefixes?: string[];
};

export type Photo = {
  contentType: string;
  title: string;
  size: number;
  data?: string;
  url?: string;
};

export type Role = {
  title: PractitionerRole;
};

export interface Member {
  id: string;
  humanName: HumanName;
  photo?: Photo;
}

export interface Practitioner extends Member {
  role?: PractitionerRole | CareTeamMemberRole;
}

export interface CareTeamMember extends Member {
  memberType: CareTeamMemberRole;
}

export type CommonPractitionerType = Practitioner | CareTeamMember;

export type ExtendedCommonPractitionerType = 
	CommonPractitionerType & {isCurrentUser?: boolean };
