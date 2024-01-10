import { IMMUNIZATIONS } from '@constants';
import { ImmunizationsType } from '@enums';

import styles from './immunizations-title.scss';

type ImmunizationsTitleProps = {
  type: ImmunizationsType;
  immunizationsCount: number;
};

export const ImmunizationsTitle =
  ({ type, immunizationsCount }: ImmunizationsTitleProps) => {
    const counterClass = []
      .concat(styles.immunizationsTitleCounter)
      .concat(styles[`immunizationsTitleCounter${type}`])
      .join(' ');

    const iconWrapperClass = []
      .concat(styles.immunizationsTitleIcon)
      .concat(styles[`immunizationsTitleIcon${type}`])
      .join(' ');

    return (
      <div className={styles.immunizationsTitle}>
        <div className={iconWrapperClass}>
          <img src={IMMUNIZATIONS[type].icon} alt={`${type} immunizations icon`}/>
          {immunizationsCount > 1 && <div className={counterClass}>{immunizationsCount}</div>}
        </div>
        <div className={styles.immunizationsTitleText}>
          <span>{IMMUNIZATIONS[type].title}</span>
          <span className={styles.immunizationsTitleTextAdditional}>
            {IMMUNIZATIONS[type].additional}
          </span>
        </div>
      </div>
    );
  };
