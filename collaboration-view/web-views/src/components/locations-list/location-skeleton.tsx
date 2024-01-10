import { DulyLoader } from '@components/duly-loader';
import { LOCATIONS_CONTENT_LOADER_WIDTH } from '@constants';

import styles from './locations-list.scss';

export const LocationsContainerSkeleton = () =>
  (
    <div className={styles.locationsContainerLoading}>
      <DulyLoader width={LOCATIONS_CONTENT_LOADER_WIDTH} />
    </div>
  );
