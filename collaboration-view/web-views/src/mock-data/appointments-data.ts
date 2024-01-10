import { AppointmentStatus, AppointmentType, PractitionerRole } from '@enums';
import { AppointmentData, DailyStatistics } from '@types';

export const appointmentsMockData: AppointmentData[] = [
  {
    id: 'qwerty1',
    title: 'Check Up',
    type: AppointmentType.ON_SITE,
    status: AppointmentStatus.PLANNED,
    timeSlot: {
      startTime: '2021-12-23T08:45:00',
      endTime: '2021-12-23T09:00:00',
    },
    location: { title: 'Room #4' },
    patient: {
      patientGeneralInfo: {
        id: 'qwerty1',
        humanName: {
          familyName: 'Miller',
          givenNames: [ 'Patricia' ],
        },
      },
      isNewPatient: false,
    },
    practitioner: {
      id: 'qwerty1',
      humanName: {
        familyName: 'Novikov',
        givenNames: [ 'Maxim' ],
        prefixes: [ 'Dr.' ],
      },
      role: PractitionerRole.PCP,
    },
  },
  {
    id: 'qwerty2',
    title: 'Physical Exam',
    type: AppointmentType.ON_SITE,
    status: AppointmentStatus.FINISHED,
    timeSlot: {
      startTime: '2021-12-23T08:00:00',
      endTime: '2021-12-23T08:45:00',
    },
    location: { title: 'Room #7' },
    patient: {
      patientGeneralInfo: {
        id: 'qwerty2',
        humanName: {
          familyName: 'Novikov',
          givenNames: [ 'Maxim' ],
        },
      },
      isNewPatient: true,
    },
    practitioner: {
      id: 'qwerty2',
      humanName: {
        familyName: 'Ling',
        givenNames: [ 'Patricia' ],
        prefixes: [ 'Dr.' ],
      },
      role: PractitionerRole.PCP,
    },
  },
  {
    id: 'qwerty3',
    title: 'Check Up',
    type: AppointmentType.ON_SITE,
    status: AppointmentStatus.ARRIVED,
    timeSlot: {
      startTime: '2021-12-23T10:00:00',
      endTime: '2021-12-23T10:15:00',
    },
    location: { title: 'Room #3' },
    patient: {
      patientGeneralInfo: {
        id: 'qwerty3',
        humanName: {
          familyName: 'Linda',
          givenNames: [ 'Davis' ],
        },
      },
      isNewPatient: true,
    },
    practitioner: {
      id: 'qwerty3',
      humanName: {
        familyName: 'Cozart',
        givenNames: [ 'Patricia' ],
      },
      role: PractitionerRole.PCP,
    },
  },
  {
    id: 'qwerty4',
    title: 'Check Up',
    type: AppointmentType.ON_SITE,
    status: AppointmentStatus.IN_PROGRESS,
    timeSlot: {
      startTime: '2021-12-23T09:15:00',
      endTime: '2021-12-23T09:30:00',
    },
    location: { title: 'Room #4' },
    patient: {
      patientGeneralInfo: {
        id: 'qwerty4',
        humanName: {
          familyName: 'Smith',
          givenNames: [ 'John' ],
        },
      },
      isNewPatient: false,
    },
    practitioner: {
      id: 'qwerty4',
      humanName: {
        familyName: 'Ling',
        givenNames: [ 'Patricia' ],
        prefixes: [ 'Dr.' ],
      },
      role: PractitionerRole.PCP,
    },
  },
  {
    id: 'qwerty5',
    title: 'Wellness Check',
    type: AppointmentType.ON_SITE,
    status: AppointmentStatus.IN_PROGRESS,
    timeSlot: {
      startTime: '2021-12-23T09:30:00',
      endTime: '2021-12-23T10:00:00',
    },
    location: { title: 'Room #5' },
    patient: {
      patientGeneralInfo: {
        id: 'qwerty5',
        humanName: {
          familyName: 'Reyes',
          givenNames: [ 'Ana', 'Maria' ],
        },
      },
      isNewPatient: true,
    },
    practitioner: {
      id: 'qwerty5',
      humanName: {
        familyName: 'Ling',
        givenNames: [ 'Patricia' ],
        prefixes: [ 'Dr.' ],
      },
      role: PractitionerRole.PCP,
    },
  },
  {
    id: 'qwerty6',
    title: 'Physical Exam',
    type: AppointmentType.ON_SITE,
    status: AppointmentStatus.IN_PROGRESS,
    timeSlot: {
      startTime: '2021-12-23T09:00:00',
      endTime: '2021-12-23T09:45:00',
    },
    location: { title: 'Room #6' },
    patient: {
      patientGeneralInfo: {
        id: 'qwerty6',
        humanName: {
          familyName: 'Barbara',
          givenNames: [ 'Williams' ],
        },
      },
      isNewPatient: false,
    },
    practitioner: {
      id: 'qwerty2',
      humanName: {
        familyName: 'Palmero',
        givenNames: [ 'Patricia' ],
      },
      role: PractitionerRole.MA,
    },
  },
  {
    id: 'qwerty7',
    title: 'Wellness Check',
    type: AppointmentType.TELEHEALTH,
    status: AppointmentStatus.IN_PROGRESS,
    timeSlot: {
      startTime: '2021-12-23T10:30:00',
      endTime: '2021-12-23T11:00:00',
    },
    location: { title: 'Room #5' },
    patient: {
      patientGeneralInfo: {
        id: 'qwerty7',
        humanName: {
          familyName: 'Lucas',
          givenNames: [ 'Garcia' ],
        },
      },
      isNewPatient: false,
    },
    practitioner: {
      id: 'qwerty1',
      humanName: {
        familyName: 'Ling',
        givenNames: [ 'Patricia' ],
        prefixes: [ 'Dr.' ],
      },
      role: PractitionerRole.PCP,
    },
  },
];

export const dailyStatisticsMockData: DailyStatistics = { newPatients: 2 };
