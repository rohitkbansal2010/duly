import { Level } from '@enums';

type HealthIndicatorType = {
  indicatorName: string;
  currentValue: string;
  previousValue: string;
  riskLevel: Level;
};

export const healthIndicators: HealthIndicatorType[] = [
  {
    indicatorName: 'Blood Pressure',
    currentValue: '150/80',
    previousValue: '143/78',
    riskLevel: Level.HIGH,
  },
  {
    indicatorName: 'Weight',
    currentValue: '135 lbs',
    previousValue: '139',
    riskLevel: Level.LOW,
  },
  {
    indicatorName: 'Body Mass Index',
    currentValue: '25.6',
    previousValue: '26.2',
    riskLevel: Level.LOW,
  },
  {
    indicatorName: 'Blood Oxygen',
    currentValue: '94%',
    previousValue: '94%',
    riskLevel: Level.HIGH,
  },
  {
    indicatorName: 'Temperature',
    currentValue: '98.6',
    previousValue: '98.6',
    riskLevel: Level.LOW,
  },
  {
    indicatorName: 'Heart Rate',
    currentValue: '70',
    previousValue: '70',
    riskLevel: Level.LOW,
  },
  {
    indicatorName: 'Respiratory Rate',
    currentValue: '18',
    previousValue: '18',
    riskLevel: Level.LOW,
  },
  {
    indicatorName: 'Pain Level',
    currentValue: '5/10',
    previousValue: '4',
    riskLevel: Level.MEDIUM,
  },
];
