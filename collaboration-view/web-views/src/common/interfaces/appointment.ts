interface HumanName {
  familyName: string;
  givenNames?: string[];
}

export type PatientHumanName = HumanName

export interface PractitionerHumanName extends HumanName {
  prefixes?: string[];
}
