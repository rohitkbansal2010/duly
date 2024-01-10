import { MedicationsTitles, MedicationsTitlesAdditional, MedicationsType } from '@enums';
import { medicationLightBlueIcon, medicineVioletIcon } from '@icons';

import styles from './medications-title.scss';

type MedicationsTitleProps = {
  type: MedicationsType;
  count: number;
};

const MEDICATIONS_TITLE_ICON = {
  [MedicationsType.REGULAR]: medicationLightBlueIcon,
  [MedicationsType.OTHER]: medicineVioletIcon,
};

export const MedicationsTitle = ({ type, count }: MedicationsTitleProps) =>{
  const counterClass = []
    .concat(styles.medicationsIconCounter)
    .concat(styles[`medicationsIconCounter${type}`])
    .join(' ');

  const iconWrapperClass = []
    .concat(styles.medicationsIconWrapper)
    .concat(styles[`medicationsIconWrapper${type}`])
    .join(' ');

  
  return (
    <div className={styles.medicationsTitleWrapper}>
      <div className={iconWrapperClass}>
        <img src={MEDICATIONS_TITLE_ICON[type]} alt={`${type} medications icon`}/>
        {count > 1 && <div className={counterClass}>{count}</div>}
      </div>
      <div className={styles.medicationsTitle}>
        <span>{MedicationsTitles[type]}</span>
        <span className={styles.medicationsTitleAdditional}>
          {MedicationsTitlesAdditional[type]}
        </span>
      </div>
    </div>
  );};
