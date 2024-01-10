import {
  AppointmentModulesAlias,
  AppointmentModuleWidgetsAlias,
  AppointmentModuleWidgetsRoutes,
  AppointmentModuleWidgetsHeaderTitles,
  AppointmentModuleWidgetsNavTitles
} from '@enums';
import {
  getAllergiesWidgetData,
  getConditionsWidgetData,
  getImmunizationsWidgetData,
  getMedicationsWidgetData,
  getPatientAppointments,
  getTodaysVitals
} from '@redux/actions';
import { WidgetDataType } from '@types';

export const appointmentModulesWidgets: Record<AppointmentModulesAlias, WidgetDataType[]> = {
  [AppointmentModulesAlias.MODULE_OVERVIEW]: [
    {
      id: 0,
      alias: AppointmentModuleWidgetsAlias.VITALS,
      route: AppointmentModuleWidgetsRoutes.VITALS,
      headerTitle: AppointmentModuleWidgetsHeaderTitles.VITALS,
      navTitle: AppointmentModuleWidgetsNavTitles.VITALS,
      getData: getTodaysVitals,
    },
    {
      id: 1,
      alias: AppointmentModuleWidgetsAlias.CONDITIONS,
      route: AppointmentModuleWidgetsRoutes.CONDITIONS,
      headerTitle: AppointmentModuleWidgetsHeaderTitles.CONDITIONS,
      navTitle: AppointmentModuleWidgetsNavTitles.CONDITIONS,
      getData: getConditionsWidgetData,
    },
    {
      id: 2,
      alias: AppointmentModuleWidgetsAlias.TIMELINE,
      route: AppointmentModuleWidgetsRoutes.TIMELINE,
      headerTitle: AppointmentModuleWidgetsHeaderTitles.TIMELINE,
      navTitle: AppointmentModuleWidgetsNavTitles.TIMELINE,
      getData: getPatientAppointments,
    },
    {
      id: 3,
      alias: AppointmentModuleWidgetsAlias.ALLERGIES,
      route: AppointmentModuleWidgetsRoutes.ALLERGIES,
      headerTitle: AppointmentModuleWidgetsHeaderTitles.ALLERGIES,
      navTitle: AppointmentModuleWidgetsNavTitles.ALLERGIES,
      getData: getAllergiesWidgetData,
    },
    {
      id: 4,
      alias: AppointmentModuleWidgetsAlias.MEDICATIONS,
      route: AppointmentModuleWidgetsRoutes.MEDICATIONS,
      headerTitle: AppointmentModuleWidgetsHeaderTitles.MEDICATIONS,
      navTitle: AppointmentModuleWidgetsNavTitles.MEDICATIONS,
      getData: getMedicationsWidgetData,
    },
    {
      id: 5,
      alias: AppointmentModuleWidgetsAlias.IMMUNIZATIONS,
      route: AppointmentModuleWidgetsRoutes.IMMUNIZATIONS,
      headerTitle: AppointmentModuleWidgetsHeaderTitles.IMMUNIZATIONS,
      navTitle: AppointmentModuleWidgetsNavTitles.IMMUNIZATIONS,
      getData: getImmunizationsWidgetData,
    },
  ],
  [AppointmentModulesAlias.MODULE_CARE_PLAN]: [],
  [AppointmentModulesAlias.MODULE_EDUCATION]: [],
  [AppointmentModulesAlias.MODULE_RESULTS]: [],
  [AppointmentModulesAlias.MODULE_TELEHEALTH]: [],
};
