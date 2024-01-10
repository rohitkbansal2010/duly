import { useDispatch, useSelector } from 'react-redux';

import { FadedScroll } from '@components/faded-scroll-2';
import { RecommendedImmunizationsPlate, PastImmunizationsPlate } from '@components/immunizations-plates';
import { ImmunizationsProgressPanel } from '@components/immunizations-progress-panel';
import { AppointmentModuleWidgetsAlias } from '@enums';
import { useWidgetSkeleton } from '@hooks';
import { addAccordion, clearAccordions, deleteAccordion } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';

import { ImmunizationsWidgetSkeleton } from './immunizations-widget-skeleton';
import styles from './immunizations-widget.scss';
import { NoImmunizationsWidget } from './no-immunizations-widget';

export const ImmunizationsWidget = () => {
  const dispatch: AppDispatch = useDispatch();

  const immunizations = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.immunizations);
  const accordions = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.accordions);

  const isSkeletonShown = useWidgetSkeleton(AppointmentModuleWidgetsAlias.IMMUNIZATIONS);

  const recommendedImmunizations = immunizations?.recommendedImmunizations || [];
  const pastImmunizations = immunizations?.pastImmunizations || [];
  const progress = immunizations?.progress;

  const countOfRecommended = recommendedImmunizations.length;
  const countOfPast = pastImmunizations.length;
  const hasAnyImmunizations = immunizations && (countOfRecommended || countOfPast);

  const handleClickPastImmunizationGroup = (groupId: string) =>
    (isCurrentEventKey: boolean) => {
      if (isCurrentEventKey && accordions[groupId]) {
        dispatch(clearAccordions(groupId));
      }
    };

  const handleClickImmunizationGroupItem = (groupId: string, accordionId: string) =>
    () => {
      if (!accordions[groupId]?.includes(accordionId)) {
        dispatch(addAccordion(groupId, accordionId));
      } else {
        dispatch(deleteAccordion(groupId, accordionId));
      }
    };

  if (isSkeletonShown) {
    return <ImmunizationsWidgetSkeleton />;
  }

  if (!hasAnyImmunizations) {
    return <NoImmunizationsWidget/>;
  }

  return (
    <div className={styles.immunizationsWidgetContainer}>
      {progress && <ImmunizationsProgressPanel {...progress}/>}
      <div className={styles.immunizationsWidgetContent}>
        <FadedScroll height="100%">
          <RecommendedImmunizationsPlate
            immunizations={recommendedImmunizations}
            immunizationsCount={countOfRecommended}
            accordions={accordions}
            handleClickImmunizationGroupItem={handleClickImmunizationGroupItem}
          />
          <PastImmunizationsPlate
            immunizations={pastImmunizations}
            immunizationsCount={countOfPast}
            accordions={accordions}
            handleClickImmunizationGroupItem={handleClickImmunizationGroupItem}
            handleClickPastImmunizationGroup={handleClickPastImmunizationGroup}
          />
        </FadedScroll>
      </div>
    </div>
  );
};
