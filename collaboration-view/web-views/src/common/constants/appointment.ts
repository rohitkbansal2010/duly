import { AppointmentServiceType, AppointmentStatus } from '@enums';
import { checkFillDarkBlueIcon, userCircleDarkBlueIcon } from '@icons';

type AppointmentStatusType = {
  [key in AppointmentStatus]: {
    icon: string;
    name: string;
  }
}

export const appointmentStatus: AppointmentStatusType = {
  [AppointmentStatus.CANCELED]: {
    icon: checkFillDarkBlueIcon,
    name: 'CANCELED',
  },
  [AppointmentStatus.CONFIRMED]: {
    icon: checkFillDarkBlueIcon,
    name: 'CONFIRMED',
  },
  [AppointmentStatus.ARRIVED]: {
    icon: checkFillDarkBlueIcon,
    name: 'ARRIVED',
  },
  [AppointmentStatus.IN_PROGRESS]: {
    icon: userCircleDarkBlueIcon,
    name: 'IN PROGRESS',
  },
  [AppointmentStatus.PLANNED]: {
    icon: checkFillDarkBlueIcon,
    name: 'PLANNED',
  },
  [AppointmentStatus.FINISHED]: {
    icon: checkFillDarkBlueIcon,
    name: 'FINISHED',
  },
  [AppointmentStatus.COMPLETED]: {
    icon: checkFillDarkBlueIcon,
    name: 'COMPLETED',
  },
};

type AppointmentColorPalleteType = {
  [key in AppointmentServiceType]: string;
}

export const appointmentColorPallete: AppointmentColorPalleteType = {
  [AppointmentServiceType.PHYSICAL_EXAM]: 'Orange',
  [AppointmentServiceType.WELLNESS_CHECK]: 'Blue',
  [AppointmentServiceType.CHECK_UP]: 'Violet',
};

export const appointmentItemUI = {
  BASIC_SCREEN_WIDTH: 1920,
  MIN_WIDTH: 52,
};
