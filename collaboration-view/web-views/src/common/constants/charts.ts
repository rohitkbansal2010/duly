import { CardType, ChartMeasureKeys, RangeKeys } from '@enums';
import { BoundsFillColors, Legend, LegendItem } from '@interfaces';

const NORMAL_RANGE_LEGEND: LegendItem = {
  title: 'Normal range',
  color: 'Normal',
};

export const LEGEND: Legend = {
  [CardType.BLOOD_PRESSURE]: [
    {
      title: 'Systolic',
      color: 'Magenta',
    },
    {
      title: 'Diastolic',
      color: 'Violet',
    },
    NORMAL_RANGE_LEGEND,
  ],
  [CardType.BLOOD_OXYGEN]: [],
  [CardType.HEART_RATE]: [ NORMAL_RANGE_LEGEND ],
  [CardType.TEMPERATURE]: [ NORMAL_RANGE_LEGEND ],
  [CardType.RESPIRATORY_RATE]: [ NORMAL_RANGE_LEGEND ],
  [CardType.PAIN_LEVEL]: [],
  [CardType.WEIGHT_HEIGHT]: [],
  [CardType.BODY_MASS_INDEX]: [ NORMAL_RANGE_LEGEND ],
};

export const BOUNDS_FILL_COLORS: BoundsFillColors = {
  [RangeKeys.NORMAL]: 'rgba(71, 10, 104, 0.05)',
  [RangeKeys.CONCERNING]: 'rgba(71, 10, 104, 0.08)',
  [RangeKeys.LOW]: 'rgba(71, 10, 104, 0.11)',
  [RangeKeys.BRAIN_AFFECTING]: 'rgba(71, 10, 104, 0.14)',
};

export const TOOLTIP_MEASUREMENTS = [
  { [ChartMeasureKeys.BLOOD_PRESSURE]: '' },
  { [ChartMeasureKeys.HEART_RATE]: 'BPM' },
  { [ChartMeasureKeys.RESPIRATORY_RATE]: 'BPM' },
  { [ChartMeasureKeys.TEMPERATURE_FAHRENHEIT]: '°F' },
  { [ChartMeasureKeys.TEMPERATURE_CELSIUS]: '°C' },
  { [ChartMeasureKeys.WEIGHT_KG]: 'KG' },
  { [ChartMeasureKeys.WEIGHT_LBS]: 'LBS' },
];

export const VITALS_CHART_HEIGHT = 428;
export const VITALS_CHART_HEIGHT_BMI = 244;
export const TEST_RESULTS_CHART_HEIGHT = 310;

export const DAY_STEP_SIZE = 1;
export const YEAR_STEP_SIZE = 12;
