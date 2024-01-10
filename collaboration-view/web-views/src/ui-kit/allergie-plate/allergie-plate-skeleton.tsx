import { DulyLoader } from '@components/duly-loader';
import { ALLERGIES_LOADER_WIDTH } from '@constants';

import styles from './allergie-plate.module.scss';

export const AllergiePlateSkeleton = () => 
  (
    <div className={styles.allergiePlateSkeleton}>
      <DulyLoader width={ALLERGIES_LOADER_WIDTH} />
    </div>
  );
