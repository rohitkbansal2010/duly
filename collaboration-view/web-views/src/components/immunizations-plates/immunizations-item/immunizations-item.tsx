import { DEFAULT_IMMUNIZATIONS_VALUE } from '@constants';
import { ImmunizationStatus } from '@enums';
import { speechBubbleOrangeIcon } from '@icons';
import { DoseType } from '@types';
import { formatDose, formatMDYYYYDate } from '@utils';

import styles from './immunizations-item.scss';

type ImmunizationsItemProps = {
  title?: string;
  dateTitle: string;
  status?: ImmunizationStatus;
  date?: string;
  notes?: string;
  dose?: DoseType;
}

const immunizationStatusesMap: Record<ImmunizationStatus, string> = {
  [ImmunizationStatus.ADDRESSED]: 'Addressed',
  [ImmunizationStatus.COMPLETED]: 'Completed',
  [ImmunizationStatus.NOT_DUE]: 'Not Due',
  [ImmunizationStatus.DUE_SOON]: 'Due Soon',
  [ImmunizationStatus.DUE_ON]: 'Due On',
  [ImmunizationStatus.OVERDUE]: 'Overdue',
  [ImmunizationStatus.POSTPONED]: 'Postponed',
};

export const ImmunizationsItem = ({
  title, dateTitle, status, date, notes, dose,
}: ImmunizationsItemProps) => {
  const speechBubbleStatuses = [ ImmunizationStatus.OVERDUE ];
  const itemDueClass = styles[`immunizationsItemDue${title ? '' : 'Recommended'}`];

  return (
    <div className={styles.immunizationsItem}>
      {title && (
        <div className={styles.immunizationsItemVaccine}>
          <span className={styles.immunizationsItemTitle}>vaccine</span>
          <span className={styles.immunizationsItemValue}>
            {title}
          </span>
        </div>
      )}
      <div className={itemDueClass}>
        <span className={styles.immunizationsItemTitle}>
          {dateTitle}
        </span>
        <span className={styles.immunizationsItemValue}>
          {date ? formatMDYYYYDate(date) : DEFAULT_IMMUNIZATIONS_VALUE}
        </span>
      </div>
      <div className={styles.immunizationsItemStatus}>
        <span className={styles.immunizationsItemTitle}>
          {status ? 'status' : 'dose'}
          {speechBubbleStatuses.includes(status as ImmunizationStatus) && (
            <img
              src={speechBubbleOrangeIcon}
              alt="Speech bubble orange"
            />
          )}
        </span>
        <span className={styles.immunizationsItemValue}>
          {status ? immunizationStatusesMap[status] : formatDose(dose)}
        </span>
      </div>
      {notes && (
        <div className={styles.immunizationsItemNotes}>
          <span className={styles.immunizationsItemTitle}>notes</span>
          <span className={styles.immunizationsItemNotesValue}>{notes}</span>
        </div>
      )}
    </div>
  );
};
