import { CardType } from '@enums';
import { formatMDYYYYDate } from '@utils';

import { MeasurementComponentType } from '../config-vital-panels';

import styles from './vital-panel-measurement.scss';

export type VitalPanelMeasurementPropsType = MeasurementComponentType & {
  cardType: CardType;
  isVisible: boolean;
  convertedHeightUnit: string | null;
  convertedWeightUnit: string | null
  convertedHeightValue?: string | null;
  convertedWeightValue?: number | null;
  toggle: string;
};

export const VitalPanelMeasurement = ({
  cardType,
  isVisible,
  text,
  value,
  convertedWeightValue,
  convertedHeightValue,
  convertedHeightUnit,
  convertedWeightUnit,
  style,
  unit,
  maxScaleValue,
  additionalValue,
  icon,
  date,
}: VitalPanelMeasurementPropsType) => {
  if (!isVisible) return null;
  
  return (
    
    
    <div className={styles.vitalPanelMeasurementContainer} style={style}>     
      {icon && (
        <div className={styles.vitalPanelMeasurementContainerIcon}>
          <img
            src={icon}
            alt="vital icon"
            className={styles[`vitalPanelMeasurementContainerIcon${cardType}`]}
            date-test="vital-panel-measurement-icon"
          />
        </div>
      )}
      <div className={styles.vitalPanelMeasurementContainerAdditionalInfo}>
        <div className={styles.vitalPanelMeasurementContainerAdditionalInfoTopBlock}>
          <div
            className={styles.vitalPanelMeasurementContainerAdditionalInfoText}
            date-test="vital-panel-measurement-text"
          >
            {text}
          </div>
          {date && (
            <div
              className={styles.vitalPanelMeasurementContainerAdditionalInfoDate}
              date-test="vital-panel-measurement-date"
            >
              {formatMDYYYYDate(new Date(date))}
            </div>
          )}
        </div>
        <div className={styles.vitalPanelMeasurementContainerAdditionalInfoBottomBlock}>
          <div
            className={styles.vitalPanelMeasurementContainerAdditionalInfoValue}
            date-test="vital-panel-measurement-value"
          >
            {text === 'WEIGHT' && convertedWeightValue ? convertedWeightValue : 
            text === 'HEIGHT' && convertedHeightValue ? convertedHeightValue : value}
          </div>
          {!additionalValue && unit && (
            <div
              className={styles.vitalPanelMeasurementContainerAdditionalInfoUnit}
              date-test="vital-panel-measurement-unit"
            >
              {text === 'WEIGHT' && convertedWeightUnit ? convertedWeightUnit : 
            text === 'HEIGHT' && convertedHeightUnit ? '' : unit}
            </div>
          )}
          {maxScaleValue && (
            <div
              className={styles.vitalPanelMeasurementContainerAdditionalInfoMaxScaleValue}
              date-test="vital-panel-measurement-max-scale-value"
            >
              {`/ ${maxScaleValue}`}
            </div>
          )}
          {additionalValue && (
            <>
              <div
                className={styles.vitalPanelMeasurementContainerAdditionalInfoAdditionalValue}
                date-test="vital-panel-measurement-additional-value"
              >
                {`/ ${additionalValue}`}
              </div>
              <div className={styles.vitalPanelMeasurementContainerAdditionalInfoUnit}>
                {unit}
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
};
