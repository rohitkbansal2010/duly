import {
  MINUTES_PER_HOUR,
  OF_MILLISECONDS,
  SECONDS_PER_MINUTE,
  UNIX_START_YEAR,
  DEFAULT_WORK_DAY_START_TIME,
  DEFAULT_WORK_DAY_END_TIME
} from '@constants';
import { PeriodName } from '@enums';
import { formatHourToStringPmAm } from '@utils';

export const addZeroIfOneDigit = (num: number| string): string | number =>
  Number(num) < 10 ? `0${Number(num)}` : `${Number(num)}`;

const getTime = (date: Date): string =>
  formatHourToStringPmAm(date.getHours(), date.getMinutes());

export const makeHHMMFromDateString = (dateString: string, is12HourClock?: boolean) => {
  const MERIDIEM_TIME = 12;
  const date = new Date(dateString);
  const hours = date.getHours();
  const minutes = date.getMinutes();
  if (is12HourClock) {
    const period = hours < MERIDIEM_TIME ? PeriodName.AM : PeriodName.PM;
    return `${hours % MERIDIEM_TIME || MERIDIEM_TIME}:${addZeroIfOneDigit(minutes)} ${period}`;
  }
  return `${hours}:${addZeroIfOneDigit(minutes)}`;
};

type GetUTCOffsetType = (date?: Date | string | number) => string

export const getUTCOffset: GetUTCOffsetType = (date = new Date): string => {
  const rawOffset = new Date(date).getTimezoneOffset() / MINUTES_PER_HOUR;

  if (rawOffset === 0) {
    return '';
  }

  const sign = rawOffset > 0 ? '-' : '+';

  return `${sign}${addZeroIfOneDigit(Math.abs(rawOffset))}:00`;
};

export const writeTimeInterval = (startDate: Date, endDate: Date): string =>
  `${getTime(startDate)} - ${getTime(endDate)}`;

export const calcDurationInMinutes = (startTime: Date, endTime: Date): number =>
  (endTime.getTime() - startTime.getTime()) / (OF_MILLISECONDS * SECONDS_PER_MINUTE);

export const calculateYears = (
  untilDate?: Date | string | number,
  fromDate?: Date | string | number
): number => {
  if (untilDate) {
    const untilDateTimestamp: number = new Date(untilDate).valueOf();
    const fromDateTimestamp: number = fromDate
      ? new Date(fromDate).valueOf()
      : new Date().valueOf();
    const timestampDifference: number = fromDateTimestamp - untilDateTimestamp;
    const yearsOld = new Date(timestampDifference).getFullYear() - UNIX_START_YEAR;

    return yearsOld;
  }

  return 0;
};

export const transformDateMMYYYYToTimestamp = (dateString?: string): number => {
  if (dateString) {
    const [ month, year ] = dateString.split('/');
    const dateTimestamp = new Date(Number(year), Number(month)).valueOf();

    return dateTimestamp;
  }

  return 0;
};

const removeDotsAMPMFromDateTooltip = (dateString: string) => 
  dateString.replace(/\./g, '');

export const dateStringToMDTooltip = (dateString: string) => {
  const fullDate = new Date(removeDotsAMPMFromDateTooltip(dateString));
  const month = fullDate.toLocaleString('default', { month: 'long' });
  const day = fullDate.getDate();

  return `${month} ${day}`;
};

export const dateStringToMDYTooltip = (dateString?: string) => {
  if (dateString) {
    const startOfDate = 0;
    const endOfDate = dateString.lastIndexOf(',');
    return dateString.slice(startOfDate, endOfDate);
  }
};

export const formatMDYYYYDate = (date: Date | string | number): string => {
  const currentDate = new Date(date);

  return [
    currentDate.getMonth() + 1,
    currentDate.getDate(),
    currentDate.getFullYear(),
  ].join('/');
};

export const formatMMDDYYYYDate = (date: Date | string | number): string => {
  const currentDate = new Date(date);

  return [
    currentDate.getFullYear(),
    addZeroIfOneDigit(currentDate.getMonth() + 1),
    addZeroIfOneDigit(currentDate.getDate()),
  ].join('-');
};

const formatWorkDayTime = (hours?: string): string =>
  hours ? `${addZeroIfOneDigit(hours)}:00` : '';

export const getDatesForAppointments = (date = new Date()) => {
  const day = addZeroIfOneDigit(date.getDate());
  const month = addZeroIfOneDigit(date.getMonth() + 1);
  const year = date.getFullYear();
  const dateString = `${year}-${month}-${day}`;
  const offset = getUTCOffset(date) || '+00:00';
  const workDayStartTime = formatWorkDayTime(window?.env?.WORK_DAY_START)
    || DEFAULT_WORK_DAY_START_TIME;
  const workDayEndTime = formatWorkDayTime(window?.env?.WORK_DAY_END)
    || DEFAULT_WORK_DAY_END_TIME;

  return {
    startDate: `${dateString}T${workDayStartTime}${offset}`,
    endDate: `${dateString}T${workDayEndTime}${offset}`,
  };
};
export const formatDateStringAddComma = (dateString: string): string => {
  const [ day, month, dd, year ] = dateString.split(' ');
  return `${day}, ${month} ${dd}, ${year}`;
};
