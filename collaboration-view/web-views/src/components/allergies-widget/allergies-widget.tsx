import sortBy from 'lodash/sortBy';
import { useMemo } from 'react';
import { useSelector } from 'react-redux';

import { FadedScroll } from '@components/faded-scroll-2';
import { AppointmentModuleWidgetsAlias } from '@enums';
import { useWidgetSkeleton } from '@hooks';
import { RootState } from '@redux/reducers';
import { AllergiesData } from '@types';
import { AllergiePlate } from '@ui-kit/allergie-plate';

import { AllergiesWidgetSkeleton } from './allergies-widget-skeleton';
import styles from './allergies.module.scss';
import { NoAllergiesWidget } from './no-allergies';

export const AllergiesWidget = () => {
  const allergies = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.allergies);

  const isSkeletonShown = useWidgetSkeleton(AppointmentModuleWidgetsAlias.ALLERGIES);

  const sortedAllergies = useMemo(() =>
    sortBy(allergies, allergy =>
      new Date(allergy.recorded)).reverse(), [ allergies ]);

  if (isSkeletonShown) {
    return <AllergiesWidgetSkeleton/>;
  }

  if (!allergies?.length) {
    return <NoAllergiesWidget/>;
  }

  const isOddLength = allergies && allergies?.length % 2 !== 0;

  return (
    <FadedScroll height="100%" className={styles.allergiesWidgetContainer}>
      <div className={styles.allergiesWidgetContent}>
        {sortedAllergies.map((allergy: AllergiesData) =>
            (
              <div key={allergy.id} className={styles.allergiesWidgetContentItem}>
                <AllergiePlate {...allergy} />
              </div>
            ))}
        {isOddLength && (
        <div className={`${styles.allergiesWidgetContentEmptyItem} ${styles.allergiesWidgetContentItem}`} />
          )}
      </div>
    </FadedScroll>
  );
};
