import { ChartsMockData } from '@interfaces';

export const bloodPressureMockData: ChartsMockData = {
  datasets: [
    {
      label: 'Systolic',
      data: {
        values:
          [
            {
              x: '2019-10-31T17:31:00',
              y: 95.0,
            },
            {
              x: '2019-09-30T17:29:00',
              y: 95.0,
            },
          ],
        dimension: 'mmHG',
      },
      range: {
        min: 110,
        max: 160,
      },
      visible: true,
    },
    {
      label: 'Diastolic',
      data: {
        values:
          [
            {
              x: '2019-10-31T17:31:00',
              y: 90,
            },
            {
              x: '2019-09-30T17:29:00',
              y: 90,
            },
          ],
        dimension: 'mmHG',
      },
      range: {
        min: 80,
        max: 90,
      },
      visible: true,
    },
  ],
  chartOptions: {
    chartScales: {
      yAxis: {
        max: 240,
        min: 0,
        stepSize: 40,
      },
    },
  },
};
