import { ImmunizationsPlateSkeleton } from '@components/immunizations-plates';
import { ImmunizationsProgressPanelSkeleton } from '@components/immunizations-progress-panel';

import styles from './immunizations-widget.scss';

export const ImmunizationsWidgetSkeleton = () =>
  (
    <div className={styles.immunizationsWidgetContainer}>
      <ImmunizationsProgressPanelSkeleton />
      <ImmunizationsPlateSkeleton />
      <ImmunizationsPlateSkeleton isCollapsed />
    </div>
  );
