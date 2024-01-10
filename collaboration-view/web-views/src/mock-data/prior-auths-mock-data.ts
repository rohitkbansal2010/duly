import { PriorAuths } from '@types';

import { pickPatientPDById, pickCareAllyPDById } from './personal-data';

export const priorAuthsMockData: PriorAuths[] = [
  {
    patient: pickPatientPDById(101),
    assignedTo: pickCareAllyPDById(201),
    date: 'Overdue Today 10:00 am',
    reasons: [ 'Procedure' ],
  },
  {
    patient: pickPatientPDById(102),
    assignedTo: pickCareAllyPDById(202),
    date: 'Due Today 12:15 pm',
    reasons: [ 'Medication' ],
  },
  {
    patient: pickPatientPDById(103),
    assignedTo: pickCareAllyPDById(201),
    date: 'Due Today 12:45 pm',
    reasons: [ 'Hospitalization' ],
  },
];
