import { AlertStatus } from '@enums';
import { Alerts } from '@types';

import { pickPatientPDById } from './personal-data';

export const alerts: Alerts[] = [
  {
    patient: pickPatientPDById(101),
    date: 'Generated Today 11:15 am',
    preview: 'Possible care barrier, risk to adherence',
    status: AlertStatus.NEW,
  },
  {
    patient: pickPatientPDById(102),
    date: 'Generated Today 10:45 am',
    preview: 'Significant decrease in SpO2',
    status: AlertStatus.NEW,
  },
  {
    patient: pickPatientPDById(103),
    date: 'Updated Today 9:45 am',
    preview: 'Home glucometer results elevated',
    status: AlertStatus.IN_PROGRESS,
  },
  {
    patient: pickPatientPDById(104),
    date: 'Updated Today 8:15 am',
    preview: 'Significant body weight increase',
    status: AlertStatus.IN_PROGRESS,
  },
  {
    patient: pickPatientPDById(105),
    date: 'Updated Last Friday 1:30 pm',
    preview: 'Medication non-adherence',
    status: AlertStatus.IN_PROGRESS,
  },
];
