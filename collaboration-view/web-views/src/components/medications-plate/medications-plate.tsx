import { MedicationsType } from '@enums';
import { MedicationsData } from '@types';
import { sortMedications } from '@utils';

import { MedicationsHeader } from './medications-header';
import { MedicationsItem } from './medications-item';
import styles from './medications-plate.scss';
import { MedicationsTitle } from './medications-title';

type MedicationsPlateProps = {
  type: MedicationsType;
  medications: MedicationsData[];
}

export const MedicationsPlate = ({ type, medications }: MedicationsPlateProps) => 
  (
    <div className={styles.medicationsPlateContainer}>
      <MedicationsTitle type={type} count={medications.length}/>
      <MedicationsHeader type={type}/>
      <div className={styles.medicationsPlateMain}>
        {sortMedications(medications).map(medication => 
          <MedicationsItem key={medication.id} {...medication}/>)}
      </div>
    </div>
  );
