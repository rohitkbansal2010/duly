import {
  addZeroIfOneDigit,
  calculateYears,
  dateStringToMDTooltip,
  makeHHMMFromDateString,
  writeTimeInterval,
  dateStringToMDYTooltip,
  formatMDYYYYDate,
  formatMMDDYYYYDate,
  getDatesForAppointments
} from '../date';

const mockDate1 = '2022-03-24T11:15:00-00:00';
const mockDate2 = '2020-03-03T01:01:00-00:00';
const mockDate3 = '2022-11-03T13:15:00-00:00';
const mockTooltipDate = '2/2/2022, 12:00:00 a.m.';

describe('dateUtils', () => {
  const mockGetHours = jest.spyOn(Date.prototype, 'getHours');
  const mockGetMinutes = jest.spyOn(Date.prototype, 'getMinutes');

  afterAll(() => {
    jest.resetAllMocks();
  });

  it('addZeroIfOneDigit', () => {
    expect(addZeroIfOneDigit(1)).toEqual('01');
    expect(addZeroIfOneDigit('1')).toEqual('01');
    expect(addZeroIfOneDigit('01')).toEqual('01');
    expect(addZeroIfOneDigit('11')).toEqual('11');
  });

  it('makeHHMMFromDateString', () => {
    mockGetHours.mockImplementation(() =>
      new Date(mockDate1).getUTCHours());

    mockGetMinutes.mockImplementation(() =>
      new Date(mockDate1).getUTCMinutes());

    expect(makeHHMMFromDateString(mockDate1)).toEqual('11:15');
    expect(makeHHMMFromDateString(mockDate1, true)).toEqual('11:15 AM');

    mockGetHours.mockImplementation(() =>
      new Date(mockDate2).getUTCHours());

    mockGetMinutes.mockImplementation(() =>
      new Date(mockDate2).getUTCMinutes());

    expect(makeHHMMFromDateString(mockDate2)).toEqual('1:01');
    expect(makeHHMMFromDateString(mockDate2, true)).toEqual('1:01 AM');

    mockGetHours.mockImplementation(() =>
      new Date(mockDate3).getUTCHours());

    mockGetMinutes.mockImplementation(() =>
      new Date(mockDate3).getUTCMinutes());

    expect(makeHHMMFromDateString(mockDate3)).toEqual('13:15');
    expect(makeHHMMFromDateString(mockDate3, true)).toEqual('1:15 PM');
  });

  it('writeTimeInterval', () => {
    mockGetHours.mockImplementation(() =>
      new Date(mockDate1).getUTCHours());

    mockGetMinutes.mockImplementation(() =>
      new Date(mockDate1).getUTCMinutes());

    expect(writeTimeInterval(new Date(mockDate1), new Date(mockDate1))).toEqual('11:15 am - 11:15 am');

    mockGetHours.mockImplementation(() =>
      new Date(mockDate2).getUTCHours());

    mockGetMinutes.mockImplementation(() =>
      new Date(mockDate2).getUTCMinutes());

    expect(writeTimeInterval(new Date(mockDate2), new Date(mockDate2))).toEqual('1:01 am - 1:01 am');
  });

  it('calculateYears', () => {
    expect(calculateYears(mockDate2, mockDate1)).toEqual(2);
    expect(calculateYears(mockDate1, mockDate3)).toEqual(0);
  });

  it('dateStringToMDTooltip', () => {
    expect(dateStringToMDTooltip(mockTooltipDate)).toEqual('February 2');
  });

  it('dateStringToMDYTooltip', () => {
    expect(dateStringToMDYTooltip(mockTooltipDate)).toEqual('2/2/2022');
  });

  it('formatMDYYYYDate', () => {
    expect(formatMDYYYYDate(mockDate1)).toEqual('3/24/2022');
    expect(formatMDYYYYDate(mockDate2)).toEqual('3/3/2020');
  });

  it('formatMMDDYYYYDate', () => {
    expect(formatMMDDYYYYDate(mockDate1)).toEqual('2022-03-24');
    expect(formatMMDDYYYYDate(mockDate2)).toEqual('2020-03-03');
  });

  it('getDatesForAppointments', () => {
    jest.spyOn(Date.prototype, 'getTimezoneOffset')
      .mockImplementation(() =>
        0);

    expect(getDatesForAppointments(new Date(mockDate1)))
      .toEqual({
        endDate: '2022-03-24T19:00+00:00',
        startDate: '2022-03-24T07:00+00:00',
      });
  });
});
