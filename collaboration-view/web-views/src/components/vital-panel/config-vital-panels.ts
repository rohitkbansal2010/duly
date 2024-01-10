import { CSSProperties } from 'react';

import { vitalsChartsMap } from '@constants';
import { CardType, SortDirection } from '@enums';
import { ChartPoint } from '@interfaces';
import { DateTimeDoubleChartDataset, Measurement } from '@types';
import { calculateYears } from '@utils';

import { getTextMeasurement, getUnitMeasurement } from './helpers';

type GetConfigVitalPanelByTypeParameters = {
  datasets: DateTimeDoubleChartDataset[];
  cardType: CardType,
  measurements: Measurement[],
  birthDate?: string
};

type GetMeasurementsComponentsByTypeParameters = GetConfigVitalPanelByTypeParameters;

type GeneralAdviceComponentType = {
  title: string;
  text: string;
  style: CSSProperties;
};

export type MeasurementComponentType = {
  style?: CSSProperties;
  text?: string;
  value?: number ;
  date?: string;
  unit?: string;
  icon?: string;
  additionalValue?: number | string;
  maxScaleValue?: number;
  convertedValue?: string | number | undefined;
  convertedUnit?: string | undefined;
};

type ConfigVitalPanelType = {
  measurementsComponents: MeasurementComponentType[];
  generalAdviceComponent?: GeneralAdviceComponentType;
}

const generalAdvices = new Map([
  [ CardType.BLOOD_PRESSURE, {
    title: 'General Advice',
    text: 'Blood pressure numbers of less than 120/80 mm Hg are considered within the normal range. If your results fall into this category, stick with heart-healthy habits.',
    style: { width: '56%' },
  } ],
  [ CardType.HEART_RATE, {
    title: 'General Advice',
    text: 'A normal rate is usually started as 60 to 100 beats per minute. Slower than 60 is bradycardia ("slow heart"), faster than 100 is tachycardia ("fast heart"). But an ideal resting heart is closer to 50 to 70.',
    style: { width: '59%' },
  } ],
  [ CardType.TEMPERATURE, {
    title: 'General Advice',
    text: '97.5-98.8 F is generally normal reported range for normal body.',
    style: { width: '56%' },
  } ],
  [ CardType.RESPIRATORY_RATE, {
    title: 'General Advice',
    text: 'The normal respiratory rate for healthy adults is between 12 and 20 breaths per minute. Breathing rates of below 12 or above 20 can mean a disruption in normal breathing processes.',
    style: { width: '56%' },
  } ],
  [ CardType.PAIN_LEVEL, {
    title: 'General Advice',
    text: 'Report to your doctor if you\'re experiencing any aching, biting, burning, cramping, dull, gnawing, heavy, hot, piercing, pinching, sharp, shooting, sickening, sore, splitting, stabbing, tender, tingling or throbbing feeling.',
    style: { width: '61%' },
  } ],
  [ CardType.WEIGHT_HEIGHT, {
    title: 'General Advice',
    text: 'Daily weight fluctuation is normal.  The average adult\'s weight fluctuates up to 5 or 6 pounds per day. It all comes down to what and when you eat, drink, exercise, and even sleep.',
    style: { width: '48%' },
  } ],
]);

