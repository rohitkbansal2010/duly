import { FadedScroll } from '@components/faded-scroll-2';
import { TestReport } from '@types';

import { ResultsMenuItem } from './results-menu-item';
import { ResultsMenuSkeleton } from './results-menu-skeleton';
import styles from './results-menu.scss';

type ResultsMenuProps = {
  testReports: TestReport[] | null;
  isTestReportsSkeletonShown: boolean;
  onClick: (id: string) => () => void;
  activeTestReportId: string;
  page: number;
}

export const ResultsMenu = ({
  isTestReportsSkeletonShown,
  testReports,
  onClick,
  activeTestReportId,
  page,
}: ResultsMenuProps) =>
  (
    isTestReportsSkeletonShown
      ? <ResultsMenuSkeleton />
      : (
        <FadedScroll height="100%" oversize>
          <div className={styles.resultsMenu}>
            {testReports?.map(({ id, ...testReport }) =>
              (
                <ResultsMenuItem
                  key={id}
                  isActive={id === activeTestReportId}
                  onClick={onClick(id)}
                  {...testReport}
                />
              ))}
            {!(
              page >= Number(window.env.TEST_RESULTS_COUNT_OF_PERIOD) ||
              testReports && testReports.length >= Number(window.env.TEST_RESULTS_MAX_COUNT)
            ) && <ResultsMenuSkeleton />}
          </div>
        </FadedScroll>
      )
  );
