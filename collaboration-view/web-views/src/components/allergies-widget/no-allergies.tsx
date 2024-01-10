import { noAllergiesIcon } from '@icons';

import styles from './allergies.module.scss';

export const NoAllergiesWidget = () => 
  (
    <div className={styles.noAllergiesContainer}>
      <div className={styles.noAllergiesIcon}>
        <img src={noAllergiesIcon} alt="no allergies" />
      </div>
      <div className={styles.noAllergiesText}>No Known Allergies</div>
    </div>
  );
