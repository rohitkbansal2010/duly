import { Avatar } from '@components/avatar';
import { TestResultStatus } from '@enums';
import { Performer } from '@types';
import {
  getSrcAvatar,
  getTestReportDate,
  getUserRole,
  showFirstPrefixItem,
  getTestReportTime,
  getPerformerInfo
} from '@utils';

import styles from './results-details.scss';

type ResultsDetailsProps = {
  effectiveDate?: string;
  issued?: string;
  status?: TestResultStatus;
  performers?: Performer[];
}

export const ResultsDetails = ({
  effectiveDate = '', issued = '', status, performers = [], 
}: ResultsDetailsProps) => {
  const {
    familyName,
    givenNames,
    prefixes,
    photo,
    role,
  } = getPerformerInfo(performers);

  const indicatorClasses = []
    .concat(styles.resultsDetailsBodyStatusIndicator)
    .concat(styles[`resultsDetailsBodyStatusIndicator${status}`])
    .join(' ');
  
  return (
    <div className={styles.resultsDetails}>
      <div className={styles.resultsDetailsHeader}>
        <span className={styles.resultsDetailsHeaderOrdered}>ordered by</span>
        <span className={styles.resultsDetailsHeaderCollected}>collected on</span>
        <span className={styles.resultsDetailsHeaderResulted}>resulted on</span>
        <span className={styles.resultsDetailsHeaderStatus}>status</span>
      </div>
      <div className={styles.resultsDetailsBody}>
        <div className={styles.resultsDetailsBodyOrdered}>
          {performers[0] && (
            <Avatar
              width={2.125}
              src={getSrcAvatar(photo)}
              alt={`${showFirstPrefixItem(givenNames)}${familyName}`.trim()}
              role={getUserRole({ role, prefixes })}
            />
          )}
          <span className={styles.resultsDetailsBodyOrderedDoctor}>
            {prefixes && prefixes.join(' ')}
            {familyName}
          </span>
        </div>
        <span className={styles.resultsDetailsBodyCollected}>
          {effectiveDate &&
            `${getTestReportDate(effectiveDate)} - ${getTestReportTime(effectiveDate)}`}
        </span>
        <span className={styles.resultsDetailsBodyResulted}>
          {issued && `${getTestReportDate(issued)} - ${getTestReportTime(issued)}`}
        </span>
        <span className={styles.resultsDetailsBodyStatus}>
          {status}
          <div className={indicatorClasses} />
        </span>
      </div>
    </div>
  );
};
