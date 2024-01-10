import {
  PractitionerRole,
  PatientAppointmentAlias,
  PatientAppointmentStatus
} from '@enums';
import { PatientAppointments } from '@types';

export const timelineAppointmentsMockData: PatientAppointments = {
  [PatientAppointmentAlias.RECENT]: [
    {
      title: 'Physical Therapy',
      nearestAppointmentDate: '2021-01-05',
      farthestAppointmentDate: '2018-01-05',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Ling',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          minutesDuration: 30,
          status: PatientAppointmentStatus.COMPLETED,
        },
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Hip check - Physical Therapy',
          minutesDuration: 60,
          status: PatientAppointmentStatus.NO_SHOW,
        },
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Hip check - Physical Therapy',
          minutesDuration: 60,
          status: PatientAppointmentStatus.CANCELLED,
          isTelehealth: true,
        },
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ree-Susmann',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          status: PatientAppointmentStatus.COMPLETED,
        },
      ],
    },
    {
      title: 'Wellness Check',
      nearestAppointmentDate: '2021-01-05',
      farthestAppointmentDate: '2018-01-05',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Renolds',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          status: PatientAppointmentStatus.COMPLETED,
        },
      ],
    },
    {
      title: 'Wellness Check',
      nearestAppointmentDate: '2021-01-05',
      farthestAppointmentDate: '2018-01-05',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Renolds',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          minutesDuration: 30,
          status: PatientAppointmentStatus.COMPLETED,
        },
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Hip check - Physical Therapy',
          status: PatientAppointmentStatus.COMPLETED,
        },
      ],
    },
    {
      title: 'Check up',
      nearestAppointmentDate: '2021-01-05',
      farthestAppointmentDate: '2018-01-05',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Ree-Susmann',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          minutesDuration: 30,
          status: PatientAppointmentStatus.COMPLETED,
          isTelehealth: true,
        },
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Hip check - Physical Therapy',
          minutesDuration: 60,
          status: PatientAppointmentStatus.CANCELLED,
        },
      ],
    },
    {
      title: 'Wellness Check',
      nearestAppointmentDate: '2021-01-05',
      farthestAppointmentDate: '2018-01-05',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Renolds',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          status: PatientAppointmentStatus.CANCELLED,
        },
      ],
    },
    {
      title: 'Check up',
      nearestAppointmentDate: '2021-01-05',
      farthestAppointmentDate: '2018-01-05',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Ree-Susmann',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          minutesDuration: 30,
          status: PatientAppointmentStatus.CANCELLED,
        },
      ],
    },
  ],
  [PatientAppointmentAlias.UPCOMING]: [
    {
      title: 'Wellness Check',
      nearestAppointmentDate: '2021-01-05',
      farthestAppointmentDate: '2018-01-05',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Renolds',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Hip check - Physical Therapy',
          minutesDuration: 60,
          status: PatientAppointmentStatus.SCHEDULED,
        },
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Hip check - Physical Therapy',
          status: PatientAppointmentStatus.SCHEDULED,
          isTelehealth: true,
        },
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ree-Susmann',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          minutesDuration: 30,
          status: PatientAppointmentStatus.SCHEDULED,
        },
      ],
    },
    {
      title: 'Physical Therapy',
      nearestAppointmentDate: '2021-01-05',
      farthestAppointmentDate: '2018-01-05',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Ling',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ree-Susmann',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          minutesDuration: 30,
          status: PatientAppointmentStatus.SCHEDULED,
        },
      ],
    },
    {
      title: 'Check up',
      nearestAppointmentDate: '2021-01-05',
      farthestAppointmentDate: '2018-01-05',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Ree-Susmann',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: '2018-01-05T13:30:00Z',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ree-Susmann',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          minutesDuration: 30,
          status: PatientAppointmentStatus.SCHEDULED,
        },
      ],
    },
  ],
};
