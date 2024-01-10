export type Stepper = {
  text: string;
  id: any;
  value: string;
  completed: boolean;
  data?: any;
  defaultIcon: string;
  currentIcon: string;
  completeIcon: string;
  partialIcon?: string;
  skipIcon: string;
};
