import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';

import { CardType } from '@enums';
import { RootState } from '@redux/reducers';
import { DateTimeDoubleChartDataset, Vital } from '@types';
import { convertHeightToFeet, convertWeightToLBS } from '@utils';

import { getConfigVitalPanelByType } from './config-vital-panels';
import { VitalPanelGeneralAdvice } from './vital-panel-general-advice';
import { VitalPanelMeasurement } from './vital-panel-measurement';
import styles from './vital-panel.scss';

export type VitalPanelPropsType = {
  toggle: string;
  cardType: CardType;
  vitals: Vital[];
};

export const VitalPanel = ({
  toggle,
  cardType,
  vitals,
}: VitalPanelPropsType) => {
  const patientData = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientData);
  const { datasets } = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.chart) || { datasets: [] as DateTimeDoubleChartDataset[] };

  const measurements = vitals.map(({ measurements }) =>
    measurements).flat();
  const {
    measurementsComponents,
    generalAdviceComponent,
  } = getConfigVitalPanelByType({
    datasets,
    cardType,
    measurements,
    birthDate: patientData?.birthDate,
  });
  const [ convertedHeightValue, setConvertedHeightValue ] = useState<string | null>(null);
  const [ convertedWeightValue, setConvertedWeightValue ] = useState<number | null>(null);
  const [ convertedWeightUnit, setConvertedWeightUnit ] = useState<string | null>(null);
  const [ convertedHeightUnit, setConvertedHeightUnit ] = useState<string | null>(null);
  
  useEffect(() => {
    if(toggle === 'off' && (cardType === 'WeightAndHeight' || cardType === 'BodyMassIndex')) {
      const heightAtIndex = vitals.findIndex(ele => 
        ele.vitalType === 'Height');
      const weightAtIndex = vitals.findIndex(ele => 
        ele.vitalType === 'Weight');
      setConvertedHeightValue(convertHeightToFeet(vitals[heightAtIndex].measurements[0].value));
      setConvertedWeightValue(convertWeightToLBS(vitals[weightAtIndex].measurements[0].value));
      setConvertedWeightUnit('lbs');
      setConvertedHeightUnit('inch');
    }
    else if(toggle === 'on' && (cardType === 'WeightAndHeight' || cardType === 'BodyMassIndex')){
      setConvertedHeightValue(null);
      setConvertedWeightValue(null);
      setConvertedWeightUnit(null);
      setConvertedHeightUnit(null);
    }
  }, [ toggle, cardType ]);

  return (
    <div className={styles.vitalPanelContainer}>
      {measurementsComponents.map((measurementComponent, index) =>
        (
          <>
            <VitalPanelMeasurement
              key={index}
              cardType={cardType}
              isVisible={!!measurementComponent.value}
              toggle={toggle}
              convertedHeightValue={convertedHeightValue}
              convertedWeightValue={convertedWeightValue}
              convertedWeightUnit={convertedWeightUnit}
              convertedHeightUnit={convertedHeightUnit}
              {...measurementComponent}
            /></>
        ))}
      {generalAdviceComponent && (
        <VitalPanelGeneralAdvice {...generalAdviceComponent} />
      )}
    </div>
  );
};
