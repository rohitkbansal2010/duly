import {
  AppointmentModuleWidgetsAlias,
  AppointmentModulesAlias,
  WidgetsPriority
} from '@enums';
import { OverviewWidgetsAction } from '@redux/actions';

export type AppointmentModuleType = {
  id: number;
  alias: AppointmentModulesAlias;
  title: string;
  route?: string;
  icon?: string;
  module?: React.ElementType;
  externalLink?: React.ComponentType;
}

export type GetDataType = (params: {
  appointmentId: string;
  patientId: string;
}) => OverviewWidgetsAction

export type WidgetDataType = {
  id: number;
  alias: AppointmentModuleWidgetsAlias;
  route: string;
  headerTitle: string;
  navTitle: string;
  getData?: GetDataType;
}

export type ModuleDataType = {
  alias: string;
  widgets: WidgetDataType[];
}

export type ModulesDetailsDataType = {
  modules: ModuleDataType[];
}

export type ModulesDataType = {
  details: ModulesDetailsDataType;
  id: string;
  targetAreaType: string;
};

export type WidgetItem = {
  alias: AppointmentModuleWidgetsAlias;
  isSkeletonShown: boolean;
  priority: WidgetsPriority;
}
