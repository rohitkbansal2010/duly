import { MedicationsData } from '@types';
import { formatMDYYYYDate, showFirstPrefixItem } from '@utils';

import styles from './medications-item.scss';

export const MedicationsItem = ({
  title = '',
  reason = '',
  startDate,
  instructions = '',
  provider,
}: MedicationsData) => {
  const prescriber = provider && `${showFirstPrefixItem(provider.prefixes)}${provider.familyName}`.trim();

  return (
    <div className={styles.medicationsMainItem}>
      <div className={styles.medicationsMainItemMedicine}>
        <span className={styles.medicationsMainItemMedicineTitle}>{title}</span>
        <span className={styles.medicationsMainItemMedicineReason}>{reason}</span>
        <span className={styles.medicationsMainItemMedicineAdditional}>
          {startDate && `Started: ${formatMDYYYYDate(startDate)} `}
          {prescriber && `Prescriber: ${prescriber}`}
        </span>
      </div>
      <div className={styles.medicationsMainItemInstructions}>
        {instructions}
      </div>
    </div>
  );};
