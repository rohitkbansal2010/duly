import { PatientAppointmentAlias } from '@enums';
import { calendarTwoBlueIcon, calendarTwoMagentaIcon } from '@icons';

type TimelineAppointmentsMapType = {
  [key in PatientAppointmentAlias]: {
    title: string;
    colorTitle: string;
    icon: string;
    backgroundIcon: string;
    backgroundCount: string;
    skeletonCount?: number;
    showNoShowStatusCount: boolean,
    showNoShowCancelledCount: boolean,
  };
};

export const timelineAppointmentsMap: TimelineAppointmentsMapType = {
  [PatientAppointmentAlias.RECENT]: {
    title: 'Recent Appointments',
    colorTitle: 'LightBlue',
    icon: calendarTwoBlueIcon,
    backgroundIcon: 'Blue',
    backgroundCount: 'DarkBlue',
    skeletonCount: 6,
    showNoShowStatusCount: true,
    showNoShowCancelledCount: true,
  },
  [PatientAppointmentAlias.UPCOMING]: {
    title: 'Upcoming Appointments',
    colorTitle: 'Magenta',
    icon: calendarTwoMagentaIcon,
    backgroundIcon: 'Magenta',
    backgroundCount: 'Violet',
    skeletonCount: 6,
    showNoShowStatusCount: false,
    showNoShowCancelledCount: true,
  },
};
