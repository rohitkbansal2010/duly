import { MedicationsData } from '@types';
import { transformDateMMYYYYToTimestamp } from '@utils';

export const sortMedications = (medications: MedicationsData[]): MedicationsData[] =>
  medications
    .sort((a, b) => 
      a.title > b.title ? 1 : -1)
    .sort((a, b) => 
      transformDateMMYYYYToTimestamp(b.startDate) - transformDateMMYYYYToTimestamp(a.startDate));
    
