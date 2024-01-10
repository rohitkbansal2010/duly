import { HealthJourneyType, PatientRoutes } from '@enums';
import { HealthJourney } from '@types';

export const healthJourneyMockData: HealthJourney[] = [
  {
    type: HealthJourneyType.APPOINTMENTS,
    title: 'You have new appointments to confirm!',
    link: '',
  },
  {
    type: HealthJourneyType.VISIT_SUMMARY,
    title: 'You have a new after visit summary',
    link: PatientRoutes.after_visit_summary,
  },
  {
    type: HealthJourneyType.MEETING,
    title: 'Healthy Meals ',
    link: '',
    preview: 'Zoom Meeting',
    time: '4:00 pm',
  },
];