const getMeasurementsComponentsByType = ({
  datasets,
  measurements,
  cardType,
  birthDate,
}: GetMeasurementsComponentsByTypeParameters): MeasurementComponentType[] => {
  const [ measurementOne, measurementTwo, measurementThree ] = measurements;
  const {
    measurementType,
    measured,
    unit,
    maxScaleValue,
    value,
  } = measurementOne;

  const defaultMeasurementProps = {
    icon: vitalsChartsMap[cardType].icon,
    text: getTextMeasurement(measurementType, measured),
    date: measured,
    unit: getUnitMeasurement(measurementType, unit),
    additionalValue: cardType.includes(CardType.BLOOD_PRESSURE) ? measurements[1].value : '',
    value,
  };

  const xyValues = Array.from(
    datasets.map(({ data: { values } }) =>
      values)
      .flat()
      .map(({ x, y }) =>
        (
          {
            x,
            y,
          }
        ))
  );

  const sortDataByValuesToDirection = (
    data: ChartPoint[],
    direction = SortDirection.TO_MIN
  ): ChartPoint[] =>
    [ ...data ].sort((a, b) => {
      if (a.y === b.y) {
        return new Date(a.x).getMilliseconds() - new Date(b.x).getMilliseconds();
      }

      return direction === SortDirection.TO_MAX ? a.y - b.y : b.y - a.y;
    });

  const sortedDataByValuesToMax = sortDataByValuesToDirection(xyValues, SortDirection.TO_MAX);
  const sortedDataByValuesToMin = sortDataByValuesToDirection(xyValues);
    
  switch (cardType) {
    case CardType.BLOOD_PRESSURE:
      return [ {
        ...defaultMeasurementProps,
        style: { width: '44%' },
      } ];

    case CardType.BLOOD_OXYGEN:
      return [
        {
          ...defaultMeasurementProps,
          style: { width: '36%', padding: '1.25rem 0 1.25rem 3.125rem' },
        },
        {
          text: 'LOWEST',
          value: sortedDataByValuesToMax[0].y,
          style: { width: '33%', padding: '1.25rem 4rem' },
          date: sortedDataByValuesToMax[0].x,
          unit: '%',
        },
        {
          text: 'HIGHEST',
          value: sortedDataByValuesToMin[0].y,
          style: { width: '31%', padding: '1.25rem 3.125rem 1.25rem 4rem' },
          date: sortedDataByValuesToMin[0].x,
          unit: '%',
        },
      ];

    case CardType.HEART_RATE:
      return [ {
        ...defaultMeasurementProps,
        style: { width: '41%' },
      } ];

    case CardType.TEMPERATURE:
      return [ {
        ...defaultMeasurementProps,
        style: { width: '44%' },
      } ];

    case CardType.RESPIRATORY_RATE:
      return [ {
        ...defaultMeasurementProps,
        style: { width: '44%' },
      } ];

    case CardType.PAIN_LEVEL:
      return [ {
        ...defaultMeasurementProps,
        style: { width: '39%' },
        maxScaleValue,
      } ];

    case CardType.WEIGHT_HEIGHT:
      return [
        {
          ...defaultMeasurementProps,
          date: '',
          style: { minWidth: '26%', padding: '1.25rem 5.56rem 1.25rem 2.75rem' },
        },
        {
          text: measurementTwo &&
            getTextMeasurement(measurementTwo.measurementType, measurementTwo.measured),
          date: '',
          value: measurementTwo && measurementTwo.value,
          unit: measurementTwo &&
            getUnitMeasurement(measurementTwo.measurementType, measurementTwo.unit),
          style: { minWidth: '26%', padding: '1.25rem 2.75rem' },
        },
      ];

    case CardType.BODY_MASS_INDEX:
      return [
        {
          ...defaultMeasurementProps,
          date: '',
          value: calculateYears(birthDate, measured),
          unit: 'years',
          style: { width: '38%', padding: '1.25rem 4rem 1.25rem 7.25rem' },
        },
        {
          text: measurementTwo &&
            getTextMeasurement(measurementTwo.measurementType, measurementTwo.measured),
          date: '',
          value: measurementTwo && measurementTwo.value,
          unit: measurementTwo &&
            getUnitMeasurement(measurementTwo.measurementType, measurementTwo.unit),
          style: { minWidth: '32%', padding: '1.25rem 4.2rem 1.25rem 4rem' },
        },
        {
          text: measurementThree &&
            getTextMeasurement(measurementThree.measurementType, measurementTwo.measured),
          date: '',
          value: measurementThree && measurementThree.value,
          unit: measurementThree &&
            getUnitMeasurement(measurementThree.measurementType, measurementThree.unit),
          style: { minWidth: '30%', padding: '1.25rem 7.25rem 1.25rem 4rem' },
        },
      ];
  }
};

export const getConfigVitalPanelByType = ({
  datasets,
  cardType,
  measurements,
  birthDate = '',
}: GetConfigVitalPanelByTypeParameters): ConfigVitalPanelType =>
  ({
    measurementsComponents:
      getMeasurementsComponentsByType({
        datasets,
        cardType,
        measurements,
        birthDate,
      }),
    generalAdviceComponent: generalAdvices.get(cardType),
  });
