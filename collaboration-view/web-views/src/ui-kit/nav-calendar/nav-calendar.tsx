import { Link } from 'react-router-dom';

import { CollaborationViewRoutes } from '@enums';
import { calendarDarkBlueIcon, calendarWhiteVioletRoundedIcon } from '@icons';

import styles from './nav-calendar.scss';

type NavCalendarProps = {
  status?: string,
  onClick?: () => void;
}

export const NavCalendar = ({ status = 'default', onClick }: NavCalendarProps) => {
  const active = status === 'active' ? 'Active' : '';

  return (
    <div
      className={styles[`navCalendar${active}`]}
    >
      <Link
        to={CollaborationViewRoutes.home}
        onClick={() => 
          onClick && onClick()}
      >
        <div
          className={styles[`navCalendar${active}IconWrapper`]}
        >
          <img
            src={`${status === 'default' ?
              calendarDarkBlueIcon : status === 'active' &&
              calendarWhiteVioletRoundedIcon}`}
            alt="collaboration view calendar icon"
          />
        </div>
      </Link>
    </div>
  );
};
