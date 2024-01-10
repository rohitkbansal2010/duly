import { DulyLoader } from '@components/duly-loader';
import { TIMELINE_LOADER_WIDTH } from '@constants';
import { PatientAppointmentAlias } from '@enums';

import { timelineAppointmentsMap } from './timeline-appointments-map';
import styles from './timeline-appointments.scss';

type TimelineAppointmentsSkeletonProps = {
  alias: PatientAppointmentAlias;
}

export const TimelineAppointmentsSkeleton = ({ alias }: TimelineAppointmentsSkeletonProps) => {
  const {
    colorTitle,
    title,
    skeletonCount,
  } = timelineAppointmentsMap[alias];

  const titleClass =
    `${styles.timelineAppointmentsSkeletonTitle} ${styles[`timelineAppointmentsSkeletonTitle${colorTitle}`]}`;

  return (
    <div className={styles.timelineAppointmentsSkeleton}>
      <span className={titleClass}>
        {title}
      </span>
      {[ ...new Array(skeletonCount) ].map((_, index) =>
        (
          <div
            key={index}
            className={styles.timelineAppointmentsSkeletonItem}
          >
            <DulyLoader width={TIMELINE_LOADER_WIDTH} />
          </div>
        ))}
    </div>
  );};
