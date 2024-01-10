import { useSelector } from 'react-redux';

import { PatientAppointmentAlias, AppointmentModuleWidgetsAlias } from '@enums';
import { useWidgetSkeleton } from '@hooks';
import { RootState } from '@redux/reducers';

import {
  TimelineApppointmentsGroups,
  TimelineApppointmentsGroupsNoData
} from './timeline-appointments-groups';
import { timelineAppointmentsMap } from './timeline-appointments-map';
import { TimelineAppointmentsSkeleton } from './timeline-appointments-skeleton';
import styles from './timeline-appointments.scss';

export type TimelineAppointmentsPropsType = {
  alias: PatientAppointmentAlias;
};

export const TimelineAppointments = ({ alias }: TimelineAppointmentsPropsType) => {
  const patientAppointments = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.patientAppointments);

  const isSkeletonShown = useWidgetSkeleton(AppointmentModuleWidgetsAlias.TIMELINE);

  const {
    title,
    colorTitle,
    icon,
    backgroundIcon,
    showNoShowStatusCount,
    showNoShowCancelledCount,
    ...rest
  } = timelineAppointmentsMap[alias];

  const isAppointmentGroups =
    patientAppointments &&
    patientAppointments[alias] &&
    patientAppointments[alias].length;
  const classesTimelineAppointmentsTitle = [ styles.timelineAppointmentsContainerTitle ]
    .concat(styles[`timelineAppointmentsContainerTitle${colorTitle}`])
    .join(' ');

  if (isSkeletonShown) {
    return (
      <TimelineAppointmentsSkeleton alias={alias} />
    );
  }

  return (
    <div className={styles.timelineAppointmentsContainer}>
      <h4 className={classesTimelineAppointmentsTitle}>{title}</h4>
      {isAppointmentGroups
        ? (
          <div className={styles.timelineAppointmentsContainerContent}>
            <TimelineApppointmentsGroups
              icon={icon}
              backgroundIcon={backgroundIcon}
              alias={alias}
              groups={patientAppointments[alias]}
              showNoShowStatusCount={showNoShowStatusCount}
              showNoShowCancelledCount={showNoShowCancelledCount}
              {...rest}
            />
          </div>
        )
        : (
          <TimelineApppointmentsGroupsNoData
            icon={icon}
            backgroundIcon={backgroundIcon}
            title={title}
            colorTitle={colorTitle}
          />
        )
      }
    </div>
  );
};
