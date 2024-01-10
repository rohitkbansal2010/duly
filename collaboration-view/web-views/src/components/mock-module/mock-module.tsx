import { useParams } from 'react-router-dom';

import styles from './mock-module.scss';

// TODO: remove mock
export const MockModule = () => {
  const { appointmentModuleRoute } = useParams<{appointmentModuleRoute: string}>();

  return (
    <main className={styles.cvAppointmentPageContentWrapper}>
      { `${appointmentModuleRoute} module`}
    </main>
  );
};
