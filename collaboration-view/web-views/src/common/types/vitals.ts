import { CardType, MeasurementType, VitalType } from '@enums';

export type Measurement = {
  measurementType: MeasurementType;
  value: number;
  measured: string;
  unit?: string;
  maxScaleValue?: number;
  convertedValue?: number | string;
  convertedUnit?: string;
}


export type Vital = {
  vitalType: VitalType,
  measurements: Measurement[];
}

export type TodayVitalCard = {
  cardType: CardType;
  vitals: Vital[]
}

type DateTimeDoubleChartAxis = {
  min: number | string;
  max: number | string;
  stepSize: number | string;
}

type DateTimeDoubleChartScales = {
  yAxis: DateTimeDoubleChartAxis;
  xAxis: DateTimeDoubleChartAxis;
}

type DateTimeDoubleChartOptions = {
  chartScales: DateTimeDoubleChartScales;
}

type DateTimeDoubleChartDataValue = {
  x: string;
  y: number;
}

type DateTimeDoubleChartData = {
  values: DateTimeDoubleChartDataValue[]
  dimension: string | null;
}

type DoubleExpectedRange = {
  min: number;
  max: number;
}

export type DateTimeDoubleChartDataset = {
  label: string | null;
  data: DateTimeDoubleChartData;
  range: DoubleExpectedRange;
  visible: boolean
}

export type DateTimeDoubleChart = {
  datasets: DateTimeDoubleChartDataset[]
  chartOptions: DateTimeDoubleChartOptions
}

export type DateTimeDoubleChartResponse = {
  chart: DateTimeDoubleChart
}
