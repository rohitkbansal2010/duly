/* eslint-disable jsx-a11y/no-noninteractive-element-interactions */
/* eslint-disable jsx-a11y/click-events-have-key-events */
import { Avatar } from '@components/avatar';
import { CareTeamMemberRole, PractitionerRole } from '@enums';
import { useLogoutModal } from '@hooks';
import { CareTeamMember, Practitioner as PractitionerType } from '@types';
import { getUserRole, getSrcAvatar, showFirstPrefixItem } from '@utils';

import styles from './practitioner.scss';

type MemberType = Omit<PractitionerType, 'role'> | Omit<CareTeamMember, 'memberType'>;

export type PractitionerPropsType = MemberType & {
  role: PractitionerRole | CareTeamMemberRole;
  isCurrentUser?: boolean;
  isTodaysAppointment?: boolean;
  isOtherPractitioner?: boolean;
};

export const Practitioner = ({
  humanName: {
    familyName,
    givenNames,
    prefixes,
  },
  photo,
  role,
  isCurrentUser = false,
  isTodaysAppointment = false,
  isOtherPractitioner = false,
}: PractitionerPropsType) =>{
  const { onShowLogoutModal } = useLogoutModal();

  const practitionerClass = []
    .concat(styles.practitioner)
    .concat(!isTodaysAppointment ? styles.practitionerHorizontal : '')
    .concat(isOtherPractitioner ? styles.practitionerOther : '')
    .join(' ');

  const practitionerHumanNameClass = []
    .concat(styles.practitionerInfoHumanName)
    .concat(isOtherPractitioner ? styles.practitionerOtherHumanName : '')
    .join(' ');

  return (
    <button
      onClick={isCurrentUser ? onShowLogoutModal : undefined}
      className={practitionerClass}
      style={{ cursor: isCurrentUser ? 'pointer' : 'default' }}
    >
      <Avatar
        src={getSrcAvatar(photo)}
        alt={`${showFirstPrefixItem(givenNames)}${familyName}`.trim()}
        width={3}
        hasBorder={isCurrentUser}
        role={getUserRole({ role })}
      />
      <div className={styles.practitionerInfo}>
        <div className={practitionerHumanNameClass}>
          {prefixes && prefixes.length
          ? `${showFirstPrefixItem(prefixes)}${familyName}`.trim()
          : givenNames.join(' ')
        }
        </div>
        <div className={styles.practitionerInfoRole}>
          {role !== PractitionerRole.UNKNOWN && role}
        </div>
      </div>
    </button>
  );
};
