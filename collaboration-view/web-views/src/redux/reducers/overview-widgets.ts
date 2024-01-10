import { CardType } from '@enums';
import {
  OverviewWidgetsAction,
  OverviewWidgetsType,
  UIActions,
  UIType
} from '@redux/actions';
import {
  AllergiesData,
  DateTimeDoubleChart,
  HealthConditionsWidgetDataType,
  ImmunizationsWidgetDataType,
  MedicationsWidgetDataType,
  TodayVitalCard,
  PatientAppointments
} from '@types';
import { convertWeightToKg, convertWeightToLBS } from '@utils';

export type OverviewWidgetsStateType = {
  medications: MedicationsWidgetDataType | null
  allergies: AllergiesData[] | null
  todaysVitals: TodayVitalCard[] | null;
  currentCardType: CardType | null;
  chart: DateTimeDoubleChart | null;
  immunizations: ImmunizationsWidgetDataType | null;
  conditions: HealthConditionsWidgetDataType | null;
  patientAppointments: PatientAppointments | null;
};

const overviewWidgetsInitialState: OverviewWidgetsStateType = {
  medications: null,
  allergies: null,
  todaysVitals: null,
  currentCardType: null,
  chart: null,
  immunizations: null,
  conditions: null,
  patientAppointments: null,
};

export const overviewWidgetsReducer = (
  state = overviewWidgetsInitialState,
  action: OverviewWidgetsAction | UIActions
): OverviewWidgetsStateType => {
  switch (action.type) {
    case OverviewWidgetsType.SET_MEDICATIONS_WIDGET_DATA:
      return {
        ...state,
        medications: action.payload.medications,
      };

    case OverviewWidgetsType.SET_ALLERGIES_WIDGET_DATA:
      return {
        ...state,
        allergies: action.payload.allergies,
      };

    case OverviewWidgetsType.SET_TODAYS_VITALS:
      return {
        ...state,
        todaysVitals: action.payload.todaysVitals,
      };

    case OverviewWidgetsType.SET_CURRENT_CARD_TYPE:
      return {
        ...state,
        currentCardType: action.payload.currentCardType,
      };

    case OverviewWidgetsType.SET_CHART_DATA:
      return {
        ...state,
        chart: action.payload.chart,
      };
    
    case OverviewWidgetsType.SET_CHART_DATA_METRIC:
      return {
        ...state,
        chart: { 
          ...action.payload.chart, 
          chartOptions: {
            ...action.payload.chart.chartOptions,
            chartScales: {
              ...action.payload.chart.chartOptions.chartScales,
              yAxis: {
                ...action.payload.chart.chartOptions.chartScales.yAxis,
                max: (action.payload.chart.chartOptions.chartScales.yAxis.max) / 2,
                min: (action.payload.chart.chartOptions.chartScales.yAxis.min) / 2,
                stepSize: (action.payload.chart.chartOptions.chartScales.yAxis.stepSize) / 2,
              },
            },
          },
          datasets: action.payload.chart.datasets.map(
            ({
              data, label, visible, range, 
            })=>
              ({
                data: {
                  ...data,
                  dimension:'kg',
                  values: data.values.map(
                    ({ x, y })=>
                      ({
                        x,
                        y: convertWeightToKg(y),
                      })
                  ),
                },
                label,
                visible,
                range,
              })
          ),
        },
      };
    case OverviewWidgetsType.SET_CHART_DATA_NON_METRIC:
      return {
        ...state,
        chart: { 
          ...action.payload.chart, 
          chartOptions: {
            ...action.payload.chart.chartOptions,
            chartScales: {
              ...action.payload.chart.chartOptions.chartScales,
              yAxis: {
                ...action.payload.chart.chartOptions.chartScales.yAxis,
                max: 2 * (action.payload.chart.chartOptions.chartScales.yAxis.max),
                min: 2 * (action.payload.chart.chartOptions.chartScales.yAxis.min),
                stepSize: 2 * (action.payload.chart.chartOptions.chartScales.yAxis.stepSize),
              },
            },
          },
          datasets: action.payload.chart.datasets.map(
            ({
              data, label, visible, range, 
            })=>
              ({
                data: {
                  ...data,
                  dimension:'lbs',
                  values: data.values.map(
                    ({ x, y })=>
                      ({
                        x,
                        y: convertWeightToLBS(y),
                      })
                  ),
                },
                label,
                visible,
                range,
              })
          ),
        },
      };
    case OverviewWidgetsType.SET_IMMUNIZATIONS_WIDGET_DATA:
      return {
        ...state,
        immunizations: action.payload.immunizations,
      };

    case OverviewWidgetsType.SET_CONDITIONS_WIDGET_DATA:
      return {
        ...state,
        conditions: action.payload.conditions,
      };

    case OverviewWidgetsType.SET_PATIENT_APPOINTMENTS:
      return {
        ...state,
        patientAppointments: action.payload.patientAppointments,
      };

    case UIType.START_DATA_FETCH:
      return { ...overviewWidgetsInitialState };
    default:
      return state;
  }
};
