import { CardType } from '@enums';
import {
  conditionLightBlueIcon,
  conditionIcon,
  bloodPressureLightBlueIcon,
  bloodPressureIcon,
  heartRateLightBlueIcon,
  heartRateIcon,
  temperatureLightBlueIcon,
  temperatureIcon,
  respiratoryRateLightBlueIcon,
  respiratoryRateIcon,
  painLevelLightBlueIcon,
  painLevelIcon,
  weightLightBlueIcon,
  weightIcon,
  bmiLightBlueIcon,
  bmiIcon
} from '@icons';

type VitalsCardsMapType = {
  [key in CardType]: {
    title: string;
    icon: string;
    activeIcon: string;
  };
};

export const vitalsCardsMap: VitalsCardsMapType = {
  [CardType.BLOOD_PRESSURE]: {
    title: 'BLOOD PRESSURE',
    icon: conditionLightBlueIcon,
    activeIcon: conditionIcon,
  },
  [CardType.BLOOD_OXYGEN]: {
    title: 'BLOOD OXYGEN',
    icon: bloodPressureLightBlueIcon,
    activeIcon: bloodPressureIcon,
  },
  [CardType.HEART_RATE]: {
    title: 'HEART RATE',
    icon: heartRateLightBlueIcon,
    activeIcon: heartRateIcon,
  },
  [CardType.TEMPERATURE]: {
    title: 'TEMPERATURE',
    icon: temperatureLightBlueIcon,
    activeIcon: temperatureIcon,
  },
  [CardType.RESPIRATORY_RATE]: {
    title: 'RESPIRATORY RATE',
    icon: respiratoryRateLightBlueIcon,
    activeIcon: respiratoryRateIcon,
  },
  [CardType.PAIN_LEVEL]: {
    title: 'PAIN LEVEL',
    icon: painLevelLightBlueIcon,
    activeIcon: painLevelIcon,
  },
  [CardType.WEIGHT_HEIGHT]: {
    title: 'WEIGHT & HEIGHT',
    icon: weightLightBlueIcon,
    activeIcon: weightIcon,
  },
  [CardType.BODY_MASS_INDEX]: {
    title: 'BMI',
    icon: bmiLightBlueIcon,
    activeIcon: bmiIcon,
  },
};
