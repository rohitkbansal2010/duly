import { PatientRoutes } from '@enums';
import {
  calendarDarkGrayIcon,
  messagesDarkGrayIcon,
  readerDarkGrayIcon,
  heartRateMachineDarkGrayIcon,
  medicineBottleDarkGrayIcon,
  thermometerDarkGrayIcon,
  medicalKitDarkGrayIcon
} from '@icons';

export type QuickLink = {
  id: number;
  icon: string;
  title: string;
  route?: string;
  alertCount?: number;
}

export const quickLinks: QuickLink[] = [
  {
    id: 1,
    icon: calendarDarkGrayIcon,
    title: 'appointments',
    route: PatientRoutes.patient_calendar,
    alertCount: 3,
  },
  {
    id: 2,
    icon: messagesDarkGrayIcon,
    title: 'messages',
    route: PatientRoutes.messages,
    alertCount: 3,
  },
  {
    id: 3,
    icon: heartRateMachineDarkGrayIcon,
    title: 'test results',
    route: PatientRoutes.home,
    alertCount: 1,
  },
  {
    id: 4,
    icon: medicineBottleDarkGrayIcon,
    title: 'medications',
    route: PatientRoutes.medications,
  },
  {
    id: 5,
    icon: thermometerDarkGrayIcon,
    title: 'symptoms',
    route: PatientRoutes.symptoms,
  },
  {
    id: 6,
    icon: medicalKitDarkGrayIcon,
    title: 'providers',
    route: PatientRoutes.providers,
  },
  {
    id: 7,
    icon: readerDarkGrayIcon,
    title: 'health summary',
    route: PatientRoutes.health_summary,
  },
];
