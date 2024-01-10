import { Level } from '@enums';
import { PatientsPD } from '@interfaces';

import { pickPDById } from './all-persons';

export const patientsMockData: PatientsPD[] = [
  {
    ...pickPDById(101),
    isNewPatient: true,
    conditions: [ 'COPD', 'Type II Diabetes', 'Osteoarthritis' ],
    allergies: [ 'Penicillins', 'Abciximab' ],
    riskLevel: Level.HIGH,
  },
  {
    ...pickPDById(102),
    isNewPatient: true,
    conditions: [ 'COPD', 'Type II Diabetes', 'Osteoarthritis' ],
    allergies: [ 'Penicillins', 'Abciximab' ],
    riskLevel: Level.HIGH,
  },
  {
    ...pickPDById(103),
    conditions: [ 'COPD', 'Type II Diabetes', 'Osteoarthritis' ],
    allergies: [ 'Penicillins', 'Abciximab' ],
    riskLevel: Level.MEDIUM,
  },
  {
    ...pickPDById(104),
    conditions: [ 'COPD', 'Type II Diabetes', 'Osteoarthritis' ],
    allergies: [ 'Penicillins', 'Abciximab' ],
    riskLevel: Level.MEDIUM,
  },
  {
    ...pickPDById(105),
    conditions: [ 'COPD', 'Type II Diabetes', 'Osteoarthritis' ],
    allergies: [ 'Penicillins', 'Abciximab' ],
    riskLevel: Level.LOW,
  },
];

export const pickPatientPDById = (pickedId: number): PatientsPD =>
  patientsMockData.filter(({ id }) =>
    (id === pickedId))[0];
