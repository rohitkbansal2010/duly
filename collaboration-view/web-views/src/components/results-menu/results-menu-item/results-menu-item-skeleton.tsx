import { DulyLoader } from '@components/duly-loader';
import { RESULTS_MENU_LOADER_WIDTH } from '@constants';

import styles from './results-menu-item.scss';

export const ResultsMenuItemSkeleton = () =>
  (
    <div className={styles.resultsMenuItemSkeleton}>
      <DulyLoader width={RESULTS_MENU_LOADER_WIDTH} />
    </div>
  );
