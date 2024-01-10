import { useSelector } from 'react-redux';

import { RootState } from '@redux/reducers';

import { ResultsComponents } from './results-components';
import { ResultsDetails } from './results-details';
import { ResultsHeader } from './results-header';
import { TestResultsSkeleton } from './test-results-skeleton';
import styles from './test-results.scss';

export const TestResults = () => {
  const testReportsResults = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.testReportsResults);
  const isTestReportResultsSkeletonShown = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.isTestReportResultsSkeletonShown);

  const {
    id,
    effectiveDate,
    title,
    issued,
    status,
    performers,
    results,
  } = testReportsResults || {};

  if (isTestReportResultsSkeletonShown) {
    return <TestResultsSkeleton />;
  }

  if (!testReportsResults) {
    return null;
  }

  return (
    <div className={styles.results}>
      <ResultsHeader
        title={title}
        effectiveDate={effectiveDate}
      />
      <ResultsDetails
        effectiveDate={effectiveDate}
        issued={issued}
        status={status!}
        performers={performers}
      />
      <ResultsComponents
        currentTestResultsId={id}
        results={results}
      />
    </div>
  );
};
