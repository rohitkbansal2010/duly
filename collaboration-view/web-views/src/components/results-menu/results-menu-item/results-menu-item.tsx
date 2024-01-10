import { speechBubbleOrangeIcon, testResultsLightBlueIcon } from '@icons';
import { TestReport } from '@types';
import { getTestReportDate } from '@utils';

import styles from './results-menu-item.scss';

type ResultsMenuItemProps = Omit<TestReport, 'id'> & {
  isActive: boolean;
  onClick: () => void;
}

export const ResultsMenuItem = ({
  title, date, hasAbnormalResults, isActive, onClick,
}: ResultsMenuItemProps) => {
  const resultsMenuItemClasses = []
    .concat(styles.resultsMenuItem)
    .concat(isActive ? styles.resultsMenuItemActive : '')
    .join(' ');
  
  return (
    <button
      className={resultsMenuItemClasses}
      onClick={onClick}
    >
      <div className={styles.resultsMenuItemIconWrapper}>
        <img
          src={testResultsLightBlueIcon}
          alt="test result icon"
          className={styles.resultsMenuItemIcon}
        />
        {hasAbnormalResults && (
          <img
            src={speechBubbleOrangeIcon}
            alt="abnormal result icon"
            className={styles.resultsMenuItemIconAbnormal}
          />
        )}
      </div>
      <div className={styles.resultsMenuItemTitles}>
        <span className={styles.resultsMenuItemTitlesMain}>
          {title}
        </span>
        <span className={styles.resultsMenuItemTitlesAdditional}>
          {date && getTestReportDate(date)}
        </span>
      </div>
    </button>
  );
};
