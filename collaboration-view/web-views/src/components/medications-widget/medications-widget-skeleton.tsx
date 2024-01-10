import { MedicationsPlateSkeleton } from '@components/medications-plate';

import styles from './medications-widget.scss';

export const MedicationsWidgetSkeleton = () => 
  (
    <div className={styles.medicationsSkeleton}>
      <MedicationsPlateSkeleton/>
      <MedicationsPlateSkeleton/>
    </div>
  );
