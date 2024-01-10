import { getUTCOffset } from '@utils';

import styles from './build-number.scss';

export const BuildNumber = () => {
  const buildNumber = process.env.BUILD_NUMBER
    ? process.env.BUILD_NUMBER
    : process.env.GIT_VERSION;
  const showBuildNumber = (window?.env?.SHOW_BUILD_NUMBER || '').toLowerCase() === 'true';

  if (!showBuildNumber) return null;

  return (
    <div className={styles.buildNumber}>
      {`Build number: ${buildNumber} (UTC${getUTCOffset()})`}
    </div>
  );
};
