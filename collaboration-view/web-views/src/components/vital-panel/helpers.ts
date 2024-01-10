import { measurementsUnitsMap } from '@constants';
import { MeasurementType } from '@enums';

const isToday = (date: Date): boolean => {
  const today = new Date();

  const year = today.getFullYear();
  const month = today.getMonth();
  const day = today.getDate();

  return date.getFullYear() === year &&
    date.getMonth() === month &&
    date.getDate() === day;
};

export const getUnitMeasurement = (measurementType: MeasurementType, unit?: string): string => {
  if (measurementType === MeasurementType.SYSTOLIC_BLOOD_PRESSURE) {
    return measurementsUnitsMap[MeasurementType.DIASTOLIC_BLOOD_PRESSURE][unit as string];
  }

  return measurementType !== MeasurementType.PAIN_LEVEL
    ? measurementsUnitsMap[measurementType][unit as string]
    : '' ;
};

export const getTextMeasurement = (measurementType: MeasurementType, date: string): string => {
  switch (measurementType) {
    case MeasurementType.BODY_HEIGHT:
      return 'HEIGHT';
    case MeasurementType.BODY_WEIGHT:
      return 'WEIGHT';
    case MeasurementType.BODY_MASS_INDEX:
      return 'AGE';

    default:
      return isToday(new Date(date)) ? `TODAY's` : 'MOST RECENT';
  }
};
