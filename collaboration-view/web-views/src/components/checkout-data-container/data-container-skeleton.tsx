import { DulyLoader } from '@components/duly-loader';
import { CHECKOUT_CONTENT_LOADER_WIDTH } from '@constants';

import styles from './checkout-data-container.scss';

export const DataContainerSkeleton = () => 
  (
    <div className={styles.dataContainerLoading}>
      <DulyLoader width={CHECKOUT_CONTENT_LOADER_WIDTH} />
    </div>
  );
