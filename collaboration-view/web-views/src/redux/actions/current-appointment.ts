import { AppointmentModulesAlias, AppointmentModuleWidgetsAlias } from '@enums';
import {
  ModulesDataType,
  ModuleDataType,
  CareTeamMember,
  PatientData,
  WidgetItem,
  TestReport,
  TestReportRequest,
  TestReportsResult
} from '@types';

export enum CurrentAppointmentType {
  SHOW_MODULE_MENU = 'SHOW_MODULE_MENU',
  SHOW_CARE_TEAM = 'SHOW_CARE_TEAM',
  SET_CURRENT_APPOINTMENT_ID = 'SET_CURRENT_APPOINTMENT_ID',
  GET_CONFIGURATIONS_REQUEST = 'GET_CONFIGURATION_REQUEST',
  SET_CURRENT_APPOINTMENT_MODULES = 'SET_CURRENT_APPOINTMENT_MODULES',
  GET_CARE_TEAM_MEMBERS = 'GET_CARE_TEAM_MEMBERS',
  SET_CARE_TEAM_MEMBERS = 'SET_CARE_TEAM_MEMBERS',
  GET_PATIENT_DATA = 'GET_PATIENT_DATA',
  SET_PATIENT_DATA = 'SET_PATIENT_DATA',
  SET_PATIENT_ID = 'SET_PATIENT_ID',
  SET_PRACTITIONER_ID = 'SET_SET_PRACTITIONER_ID_ID',
  SHOW_PATIENT_VIEW_MODAL = 'SHOW_PATIENT_VIEW_MODAL',
  HIDE_PATIENT_VIEW_MODAL = 'HIDE_PATIENT_VIEW_MODAL',
  SHOW_CHART_SKELETON = 'SHOW_CHART_SKELETON',
  HIDE_CHART_SKELETON = 'HIDE_CHART_SKELETON',
  SET_WIDGETS_LIST = 'SET_WIDGETS_LIST',
  ADD_ACCORDION = 'ADD_ACCORDION',
  DELETE_ACCORDION = 'DELETE_ACCORDION',
  CLEAR_ACCORDIONS = 'CLEAR_ACCORDIONS',
  CLEAR_ALL_ACCORDIONS = 'CLEAR_ALL_ACCORDIONS',
  GET_WIDGETS_DATA = 'GET_WIDGETS_DATA',
  SET_SKELETON_SHOWN = 'SET_SKELETON_SHOWN',
  GET_TEST_REPORTS = 'GET_TEST_REPORTS',
  SET_TEST_REPORTS = 'SET_TEST_REPORTS',
  GET_TEST_REPORTS_RESULTS = 'GET_TEST_REPORTS_RESULTS',
  SET_TEST_REPORTS_RESULTS = 'SET_TEST_REPORTS_RESULTS',
  SET_TEST_REPORTS_SKELETON = 'SET_TEST_REPORTS_SKELETON',
  SET_TEST_REPORTS_RESULTS_SKELETON = 'SET_TEST_REPORTS_RESULTS_SKELETON',
  SET_TEST_RESULTS_MOUNTED = 'SET_TEST_RESULTS_MOUNTED',
  SET_TEST_RESULTS_NEXT_PAGE = 'SET_TEST_RESULTS_NEXT_PAGE',
}

type ShowModuleMenuAction = {
  type: typeof CurrentAppointmentType.SHOW_MODULE_MENU;
  payload: { isModuleMenuShown: boolean };
};

type ShowCareTeamAction = {
  type: typeof CurrentAppointmentType.SHOW_CARE_TEAM;
  payload: { isCareTeamShown: boolean };
};

type SetCurrentAppointmentIdAction = {
  type: typeof CurrentAppointmentType.SET_CURRENT_APPOINTMENT_ID;
  payload: { appointmentId: string };
};

export type GetConfigurationsAction = {
  type: typeof CurrentAppointmentType.GET_CONFIGURATIONS_REQUEST;
  payload: { patientId: string };
};

type SetCurrentAppointmentModulesAction = {
  type: typeof CurrentAppointmentType.SET_CURRENT_APPOINTMENT_MODULES;
  payload: { modules: ModuleDataType[] };
};

export type GetCareTeamMembers = {
  type: typeof CurrentAppointmentType.GET_CARE_TEAM_MEMBERS;
  payload: { appointmentId: string, patientId: string };
}

type SetCareTeamMembers = {
  type: typeof CurrentAppointmentType.SET_CARE_TEAM_MEMBERS;
  payload: { careTeamMembers: CareTeamMember[] };
}

export type GetPatientData = {
  type: typeof CurrentAppointmentType.GET_PATIENT_DATA;
  payload: { patientId: string };
};

type SetPatientData = {
  type: typeof CurrentAppointmentType.SET_PATIENT_DATA;
  payload: { patientData: PatientData };
};

type SetPatientId = {
  type: typeof CurrentAppointmentType.SET_PATIENT_ID;
  payload: { patientId: string };
};

