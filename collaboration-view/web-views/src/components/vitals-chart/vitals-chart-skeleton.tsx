import { DulyLoader } from '@components/duly-loader';
import { VITALS_CHART_LOADER_WIDTH } from '@constants';

import styles from './vitals-chart.scss';

export const VitalsChartSkeleton = () =>
  (
    <div className={styles.vitalsWidgetContainerChart}>
      <DulyLoader width={VITALS_CHART_LOADER_WIDTH} />
    </div>
  );
