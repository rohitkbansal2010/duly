import { Gender, UserRoles } from '@enums';
import { PersonalData } from '@types';

const allPersons: PersonalData[] = [
  {
    id: 101,
    firstName: 'Ana',
    secondName: 'Maria',
    lastName: 'Reyes',
    avatarSmall: '/images/ana_reyes.png',
    avatarLarge: '/images/Reyes-142-142.png',
    birthDate: '04/02/1951',
    gender: Gender.FEMALE,
    userRole: UserRoles.PATIENT,
  },
  {
    id: 102,
    firstName: 'Annabell',
    lastName: 'Collins',
    avatarSmall: '/images/annabell_collins.png',
    avatarLarge: '/images/annabell_collins.png',
    birthDate: '04/02/1951',
    gender: Gender.FEMALE,
    userRole: UserRoles.PATIENT,
  },
  {
    id: 103,
    firstName: 'Duncan',
    lastName: 'Feeney',
    avatarSmall: '/images/duncan_feeney.png',
    avatarLarge: '/images/duncan_feeney.png',
    birthDate: '04/02/1951',
    gender: Gender.MALE,
    userRole: UserRoles.PATIENT,
  },
  {
    id: 104,
    firstName: 'Chelsey',
    lastName: 'Clemons',
    avatarSmall: '/images/chelsey_clemons.png',
    avatarLarge: '/images/chelsey_clemons.png',
    birthDate: '04/02/1951',
    gender: Gender.MALE,
    userRole: UserRoles.PATIENT,
  },
  {
    id: 105,
    firstName: 'Bradlee',
    lastName: 'Rhodes',
    avatarSmall: '/images/bradlee_rhodes.png',
    avatarLarge: '/images/bradlee_rhodes.png',
    birthDate: '04/02/1951',
    gender: Gender.MALE,
    userRole: UserRoles.PATIENT,
  },
  {
    id: 106,
    firstName: 'Patsy',
    lastName: 'Taylor',
    avatarSmall: '/images/patsy_taylor.png',
    avatarLarge: '/images/patsy_taylor.png',
    birthDate: '04/02/1951',
    gender: Gender.FEMALE,
    userRole: UserRoles.PATIENT,
  },
  {
    id: 201,
    firstName: 'Helen',
    lastName: 'Wilson',
    avatarSmall: '/images/Helen_Wilson.jpg',
    avatarLarge: '/images/Helen_Wilson.jpg',
    birthDate: '04/02/1951',
    gender: Gender.FEMALE,
    userRole: UserRoles.CARE_ALLY,
  },
  {
    id: 202,
    firstName: 'Susan',
    lastName: 'Ling',
    avatarSmall: '/images/Dr_Susan_Ling.jpg',
    avatarLarge: '/images/Dr_Susan_Ling.jpg',
    birthDate: '04/02/1951',
    gender: Gender.FEMALE,
    userRole: UserRoles.CARE_ALLY,
  },
];

// this util ought to stay here since it's connected to the fake data
export const pickPDById = (pickedId: number): PersonalData =>
  allPersons.filter(({ id }: PersonalData) =>
    (id === pickedId))[0];
