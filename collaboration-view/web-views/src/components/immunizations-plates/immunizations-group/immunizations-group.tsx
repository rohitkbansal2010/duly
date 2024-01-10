import Accordion from 'react-bootstrap/Accordion';

import { speechBubbleOrangeIcon } from '@icons';
import { VaccinationType } from '@types';

import { ImmunizationsItem } from '../immunizations-item';
import { ImmunizationsToggle } from '../immunizations-toggle';

import styles from './immunizations-group.scss';

type ImmunizationsGroupProps = {
  title: string;
  vaccinations: VaccinationType[];
  eventKey: string;
  activeKey: string | null;
  isSpeechBubbleIcon: boolean;
  handleClickImmunization: () => void;
}

export const ImmunizationsGroup = ({
  title,
  vaccinations,
  eventKey,
  activeKey,
  isSpeechBubbleIcon,
  handleClickImmunization,
}: ImmunizationsGroupProps) =>
  (
    <Accordion
      className={styles.immunizationsGroup}
      activeKey={activeKey as string}
    >
      <ImmunizationsToggle
        eventKey={eventKey}
        callback={handleClickImmunization}
      >
        <span className={styles.immunizationsGroupTitle}>
          {title}
          {isSpeechBubbleIcon && (
            <img
              src={speechBubbleOrangeIcon}
              alt="Speech bubble orange"
            />
          )}
        </span>
      </ImmunizationsToggle>
      <Accordion.Collapse eventKey={eventKey}>
        <div className={styles.immunizationsGroupBody}>
          {vaccinations.map((vaccination, index) =>
            <ImmunizationsItem key={index} {...vaccination}/>)}
        </div>
      </Accordion.Collapse>
    </Accordion>
  );
