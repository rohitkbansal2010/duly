import { conditionsIcon, currentConditionsIcon } from '@icons';
import { formatMDYYYYDate } from '@utils';

import styles from './condition-plate.module.scss';

export type ConditionPlateProps = {
  title: string;
  date?: string;
  isCurrent?: boolean;
};

export const ConditionPlate = ({ title, date, isCurrent }: ConditionPlateProps) => 
  (
    <div className={styles.conditionPlate}>
      <div className={styles[`conditionPlateIcon${isCurrent ? 'Current' : ''}`]}>
        <img
          src={isCurrent ? currentConditionsIcon : conditionsIcon}
          alt="conditions icon"
        />
      </div>
      <div className={styles.conditionPlateData}>
        <h2 className={styles.conditionPlateDataName}>{title}</h2>
        <span className={styles.conditionPlateDataDate}>{date && formatMDYYYYDate(date)}</span>
      </div>
    </div>
  );
