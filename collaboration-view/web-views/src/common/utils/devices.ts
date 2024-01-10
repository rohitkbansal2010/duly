const MOBILE_WIDTH = 440;
const PORTRAIT_WIDTH = 1024;

export const isMobile = (width: number): boolean =>
  width <= MOBILE_WIDTH;

export const isPortrait = (width: number): boolean =>
  width <= PORTRAIT_WIDTH;
