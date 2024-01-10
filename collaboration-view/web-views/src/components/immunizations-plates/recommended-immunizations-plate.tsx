import { GROUP_ID_RECOMMENDED_IMMUNIZATION } from '@constants';
import { ImmunizationsType } from '@enums';
import { ImmunizationsGroupType } from '@types';

import { ImmunizationsBody } from './immunizations-body';
import styles from './immunizations-plates.scss';
import { ImmunizationsTitle } from './immunizations-title';
import { NoImmunizationsPlate } from './no-immunizations-plate';

export type ImmunizationsPlateProps = {
  immunizations: ImmunizationsGroupType[];
  immunizationsCount: number;
  accordions: { [key in string]: string[] };
  handleClickImmunizationGroupItem: (groupId: string, accordionId: string) => () => void;
}

const type = ImmunizationsType.RECOMMENDED;

export const RecommendedImmunizationsPlate =
  ({
    immunizations,
    immunizationsCount,
    accordions,
    handleClickImmunizationGroupItem,
  }: ImmunizationsPlateProps) =>
    (immunizationsCount
      ? (
        <div className={styles.immunizationsPlateContainer}>
          <ImmunizationsTitle immunizationsCount={immunizationsCount} type={type}/>
          <ImmunizationsBody
            immunizations={immunizations}
            groupId={GROUP_ID_RECOMMENDED_IMMUNIZATION}
            accordionsGroup={accordions[GROUP_ID_RECOMMENDED_IMMUNIZATION]}
            handleClickImmunizationGroupItem={handleClickImmunizationGroupItem}
          />
        </div>
      )
      : <NoImmunizationsPlate type={type}/>
    );
