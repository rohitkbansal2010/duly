import { HealthJourneyType } from '@enums';

export type HealthJourney = {
  type: HealthJourneyType;
  title: string;
  link: string;
  preview?: string;
  time?: string;
};
