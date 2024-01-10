import { CardType, MeasurementType, VitalType } from '@enums';

import { getTitleByCardType } from '../vitals';

describe('vitalUtils', () => {
  it('getTitleByCardType', () => {
    expect(getTitleByCardType([ {
      vitalType: VitalType.BLOOD_PRESSURE,
      measurements: [
        {
          measurementType: MeasurementType.SYSTOLIC_BLOOD_PRESSURE,
          value: 135.14,
          measured: '2021-12-01T11:52:37.583195+00:00',
        },
      ],
    } ], CardType.BLOOD_PRESSURE, true)).toEqual('Blood Pressure');

    expect(getTitleByCardType([ {
      vitalType: VitalType.BLOOD_PRESSURE,
      measurements: [
        {
          measurementType: MeasurementType.SYSTOLIC_BLOOD_PRESSURE,
          value: 135.14,
          measured: '2021-12-01T11:52:37.583195+00:00',
        },
      ],
    } ], CardType.BLOOD_PRESSURE, false)).toEqual('BLOOD PRESSURE');

    expect(getTitleByCardType([ {
      vitalType: VitalType.HEIGHT,
      measurements: [
        {
          measurementType: MeasurementType.BODY_HEIGHT,
          value: 135.14,
          measured: '2021-12-01T11:52:37.583195+00:00',
        },
      ],
    } ], CardType.WEIGHT_HEIGHT, false)).toEqual('WEIGHT & HEIGHT');

    expect(getTitleByCardType([ {
      vitalType: VitalType.HEIGHT,
      measurements: [
        {
          measurementType: MeasurementType.BODY_HEIGHT,
          value: 135.14,
          measured: '2021-12-01T11:52:37.583195+00:00',
        },
      ],
    } ], CardType.WEIGHT_HEIGHT, true)).toEqual('Weight and Height');
  });
});
