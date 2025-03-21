export const CHART_MOCK_DATA = {
  chartOptions: {
    chartScales: {
      yAxis: {
        max: 110,
        min: 70,
        stepSize: 10,
      },
    },
  },
  datasets: [
    {
      label: 'Past Results',
      data: {
        values: [
          { x: '2021-12-15T22:44:00', y: 90.7 },
          { x: '2021-11-15T22:44:00', y: 95.7 },
          { x: '2021-10-15T22:44:00', y: 97.7 },
          { x: '2021-09-15T22:44:00', y: 100.7 },
          { x: '2021-08-15T22:44:00', y: 105.7 },
          { x: '2021-07-15T22:44:00', y: 95.7 },
          { x: '2021-06-15T22:44:00', y: 108.7 },
          { x: '2021-05-15T22:44:00', y: 107.7 },
          { x: '2021-04-15T22:44:00', y: 80.7 },
          { x: '2021-03-15T22:44:00', y: 85.7 },
          { x: '2021-02-15T22:44:00', y: 90.7 },
          { x: '2021-01-15T22:44:00', y: 100.7 },
          { x: '2022-01-15T22:44:00', y: 100.7 },
          { x: '2022-02-15T22:44:00', y: 100.7 },
        ],
        dimension: 'mg/dl',
      },
    },
  ],
};
