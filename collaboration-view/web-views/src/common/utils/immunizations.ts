import { DEFAULT_IMMUNIZATIONS_VALUE } from '@constants';
import { DoseType } from '@types';

export const formatDose = (dose?: DoseType): string => {
  if (!dose) {
    return DEFAULT_IMMUNIZATIONS_VALUE;
  }

  const { amount, unit } = dose;

  return `${amount} ${unit ? unit : ''}`.trim();
};
