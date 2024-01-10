import { AppointmentModulesRoutes } from '@enums';
import {
  TestResultMeasurement,
  ReferenceRange,
  Performer,
  PerformerInfo
} from '@types';

import { getLocationByModule } from './appointment-page';

export const getTestReportDate = (date: Date | string | number): string =>
  new Date(date).toLocaleDateString('en-US', {
    weekday: 'short', month: 'short', day: 'numeric', year: 'numeric', 
  });

export const getTestReportTime = (date: Date | string | number): string =>
  new Date(date).toLocaleTimeString('en-US', { hour: 'numeric', minute: 'numeric' });

const YYYY_MM_DD_LENGHT = 10;
const ONE_DAY = 1;

const getDateBefore = (date: string): Date => {
  const currentDate = new Date(date);
  const currentDay = currentDate.getDate();
  const dateBefore = currentDate.setDate(currentDay - ONE_DAY);

  return new Date(dateBefore);
};

export const getTestReportPeriod = (fromDate: Date = new Date()) => {
  const endDate = fromDate
    .toISOString()
    .slice(0, YYYY_MM_DD_LENGHT);
  const startDateMonth = fromDate.getDate() - Number(window.env.TEST_RESULTS_DAY_PERIOD);
  const startRawDate = fromDate.setDate(startDateMonth);
  const startDate = new Date(startRawDate)
    .toISOString()
    .slice(0, YYYY_MM_DD_LENGHT);

  return { startDate, endDate };
};

export const getTestReportPeriods = () =>
  [ ...new Array(Number(window.env.TEST_RESULTS_COUNT_OF_PERIOD)) ].reduce((acc) => {
    const fromDate = acc.length ? getDateBefore(acc[acc.length - 1]?.startDate) : new Date();
    
    return [ ...acc, getTestReportPeriod(fromDate) ];
  }, []);

export const getMeasurementValue = (measurement?: TestResultMeasurement): string | number =>
  measurement?.value || '';

export const getMeasurementUnit = (measurement?: TestResultMeasurement): string =>
  measurement?.unit || '';

export const getReferenceRange = (referenceRange?: ReferenceRange): string => {
  if (referenceRange) {
    const { text, high, low } = referenceRange;
    const unit = getMeasurementUnit(high) || getMeasurementUnit(low);

    return text || `${getMeasurementValue(low)} - ${getMeasurementValue(high)} ${unit}`;
  }

  return '';
};

export const getPerformerInfo = (performers: Performer[] = []): PerformerInfo => {
  if (performers[0]) {
    const {
      humanName: {
        familyName,
        givenNames,
        prefixes,
      },
      photo,
      role,
    } = performers[0];

    return {
      familyName,
      givenNames,
      prefixes,
      photo,
      role, 
    };
  }

  return {} as PerformerInfo;
};

export const getCurrentTestResultUrl = (testReportId: string): string => 
  `${getLocationByModule(AppointmentModulesRoutes.MODULE_RESULTS)}/${testReportId}`;

export const formattedNotes = (notes: string[] = []): string =>
  notes
    .join('\n')
    .replace(/\\r\\n/g, '\n')
    .trim();

export const formattedInterpretations = (interpretations: string[] = []): string => {
  if (interpretations.length) {
    const interpretationsValue = interpretations
      .map(value =>
        value
          .replace(/\\r\\n/g, '')
          .trim())
      .join(' ');

    return `Observation Interpretation: ${interpretationsValue}\n`;
  }

  return '';
};
  
