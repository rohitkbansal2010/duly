import { useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';

import { AppointmentDailyStatistics } from '@components/appointment-daily-statistics';
import { DulyLoader } from '@components/duly-loader';
import { Scheduler } from '@components/scheduler';
import { TODAYS_LOADER_WIDTH } from '@constants';
import { RootState } from '@redux/reducers';

import styles from './content.scss';

export const Content = () => {
  const contentCalendarRef = useRef<HTMLDivElement>(null);

  const isSpinnerShown = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.isSpinnerShown);
  const isDataFetched = useSelector(({ UI }: RootState) =>
    UI.isDataFetched);

  const scrollToTop = () => {
    contentCalendarRef?.current?.scrollTo({ top: 0 });
  };

  const contentCalendarClass = []
    .concat(styles.cvWrapperContentCalendar)
    .concat(isSpinnerShown ? styles.cvWrapperContentCalendarLocked : '')
    .join(' ');

  useEffect(() => {
    !isDataFetched && scrollToTop();
  }, [ isDataFetched ]);

  return (
    <main className={styles.cvWrapper}>
      <header className={styles.cvWrapperHeader}>
        <h1 className={styles.cvWrapperHeaderText}>
          Today’s <span>Appointments</span>
        </h1>
      </header>
      <div className={styles.cvWrapperContent}>
        <div className={styles.cvWrapperContentStatistics}>
          <AppointmentDailyStatistics />
        </div>
        <div
          className={contentCalendarClass}
          ref={contentCalendarRef}
        >
          {isSpinnerShown && <DulyLoader width={TODAYS_LOADER_WIDTH} />}
          <Scheduler />
        </div>
      </div>
    </main>
  );
};
