import {
  ACCURACY,
  DEFAULT_FONT_SIZE,
  SH1_RATIO,
  SH1_WIDTH 
} from '@constants';

export const getRatio = (width: number, height: number): number => 
  Number((width / height).toFixed(ACCURACY));

export const getWithinSh1Size = (width: number, ratio: number): boolean => 
  width <= SH1_WIDTH && ratio >= SH1_RATIO;

export const getRemCoefficient = (width: number): number => 
  Number((width * DEFAULT_FONT_SIZE / SH1_WIDTH).toFixed(ACCURACY));
