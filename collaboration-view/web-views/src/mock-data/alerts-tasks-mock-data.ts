import { AlertStatus } from '@enums';
import { AlertTasks } from '@types';

import { pickPatientPDById } from './personal-data';

export const alertTasksMockData: AlertTasks[] = [
  {
    patient: pickPatientPDById(101),
    preview: 'Possible care barrier, risk to adherence',
    time: 'Generated Today 11:15 am',
    status: AlertStatus.NEW,
  },
  {
    patient: pickPatientPDById(102),
    preview: 'Significant decrease in SpO2',
    time: 'Generated Today 10:45 am',
    status: AlertStatus.NEW,
  },
  {
    patient: pickPatientPDById(103),
    preview: 'Home glucometer results elevated',
    time: 'Updated Today 9:45 am',
    status: AlertStatus.IN_PROGRESS,
  },
  {
    patient: pickPatientPDById(104),
    preview: 'Significant body weight increase',
    time: 'Updated Today 8:15 am',
    status: AlertStatus.IN_PROGRESS,
  },
  {
    patient: pickPatientPDById(105),
    preview: 'Medication non-adherence',
    time: 'Updated Last Friday 1:30 pm',
    status: AlertStatus.IN_PROGRESS,
  },
];
