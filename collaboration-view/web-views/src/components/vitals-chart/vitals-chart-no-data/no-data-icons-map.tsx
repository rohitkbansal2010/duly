import { CardType } from '@enums';
import {
  bloodPressureMagentaIcon,
  bmiMagentaIcon,
  conditionMagentaIcon,
  heartRateMagentaIcon,
  painLevelMagentaIcon,
  respiratoryRateMagentaIcon,
  temperatureMagentaIcon,
  weightMagentaIcon 
} from '@icons';

type NoDataIconsMapType = {
  [key in CardType]: string;
};

export const noDataIconsMap: NoDataIconsMapType = {
  [CardType.BLOOD_PRESSURE]: conditionMagentaIcon,
  [CardType.BLOOD_OXYGEN]: bloodPressureMagentaIcon,
  [CardType.HEART_RATE]: heartRateMagentaIcon,
  [CardType.TEMPERATURE]: temperatureMagentaIcon,
  [CardType.RESPIRATORY_RATE]: respiratoryRateMagentaIcon,
  [CardType.PAIN_LEVEL]: painLevelMagentaIcon,
  [CardType.WEIGHT_HEIGHT]: weightMagentaIcon,
  [CardType.BODY_MASS_INDEX]: bmiMagentaIcon,
};
