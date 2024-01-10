import { CardType, MeasurementType } from '@enums';
import {
  conditionMagentaIcon,
  heartRateMagentaIcon,
  painLevelMagentaIcon,
  respiratoryRateMagentaIcon,
  temperatureMagentaIcon
} from '@icons';

type MeasurementsUnitsMapType = {
  [key in Exclude<MeasurementType, MeasurementType.PAIN_LEVEL>]: {
    [key in string]: string;
  }
};

export const measurementsUnitsMap: MeasurementsUnitsMapType = {
  [MeasurementType.SYSTOLIC_BLOOD_PRESSURE]: { 'mm[Hg]': '' },
  [MeasurementType.DIASTOLIC_BLOOD_PRESSURE]: { 'mm[Hg]': 'mmHg' },
  [MeasurementType.OXYGEN_SATURATION]: { '%': '%' },
  [MeasurementType.HEART_RATE]: { '/min': 'BPM' },
  [MeasurementType.BODY_TEMPERATURE]: { 'Cel': '°C', '[degF]': '°F' },
  [MeasurementType.RESPIRATORY_RATE]: { '/min': 'BPM' },
  [MeasurementType.BODY_WEIGHT]: { 'kg': 'kg', 'lbs': 'lbs' },
  // [MeasurementType.BODY_WEIGHT]: { 'lbs': 'lbs' },
  [MeasurementType.BODY_HEIGHT]: { 'cm': 'cm', 'inch': '' },
  // [MeasurementType.BODY_HEIGHT]: { '': '' },
  [MeasurementType.BODY_MASS_INDEX]: { 'kg/m2': '' },
};

type VitalChartType = {
  title: string;
  icon?: string;
};

export const vitalsChartsMap: Record<CardType, VitalChartType> = {
  [CardType.BLOOD_PRESSURE]: {
    title: 'Blood Pressure',
    icon: conditionMagentaIcon,
  },
  [CardType.BLOOD_OXYGEN]: { title: 'Blood Oxygen' },
  [CardType.HEART_RATE]: {
    title: 'Heart Rate',
    icon: heartRateMagentaIcon,
  },
  [CardType.TEMPERATURE]: {
    title: 'Temperature',
    icon: temperatureMagentaIcon,
  },
  [CardType.RESPIRATORY_RATE]: {
    title: 'Respiratory Rate',
    icon: respiratoryRateMagentaIcon,
  },
  [CardType.PAIN_LEVEL]: {
    title: 'Pain Level',
    icon: painLevelMagentaIcon,
  },
  [CardType.WEIGHT_HEIGHT]: { title: 'Weight and Height' },
  [CardType.BODY_MASS_INDEX]: { title: 'BMI' },
};
