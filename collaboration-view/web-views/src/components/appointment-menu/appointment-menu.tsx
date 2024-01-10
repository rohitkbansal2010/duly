import noop from 'lodash/noop';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';

import { AppointmentNav } from '@components/appointment-nav';
import { CareTeamMembers } from '@components/care-team-members';
import { startDataFetch } from '@redux/actions';
import { AppDispatch } from '@redux/store';

import styles from './appointment-menu.scss';


export const AppointmentMenu = () => {
  const dispatch: AppDispatch = useDispatch();

  const handleClick = useCallback((route) => {
    if (!route) return;
    dispatch(startDataFetch());
  }, [ dispatch ]);

  const outerClassName = []
    .concat(styles.appointmentMenuIsOpen)
    .concat(styles.appointmentMenuOuter)
    .join(' ');

  return (
    <div className={outerClassName}>
      <div
        className={styles.appointmentMenuInner}
        tabIndex={-1}
        role="button"
        onKeyDown={noop}
      >
        <AppointmentNav onClick={handleClick} />
        <CareTeamMembers />
      </div>
    </div>
  );
};
