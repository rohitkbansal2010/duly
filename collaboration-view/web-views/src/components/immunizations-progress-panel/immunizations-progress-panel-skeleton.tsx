import { DulyLoader } from '@components/duly-loader';
import { IMMUNIZATIONS_LOADER_WIDTH } from '@constants';

import styles from './immunizations-progress-panel.scss';

export const ImmunizationsProgressPanelSkeleton = () =>
  (
    <div className={styles.progressPanelSkeleton}>
      <DulyLoader width={IMMUNIZATIONS_LOADER_WIDTH} />      
    </div>
  );
