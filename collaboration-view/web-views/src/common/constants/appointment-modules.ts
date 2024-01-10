import { MockModule } from '@components/mock-module';
import { ResultsModule } from '@components/results-module';
import { TelehealthLink } from '@components/telehealth-link';
import {
  AppointmentModulesAlias,
  AppointmentModulesRoutes,
  AppointmentModulesTitles
} from '@enums';
import {
  carePlanWhiteIcon,
  educationWhiteIcon,
  overviewWhiteIcon,
  resultsWhiteIcon,
  telehealthWhiteIcon
} from '@icons';
import { AppointmentModuleType } from '@types';

export const appointmentModulesIcons = {
  MODULE_OVERVIEW: overviewWhiteIcon,
  MODULE_CARE_PLAN: carePlanWhiteIcon,
  MODULE_EDUCATION: educationWhiteIcon,
  MODULE_RESULTS: resultsWhiteIcon,
  MODULE_TELEHEALTH: telehealthWhiteIcon,
};

export const appointmentModules: AppointmentModuleType[] = [
  {
    id: 1,
    alias: AppointmentModulesAlias.MODULE_OVERVIEW,
    route: AppointmentModulesRoutes.MODULE_OVERVIEW,
    icon: appointmentModulesIcons.MODULE_OVERVIEW,
    title: AppointmentModulesTitles.MODULE_OVERVIEW,
  },
  {
    id: 2,
    alias: AppointmentModulesAlias.MODULE_CARE_PLAN,
    route: AppointmentModulesRoutes.MODULE_CARE_PLAN,
    icon: appointmentModulesIcons.MODULE_CARE_PLAN,
    title: AppointmentModulesTitles.MODULE_CARE_PLAN,
    module: MockModule,
  },
  {
    id: 3,
    alias: AppointmentModulesAlias.MODULE_EDUCATION,
    route: AppointmentModulesRoutes.MODULE_EDUCATION,
    icon: appointmentModulesIcons.MODULE_EDUCATION,
    title: AppointmentModulesTitles.MODULE_EDUCATION,
    module: MockModule,
  },
  {
    id: 4,
    alias: AppointmentModulesAlias.MODULE_RESULTS,
    route: AppointmentModulesRoutes.MODULE_RESULTS,
    icon: appointmentModulesIcons.MODULE_RESULTS,
    title: AppointmentModulesTitles.MODULE_RESULTS,
    module: ResultsModule,
  },
  {
    id: 5,
    alias: AppointmentModulesAlias.MODULE_TELEHEALTH,
    title: AppointmentModulesTitles.MODULE_TELEHEALTH,
    externalLink: TelehealthLink,
  },
];
