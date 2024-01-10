import {
  BaseRoutes,
  CareAllyRoutes,
  NonAuthRoutes,
  PatientRoutes
} from '@enums';
import { HumanName } from '@types';

export const getUserRoleRoute = (url: string): BaseRoutes => {
  switch (url) {
    case NonAuthRoutes.login:
      return PatientRoutes.home;
    case NonAuthRoutes.care_ally_login:
      return CareAllyRoutes.home;
    default:
      return BaseRoutes.collaboration_view;
  }
};

export const writeHumanName = (arrayString: string[] | undefined, familyName: string): string =>
  `${arrayString ? arrayString.join(' ') : ''} ${familyName}`;

  

export const showFirstPrefixItem = (itemsList?: string[]) => {
  const firstItem = itemsList && !!itemsList[0] && itemsList[0].split(' ')[0];
  return firstItem ? `${firstItem} ` : '';
};

export const mergeHumanName = ({
  familyName = '',
  givenNames = [],
  prefixes = [],
}: HumanName): string =>
  `${familyName}${givenNames?.join('')}${prefixes?.join('')}`;
