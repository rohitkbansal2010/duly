import { Level } from '@enums';
import { PersonalData } from '@types';

export interface PatientsPD extends PersonalData {
  isNewPatient?: boolean;
  conditions: string[];
  allergies: string[];
  riskLevel: Level;
}

export interface CareAlliesPD extends PersonalData {
  role: string;
}
