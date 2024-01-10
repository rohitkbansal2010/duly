import { Accordion } from 'react-bootstrap';

import { CustomAccordionToggle } from '@components/custom-accordion-toggle';
import { LevelsChart } from '@components/levels-chart';
import { TEST_RESULTS_CHART_HEIGHT, NO_SHOWING_TEXT } from '@constants';
import { TestResultsAccordions } from '@enums';
import {
  noteResultDarkBlueIcon,
  pastResultLightBlueIcon,
  speechBubbleOrangeIcon
} from '@icons';
import { CHART_MOCK_DATA } from '@mock-data';
import { TestResultMeasurement, ReferenceRange } from '@types';
import {
  formattedInterpretations,
  formattedNotes,
  getMeasurementUnit,
  getMeasurementValue,
  getReferenceRange
} from '@utils';

import styles from './results-item.scss';

type ResultsComponentProps = {
  componentName: string;
  isAbnormalResult: boolean;
  measurement?: TestResultMeasurement;
  referenceRange?: ReferenceRange;
  valueText?: string;
  notes?: string[];
  interpretations?: string[];
  eventKey: string;
  activeKey: string;
  onClickNotes: () => void;
  onClickPastResults: () => void;
}

export const ResultsItem = ({
  componentName,
  isAbnormalResult,
  measurement,
  referenceRange,
  valueText = '',
  notes = [],
  interpretations = [],
  activeKey,
  eventKey,
  onClickNotes,
  onClickPastResults,
}: ResultsComponentProps) => {
  const resultComponentsItemClasses = []
    .concat(styles.resultsComponentsItem)
    .concat(isAbnormalResult ? styles.resultsComponentsItemAbnormal : '')
    .join(' ');

  const isNotesShown = Boolean(notes.length || interpretations.length);

  return (
    <Accordion
      activeKey={activeKey}
      className={styles.resultsComponentsItemAccordion}
    >
      <div className={resultComponentsItemClasses}>
        <div className={styles.resultsComponentsItemName}>
          {componentName}
        </div>
        <div className={styles.resultsComponentsItemValue}>
          {(!valueText.toLowerCase().includes(NO_SHOWING_TEXT) && valueText) ||
            (measurement && `${getMeasurementValue(measurement)} ${getMeasurementUnit(measurement)}`)}
          {isAbnormalResult && <img src={speechBubbleOrangeIcon} alt="abnormal result" />}
        </div>
        <div className={styles.resultsComponentsItemRange}>
          {getReferenceRange(referenceRange)}
        </div>
        <div className={styles.resultsComponentsItemAdditional}>
          <CustomAccordionToggle
            eventKey={`${eventKey}-${TestResultsAccordions.PAST_RESULTS}`}
            callback={onClickPastResults}
          >
            {decoratedOnClick =>
                (
                  <button
                    className={styles.resultsComponentsItemAdditionalButton}
                    onClick={decoratedOnClick}
                  >
                    <img src={pastResultLightBlueIcon} alt="past results" />
                    <span>past results</span>
                  </button>
                )}
          </CustomAccordionToggle>
          {isNotesShown && (
            <CustomAccordionToggle
              eventKey={`${eventKey}-${TestResultsAccordions.NOTES}`}
              callback={onClickNotes}
            >
              {decoratedOnClick =>
                (
                  <button
                    className={styles.resultsComponentsItemAdditionalButton}
                    onClick={decoratedOnClick}
                  >
                    <img src={noteResultDarkBlueIcon} alt="notes" />
                    <span>notes</span>
                  </button>
                )}
            </CustomAccordionToggle>
          )}
        </div>
      </div>
      <Accordion.Collapse
        eventKey={`${eventKey}-${TestResultsAccordions.NOTES}`}
      >
        <div className={styles.resultsComponentsItemExpand}>
          <span className={styles.resultsComponentsItemExpandTitle}>notes</span>
          <div className={styles.resultsComponentsItemExpandText}>
            {formattedInterpretations(interpretations)}
            {formattedNotes(notes)}
          </div>
        </div>
      </Accordion.Collapse>
      <Accordion.Collapse
        eventKey={`${eventKey}-${TestResultsAccordions.PAST_RESULTS}`}
      >
        <div className={styles.resultsComponentsItemExpand}>
          <div className={styles.resultsComponentsItemExpandChart}>
            <LevelsChart
              chartId="past result chart"
              chartData={CHART_MOCK_DATA}
              height={TEST_RESULTS_CHART_HEIGHT}
            />
          </div>
        </div>
      </Accordion.Collapse>
    </Accordion>
  );
};
