import capitalize from 'lodash/capitalize';
import sortBy from 'lodash/sortBy';
import { useMemo } from 'react';

import {
  environmentAllergieIcon,
  foodAllergieIcon,
  biologicAllergieIcon,
  medicalAllergieIcon,
  severeLevelWarning,
  noAllergiesIcon as commonAllergyIcon, otherAllergiesIcon
} from '@icons';
import { AllergieCategoryType, AllergiesData } from '@types';
import { formatMDYYYYDate } from '@utils';

import styles from './allergie-plate.module.scss';
import { BottomPartItem } from './bottom-part-item';

const IconsMap: {
  [K in AllergieCategoryType]: string;
} = {
  Food: foodAllergieIcon,
  Medication: medicalAllergieIcon,
  Biologic: biologicAllergieIcon,
  Environment: environmentAllergieIcon,
  Other: otherAllergiesIcon,
};

const getIcon = (categories: AllergieCategoryType[] = []): string => {
  if (categories.length > 1 || categories.length === 0) {
    return commonAllergyIcon;
  }
  return IconsMap[categories[0]];
};

type SeverityItemLabelProps = { isSevereAllergie: boolean };

const SeverityItemLabel = ({ isSevereAllergie }: SeverityItemLabelProps) =>
  (
    <>
      Severity
      {isSevereAllergie && (
      <img
        className={styles.allergiePlateBottomPartIconWarning}
        src={severeLevelWarning}
        alt="severe level warning"
      />
      )}
    </>
  );

export const AllergiePlate = ({
  title = '',
  recorded = '',
  categories = [],
  reactions = [],
}: AllergiesData) => {
  const iconSrc = getIcon(categories);

  const sortedCategories = useMemo(() =>
    categories.sort(), [ categories ]);

  const isSevereAllergy = useMemo(
    () =>
      reactions.some(({ severity }) =>
        severity === 'Severe'),
    [ reactions ]
  );

  const sortedReactions = useMemo(() =>
    sortBy(reactions, [ 'title' ]), [ reactions ]);

  return (
    <div
      className={styles.allergiePlateContainer}
      data-testid="allergie-plate-item"
    >
      <div className={styles.allergiePlateTopPart}>
        <div className={styles.allergiePlateTopPartIcon}>
          <img className={styles.allergiePlateTopPartIconImg} src={iconSrc} alt="allergie icon" />
          {isSevereAllergy && (
            <img
              className={styles.allergiePlateTopPartIconWarning}
              src={severeLevelWarning}
              alt="severe level warning"
            />
          )}
        </div>
        <div className={styles.allergiePlateTopPartTitle}>{capitalize(title)}</div>
      </div>
      <div className={styles.allergiePlateBottomPart}>
        <div className={styles.allergiePlateBottomPartContainer}>
          <BottomPartItem title={<span data-testid="allergie-plate-date-recorded-title">Date Recorded</span>}>
            <span data-testid="allergie-plate-date-recorded-value">{formatMDYYYYDate(recorded)}</span>
          </BottomPartItem>
          <BottomPartItem title={<span data-testid="allergie-plate-category-title">Category</span>}>
            {sortedCategories.map(category =>
            (
              <span data-testid="allergie-plate-category-item" data-testval={capitalize(category)} key={category}>{capitalize(category)}</span>
            ))}
          </BottomPartItem>
          <div className={styles.allergiePlateBottomPartReactionContainer}>
            <BottomPartItem
              title={(
                <div className={styles.allergiePlateBottomPartReactionContainerItem}>
                  <span data-testid="allergie-plate-severity-title">
                    <SeverityItemLabel isSevereAllergie={isSevereAllergy} />
                  </span>
                  <span data-testid="allergie-plate-reaction-title">Reaction</span>
                </div>
              )}
            >
              {sortedReactions.map(({ title, severity }) =>
              (
                <div
                  key={title + severity}
                  className={styles.allergiePlateBottomPartReactionContainerItem}
                >
                  <span data-testid="allergie-plate-severity-item" data-testval={capitalize(severity)}>{capitalize(severity)}</span>
                  <span data-testid="allergie-plate-reaction-item" data-testval={capitalize(title)}>{capitalize(title)}</span>
                </div>
              ))}
            </BottomPartItem>
          </div>
        </div>
      </div>
    </div>
  );
};
