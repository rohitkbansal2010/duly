import { TimelineAppointments } from '@components/timeline-appointments';
import { PatientAppointmentAlias } from '@enums';

import styles from './timeline-widget.scss';

export const TimelineWidget = () =>
  (
    <div className={styles.timelineWidgetContainer}>
      {Object.values(PatientAppointmentAlias).map(alias =>
        (
          <TimelineAppointments key={alias} alias={alias} />
        ))}
    </div>
  );
