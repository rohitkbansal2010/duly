import {
  getTestReportDate,
  getTestReportTime,
  getMeasurementValue,
  getMeasurementUnit,
  getReferenceRange,
  getPerformerInfo,
  formattedNotes,
  formattedInterpretations
} from '../test-results';

const mockDate1 = '2022-03-24';

describe('testResultsUtils', () => {
  it('getTestReportDate', () => {
    expect(getTestReportDate(mockDate1)).toEqual('Thu, Mar 24, 2022');
  });

  it('getTestReportTime', () => {
    jest.spyOn(Date.prototype, 'toLocaleTimeString').mockImplementation(() =>
      '11:15 AM');

    expect(getTestReportTime(mockDate1)).toEqual('11:15 AM');
  });

  it('getMeasurementValue', () => {
    expect(getMeasurementValue({
      value: 138,
      unit: 'mmol/L',
    })).toEqual(138);

    expect(getMeasurementValue({ unit: 'mmol/L' })).toEqual('');
  });

  it('getMeasurementUnit', () => {
    expect(getMeasurementUnit({
      value: 138,
      unit: 'mmol/L',
    })).toEqual('mmol/L');

    expect(getMeasurementUnit({ value: 138 })).toEqual('');
  });

  it('getReferenceRange', () => {
    expect(getReferenceRange()).toEqual('');

    expect(getReferenceRange({
      high: {
        value: 145,
        unit: 'mmol/L',
      },
      low: {
        value: 136,
        unit: 'mmol/L',
      },
      text: '136 - 145 mmol/L', 
    })).toEqual('136 - 145 mmol/L');

    expect(getReferenceRange({
      high: { value: 145 },
      low: {
        value: 136,
        unit: 'mmol/L',
      },
    })).toEqual('136 - 145 mmol/L');

    expect(getReferenceRange({
      high: {
        value: 145,
        unit: 'mmol/L',
      },
      low: { value: 136 },
    })).toEqual('136 - 145 mmol/L');
  });

  it('getPerformerInfo', () => {
    expect(getPerformerInfo([])).toEqual({});

    expect(getPerformerInfo([ {
      id: 'qwerty1',
      humanName: {
        familyName: 'Reyes',
        givenNames: [ 'Ana', 'Maria' ],
        prefixes: [ 'Dr.' ],
      },
      photo: {
        contentType: 'image/x-png',
        title: 'Photo',
        size: 0,
      },
      role: 'PCP',
    } ])).toEqual({
      familyName: 'Reyes',
      givenNames: [ 'Ana', 'Maria' ],
      prefixes: [ 'Dr.' ],
      photo: {
        contentType: 'image/x-png',
        title: 'Photo',
        size: 0,
      },
      role: 'PCP', 
    });
  });

  it('formattedNotes', () => {
    expect(formattedNotes([
      'Desirable  <200 mg/dL\\r\\nBorderline  200-239 mg/dL\\r\\nHigh      >=240 mg/dL\\r\\n\\r\\n',
    ])).toEqual(
      `Desirable  <200 mg/dL
Borderline  200-239 mg/dL
High      >=240 mg/dL`
    );
  });

  it('formattedInterpretations', () => {
    expect(formattedInterpretations([])).toEqual('');

    expect(formattedInterpretations([
      'High\\r\\n Low',
    ])).toEqual(`Observation Interpretation: High Low
`);
  });
});
