import { PatientAppointmentAlias, PatientAppointmentStatus } from '@enums';

import { Practitioner } from './member';

export type PatientAppointment = {
  practitioner: Practitioner;
  isTelehealth?: boolean;
  reason: string;
  status: PatientAppointmentStatus;
  startTime: string;
  minutesDuration?: number;
};

export type PatientAppointmentsGroup = {
  title: string;
  appointments: PatientAppointment[];
  nearestAppointmentPractitioner: Practitioner;
  nearestAppointmentDate: string;
  farthestAppointmentDate?: string;
};

export type PatientAppointments = {
  [key in PatientAppointmentAlias]: PatientAppointmentsGroup[];
};
