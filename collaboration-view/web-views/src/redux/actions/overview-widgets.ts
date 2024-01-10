import { CardType } from '@enums';
import {
  AllergiesData,
  DateTimeDoubleChart,
  HealthConditionsWidgetDataType,
  ImmunizationsWidgetDataType,
  MedicationsWidgetDataType,
  TodayVitalCard,
  PatientAppointments
} from '@types';

export enum OverviewWidgetsType {
  GET_MEDICATIONS_WIDGET_DATA = 'GET_MEDICATIONS_WIDGET_DATA',
  SET_MEDICATIONS_WIDGET_DATA = 'SET_MEDICATIONS_WIDGET_DATA',
  SET_ALLERGIES_WIDGET_DATA = 'SET_ALLERGIES_WIDGET_DATA',
  GET_ALLERGIES_WIDGET_DATA = 'GET_ALLERGIES_WIDGET_DATA',
  GET_TODAYS_VITALS = 'GET_TODAYS_VITALS',
  SET_TODAYS_VITALS = 'SET_TODAYS_VITALS',
  SET_CURRENT_CARD_TYPE = 'SET_CURRENT_CARD_TYPE',
  GET_CHART_DATA = 'GET_CHART_DATA',
  SET_CHART_DATA = 'SET_CHART_DATA',
  GET_IMMUNIZATIONS_WIDGET_DATA = 'GET_IMMUNIZATIONS_WIDGET_DATA',
  SET_IMMUNIZATIONS_WIDGET_DATA = 'SET_IMMUNIZATIONS_WIDGET_DATA',
  SET_CONDITIONS_WIDGET_DATA = 'SET_CONDITIONS_WIDGET_DATA',
  GET_CONDITIONS_WIDGET_DATA = 'GET_CONDITIONS_WIDGET_DATA',
  GET_PATIENT_APPOINTMENTS = 'GET_PATIENT_APPOINTMENTS',
  SET_PATIENT_APPOINTMENTS = 'SET_PATIENT_APPOINTMENTS',
  SET_CHART_DATA_METRIC = 'SET_CHART_DATA_METRIC',
  SET_CHART_DATA_NON_METRIC = 'SET_CHART_DATA_NON_METRIC'
}

export type GetConditionsWidgetDataAction = {
  type: typeof OverviewWidgetsType.GET_CONDITIONS_WIDGET_DATA;
  payload: { patientId: string };
};

export type GetMedicationsWidgetDataAction = {
  type: typeof OverviewWidgetsType.GET_MEDICATIONS_WIDGET_DATA;
  payload: { patientId: string };
};

export type GetChartDataAction = {
  type: typeof OverviewWidgetsType.GET_CHART_DATA;
  payload: { patientId: string, vitalCardType: CardType };
};

export type SetChartDataAction = {
  type: typeof OverviewWidgetsType.SET_CHART_DATA;
  payload: { chart: DateTimeDoubleChart | null };
};

export type SetChartDataMetricAction = {
  type: typeof OverviewWidgetsType.SET_CHART_DATA_METRIC;
  payload: { chart: DateTimeDoubleChart | null };
};
export type SetChartDataNonMetricAction = {
  type: typeof OverviewWidgetsType.SET_CHART_DATA_NON_METRIC;
  payload: { chart: DateTimeDoubleChart | null };
};

export type GetAllergiesWidgetDataAction = {
  type: typeof OverviewWidgetsType.GET_ALLERGIES_WIDGET_DATA;
  payload: { patientId: string };
};

type SetConditionsWidgetDataAction = {
  type: typeof OverviewWidgetsType.SET_CONDITIONS_WIDGET_DATA;
  payload: { conditions: HealthConditionsWidgetDataType };
};

type SetMedicationsWidgetDataAction = {
  type: typeof OverviewWidgetsType.SET_MEDICATIONS_WIDGET_DATA;
  payload: { medications: MedicationsWidgetDataType };
};

type SetAllergiesWidgetDataAction = {
  type: typeof OverviewWidgetsType.SET_ALLERGIES_WIDGET_DATA;
  payload: { allergies: AllergiesData[] };
};

export type GetTodaysVitalsAction = {
  type: typeof OverviewWidgetsType.GET_TODAYS_VITALS;
  payload: { patientId: string };
};

type SetTodaysVitalsAction = {
  type: typeof OverviewWidgetsType.SET_TODAYS_VITALS;
  payload: { todaysVitals: TodayVitalCard[] };
};

type SetCurrentCardTypeAction = {
  type: typeof OverviewWidgetsType.SET_CURRENT_CARD_TYPE;
  payload: { currentCardType: CardType };
};

