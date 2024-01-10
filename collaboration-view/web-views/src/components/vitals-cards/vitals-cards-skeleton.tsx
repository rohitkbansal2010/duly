import { VitalCardSkeleton } from '@components/vital-card';

import styles from './vitals-cards.scss';

export const VitalsCardsSkeleton = () =>
  (
    <div className={styles.todaysVitalsCardsSkeleton}>
      <VitalCardSkeleton/>
      <VitalCardSkeleton/>
      <VitalCardSkeleton/>
      <VitalCardSkeleton/>
      <VitalCardSkeleton/>
      <VitalCardSkeleton/>
      <VitalCardSkeleton/>
      <VitalCardSkeleton/>
    </div>
  );
