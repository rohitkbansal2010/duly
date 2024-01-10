import { Level } from '@enums';
import { PatientsPD } from '@interfaces';

export type Tasks = {
  patient: PatientsPD;
  date: string;
  preview: string;
  priority: Level;
};
