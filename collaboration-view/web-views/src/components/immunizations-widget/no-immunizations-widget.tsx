import { immunizationLightBlueIcon } from '@icons';

import styles from './immunizations-widget.scss';

export const NoImmunizationsWidget = () =>
  (
    <div className={styles.noImmunizationsContainer}>
      <div className={styles.noImmunizationsIconWrapper}>
        <img src={immunizationLightBlueIcon} alt="no immunizations" />
      </div>
      <div className={styles.noImmunizationsTitle}>No immunizations Available</div>
    </div>
  );
