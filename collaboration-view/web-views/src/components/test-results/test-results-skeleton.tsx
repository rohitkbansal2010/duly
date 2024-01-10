import { DulyLoader } from '@components/duly-loader';
import { TEST_RESULTS_LOADER_WIDTH } from '@constants';

import styles from './test-results.scss';

export const TestResultsSkeleton = () =>
  (
    <div className={styles.resultsSkeleton}>
      <DulyLoader width={TEST_RESULTS_LOADER_WIDTH} />
    </div>
  );
