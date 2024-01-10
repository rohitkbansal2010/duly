import {
  healthBlackIcon,
  calendarBlackIcon,
  messagesBlackIcon,
  listUiAltIcon,
  archeryIcon
} from '@icons';

type PatientWidgetType = {
  icon: string;
  title: string;
  count?: number;
};

export const patientWidgets: PatientWidgetType[] = [
  {
    icon: calendarBlackIcon,
    title: 'Upcoming Appointments',
    count: 2,
  },
  {
    icon: messagesBlackIcon,
    title: 'Messages',
    count: 3,
  },
  {
    icon: healthBlackIcon,
    title: 'Care Team',
  },
  {
    icon: archeryIcon,
    title: 'Goals',
  },
  {
    icon: listUiAltIcon,
    title: 'Tasks',
  },
];

export const emptyPatientWidget: PatientWidgetType = {
  icon: '',
  title: '',
  count: 0,
};
