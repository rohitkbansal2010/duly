import { TestResultsAccordions } from '@enums';
import { TestResultsAccordionTypes } from '@types';

export const NO_SHOWING_TEXT = 'cannot be displayed';

export const TEST_RESULTS_ACCORDION_TYPES: TestResultsAccordionTypes = {
  [TestResultsAccordions.NOTES]: {
    current: TestResultsAccordions.NOTES,
    other: TestResultsAccordions.PAST_RESULTS,
  },
  [TestResultsAccordions.PAST_RESULTS]: {
    current: TestResultsAccordions.PAST_RESULTS,
    other: TestResultsAccordions.NOTES,
  },
};
