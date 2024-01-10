import { vitalsCardsMap } from '@components/vital-card';
import { vitalsChartsMap } from '@constants';
import { CardType, VitalType } from '@enums';
import { Vital } from '@types';

export const getTitleByCardType = (
  vitals: Vital[],
  cardType: CardType,
  isVitalChart?: boolean
): string => {
  const title = !isVitalChart ? vitalsCardsMap[cardType].title : vitalsChartsMap[cardType].title;
  const separator = !isVitalChart ? ' & ' : ' and ';

  if (cardType.includes(CardType.WEIGHT_HEIGHT) && vitals?.length) {
    const { vitalType } = vitals[0];
    const [ weightTitle, heightTitle ] = title.split(separator);

    if (
      !vitals[1] &&
      (vitalType.includes(VitalType.WEIGHT) || vitalType.includes(VitalType.HEIGHT))
    ) {
      return [ weightTitle, heightTitle ].join(separator);
    }

    if (vitals[1] && vitalType.includes(VitalType.HEIGHT)) {
      return [ heightTitle, weightTitle ].join(separator);
    }
  }

  return title;
};
export const convertWeightToLBS = (value : number): number => 
  +(value! / 0.45359237).toFixed(1);
export const convertWeightToKg = (value : number): number =>
  +(value * 0.45359237).toFixed(1);
export const convertHeightToFeet = (
  value : number
): string => {
  const realFeet = ((value! * 0.393700) / 12);
  const feet = Math.floor(realFeet);
  const inches = Math.round((realFeet - feet) * 12);
  return (`${feet}'${inches}''`);
};
