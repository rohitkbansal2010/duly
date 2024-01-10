import { Level } from '@enums';
import { Tasks } from '@types';

import { pickPatientPDById } from './personal-data';

export const tasksMockData: Tasks[] = [
  {
    patient: pickPatientPDById(101),
    date: 'Overdue Today 11:00 am',
    preview: 'Scheduling: Check appt/ride confirmed',
    priority: Level.HIGH,
  },
  {
    patient: pickPatientPDById(102),
    date: 'Due Today 1:00 pm',
    preview: 'Home Visits: Prepare route',
    priority: Level.HIGH,
  },
  {
    patient: pickPatientPDById(103),
    date: 'Due Tomorrow 12:45 pm',
    preview: 'Group Session:  Add Invitees',
    priority: Level.MEDIUM,
  },
  {
    patient: pickPatientPDById(104),
    date: 'Due Tomorrow 12:15 pm',
    preview: 'Scheduling:  PFT Test',
    priority: Level.MEDIUM,
  },
  {
    patient: pickPatientPDById(105),
    date: 'Due This Friday 3:30 pm',
    preview: 'Patient Education: Send foot care material',
    priority: Level.LOW,
  },
];
