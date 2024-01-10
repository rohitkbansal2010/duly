import { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useHistory, useParams } from 'react-router-dom';

import { ResultsMenu } from '@components/results-menu';
import { TestResults } from '@components/test-results';
import {
  clearAllAccordions,
  getTestReports,
  getTestReportsResults
} from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { getCurrentTestResultUrl, getTestReportPeriods } from '@utils';

import { NoResultsModule } from './no-results-module';
import styles from './results-module.scss';

export const ResultsModule = () => {
  const dispatch: AppDispatch = useDispatch();
  const history = useHistory();

  const [ datePeriods, setDatePeriods ] = useState(null);

  const { appointmentWidgetRoute: activeTestReportId } =
    useParams<{ appointmentWidgetRoute: string }>();

  const patientId = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientId);
  const testReports = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.testReports);
  const isTestReportsSkeletonShown = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.isTestReportsSkeletonShown);
  const isTestResultsMounted = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.isTestResultsMounted);
  const page = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.page);
  const isTestReportFetching = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.isTestReportFetching);

  const onClick = useCallback((currentTestReportId: string) =>
    () => {
      if (activeTestReportId !== currentTestReportId) {
        history.push(getCurrentTestResultUrl(currentTestReportId));
        dispatch(getTestReportsResults(currentTestReportId));
        dispatch(clearAllAccordions());
      }
    }, [ dispatch, history, activeTestReportId ]);

  useEffect(() => {
    setDatePeriods(getTestReportPeriods());
  }, []);

  useEffect(() => {
    if (testReports && !isTestResultsMounted) {
      const currentTestReportId = activeTestReportId || testReports[0]?.id || '';

      history.replace(getCurrentTestResultUrl(currentTestReportId));
      dispatch(getTestReportsResults(currentTestReportId));
    }
  }, [ dispatch, history, testReports, isTestResultsMounted ]);

  useEffect(() => {
    if (
      page < Number(window.env.TEST_RESULTS_COUNT_OF_PERIOD) &&
        !isTestReportFetching &&
        datePeriods &&
        (testReports === null || testReports.length < Number(window.env.TEST_RESULTS_MAX_COUNT))
    ) {
      const { startDate, endDate } = datePeriods[page];

      dispatch(getTestReports({ patientId, startDate, endDate }));
    }
  }, [ dispatch, patientId, page, isTestReportFetching, datePeriods, testReports ]);

  return (
    <main className={styles.results}>
      <h2 className={styles.resultsTitle}>test results</h2>
      {testReports?.length || isTestReportsSkeletonShown
        ? (
          <div className={styles.resultsContainer}>
            <div className={styles.resultsMenu}>
              <ResultsMenu
                testReports={testReports}
                isTestReportsSkeletonShown={isTestReportsSkeletonShown}
                onClick={onClick}
                activeTestReportId={activeTestReportId}
                page={page}
              />
            </div>
            <div className={styles.resultsContent}>
              <TestResults />
            </div>
          </div>
          )
        : <NoResultsModule />}
    </main>
  );
};
