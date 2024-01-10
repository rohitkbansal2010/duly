import {
  ACCESS_TOKEN,
  ACCOUNT,
  EXPIRES_ON,
  TIME_OF_AUTO_LOGOUT
} from '@constants';
import { AvatarByRole, CareTeamMemberRole, DulyRoles } from '@enums';

import { removeLocalStorageItem, setLocalStorageItem } from './local-storage';

type ChangeAvatarByRoleProps = {
  role?: string,
  prefixes?: string[],
}

export const getUserRole = ({
  role = '',
  prefixes = [],
}: ChangeAvatarByRoleProps) => {
  const nonEmployeeRoles: string[] = [
    DulyRoles.PATIENT,
    DulyRoles.CAREGIVER,
    CareTeamMemberRole.ANOTHER,
  ];

  const doctorRoles: string[] = [
    DulyRoles.PCP,
    CareTeamMemberRole.DOCTOR,
  ];

  if (prefixes && prefixes?.length || doctorRoles.includes(role)) {
    return AvatarByRole.MEDICAL_DOCTOR;
  }

  if (nonEmployeeRoles.includes(role)) {
    return AvatarByRole.NON_EMPLOYEE;
  }

  return AvatarByRole.REGULAR_EMPLOYEE;
};

export const cleanAccessData = () => {
  removeLocalStorageItem(ACCESS_TOKEN);
  removeLocalStorageItem(ACCOUNT);
  removeLocalStorageItem(EXPIRES_ON);
  removeLocalStorageItem(TIME_OF_AUTO_LOGOUT);
};

export const setTimeOfAutoLogout = () => {
  const date = new Date();
  date.setMinutes(date.getMinutes() + +window?.env?.LOGOUT_AFTER_MINUTES || 5);

  setLocalStorageItem(TIME_OF_AUTO_LOGOUT, String(date.getTime()));
};
