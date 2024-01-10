import { ListGroup, ListGroupItem } from 'react-bootstrap';

import { calculateCalendarDivisions } from '@utils';

import styles from './scheduler-scale-marks.scss';

export type SchedulerScaleMarksPropsType = {
  workDayStart: number;
  workDayEnd: number;
};

export const SchedulerScaleMarks = ({
  workDayStart,
  workDayEnd,
}: SchedulerScaleMarksPropsType) => {
  const schedulerDivisions = calculateCalendarDivisions(workDayStart, workDayEnd);

  const renderListItem = (division: string) =>
    (
      <ListGroupItem key={division} className={styles.schedulerScaleMarksItem}>
        <div>
          <div className={styles.schedulerScaleMarksItemDivision}>
            {division}
          </div>
          <div className={styles.schedulerScaleMarksItemLine} />
        </div>
      </ListGroupItem>
    );

  return (
    <ListGroup variant="flush" className={styles.schedulerScaleMarks}>
      {schedulerDivisions.map(renderListItem)}
    </ListGroup>
  );
};
