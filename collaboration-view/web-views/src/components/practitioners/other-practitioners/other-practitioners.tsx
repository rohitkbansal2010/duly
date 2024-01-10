import Dropdown from 'react-bootstrap/Dropdown';

import { CommonPractitionerType } from '@types';

import { Practitioner } from '../practitioner';

import { OtherPractitionersToggle } from './other-practitioners-toggle';
import styles from './other-practitioners.scss';

export type OtherPractitionersProps = {
  practitioners: CommonPractitionerType[];
  isTodaysAppointment?: boolean;
};

export const OtherPractitioners = ({
  practitioners,
  isTodaysAppointment = false,
}: OtherPractitionersProps) =>
{
  const wrapperClass = []
    .concat(styles.otherPractitionersWrapper)
    .concat(isTodaysAppointment ? styles.otherPractitionersWrapperVertical : '')
    .join(' ');

  return (
    <Dropdown drop={isTodaysAppointment ? 'end' : 'up'}>
      <Dropdown.Toggle as={OtherPractitionersToggle}>
        {practitioners.length}
      </Dropdown.Toggle>
      <Dropdown.Menu className={wrapperClass} renderOnMount>
        <div className={styles.otherPractitioners}>
          {practitioners.map(practitioner =>
            (
              <Practitioner
                key={practitioner.id}
                role={'role' in practitioner ? practitioner.role : practitioner.memberType}
                isTodaysAppointment={isTodaysAppointment}
                isOtherPractitioner
                {...practitioner}
              />
            ))}
        </div>
      </Dropdown.Menu>
    </Dropdown>
  );};
