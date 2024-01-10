import { IMMUNIZATIONS } from '@constants';
import { ImmunizationsType } from '@enums';
import { immunizationLightBlueIcon } from '@icons';

import styles from './immunizations-plates.scss';

type NoImmunizationsPlateProps = {
  type: ImmunizationsType;
}

export const NoImmunizationsPlate = ({ type }: NoImmunizationsPlateProps) =>
  (
    <div className={styles.noImmunizationsPlateContainer}>
      <div className={styles.noImmunizationsPlateIconWrapper}>
        <img src={immunizationLightBlueIcon} alt={IMMUNIZATIONS[type].noAvailableText} />
      </div>
      <div className={styles.noImmunizationsPlateTitle}>{IMMUNIZATIONS[type].noAvailableText}</div>
    </div>
  );
