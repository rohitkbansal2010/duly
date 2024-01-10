import { HealthConditionsWidgetDataType } from '@types';

export const healthConditionsMockData: HealthConditionsWidgetDataType = {
  previousHealthConditions: [
    {
      title: 'Pneumonia',
      date: '2022-02-11T00:00:00Z',
    },
    {
      title: 'Dehydration',
      date: '2022-02-01T00:00:00Z',
    },
    {
      title: 'Myocardial infarction (STEMI)',
      date: '2021-12-23T00:00:00Z',
    },
    { title: 'Hypertension associated with diabetes (HCC)' },
    { title: 'Kidney stones' },
  ],
  currentHealthConditions: [
    {
      title: 'Pneumonia',
      date: '2022-02-16T00:00:00Z',
    },
    {
      title: 'Chronic sinusitis',
      date: '2019-05-28T00:00:00Z',
    },
    { title: 'Diabetes mellitus, type II' },
    { title: 'Osteoarthritis' },
  ],
};
