import { useState } from 'react';
import { useInterval } from 'usehooks-ts';

import { OF_MILLISECONDS, SECONDS_PER_MINUTE } from '@constants';
import { calculateCurrentTimeBarOptions } from '@utils';

import styles from './current-time-bar.scss';

const workDayStart = window?.env?.WORK_DAY_START || '';
const workDayEnd = window?.env?.WORK_DAY_END || '';

export const CurrentTimeBar = () => {
  const now = new Date();
  const currentHour = now.getHours();

  const isBarVisible = !(currentHour < +workDayStart || currentHour >= +workDayEnd);

  const [ timerDuration, setTimerDuration ] = useState(
    (SECONDS_PER_MINUTE - now.getSeconds()) * OF_MILLISECONDS
  );

  const [ timeBarOptions, setTimeBarOptions ] = useState(calculateCurrentTimeBarOptions());

  useInterval(() => {
    setTimerDuration(SECONDS_PER_MINUTE * OF_MILLISECONDS);
    setTimeBarOptions(calculateCurrentTimeBarOptions());
  }, timerDuration);

  return isBarVisible ? (
    <div
      data-testid="current-time-bar__container"
      className={styles.barWrapper}
      style={{ top: `${timeBarOptions.position - 0.05}rem` }}
    >
      <span
        data-testid="current-time-bar__text"
        className={styles.currentTime}
      >
        {timeBarOptions.time}
      </span>
      <div className={styles.currentTimeBar} />
    </div>
  ) : null;
};
