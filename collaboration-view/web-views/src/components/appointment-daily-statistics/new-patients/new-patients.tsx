import { Row } from 'react-bootstrap';

import { newPatientsMagenta } from '@icons';
import { DailyStatistics } from '@types';

import styles from './new-patients.scss';

type NewPatientsTypeProps = Pick<DailyStatistics, 'newPatients'>;

export const NewPatients = ({ newPatients }: NewPatientsTypeProps) => {
  if (!newPatients) return null;

  const newPatientsLabel = newPatients > 1 ? 'New Patients' : 'New Patient';

  return (
    <Row className={styles.newPatientsRow}>
      <span className={styles.newPatientsRowContainer}>
        <img src={newPatientsMagenta} alt="new patients icon" />
        <span className={styles.newPatientsRowCount}>{newPatients}</span>
        <span className={styles.newPatientsRowLabel}>{newPatientsLabel}</span>
      </span>
    </Row>
  );
};
