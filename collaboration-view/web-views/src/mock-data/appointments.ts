import { AppointmentStatus, AppointmentType, PractitionerRole } from '@enums';
import { AppointmentData } from '@types';

const getTimeSlot = () => {
  const startHour = Math.floor(Math.random() * 3) + 8;
  const startMin = Math.floor(Math.random() * 4) * 15;
  const duration = (Math.floor(Math.random() * 5) + 1) * 15;
  const endHour = (startHour + Math.floor((startMin + duration) / 60));
  const endMin = (startMin + duration) % 60;

  return { 
    startTime: `2022-01-01T${String(startHour).padStart(2, '0')}:${String(startMin).padStart(2, '0')}:00`, 
    endTime: `2022-01-01T${String(endHour).padStart(2, '0')}:${String(endMin).padStart(2, '0')}:00`, 
  };
};

const getIsProtectedByBtg = () => 
  !!Math.round(Math.random());

export const appointment: AppointmentData = 
  {
    id: '700011433',
    title: 'Established Patient Office Visit',
    type: AppointmentType.TELEHEALTH,
    status: AppointmentStatus.SCHEDULED,
    timeSlot: { 
      startTime: '2022-01-01T08:00:00', 
      endTime: '2022-01-01T08:30:00', 
    },
    patient: {
      patientGeneralInfo: {
        id: 'eug3DevbSDb7srIov6WmLuQ3',
        humanName: {
          familyName: 'Epam', 
          givenNames: [ 'Test1' ], 
        },
      },
      isNewPatient: true,
    },
    practitioner: {
      id: 'e0-aXEipNKuPaenqOQrCbZg3',
      humanName: {
        familyName: 'Fitzgerald', 
        givenNames: [ 'Michael', 'E' ], 
        prefixes: [ 'Dr.' ], 
      },
      photo: {
        contentType: 'image/jpg',
        title: 'photo',
        size: 0,
        url: 'https://dmgwebprodstorage.blob.core.windows.net/dmgprodweb/physician-headshots/Fitzgerald_Michael_FM-003websize.jpg',
      },
      role: PractitionerRole.PCP,
    },
    isProtectedByBtg: false,
  };

export const getMockAppointments = (quantity: number): AppointmentData[] => 
  new Array(quantity).fill('0')
    .map((el, i) => 
      ({ 
        ...appointment, 
        id: appointment.id + i,
        timeSlot: getTimeSlot(),
        isProtectedByBtg: getIsProtectedByBtg(),
      }));
