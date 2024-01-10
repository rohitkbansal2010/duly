import { measurementsUnitsMap } from '@constants';
import { MeasurementType } from '@enums';
import { Measurement } from '@types';

import styles from './vital-card.scss';

type MeasurementsTypeProps = {
  index: number;
  measurements: Measurement[];
  activeClass: string;
  isWeighHeightCard: boolean;
  isVitalHeight: boolean;
  isVitalWeight: boolean;
};

export const Measurements = ({
  index,
  measurements,
  activeClass,
  isWeighHeightCard,
  isVitalHeight,
  isVitalWeight,
}:
  MeasurementsTypeProps) => {
  const measurementsTypesWeightAndHeight = [
    MeasurementType.BODY_WEIGHT,
    MeasurementType.BODY_HEIGHT,
  ];

  const renderMeasurement = ({
    measurementType,
    value,
    unit,
    convertedValue,
    convertedUnit,
    maxScaleValue,
  }: Measurement) => {
    const metric = sessionStorage.getItem('metric') === 'on';
    const measurementValueClasses = [ styles[`todayVitalCard${activeClass}MeasurementValue`] ]
      .concat(
        measurementsTypesWeightAndHeight.includes(measurementType)
          ? styles[`todayVitalCard${activeClass}MeasurementValueWeightHeight`]
          : []
      )
      .join(' ');

    const isSiastolicBloodPressure = measurementType === MeasurementType.SYSTOLIC_BLOOD_PRESSURE;
    const isDiastolicBloodPressure = measurementType === MeasurementType.DIASTOLIC_BLOOD_PRESSURE;
    const isPainLevel = measurementType === MeasurementType.PAIN_LEVEL;

    return (
      <span key={measurementType} className={styles[`todayVitalCard${activeClass}Measurement`]}>
        {!isDiastolicBloodPressure && (
          <span className={measurementValueClasses}>
            {!metric && convertedValue ? convertedValue : value}
            {!(isSiastolicBloodPressure || maxScaleValue) && ' '}
          </span>
        )}
        {unit && !isPainLevel && (
          <span className={styles[`todayVitalCard${activeClass}MeasurementUnit`]}>
            {isDiastolicBloodPressure && (
              <>&nbsp;/&nbsp;{!metric && convertedValue ? convertedValue : value}&nbsp;</>
            )}
            {
            measurementsUnitsMap[measurementType][(!metric && convertedUnit) ? convertedUnit : unit]
            }
          </span>
        )}
        {maxScaleValue && (
          <span className={styles[`todayVitalCard${activeClass}MeasurementScaleValue`]}>
            &nbsp;/&nbsp;{maxScaleValue}
          </span>
        )}
      </span>
    );
  };

  return (
    <span className={styles[`todayVitalCard${activeClass}Measurement`]}>
      {measurements.map(renderMeasurement)}
      {isWeighHeightCard && isVitalHeight && isVitalWeight && index == 0 && <>&nbsp;/&nbsp;</>}
    </span>
  );
};
