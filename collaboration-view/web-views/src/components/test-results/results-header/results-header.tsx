import { testResultsLightBlueIcon } from '@icons';
import { getTestReportDate } from '@utils';

import styles from './results-header.scss';

type ResultsHeaderProps = {
  title?: string;
  effectiveDate?: string;
}

export const ResultsHeader = ({ title = '', effectiveDate = '' }: ResultsHeaderProps) =>
  (
    <div className={styles.resultsHeader}>
      <div className={styles.resultsHeaderIconWrapper}>
        <img
          src={testResultsLightBlueIcon}
          alt="test result icon"
          className={styles.resultsHeaderIcon}
        />
      </div>
      <div className={styles.resultsHeaderTitles}>
        <span className={styles.resultsHeaderTitlesMain}>
          {title}
        </span>
        <span className={styles.resultsHeaderTitlesAdditional}>
          {effectiveDate && getTestReportDate(effectiveDate)}
        </span>
      </div>
    </div>
  );
