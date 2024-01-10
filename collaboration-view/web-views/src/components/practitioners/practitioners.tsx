import { ExtendedCommonPractitionerType } from '@types';
import { getPractitionersGroups } from '@utils';

import { OtherPractitioners } from './other-practitioners';
import { Practitioner } from './practitioner';
import styles from './practitioners.scss';

type PractitionersPropsType = {
  practitioners: ExtendedCommonPractitionerType[];
  isTodaysAppointment?: boolean;
};

export const Practitioners = ({
  practitioners,
  isTodaysAppointment = false,
}: PractitionersPropsType) => {
  const { firstPractitioners, otherPractitioners } = getPractitionersGroups(practitioners);

  const renderPractitionersList = ({
    id,
    isCurrentUser,
    ...props
  }: ExtendedCommonPractitionerType) =>
    (
      <Practitioner
        key={id}
        id={id}
        role={'role' in props ? props.role : props.memberType}
        {...props}
        isCurrentUser={isCurrentUser}
        isTodaysAppointment={isTodaysAppointment}
      />
    );

  const practitionersFlex = isTodaysAppointment ?
    styles.flexColumn :
    styles.flexRow;

  return (
    <div
      className={`${styles.practitioners} ${practitionersFlex}`}
    >
      {firstPractitioners.map(renderPractitionersList)}
      {!!otherPractitioners.length && (
        <OtherPractitioners
          practitioners={otherPractitioners}
          isTodaysAppointment={isTodaysAppointment}
        />
      )}
    </div>
  );
};
