import noop from 'lodash/noop';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { TodaysAppointmentPractitioners } from '@components/todays-appointment-practitioners';
import { dulyLargeVioletIcon } from '@icons';
import { getTodaysAppointmentsSaga, startDataFetch, stopDataFetch } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { NavCalendar } from '@ui-kit/nav-calendar/nav-calendar';

import styles from './sidebar.scss';

export const Sidebar = () => {
  const dispatch: AppDispatch = useDispatch();

  const isDataFetched = useSelector(({ UI }: RootState) =>
    UI.isDataFetched);
  const appointments = useSelector(({ TODAYS_APPOINTMENTS }: RootState) =>
    TODAYS_APPOINTMENTS.appointments);

  const refreshButtonClass =
    `${styles.refreshButton} ${isDataFetched ? styles.refreshButtonActive : ''}`;

  const onHandleLogo = () => {
    dispatch(startDataFetch());
    dispatch(getTodaysAppointmentsSaga());
  };

  useEffect(() => {
    appointments && dispatch(stopDataFetch());
  }, [ dispatch, appointments ]);

  return (
    <aside className={styles.cvSidebar}>
      <button
        className={refreshButtonClass}
        onClick={isDataFetched ? onHandleLogo : noop}
      >
        <img
          src={dulyLargeVioletIcon}
          alt="Logo"
          className={styles.refreshButtonIcon}
        />
      </button>
      <nav className={styles.cvSidebarBottomBlockNav}>
        <NavCalendar status="active" />
      </nav>
      <div className={styles.cvSidebarBottomBlockCareTeam}>
        <TodaysAppointmentPractitioners />
      </div>
    </aside>
  );
};
