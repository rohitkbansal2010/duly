import { CareAlliesPD } from '@interfaces';

import { pickPDById } from './all-persons';

export const careAlliesMockData: CareAlliesPD[] = [
  {
    ...pickPDById(201),
    role: 'Care Ally',
  },
  {
    ...pickPDById(202),
    role: 'Internal medicine',
  },
];

export const pickCareAllyPDById = (pickedId: number): CareAlliesPD =>
  careAlliesMockData.filter(({ id }) =>
    (id === pickedId))[0];
