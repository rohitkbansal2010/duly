import { DulyLoader } from '@components/duly-loader';
import { VITALS_LOADER_WIDTH } from '@constants';
import { vitalSkeletonIcon } from '@icons';

import styles from './vital-card.scss';

export const VitalCardSkeleton = () =>
  (
    <div className={styles.todayVitalCardSkeleton}>
      <div className={styles.todayVitalCardSkeletonImage} >
        <img src={vitalSkeletonIcon} alt="vital loading" />
      </div>
      <div className={styles.todayVitalCardSkeletonLoader}>
        <DulyLoader width={VITALS_LOADER_WIDTH} />
      </div>
    </div>
  );
