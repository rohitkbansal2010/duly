import { ChartsMockData } from '@interfaces';

export type ChartOptions = {
  multiplier: number;
  data: ChartsMockData;
  zoomOnPinch?: boolean;
  zoomOnWheel?: boolean;
  zoomMode?: string;
  enablePan?: boolean;
  panMode?: string;
  xAxisType?: string;
  timeUnit?: string;
  timeUnitStepSize?: number;
  enableXAxisOffset?: boolean;
  yAxisPosition?: string;
  startDate: string;
  endDate: string;
}

export type DatasetsWithParamsFromChartData = {
  multiplier: number;
  data: ChartsMockData;
  borderColor: string[] | string;
  backgroundColorGradient: string[] | string;
  measurement: string;
  repeats: {
    [key in number]: {
      [key in string]: Array<{ x: string, y: number }>
    }
  };
}
