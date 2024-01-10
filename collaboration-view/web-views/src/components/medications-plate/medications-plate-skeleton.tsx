import { DulyLoader } from '@components/duly-loader';
import { MEDICATION_LOADER_WIDTH } from '@constants';

import styles from './medications-plate.scss';

export const MedicationsPlateSkeleton = () => 
  (
    <div className={styles.medicationsPlateSkeleton}>
      <DulyLoader width={MEDICATION_LOADER_WIDTH} />
    </div>
  );
