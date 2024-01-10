import { AppointmentModulesAlias } from '@enums';
import {
  CurrentAppointmentAction,
  CurrentAppointmentType,
  UIActions,
  UIType
} from '@redux/actions';
import {
  CareTeamMember,
  ModuleDataType,
  PatientData,
  TestReport,
  TestReportsResult,
  WidgetItem
} from '@types';
import { getWidgetsListWithPriority } from '@utils';

export type CurrentAppointmentStateType = {
  isModuleMenuShown: boolean;
  isCareTeamShown: boolean;
  appointmentId: string;
  navigation: AppointmentModulesAlias | null;
  modules: ModuleDataType[];
  careTeamMembers: CareTeamMember[];
  patientData: PatientData | null;
  patientId: string;
  practitionerId: string;
  isShowPatientViewModal: boolean;
  isSkeletonChartShown: boolean;
  widgetsList: WidgetItem[] | null;
  accordions: { [key in string]: string[] },
  testReports: TestReport[] | null;
  testReportsResults: TestReportsResult | null;
  isTestReportsSkeletonShown: boolean;
  isTestReportResultsSkeletonShown: boolean;
  page: number;
  isTestReportFetching: boolean;
  isTestResultsMounted: boolean;
};

const currentAppointmentState: CurrentAppointmentStateType = {
  isModuleMenuShown: false,
  isCareTeamShown: false,
  appointmentId: '',
  navigation: null,
  modules: [],
  careTeamMembers: [],
  patientData: null,
  patientId: '',
  practitionerId: '',
  isShowPatientViewModal: false,
  isSkeletonChartShown: true,
  widgetsList: null,
  accordions: {},
  testReports: null,
  testReportsResults: null,
  isTestReportsSkeletonShown: true,
  isTestReportResultsSkeletonShown: true,
  page: 0,
  isTestReportFetching: false,
  isTestResultsMounted: false,
};

export const currentAppointmentReducer = (
  state = currentAppointmentState,
  action: CurrentAppointmentAction | UIActions
): CurrentAppointmentStateType => {
  switch (action.type) {
    case CurrentAppointmentType.SHOW_MODULE_MENU:
      return {
        ...state,
        isModuleMenuShown: action.payload.isModuleMenuShown,
      };

    case CurrentAppointmentType.SHOW_CARE_TEAM:
      return {
        ...state,
        isCareTeamShown: action.payload.isCareTeamShown,
      };

    case CurrentAppointmentType.SET_CURRENT_APPOINTMENT_ID:
      return {
        ...state,
        appointmentId: action.payload.appointmentId,
      };

    case CurrentAppointmentType.SET_CURRENT_APPOINTMENT_MODULES:
      return {
        ...state,
        modules: action.payload.modules,
      };

    case CurrentAppointmentType.SET_CARE_TEAM_MEMBERS:
      return {
        ...state,
        careTeamMembers: action.payload.careTeamMembers,
      };

    case CurrentAppointmentType.SET_PATIENT_DATA:
      return {
        ...state,
        patientData: action.payload.patientData,
      };

    case CurrentAppointmentType.SET_PATIENT_ID:
      return {
        ...state,
        patientId: action.payload.patientId,
      };

    case CurrentAppointmentType.SET_PRACTITIONER_ID:
      return {
        ...state,
        practitionerId: action.payload.practitionerId,
      };
    case CurrentAppointmentType.SHOW_PATIENT_VIEW_MODAL:
      return {
        ...state,
        isShowPatientViewModal: true,
      };
    case CurrentAppointmentType.HIDE_PATIENT_VIEW_MODAL:
      return {
        ...state,
        isShowPatientViewModal: false,
      };
    case CurrentAppointmentType.SHOW_CHART_SKELETON:
      return {
        ...state,
        isSkeletonChartShown: true,
      };
    case CurrentAppointmentType.HIDE_CHART_SKELETON:
      return {
        ...state,
        isSkeletonChartShown: false,
      };
    case CurrentAppointmentType.SET_WIDGETS_LIST:
    {
      const { currentModuleAlias, currentWidgetAlias } = action.payload;

      const currentModuleWidgets = state.modules?.find(({ alias }) =>
        alias === currentModuleAlias)?.widgets || [];

      const widgetsList = getWidgetsListWithPriority(currentModuleWidgets, currentWidgetAlias);

      return {
        ...state,
        widgetsList,
        navigation: currentModuleAlias,
      };
    }
    case CurrentAppointmentType.SET_SKELETON_SHOWN:
    {
      const { widgetAlias, isSkeletonShown } = action.payload;
      const updatedWidgetsList = state.widgetsList?.map(widget =>
        widget.alias === widgetAlias ? { ...widget, isSkeletonShown } : widget) || [];

      return {
        ...state,
        widgetsList: updatedWidgetsList,
      };
    }
    case CurrentAppointmentType.ADD_ACCORDION:
      return {
        ...state,
        accordions: {
          ...state.accordions,
          [action.payload.groupId]:
            state.accordions[action.payload.groupId]
              ? [
                ...state.accordions[action.payload.groupId],
                action.payload.accordionId,
              ]
              : [ action.payload.accordionId ],
        },
      };
    case CurrentAppointmentType.DELETE_ACCORDION:
      return {
        ...state,
        accordions: {
          ...state.accordions,
          [action.payload.groupId]:
            state.accordions[action.payload.groupId].filter(accordion =>
              accordion !== action.payload.accordionId),
        },
      };
    case CurrentAppointmentType.CLEAR_ACCORDIONS:
      return {
        ...state,
        accordions:
          Object.fromEntries(
            Object.entries(state.accordions).filter(([ key ]) =>
              key !== action.payload.groupId)
          ),
      };
    case CurrentAppointmentType.CLEAR_ALL_ACCORDIONS:
      return {
        ...state,
        accordions: {},
      };
    case CurrentAppointmentType.GET_TEST_REPORTS:
      return {
        ...state,
        isTestReportFetching: true,
      };
    case CurrentAppointmentType.SET_TEST_REPORTS:
      return {
        ...state,
        testReports: state.testReports
          ? [ ...state.testReports, ...action.payload.testReports ]
          : action.payload.testReports,
      };
    case CurrentAppointmentType.GET_TEST_REPORTS_RESULTS:
      return {
        ...state,
        testReportsResults: null,
      };
    case CurrentAppointmentType.SET_TEST_REPORTS_RESULTS:
      return {
        ...state,
        testReportsResults: action.payload.testReportsResults,
      };
    case CurrentAppointmentType.SET_TEST_REPORTS_SKELETON:
      return {
        ...state,
        isTestReportsSkeletonShown: action.payload.isTestReportsSkeletonShown,
      };
    case CurrentAppointmentType.SET_TEST_REPORTS_RESULTS_SKELETON:
      return {
        ...state,
        isTestReportResultsSkeletonShown: action.payload.isTestReportResultsSkeletonShown,
      };
    case UIType.START_DATA_FETCH:
      return {
        ...state,
        isTestReportsSkeletonShown: true,
        isTestReportResultsSkeletonShown: true,
        testReportsResults: null,
        isTestResultsMounted: false,
        testReports: null,
        page: 0,
      };
    case CurrentAppointmentType.SET_TEST_RESULTS_MOUNTED:
      return {
        ...state,
        isTestResultsMounted: action.payload.isTestResultsMounted,
      };
    case CurrentAppointmentType.SET_TEST_RESULTS_NEXT_PAGE:
      return {
        ...state,
        isTestReportFetching: false,
        page: state.page + 1,
      };
    default:
      return state;
  }
};
