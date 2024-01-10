import { RangeKeys, CardType } from '@enums';

export interface RangeValue {
  inclusiveLowerBound: boolean;
  inclusiveUpperBound: boolean;
  min: number;
  max: number;
  key: RangeKeys;
}

export interface Range {
  key: RangeKeys;
  value: RangeValue;
}

export type ChartPoint = {
  x: string;
  y: number;
}

export interface ChartsMockData {
  datasets:
    {
      label: string,
      data: {
        values: ChartPoint[],
        dimension: string
      },
      range?: {
        min: number,
        max: number
      },
      ranges: Range[];
      visible?: boolean,
    }[],
  chartOptions: {
    chartScales: {
      yAxis: {
        max: number,
        min: number,
        stepSize: number
      }
    }
  }
}

export interface BoundChart {
  type: string;
  borderColor: string;
  borderWidth: number;
  borderDash?: number[];
  scaleID: string;
  value: number;
}

export interface BoundsFill {
  type: string;
  backgroundColor: string;
  borderWidth: number;
  yMin: number;
  yMax: number;
}

export interface GetRangesByKey {
  data: ChartsMockData;
  key: RangeKeys;
}

export interface GetBoundsFill {
  min: number;
  max: number;
  key: RangeKeys;
}

export type LegendItem = {
  title: string;
  color: string;
}

export type Legend = {
  [key in CardType]: LegendItem[];
}

export type BoundsFillColors = {
  [key in RangeKeys]: string;
}

export type GetAnnotations = {
  multiplier: number;
  ranges: Array<RangeValue[]>;
  maxValue: number;
  minValue: number;
}
