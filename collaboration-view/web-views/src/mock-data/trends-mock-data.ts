import { TrendsRating } from '@enums';
import { Trend } from '@types';

export const trendsMockData: Trend[] = [
  {
    oxygenSaturation: {
      measure: '%',
      average: {
        value: 90,
        rated: TrendsRating.BELOW_STANDARD,
      },
      highest: {
        value: 92,
        rated: TrendsRating.STANDARD,
      },
      lowest: {
        value: 84,
        rated: TrendsRating.BELOW_STANDARD,
      },
    },
  },
  {
    pulseRate: {
      average: {
        value: 57,
        rated: TrendsRating.STANDARD,
      },
      highest: {
        value: 120,
        rated: TrendsRating.ABOVE_STANDARD,
      },
      lowest: {
        value: 36,
        rated: TrendsRating.BELOW_STANDARD,
      },
    },
  },
];
