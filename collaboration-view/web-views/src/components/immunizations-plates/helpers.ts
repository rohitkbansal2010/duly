import { ImmunizationStatus } from '@enums';
import { VaccinationType } from '@types';

export const isStatusOverdue = (vaccinations: VaccinationType[]): boolean =>
  vaccinations.some(({ status }) =>
    status === ImmunizationStatus.OVERDUE);
