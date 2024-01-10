import { AppointmentStatus, AppointmentType } from '@enums';

import { Practitioner } from './member';
import { PatientPersonalData } from './personal-data';

export type AppointmentData = {
  id: string;
  title: string;
  type: AppointmentType;
  status: AppointmentStatus;
  timeSlot: {
    startTime: string;
    endTime: string;
  };
  patient: PatientPersonalData;
  practitioner: Practitioner;
  isProtectedByBtg: boolean;
};

export type DailyStatistics = {
  newPatients: number;
};

export type GroupedAppointmentType = {
  startTime: number;
  endTime: number;
  appointments: AppointmentData[];
};

export type GetAppointments = {
  siteId: string;
  startDate: string;
  endDate: string;
};
