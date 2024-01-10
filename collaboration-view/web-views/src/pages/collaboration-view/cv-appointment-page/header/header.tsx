import { PatientHeaderData } from '@components/patient-header-data';

import styles from './header.scss';

export const Header = () => 
  (
    <header className={styles.cvAppointmentPageHeaderWrapper}>
      <PatientHeaderData />
    </header>
  );
