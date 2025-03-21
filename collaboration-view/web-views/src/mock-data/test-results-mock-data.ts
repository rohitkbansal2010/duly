import { PractitionerRole, TestResultStatus } from '@enums';
import { TestReport, TestReportsResult } from '@types';

export const testReportsMockData: TestReport[] = [
  {
    id: '838b65ae-0226-4a67-ae36-949ee54c7428',
    title: 'Comprehensive Metabolic Panel',
    date: '2021-08-14',
    hasAbnormalResults: true,
  },
  {
    id: '1cb904df-efc9-4ceb-b0d5-ca6acc278566',
    title: 'Alanine Aminotransferease',
    date: '2021-03-19',
    hasAbnormalResults: false,
  },
  {
    id: '809917e9-be5b-41c8-a71a-ea1f80ca1ccb',
    title: 'Urinalysis',
    date: '2021-03-19',
    hasAbnormalResults: true,
  },
];

export const testReportsResultMockData: TestReportsResult = {
  id: '3bab8750-382b-4a66-8841-9c61872ef8b4',
  title: 'Comprehensive Metabolic Panel',
  status: TestResultStatus.FINAL,
  effectiveDate: '2021-08-14T10:17:31Z',
  performers: [ {
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
      data: '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',
    },
    role: PractitionerRole.PCP,
  } ],
  issued: '2021-08-14T10:17:31Z',
  results: [ {
    id: '06cd80cf-7230-4e53-bdd1-6956402b2ef8',
    componentName: 'Cholesterol, Total',
    isAbnormalResult: false,
    measurement: {
      value: 150,
      unit: 'mg/dL',
    },
    referenceRange: { text: '<200' },
    notes: [ 'Desirable  <200 mg/dL\\r\\nBorderline  200-239 mg/dL\\r\\nHigh      >=240 mg/dL\\r\\n\\r\\n' ],
  }, {
    id: '9bea73b9-d173-4cc4-b67b-fb43971557e4',
    componentName: 'LDL Cholesterol',
    isAbnormalResult: true,
    measurement: {
      value: 117,
      unit: 'mg/dL',
    },
    referenceRange: { text: '<100' },
    notes: [ 'Desirable <100 mg/dL \\r\\nBorderline 100-129 mg/dL \\r\\nHigh     >=130mg/dL\\r\\n\\r\\n' ],
  }, {
    id: 'c1f12ce2-323e-4c7b-b3e0-73183ad58330',
    componentName: 'Sodium',
    isAbnormalResult: false,
    measurement: {
      value: 138,
      unit: 'mmol/L',
    },
    referenceRange: {
      high: {
        value: 145,
        unit: 'mmol/L',
      },
      low: {
        value: 136,
        unit: 'mmol/L',
      },
      text: '136 - 145 mmol/L',
    },
  } ],
};
