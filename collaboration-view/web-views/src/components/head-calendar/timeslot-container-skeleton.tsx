import { DulyLoader } from'@components/duly-loader';
import { TIMESLOT_CONTENT_LOADER_WIDTH } from'@constants';

import styles from './HeadCalender.scss';

export const TimeslotContainerSkeleton = () =>
  (
    <div className={styles.timeslotContainerLoading}>
      <DulyLoader width={TIMESLOT_CONTENT_LOADER_WIDTH}/>
    </div>
  );
