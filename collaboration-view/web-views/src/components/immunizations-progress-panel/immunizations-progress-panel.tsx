import { PROGRESS_PANEL } from '@constants';
import { immunizationLightBlueIcon } from '@icons';
import { ImmunizationsProgress } from '@types';
import { PercentageDonutPieChart } from '@ui-kit/percentage-donut-pie-chart';

import styles from './immunizations-progress-panel.scss';

export const ImmunizationsProgressPanel =
  ({ percentageCompletion, recommendedGroupNumber, completionStatus }: ImmunizationsProgress) => {
    const iconWrapperClasses = []
      .concat(styles.progressPanelStatusIcon)
      .concat(styles[`progressPanelStatusIcon${completionStatus}`])
      .join(' ');

    return (
      <div className={styles.progressPanelContainer}>
        <div className={styles.progressPanelStatus}>
          <div className={iconWrapperClasses}>
            <img src={PROGRESS_PANEL[completionStatus].icon} alt={`${completionStatus} status`}/>
          </div>
          <div className={styles.progressPanelStatusTitles}>
            <span className={styles.progressPanelStatusTitle}>
              {PROGRESS_PANEL[completionStatus].title}
            </span>
            <span>
              {PROGRESS_PANEL[completionStatus].additional}
            </span>
          </div>
        </div>
        <div className={styles.progressPanelProgress}>
          <div className={styles.progressPanelProgressDoughnut}>
            <PercentageDonutPieChart percent={percentageCompletion}/>
          </div>
          <div className={styles.progressPanelProgressTitles}>
            <span className={styles.progressPanelProgressTitle}>progress</span>
            <span>
              {`${percentageCompletion}% completed`}
            </span>
          </div>
        </div>
        <div className={styles.progressPanelRecommended}>
          <div className={styles.progressPanelRecommendedIcon}>
            <img src={immunizationLightBlueIcon} alt="recommended immunizations"/>
          </div>
          <div className={styles.progressPanelRecommendedTitles}>
            <span className={styles.progressPanelRecommendedTitle}>recommended</span>
            <span>
              {recommendedGroupNumber} immunization{recommendedGroupNumber === 1 ? '' : 's'}
            </span>
          </div>
        </div>
      </div>
    );
  };
