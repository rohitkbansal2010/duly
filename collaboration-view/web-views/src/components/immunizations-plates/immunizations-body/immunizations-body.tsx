import { ImmunizationsGroupType } from '@types';

import { isStatusOverdue } from '../helpers';
import { ImmunizationsGroup } from '../immunizations-group';

import styles from './immunizations-body.scss';

type ImmunizationsBodyProps = {
  immunizations: ImmunizationsGroupType[];
  accordionsGroup: string[];
  groupId: string;
  handleClickImmunizationGroupItem: (groupId: string, accordionId: string) => () => void;
}

export const ImmunizationsBody = ({
  immunizations,
  groupId,
  accordionsGroup,
  handleClickImmunizationGroupItem,
}: ImmunizationsBodyProps) => {
  const columns = immunizations.reduce((acc, immunizationGroup, index) => {
    acc[index % 2].push(immunizationGroup);

    return acc;
  }, [ [], [] ] as Array<ImmunizationsGroupType[]>);

  const getActiveKey = (eventKey: string) =>
    accordionsGroup?.find(accordionId =>
      accordionId === eventKey) || null;

  const renderGroup = (idxCol: number) =>
    // eslint-disable-next-line react/display-name
    (immunization: ImmunizationsGroupType, index: number) =>
      (
        <ImmunizationsGroup
          key={String(index)}
          eventKey={`${idxCol}-${index}`}
          activeKey={getActiveKey(`${idxCol}-${index}`)}
          isSpeechBubbleIcon={isStatusOverdue(immunization.vaccinations)}
          handleClickImmunization={handleClickImmunizationGroupItem(groupId, `${idxCol}-${index}`)}
          {...immunization}
        />
      );

  return (
    <div className={styles.immunizationsBody}>
      {columns.map((column, index) =>
        (
          <div key={index} className={styles.immunizationsBodyGroups}>
            {column.map(renderGroup(index))}
          </div>
        ))}
    </div>
  );
};
