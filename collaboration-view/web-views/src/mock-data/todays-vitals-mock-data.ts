import { CardType, VitalType, MeasurementType } from '@enums';
import { TodayVitalCard } from '@types';

export const todaysVitalsMockData: TodayVitalCard[] = [
  {
    cardType: CardType.BLOOD_PRESSURE,
    vitals: [
      {
        vitalType: VitalType.BLOOD_PRESSURE,
        measurements: [
          {
            measurementType: MeasurementType.SYSTOLIC_BLOOD_PRESSURE,
            value: 135.14,
            measured: '2021-12-01T11:52:37.583195+00:00',
            unit: 'mm[Hg]',
          },
          {
            measurementType: MeasurementType.DIASTOLIC_BLOOD_PRESSURE,
            value: 93.07,
            measured: '2021-12-01T11:52:37.583195+00:00',
            unit: 'mm[Hg]',
          },
        ],
      },
    ],
  },
  {
    cardType: CardType.BLOOD_OXYGEN,
    vitals: [
      {
        vitalType: VitalType.BLOOD_OXYGEN,
        measurements: [
          {
            measurementType: MeasurementType.OXYGEN_SATURATION,
            value: 92.31,
            measured: '2021-12-01T11:52:37.5832382+00:00',
            unit: '%',
          },
        ],
      },
    ],
  },
  {
    cardType: CardType.HEART_RATE,
    vitals: [
      {
        vitalType: VitalType.HEART_RATE,
        measurements: [
          {
            measurementType: MeasurementType.HEART_RATE,
            value: 81,
            measured: '2021-12-01T11:52:37.5832474+00:00',
            unit: '/min',
          },
        ],
      },
    ],
  },
  {
    cardType: CardType.TEMPERATURE,
    vitals: [
      {
        vitalType: VitalType.TEMPERATURE,
        measurements: [
          {
            measurementType: MeasurementType.BODY_TEMPERATURE,
            value: 36.71,
            measured: '2021-12-01T11:52:37.583256+00:00',
            unit: 'Cel',
          },
        ],
      },
    ],
  },
  {
    cardType: CardType.RESPIRATORY_RATE,
    vitals: [
      {
        vitalType: VitalType.RESPIRATORY_RATE,
        measurements: [
          {
            measurementType: MeasurementType.RESPIRATORY_RATE,
            value: 14,
            measured: '2021-12-01T11:52:37.583264+00:00',
            unit: '/min',
          },
        ],
      },
    ],
  },
  {
    cardType: CardType.PAIN_LEVEL,
    vitals: [
      {
        vitalType: VitalType.PAIN_LEVEL,
        measurements: [
          {
            measurementType: MeasurementType.PAIN_LEVEL,
            value: 3,
            measured: '2021-12-01T11:52:37.5832723+00:00',
            maxScaleValue: 10,
          },
        ],
      },
    ],
  },
  {
    cardType: CardType.WEIGHT_HEIGHT,
    vitals: [
      {
        vitalType: VitalType.WEIGHT,
        measurements: [
          {
            measurementType: MeasurementType.BODY_WEIGHT,
            value: 65.28,
            measured: '2021-12-01T11:52:37.5832855+00:00',
            unit: 'kg',
          },
        ],
      },
      {
        vitalType: VitalType.HEIGHT,
        measurements: [
          {
            measurementType: MeasurementType.BODY_HEIGHT,
            value: 175.29,
            measured: '2021-12-01T11:52:37.5832859+00:00',
            unit: 'cm',
          },
        ],
      },
    ],
  },
  {
    cardType: CardType.BODY_MASS_INDEX,
    vitals: [
      {
        vitalType: VitalType.BODY_MASS_INDEX,
        measurements: [
          {
            measurementType: MeasurementType.BODY_MASS_INDEX,
            value: 21.27,
            measured: '2021-12-01T11:52:37.5832993+00:00',
            unit: 'kg/m2',
          },
        ],
      },
    ],
  },
];
