import uniqBy from 'lodash/uniqBy';

import {
  MAX_PETAL_PRACTITIONERS_COUNT,
  SEPARATED_PETAL_PRACTITIONERS_COUNT
} from '@constants';
import {
  CommonPractitionerType,
  ExtendedCommonPractitionerType,
  AppointmentData,
  Practitioner
} from '@types';

import { mergeHumanName } from './person';

const getIsNeedToSeparate = (practitioners: CommonPractitionerType[]): boolean =>
  practitioners.length > MAX_PETAL_PRACTITIONERS_COUNT;

const getFirstPractitioners = (
  practitioners: CommonPractitionerType[]
): CommonPractitionerType[] =>
  getIsNeedToSeparate(practitioners)
    ? practitioners.slice(0, SEPARATED_PETAL_PRACTITIONERS_COUNT)
    : practitioners;

const getOtherPractitioners = (
  practitioners: CommonPractitionerType[]
): CommonPractitionerType[] =>
  getIsNeedToSeparate(practitioners)
    ? practitioners.slice(SEPARATED_PETAL_PRACTITIONERS_COUNT, practitioners.length)
    : [];

export const getPractitionersGroups = (
  practitioners: ExtendedCommonPractitionerType[]
): { [key: string]: ExtendedCommonPractitionerType[] } =>
  ({
    firstPractitioners: getFirstPractitioners(practitioners),
    otherPractitioners: getOtherPractitioners(practitioners),
  });

export const getPractitionersFromAppointments = (
  appointments: AppointmentData[]
): Practitioner[] => {
  const allPractitioners = appointments.map(({ practitioner }) =>
    practitioner);
  const uniqPractitioners = uniqBy(allPractitioners, 'id');

  return uniqPractitioners;
};

export const getPractitionersWithoutUser = (
  practitioners: Practitioner[],
  user: Practitioner
): Practitioner[] =>
  practitioners.filter(({ humanName }) =>
    mergeHumanName(humanName) !== mergeHumanName(user.humanName));

export const setActiveUser = (
  careTeamMembers: CommonPractitionerType[]
): ExtendedCommonPractitionerType[] =>
  careTeamMembers.map((member, index) =>
    ({ ...member, isCurrentUser: index === 0 }));

export const getPractitionersSuffix = (suffixes: string[]): string => {
  let specialty = '';
  suffixes.forEach((suffix) => {
    specialty += suffix + ', ';
  });
  
  specialty = specialty.slice(0, specialty.length - 2);
  return specialty;
};
