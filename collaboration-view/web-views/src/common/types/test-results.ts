import { TestResultStatus, TestResultsAccordions } from '@enums';
import { HumanName, Photo } from '@types';

export type TestReport = {
  id: string;
  title: string;
  date: string;
  hasAbnormalResults: boolean;
}

export type TestReportRequest = {
  patientId: string;
  startDate: string;
  endDate: string;
  amount?: number;
}

export type TestResultMeasurement = {
  value?: number;
  unit?: string;
}

export type ReferenceRange = {
  text?: string;
  high?: TestResultMeasurement;
  low?: TestResultMeasurement;
}

export type TestResult = {
  id: string;
  componentName: string;
  isAbnormalResult: boolean;
  measurement?: TestResultMeasurement;
  referenceRange?: ReferenceRange;
  valueText?: string;
  notes?: string[];
  interpretations?: string[];
}

export type Performer = {
  id: string;
  humanName: HumanName;
  photo?: Photo;
  role: string;
}

export type TestReportsResult = {
  id: string;
  title: string;
  status: TestResultStatus;
  effectiveDate: string;
  performers?: Performer[];
  issued?: string;
  results?: TestResult[];
};

export type PerformerInfo = {
  familyName: string;
  givenNames: string[];
  prefixes?: string[];
  photo?: Photo;
  role: string, 
};

export type TestResultsAccordionTypes = {
  [key in TestResultsAccordions]: {
    current: TestResultsAccordions;
    other: TestResultsAccordions;
  }
}
