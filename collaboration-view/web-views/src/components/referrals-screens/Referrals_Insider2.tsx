import { ReferralsIndsiderData } from '@mock-data';

import styles from './ReferralsInsider2.scss';

export const Referrals_Insider2 = () => 
  (
    <>
      <div className={styles.mainCompoBox}>
        <div className={styles.headerTextDiv}>
          <p className={styles.MainText}>Endocrinologist</p>
        </div>

        <div className={styles.mainParaDiv}>
          <p className={styles.mainParaText}> {ReferralsIndsiderData.customMsg} </p>
        </div>
      </div>
    </>
  );
