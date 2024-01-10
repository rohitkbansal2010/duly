import styles from './timeline-appointments-groups.scss';

export type TimelineApppointmentsGroupsNoDataPropsType = {
  icon: string;
  backgroundIcon: string;
  title: string;
  colorTitle: string;
};

export const TimelineApppointmentsGroupsNoData = ({
  icon,
  backgroundIcon,
  title,
  colorTitle,
}: TimelineApppointmentsGroupsNoDataPropsType) => {
  const classesTimelineAppointmentsGroupsIcon = [ styles.timelineAppointmentsGroupsNoDataIcon ]
    .concat(styles[`timelineAppointmentsGroupsNoDataIcon${backgroundIcon}`])
    .join(' ');

  const classesTimelineAppointmentsGroupsTitle = [ styles.timelineAppointmentsGroupsNoDataTitle ]
    .concat(styles[`timelineAppointmentsGroupsNoDataTitle${colorTitle}`])
    .join(' ');

  return (
    <div className={styles.timelineAppointmentsGroupsNoData}>
      <div className={classesTimelineAppointmentsGroupsIcon}>
        <img
          src={icon}
          alt="Calendar icon"
          data-test="appointment-icon"
        />
      </div>
      <div
        className={classesTimelineAppointmentsGroupsTitle}
        data-test="appointment-title"
      >
        {`No ${title}`}
      </div>
    </div>
  );
};
