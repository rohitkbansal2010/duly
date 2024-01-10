import { DulyLoader } from '@components/duly-loader';
import { IMMUNIZATIONS_LOADER_WIDTH } from '@constants';

import styles from './immunizations-plates.scss';

type ImmunizationsPlateSkeletonProps = {
  isCollapsed?: boolean;
}
export const ImmunizationsPlateSkeleton = ({ isCollapsed }: ImmunizationsPlateSkeletonProps) =>
  (
    <div className={styles.immunizationsPlateSkeleton} style={{ flexGrow: isCollapsed ? 1 : 2 }} >
      <DulyLoader width={IMMUNIZATIONS_LOADER_WIDTH} />
    </div>
  );
