import { appointmentModulesIcons } from '@constants';
import { AppointmentModulesTitles } from '@enums';

import styles from './telehealth-link.scss';

export const TelehealthLink = () => 
  (
    <a
      className={styles.telehealthLink}
      href="msteams:"
    >
      <div className={styles.telehealthLinkWrapper}>
        <img
          src={appointmentModulesIcons.MODULE_TELEHEALTH}
          alt={AppointmentModulesTitles.MODULE_TELEHEALTH}
        />
      </div>
      {AppointmentModulesTitles.MODULE_TELEHEALTH}
    </a>
  );
