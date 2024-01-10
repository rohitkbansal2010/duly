import { ChartsMockData } from '@interfaces';

export const pulseRateMockData: ChartsMockData = {
  datasets: [ {
    label: 'Pulse Rate',
    data: {
      values: [
        {
          number: 90,
          date: '2021-09-28T08:30:00',
          trend: 'uptrend',
        },
        {
          number: 92,
          date: '2021-09-28T09:00:00',
          trend: 'uptrend',
        },
        {
          number: 93,
          date: '2021-09-28T09:30:00',
          trend: 'uptrend',
        },
        {
          number: 91,
          date: '2021-09-28T10:00:00',
          trend: 'downtrend',
        },
        {
          number: 92,
          date: '2021-09-28T10:30:00',
          trend: 'uptrend',
        },
        {
          number: 95,
          date: '2021-09-28T11:00:00',
          trend: 'uptrend',
        },
        {
          number: 90,
          date: '2021-09-28T11:30:00',
          trend: 'downtrend',
        },
        {
          number: 91,
          date: '2021-09-28T12:00:00',
          trend: 'uptrend',
        },
        {
          number: 89,
          date: '2021-09-28T12:30:00',
          trend: 'downtrend',
        },
        {
          number: 89,
          date: '2021-09-28T13:00:00',
          trend: 'downtrend',
        },
        {
          number: 88,
          date: '2021-09-28T13:30:00',
          trend: 'downtrend',
        },
        {
          number: 88,
          date: '2021-09-28T14:00:00',
          trend: 'downtrend',
        },
        {
          number: 90,
          date: '2021-09-28T14:30:00',
          trend: 'uptrend',
        },
        {
          number: 91,
          date: '2021-09-28T15:00:00',
          trend: 'uptrend',
        },
        {
          number: 90,
          date: '2021-09-28T15:30:00',
          trend: 'downtrend',
        },
      ],
      average: 82,
      dimension: 'PR/min',
    },
  } ],
  options: {
    scales: {
      y: {
        max: 250,
        min: 30,
        stepSize: 110,
      },
    },
  },
};
