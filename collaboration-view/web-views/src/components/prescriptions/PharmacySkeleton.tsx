import { DulyLoader } from '@components/duly-loader';
import { LOCATIONS_CONTENT_LOADER_WIDTH } from '@constants';

import styles from './prescriptions.scss';

export const PharmacySkeleton = () =>
  (
    <div className={styles.preferredPharmacyContainerLoading}>
      <DulyLoader width={LOCATIONS_CONTENT_LOADER_WIDTH} />
    </div>
  );