type SetPractitionerId = {
  type: typeof CurrentAppointmentType.SET_PRACTITIONER_ID;
  payload: { practitionerId: string };
};

type ShowPatientViewModal = {
  type: typeof CurrentAppointmentType.SHOW_PATIENT_VIEW_MODAL;
};

type HidePatientViewModal = {
  type: typeof CurrentAppointmentType.HIDE_PATIENT_VIEW_MODAL;
};

type ShowChartSkeleton = {
  type: typeof CurrentAppointmentType.SHOW_CHART_SKELETON;
};

type HideChartSkeleton = {
  type: typeof CurrentAppointmentType.HIDE_CHART_SKELETON;
};

type SetWidgetsListAction = {
  type: typeof CurrentAppointmentType.SET_WIDGETS_LIST;
  payload: {
    currentModuleAlias: AppointmentModulesAlias,
    currentWidgetAlias: AppointmentModuleWidgetsAlias,
  };
};

type AddAccordionAction = {
  type: typeof CurrentAppointmentType.ADD_ACCORDION;
  payload: { groupId: string; accordionId: string; };
};

type DeleteAccordionAction = {
  type: typeof CurrentAppointmentType.DELETE_ACCORDION;
  payload: { groupId: string; accordionId: string; };
};

type ClearAccordionsAction = {
  type: typeof CurrentAppointmentType.CLEAR_ACCORDIONS;
  payload: { groupId: string; };
};

type ClearAllAccordionsAction = {
  type: typeof CurrentAppointmentType.CLEAR_ALL_ACCORDIONS;
};

export type WidgetsDataRequestParams = {
  widgetsList: WidgetItem[] | null;
  navigation: AppointmentModulesAlias | null;
  patientId: string;
  appointmentId: string
};

export type GetWidgetsDataAction = {
  type: typeof CurrentAppointmentType.GET_WIDGETS_DATA;
  payload: WidgetsDataRequestParams;
};

type SetSkeletonShown = {
  type: typeof CurrentAppointmentType.SET_SKELETON_SHOWN;
  payload: {
    widgetAlias: AppointmentModuleWidgetsAlias;
    isSkeletonShown: boolean;
  }
};

export type GetTestReports = {
  type: typeof CurrentAppointmentType.GET_TEST_REPORTS;
  payload: TestReportRequest;
};

type SetTestReports = {
  type: typeof CurrentAppointmentType.SET_TEST_REPORTS;
  payload: {
    testReports: TestReport[];
  }
};

export type GetTestReportsResults = {
  type: typeof CurrentAppointmentType.GET_TEST_REPORTS_RESULTS;
  payload: { testReportId: string };
};

type SetTestReportsResults = {
  type: typeof CurrentAppointmentType.SET_TEST_REPORTS_RESULTS;
  payload: {
    testReportsResults: TestReportsResult;
  }
};

type SetTestReportsSkeleton = {
  type: typeof CurrentAppointmentType.SET_TEST_REPORTS_SKELETON;
  payload: { isTestReportsSkeletonShown: boolean };
};

type SetTestReportsResultsSkeleton = {
  type: typeof CurrentAppointmentType.SET_TEST_REPORTS_RESULTS_SKELETON;
  payload: { isTestReportResultsSkeletonShown: boolean };
};

type SetTestResultsMounted = {
  type: typeof CurrentAppointmentType.SET_TEST_RESULTS_MOUNTED;
  payload: { isTestResultsMounted: boolean };
};

type SetTestResultsNextPage = {
  type: typeof CurrentAppointmentType.SET_TEST_RESULTS_NEXT_PAGE;
};

export type CurrentAppointmentAction =
  ShowModuleMenuAction |
  ShowCareTeamAction |
  SetCurrentAppointmentIdAction |
  GetConfigurationsAction |
  SetCurrentAppointmentModulesAction |
  GetCareTeamMembers |
  SetCareTeamMembers |
  GetPatientData |
  SetPatientData |
  SetPatientId |
  SetPractitionerId |
  ShowPatientViewModal |
  HidePatientViewModal |
  ShowChartSkeleton |
  HideChartSkeleton |
  AddAccordionAction |
  DeleteAccordionAction |
  ClearAccordionsAction |
  ClearAllAccordionsAction |
  SetWidgetsListAction |
  GetWidgetsDataAction |
  SetSkeletonShown |
  GetTestReports |
  SetTestReports |
  GetTestReportsResults |
  SetTestReportsResults |
  SetTestReportsSkeleton |
  SetTestReportsResultsSkeleton |
  SetTestResultsMounted |
  SetTestResultsNextPage;

export const showModuleMenu = (isModuleMenuShown: boolean): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SHOW_MODULE_MENU,
    payload: { isModuleMenuShown },
  });

export const showCareTeam = (isCareTeamShown: boolean): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SHOW_CARE_TEAM,
    payload: { isCareTeamShown },
  });

export const setCurrentAppointmentId = (appointmentId: string): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SET_CURRENT_APPOINTMENT_ID,
    payload: { appointmentId },
  });

