import { AlertStatus, Level } from '@enums';
import { PatientsPD } from '@interfaces';

export type AlertTasks = {
  patient: PatientsPD;
  preview: string;
  time: string;
  status?: AlertStatus;
  priority?: Level;
};
