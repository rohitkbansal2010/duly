import { MedicationsType } from '@enums';

import styles from './medications-header.scss';

type MedicationsHeaderProps = { type: MedicationsType };

export const MedicationsHeader = ({ type }: MedicationsHeaderProps) =>{ 
  const headerClass = []
    .concat(styles.medicationsHeader)
    .concat(styles[`medicationsHeader${type}`])
    .join(' ');

  return (
    <div className={headerClass}>
      <div className={styles.medicationsHeaderMedicine}>medicine</div>
      <div className={styles.medicationsHeaderInstructions}>instructions</div>
    </div>
  );};
