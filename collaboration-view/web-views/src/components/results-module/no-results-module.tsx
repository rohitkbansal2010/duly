import { testResultsLightBlueIcon } from '@icons';

import styles from './results-module.scss';

export const NoResultsModule = () =>
  (
    <div className={styles.noResultsContainer}>
      <div className={styles.noResultsIconWrapper}>
        <img src={testResultsLightBlueIcon} alt="no results icon" />
      </div>
      <div className={styles.noResultsTitle}>No Test Results Available</div>
    </div>
  );
