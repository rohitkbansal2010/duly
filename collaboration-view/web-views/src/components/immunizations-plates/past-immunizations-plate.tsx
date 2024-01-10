import Accordion from 'react-bootstrap/Accordion';

import { IMMUNIZATIONS_EVENT_KEY, GROUP_ID_PAST_IMMUNIZATION } from '@constants';
import { ImmunizationsToggles, ImmunizationsType } from '@enums';
import { ImmunizationsGroupType } from '@types';

import { ImmunizationsBody } from './immunizations-body';
import styles from './immunizations-plates.scss';
import { ImmunizationsTitle } from './immunizations-title';
import { ImmunizationsToggle } from './immunizations-toggle';
import { NoImmunizationsPlate } from './no-immunizations-plate';

export type PastImmunizationsPlateProps = {
  immunizations: ImmunizationsGroupType[];
  immunizationsCount: number;
  accordions: { [key in string]: string[] };
  handleClickImmunizationGroupItem: (groupId: string, accordionId: string) => () => void;
  handleClickPastImmunizationGroup: (groupId: string) => (isCurrentEventKey: boolean) => void;
}

const type = ImmunizationsType.PAST;

export const PastImmunizationsPlate =
  ({
    immunizations,
    immunizationsCount,
    accordions,
    handleClickImmunizationGroupItem,
    handleClickPastImmunizationGroup,
  }: PastImmunizationsPlateProps) =>
    (immunizationsCount
      ? (
        <Accordion className={styles.immunizationsPlateContainer}>
          <ImmunizationsToggle
            eventKey={IMMUNIZATIONS_EVENT_KEY}
            type={ImmunizationsToggles.PLATE}
            callback={handleClickPastImmunizationGroup(GROUP_ID_PAST_IMMUNIZATION)}
          >
            <ImmunizationsTitle immunizationsCount={immunizationsCount} type={type}/>
          </ImmunizationsToggle>
          <Accordion.Collapse eventKey={IMMUNIZATIONS_EVENT_KEY}>
            <ImmunizationsBody
              immunizations={immunizations}
              groupId={GROUP_ID_PAST_IMMUNIZATION}
              accordionsGroup={accordions[GROUP_ID_PAST_IMMUNIZATION]}
              handleClickImmunizationGroupItem={handleClickImmunizationGroupItem}
            />
          </Accordion.Collapse>
        </Accordion>
      )
      : <NoImmunizationsPlate type={type}/>
    );
