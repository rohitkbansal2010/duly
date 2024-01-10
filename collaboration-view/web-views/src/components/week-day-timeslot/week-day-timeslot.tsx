import { FC } from 'react';

import styles from './week-day-timeslot.scss';

type WeekDayTimeslotProps = {
  time: string;
  isSelectedTimeslot: boolean;
  onSelectTimeSlot: () => void;
};

export const WeekDayTimeslot: FC<WeekDayTimeslotProps> = ({
  time,
  isSelectedTimeslot,
  onSelectTimeSlot,
}) => {
  const classes = [ styles.weekDayTimeslot ]
    .concat(isSelectedTimeslot ? styles.weekDayTimeslotSelected : '')
    .join(' ');

  return (
    <button
      type="button"
      onClick={onSelectTimeSlot}
      className={classes}
    >
      {time}
    </button>
  );
};
