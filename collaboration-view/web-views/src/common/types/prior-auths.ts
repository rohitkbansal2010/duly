import { CareAlliesPD, PatientsPD } from '@interfaces';

export type PriorAuths = {
  patient: PatientsPD;
  date: string;
  reasons: string[];
  assignedTo: CareAlliesPD;
};
