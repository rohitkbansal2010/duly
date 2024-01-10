import { AppointmentModulesAlias, AppointmentModulesRoutes } from '@enums';
import { CurrentAppointmentAction, getTestReports, getWidgetsData } from '@redux/actions';
import { WidgetItem } from '@types';
import { getTestReportPeriod } from '@utils';

type GetActionByModule = (
  parameters: {
    widgetsList: WidgetItem[] | null;
    navigation: AppointmentModulesAlias | null;
    patientId: string;
    appointmentId: string;
  },
  module: string
) => CurrentAppointmentAction | void;

export const getActionByModule: GetActionByModule = (
  {
    widgetsList,
    navigation,
    patientId,
    appointmentId,
  },
  module
) => {
  switch (module) {
    case AppointmentModulesRoutes.MODULE_OVERVIEW:
      return getWidgetsData({
        widgetsList,
        navigation,
        patientId,
        appointmentId,
      });

    case AppointmentModulesRoutes.MODULE_RESULTS:
      return getTestReports({
        patientId,
        ...getTestReportPeriod(),
      });

    case AppointmentModulesRoutes.MODULE_CARE_PLAN:
    case AppointmentModulesRoutes.MODULE_EDUCATION:
      break;
  }
};
