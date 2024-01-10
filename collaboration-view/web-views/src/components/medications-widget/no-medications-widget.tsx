import { medicationLightBlueIcon } from '@icons';

import styles from './medications-widget.scss';

export const NoMedicationsWidget = () => 
  (
    <div className={styles.noMedicationsContainer}>
      <div className={styles.noMedicationsIconWrapper}>
        <img src={medicationLightBlueIcon} alt="no medications" />
      </div>
      <div className={styles.noMedicationsTitle}>No Medications Available</div>
    </div>
  );