export type GetImmunizationsWidgetDataAction = {
  type: typeof OverviewWidgetsType.GET_IMMUNIZATIONS_WIDGET_DATA;
  payload: { patientId: string };
}

type SetImmunizationsWidgetDataAction = {
  type: typeof OverviewWidgetsType.SET_IMMUNIZATIONS_WIDGET_DATA;
  payload: { immunizations: ImmunizationsWidgetDataType };
};

export type GetPatientAppointmentsAction = {
  type: typeof OverviewWidgetsType.GET_PATIENT_APPOINTMENTS;
  payload: { appointmentId: string };
};

type SetPatientAppointmentsPAction = {
  type: typeof OverviewWidgetsType.SET_PATIENT_APPOINTMENTS;
  payload: { patientAppointments: PatientAppointments };
};

export type OverviewWidgetsAction =
  GetMedicationsWidgetDataAction |
  SetMedicationsWidgetDataAction |
  GetAllergiesWidgetDataAction |
  SetAllergiesWidgetDataAction |
  GetTodaysVitalsAction |
  SetTodaysVitalsAction |
  SetCurrentCardTypeAction |
  GetChartDataAction |
  SetChartDataAction |
  GetImmunizationsWidgetDataAction |
  SetImmunizationsWidgetDataAction |
  GetConditionsWidgetDataAction |
  SetConditionsWidgetDataAction |
  GetPatientAppointmentsAction |
  SetPatientAppointmentsPAction |
  SetChartDataMetricAction |
  SetChartDataNonMetricAction;

export const getMedicationsWidgetData =
  ({ patientId }: { patientId: string }): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.GET_MEDICATIONS_WIDGET_DATA,
      payload: { patientId },
    });

export const getConditionsWidgetData =
  ({ patientId }: { patientId: string }): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.GET_CONDITIONS_WIDGET_DATA,
      payload: { patientId },
    });

export const setMedicationsWidgetData =
  (medications: MedicationsWidgetDataType): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.SET_MEDICATIONS_WIDGET_DATA,
      payload: { medications },
    });

export const setConditionsWidgetData =
  (conditions: HealthConditionsWidgetDataType): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.SET_CONDITIONS_WIDGET_DATA,
      payload: { conditions },
    });

export const getAllergiesWidgetData =
  ({ patientId }: { patientId: string }): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.GET_ALLERGIES_WIDGET_DATA,
      payload: { patientId },
    });

export const setAllergiesWidgetData =
  (allergies: AllergiesData[]): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.SET_ALLERGIES_WIDGET_DATA,
      payload: { allergies },
    });

export const getChartData =
  (patientId: string, vitalCardType: CardType): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.GET_CHART_DATA,
      payload: { patientId, vitalCardType },
    });

export const setChartData =
  (chart: DateTimeDoubleChart | null): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.SET_CHART_DATA,
      payload: { chart },
    });
export const setChartDataMetric =
  (chart: DateTimeDoubleChart | null): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.SET_CHART_DATA_METRIC,
      payload: { chart },
    });
export const setChartDataNonMetric =
  (chart: DateTimeDoubleChart | null): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.SET_CHART_DATA_NON_METRIC,
      payload: { chart },
    });
export const getTodaysVitals = ({ patientId }: { patientId: string }): OverviewWidgetsAction =>
  ({
    type: OverviewWidgetsType.GET_TODAYS_VITALS,
    payload: { patientId },
  });

export const setTodaysVitals = (todaysVitals: TodayVitalCard[]): OverviewWidgetsAction =>
  ({
    type: OverviewWidgetsType.SET_TODAYS_VITALS,
    payload: { todaysVitals },
  });

export const setCurrentCardType = (currentCardType: CardType): OverviewWidgetsAction =>
  ({
    type: OverviewWidgetsType.SET_CURRENT_CARD_TYPE,
    payload: { currentCardType },
  });

export const getImmunizationsWidgetData =
  ({ patientId }: { patientId: string }): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.GET_IMMUNIZATIONS_WIDGET_DATA,
      payload: { patientId },
    });

export const setImmunizationsWidgetData =
  (immunizations: ImmunizationsWidgetDataType): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.SET_IMMUNIZATIONS_WIDGET_DATA,
      payload: { immunizations },
    });

export const getPatientAppointments =
  ({ appointmentId }: { appointmentId: string }): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.GET_PATIENT_APPOINTMENTS,
      payload: { appointmentId },
    });

export const setPatientAppointments =
  (patientAppointments: PatientAppointments): OverviewWidgetsAction =>
    ({
      type: OverviewWidgetsType.SET_PATIENT_APPOINTMENTS,
      payload: { patientAppointments },
    });