export const getConfigurationsSaga =
  (patientId: string): CurrentAppointmentAction =>
    ({
      type: CurrentAppointmentType.GET_CONFIGURATIONS_REQUEST,
      payload: { patientId },
    });

export const setCurrentAppointmentModules = (
  modulesData: ModulesDataType[]
): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SET_CURRENT_APPOINTMENT_MODULES,
    payload: { modules: modulesData[0].details.modules },
  });

export const getCareTeamMembers = (
  appointmentId: string,
  patientId: string
): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.GET_CARE_TEAM_MEMBERS,
    payload: { appointmentId, patientId },
  });

export const setCareTeamMembers = (careTeamMembers: CareTeamMember[]): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SET_CARE_TEAM_MEMBERS,
    payload: { careTeamMembers },
  });

export const getPatientData = (patientId: string): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.GET_PATIENT_DATA,
    payload: { patientId },
  });

export const setPatientData = (patientData: PatientData): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SET_PATIENT_DATA,
    payload: { patientData },
  });

export const setPatientId = (patientId: string): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SET_PATIENT_ID,
    payload: { patientId },
  });

export const setPractitionerId = (practitionerId: string): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SET_PRACTITIONER_ID,
    payload: { practitionerId },
  });

export const showPatientViewModal = (): CurrentAppointmentAction =>
  ({ type: CurrentAppointmentType.SHOW_PATIENT_VIEW_MODAL });

export const hidePatientViewModal = (): CurrentAppointmentAction =>
  ({ type: CurrentAppointmentType.HIDE_PATIENT_VIEW_MODAL });

export const showChartSkeleton = (): CurrentAppointmentAction =>
  ({ type: CurrentAppointmentType.SHOW_CHART_SKELETON });

export const hideChartSkeleton = (): CurrentAppointmentAction =>
  ({ type: CurrentAppointmentType.HIDE_CHART_SKELETON });

export const setWidgetsList = (
  currentModuleAlias: AppointmentModulesAlias,
  currentWidgetAlias: AppointmentModuleWidgetsAlias
): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SET_WIDGETS_LIST,
    payload: { currentModuleAlias, currentWidgetAlias },
  });

export const getWidgetsData = (payload: WidgetsDataRequestParams): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.GET_WIDGETS_DATA,
    payload,
  });

export const addAccordion = (groupId: string, accordionId: string): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.ADD_ACCORDION,
    payload: { groupId, accordionId },
  });

export const deleteAccordion = (groupId: string, accordionId: string): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.DELETE_ACCORDION,
    payload: { groupId, accordionId },
  });

export const clearAccordions = (groupId: string): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.CLEAR_ACCORDIONS,
    payload: { groupId },
  });

export const clearAllAccordions = (): CurrentAppointmentAction =>
  ({ type: CurrentAppointmentType.CLEAR_ALL_ACCORDIONS });

export const setSkeletonShown = (
  widgetAlias: AppointmentModuleWidgetsAlias,
  isSkeletonShown: boolean
): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SET_SKELETON_SHOWN,
    payload: { widgetAlias, isSkeletonShown },
  });

export const getTestReports = ({
  patientId,
  startDate,
  endDate,
  amount,
}: TestReportRequest): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.GET_TEST_REPORTS,
    payload: {
      patientId, startDate, endDate, amount,
    },
  });

export const setTestReports = (testReports: TestReport[]): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.SET_TEST_REPORTS,
    payload: { testReports },
  });

export const getTestReportsResults = (testReportId: string): CurrentAppointmentAction =>
  ({
    type: CurrentAppointmentType.GET_TEST_REPORTS_RESULTS,
    payload: { testReportId },
  });

export const setTestReportsResults =
  (testReportsResults: TestReportsResult): CurrentAppointmentAction =>
    ({
      type: CurrentAppointmentType.SET_TEST_REPORTS_RESULTS,
      payload: { testReportsResults },
    });

export const setTestReportsSkeleton =
  (isTestReportsSkeletonShown: boolean): CurrentAppointmentAction =>
    ({
      type: CurrentAppointmentType.SET_TEST_REPORTS_SKELETON,
      payload: { isTestReportsSkeletonShown },
    });

export const setTestReportsResultsSkeleton =
  (isTestReportResultsSkeletonShown: boolean): CurrentAppointmentAction =>
    ({
      type: CurrentAppointmentType.SET_TEST_REPORTS_RESULTS_SKELETON,
      payload: { isTestReportResultsSkeletonShown },
    });

export const setTestResultsMounted =
  (isTestResultsMounted: boolean): CurrentAppointmentAction =>
    ({
      type: CurrentAppointmentType.SET_TEST_RESULTS_MOUNTED,
      payload: { isTestResultsMounted },
    });

export const setTestResultsNextPage =
  (): CurrentAppointmentAction =>
    ({ type: CurrentAppointmentType.SET_TEST_RESULTS_NEXT_PAGE });
