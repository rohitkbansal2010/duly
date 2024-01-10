import { CardType, VitalType } from '@enums';
import { TodayVitalCard } from '@types';
import { getTitleByCardType } from '@utils';

import { Measurements } from './measurements';
import styles from './vital-card.scss';
import { vitalsCardsMap } from './vitals-cards-map';

export type VitalCardPropType = TodayVitalCard & {
  isActiveCard: boolean;
  onClickVitalCard: () => void;
}

export const VitalCard = ({
  cardType,
  vitals,
  isActiveCard,
  onClickVitalCard,
}: VitalCardPropType) => {
  const activeClass = isActiveCard ? 'Active' : '';

  const { icon, activeIcon } = vitalsCardsMap[cardType];

  const isWeighHeightCard = cardType.includes(CardType.WEIGHT_HEIGHT);
  const isBMICard = cardType.includes(CardType.BODY_MASS_INDEX);

  const isVitalHeightNoData =
    isWeighHeightCard &&
    !vitals[1] &&
    vitals[0]?.vitalType.includes(VitalType.WEIGHT);

  const isVitalWeightNoData =
    isWeighHeightCard &&
    !vitals[1] &&
    vitals[0]?.vitalType.includes(VitalType.HEIGHT);

  return (
    <button
      onClick={onClickVitalCard}
      data-testid="vital-card__container"
      className={styles[`todayVitalCard${activeClass}`]}
    >
      <div className={styles[`todayVitalCard${activeClass}Icon`]}>
        <img
          data-testid="vital-card__icon"
          src={isActiveCard ? activeIcon : icon}
          alt="vital icon"
          className={styles[`todayVitalCard${activeClass}Icon${cardType}`]}
        />
      </div>
      <div data-testid="vital-card__title" className={styles[`todayVitalCard${activeClass}Title`]}>
        {getTitleByCardType(vitals, cardType)}
      </div>
      {!vitals.length && (
        <span
          data-testid="vital-card__no-data"
          className={styles[`todayVitalCard${activeClass}MeasurementValueEmpty`]}
        >
          N/A
        </span>
      )}
      {isVitalWeightNoData && (
        <span
          data-testid="vital-card__weight__no-data"
          className={styles[`todayVitalCard${activeClass}MeasurementValueEmpty`]}
        >
          N/A&nbsp;/&nbsp;
        </span>
      )}
      {vitals
        .slice(0, !isBMICard ? vitals.length : 1)
        .map(({ vitalType, measurements }, index) =>
          (
            <Measurements
              key={vitalType}
              index={index}
              measurements={measurements}
              activeClass={activeClass}
              isWeighHeightCard={isWeighHeightCard}
              isVitalHeight={!isVitalHeightNoData}
              isVitalWeight={!isVitalWeightNoData}
              
              
            />
          ))}
      {isVitalHeightNoData && (
        <span
          data-testid="vital-card__height__no-data"
          className={styles[`todayVitalCard${activeClass}MeasurementValueEmpty`]}
        >
          &nbsp;/&nbsp;N/A
        </span>
      )}
    </button>
  );
};
