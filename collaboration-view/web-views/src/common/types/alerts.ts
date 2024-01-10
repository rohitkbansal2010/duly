import { AlertStatus } from '@enums';
import { PatientsPD } from '@interfaces';

export type Alerts = {
  patient: PatientsPD;
  date: string;
  preview: string;
  status: AlertStatus;
};
