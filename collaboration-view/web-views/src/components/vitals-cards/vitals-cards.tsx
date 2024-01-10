import { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { FadedScroll } from '@components/faded-scroll-2';
import { VitalCard } from '@components/vital-card';
import { CardType, AppointmentModuleWidgetsAlias } from '@enums';
import { useWidgetSkeleton } from '@hooks';
import { setCurrentCardType, getChartData } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { TodayVitalCard } from '@types';

import { VitalsCardsSkeleton } from './vitals-cards-skeleton';
import styles from './vitals-cards.scss';

export type VitalsCardsType = {
  toggle: string;
};

export const VitalsCards = ({ toggle }: VitalsCardsType) => {
  const dispatch: AppDispatch = useDispatch();
  const [ changeToggle, setChangeToggle ] = useState(0);

  const todaysVitals = useSelector(
    ({ OVERVIEW_WIDGETS }: RootState) => 
      OVERVIEW_WIDGETS.todaysVitals
  );
  const currentCardType = useSelector(
    ({ OVERVIEW_WIDGETS }: RootState) => 
      OVERVIEW_WIDGETS.currentCardType
  );
  const patientId = useSelector(
    ({ CURRENT_APPOINTMENT }: RootState) => 
      CURRENT_APPOINTMENT.patientId
  );

  const isSkeletonShown = useWidgetSkeleton(AppointmentModuleWidgetsAlias.VITALS);

  const onClickVitalCard = useCallback(
    (cardType: CardType, index: number) => 
      () => {
        dispatch(setCurrentCardType(cardType));

        if (todaysVitals![index].vitals.length) {
          dispatch(getChartData(patientId, cardType));
        }
      },
    [ dispatch, patientId, todaysVitals ]
  );

  const renderVitalCard = (todayVitalCard: TodayVitalCard, index: number) => 
    (
      <VitalCard
        key={todayVitalCard.cardType}
        isActiveCard={currentCardType === todayVitalCard.cardType}
        onClickVitalCard={onClickVitalCard(todayVitalCard.cardType, index)}
        {...todayVitalCard}
      />
    );

  useEffect(() => {
    setChangeToggle(changeToggle + 1);
  }, [ toggle ]);

  return (
    <div className={styles.todaysVitalsCardsContainer}>
      <FadedScroll height="100%">
        {isSkeletonShown ? (
          <VitalsCardsSkeleton />
        ) : changeToggle ? (
          <div className={styles.todaysVitalsCards}>{todaysVitals?.map(renderVitalCard)}</div>
        ) : null}
      </FadedScroll>
    </div>
  );
};
