import noop from 'lodash/noop';
import { useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { FadedScroll } from '@components/faded-scroll-2';
import { TEST_RESULTS_ACCORDION_TYPES } from '@constants';
import { TestResultsAccordions } from '@enums';
import { addAccordion, deleteAccordion } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { TestResult } from '@types';

import { ResultsItem } from '../results-item';

import styles from './results-components.scss';

type ResultsComponentsProps = {
  results?: TestResult[];
  currentTestResultsId?: string;
}

export const ResultsComponents = ({ currentTestResultsId = '', results }: ResultsComponentsProps) => {
  const dispatch = useDispatch();
  const accordions = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.accordions);

  const handleClick = useCallback((id: string, type: TestResultsAccordions) =>
    () => {
      const currentId = `${id}-${TEST_RESULTS_ACCORDION_TYPES[type].current}`;
      const otherId = `${id}-${TEST_RESULTS_ACCORDION_TYPES[type].other}`;

      dispatch(
        accordions[currentTestResultsId]?.includes(currentId)
          ? deleteAccordion(currentTestResultsId, currentId)
          : addAccordion(currentTestResultsId, currentId)
      );

      accordions[currentTestResultsId]?.includes(otherId) &&
        dispatch(deleteAccordion(currentTestResultsId, otherId));
    }, [ dispatch, accordions, currentTestResultsId ]);

  const getActiveKey = useCallback((id: string) =>
    accordions[currentTestResultsId]?.find((accordionId: string) =>
      accordionId === `${id}-${TestResultsAccordions.NOTES}` ||
        accordionId === `${id}-${TestResultsAccordions.PAST_RESULTS}`)
      || null, [ accordions, currentTestResultsId ]);

  return (
    <>
      {!!results?.length && (
      <div className={styles.resultsComponents}>
        <div className={styles.resultsComponentsHeader}>
          <div className={styles.resultsComponentsHeaderName}>components</div>
          <div className={styles.resultsComponentsHeaderValue}>your value</div>
          <div className={styles.resultsComponentsHeaderRange}>standard range</div>
          <div className={styles.resultsComponentsHeaderAdditional} />
        </div>
        <div className={styles.resultsComponentsList}>
          <FadedScroll height="100%" view="bottom-white">
            {results.map(({ id, ...rest }) =>
              (
                <ResultsItem
                  key={id}
                  eventKey={id}
                  activeKey={getActiveKey(id) as string}
                  onClickNotes={handleClick(id, TestResultsAccordions.NOTES)}
                  onClickPastResults={noop}
                  {...rest}
                />
              ))}
          </FadedScroll>
        </div>
      </div>
      )}
    </>
  );
};
