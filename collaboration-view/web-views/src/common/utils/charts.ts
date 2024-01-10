import { Chart, ChartType, ScriptableTooltipContext } from 'chart.js';
import { UnionToIntersection } from 'chart.js/types/utils';
import cloneDeep from 'lodash/cloneDeep';
import groupBy from 'lodash/groupBy';
import { useSelector } from 'react-redux';

import { ChartOptions, DatasetsWithParamsFromChartData } from '@components/levels-chart';
import { BOUNDS_FILL_COLORS, TOOLTIP_MEASUREMENTS } from '@constants';
import { CardType, RangeKeys } from '@enums';
import {
  BoundChart,
  BoundsFill,
  ChartsMockData,
  GetBoundsFill,
  GetRangesByKey,
  RangeValue,
  GetAnnotations,
  ChartPoint
} from '@interfaces';
import { RootState } from '@redux/reducers';
import { store } from '@redux/store';

import {
  calculateYears,
  dateStringToMDTooltip,
  dateStringToMDYTooltip,
  makeHHMMFromDateString,
  addZeroIfOneDigit,
  formatMMDDYYYYDate
} from './date';

export const groupDateByDays = days =>
  groupBy(days, function (date) {
    return new Date(date.x).toLocaleDateString();
  });

const getXAxisValues = (data: ChartPoint[]) =>
  data.map(({ x }) =>
    x);

const getYAxisValues = (data: ChartPoint[]) =>
  data.map(({ y }) =>
    y);

const getNonFilteredValues = (data: ChartsMockData, index = 0) =>
  data.datasets.map(({ data: { values } }) =>
    values.map(({ x, y }) =>
      ({
        x,
        y,
      })))[index];

export const sortDataByTime = (data: ChartPoint[]) =>
  data.sort((a, b) =>
    new Date(a.x) - new Date(b.x));

// function for filtering grouped by days values and showing only last value per day on chart
const filterAxisesValues = notFilteredValues =>
  Object.values(groupDateByDays(notFilteredValues)).map(val =>
    val.at(0));

export const getDaysWithMultipleValues = notFilteredValues =>
  Object.values(groupDateByDays(notFilteredValues)).map(val =>
    val.filter(el =>
      el.length > 1))
    .flat(1);

export const getDateKeysFromMultipleValuesDaysForTooltip = (data: ChartsMockData, index = 0) =>
  (getDaysWithMultipleValues(
    groupDateByDays(
      getNonFilteredValues(data, index)
    )
  ).reduce((acc, rec) =>
    ({
      ...acc,
      [`${Intl.DateTimeFormat().format(new Date(rec[0].x))}`]: rec,
    }), {}));

export const setXAxisLabelsFromChartData = (data: ChartsMockData) =>
  getXAxisValues(sortDataByTime(filterAxisesValues(getNonFilteredValues(data))));

export const setYAxisLabelsFromChartData = (data: ChartsMockData, index: number) =>
  getYAxisValues(sortDataByTime(filterAxisesValues(getNonFilteredValues(data, index))));

export const setGradientFill = (
  chartId: string,
  colorStopOneColor: string,
  colorStopTwoColor: string,
  multiplier: number
) => {
  const canvas = document.getElementById(chartId) as HTMLCanvasElement;
  const ctx = canvas!.getContext('2d');

  const gradientFill = ctx!.createLinearGradient(0, 1050 * multiplier, 0, 2.5 * multiplier);
  gradientFill.addColorStop(1, colorStopOneColor);
  gradientFill.addColorStop(0.6, colorStopTwoColor);
  return gradientFill;
};

export const setDatasetsWithParamsFromChartData = ({
  multiplier,
  data,
  borderColor,
  backgroundColorGradient,
  measurement,
  repeats,
}: DatasetsWithParamsFromChartData) =>
  data.datasets.map(({ label }, index) =>
    ({
      label,
      data: setYAxisLabelsFromChartData(data, index),
      fill: true,
      spanGaps: true,
      borderColor: borderColor[index],
      hitRadius: 12 * multiplier,
      borderWidth: 2 * multiplier,
      pointHoverBorderWidth: 4 * multiplier,
      pointBackgroundColor: borderColor[index],
      pointHoverBorderColor: '#fff',
      pointRadius: 3 * multiplier,
      pointHoverRadius: 8 * multiplier,
      pointStyle: 'circle',
      backgroundColor: backgroundColorGradient,
      tension: 0,
      measurement,
      repeats,
    }));

const createDOMElement = (
  elementType: string,
  className: string,
  innerHTML?: string
): HTMLElement => {
  const element = document.createElement(elementType);
  element.className = className;
  innerHTML && (element.innerHTML = innerHTML);
  return element;
};

const getOrCreateTooltip = (chart: UnionToIntersection<Chart>) => {
  let tooltipEl = chart.canvas.parentNode?.querySelector('div');

  if (!tooltipEl) {
    tooltipEl = createDOMElement('div', 'tooltip');

    const contentTooltip = createDOMElement('div', 'contentTooltip');

    tooltipEl.appendChild(contentTooltip);
    chart.canvas.parentNode.appendChild(tooltipEl);
  }

  return tooltipEl;
};

export const externalTooltipHandler = (context: ScriptableTooltipContext<ChartType>) => {
  const {
    chart,
    tooltip,
  } = context;
  const tooltipEl = getOrCreateTooltip(chart);

  if (tooltip.opacity === 0) {
    tooltipEl!.style.opacity = '0';
    return;
  }

  const {
    title,
    caretX,
    caretY,
    options: { padding },
    dataPoints,
  } = tooltip;

  const measurement = dataPoints[0].dataset.measurement;
  const reps = dataPoints[0].dataset.repeats;
  const formatTitleToMDY = Intl.DateTimeFormat().format(new Date(dateStringToMDYTooltip(title[0])));
  const firstDataset = [];
  const secondDataset = [];
  let resultTextArr = [];

  // eslint-disable-next-line no-prototype-builtins
  if (reps[0].hasOwnProperty(formatTitleToMDY)) {
    reps[0][formatTitleToMDY].map((val) => {
      firstDataset.push(`${makeHHMMFromDateString(val.x, true)} ${val.y}`);
    });
    if (reps[1] && Object.entries(reps[1]).length !== 0) {
      reps[1][formatTitleToMDY].map((val) => {
        secondDataset.push(`${val.y}`);
      });
    }
    resultTextArr = firstDataset.map((val, i) => {
      if (secondDataset.length) {
        return `${val}/${secondDataset[i]}`;
      }
      return `${val}`;
    });
  }

  const VITAL_VALUES_PER_DAY_LIMIT = 5;

  if (resultTextArr.length > VITAL_VALUES_PER_DAY_LIMIT) {
    resultTextArr = resultTextArr.slice(-VITAL_VALUES_PER_DAY_LIMIT);
  }

  const date = title[0];

  const $tooltipDate = createDOMElement('span', 'text-nowrap', `${dateStringToMDTooltip(date)} `);

  if (tooltip.body) {
    const $textWrapper = createDOMElement('div', 'd-flex flex-column');
    const span = createDOMElement('span', 'text-nowrap');

    let measure = ' ';

    if (measurement) {
      const measurementKey = measurement.replace(/[^\w\s]/gi, '').toLowerCase();
      const measurementObject = TOOLTIP_MEASUREMENTS.find(el =>
        el[measurementKey] !== undefined);
      measure = measurementObject ? measurementObject[measurementKey] : measurement;
    }

    let resultData;

    const createTextStructure = () =>
      resultData = resultTextArr.map((val) => {
        const span = createDOMElement('span', 'text-nowrap tooltipValue');
        const text = document.createTextNode(`${val} ${measure}`);
        span.appendChild(text);
        $textWrapper.appendChild(span);
      });

    const currentCardType = store.getState().OVERVIEW_WIDGETS.currentCardType;

    if (measure) {
      if (reps &&
        currentCardType !== CardType.WEIGHT_HEIGHT &&
        currentCardType !== CardType.BODY_MASS_INDEX) {
        createTextStructure();
      }
      resultData = `${dataPoints[0].formattedValue} ${measure}`;
    } else {
      if (reps) {
        createTextStructure();
      }
      resultData = dataPoints.reduce((acc, rec) =>
        `${acc.formattedValue}/${rec.formattedValue}`);
    }

    const text = document.createTextNode(`${resultData}`);

    span.appendChild(text);
    $textWrapper.appendChild(span);

    const contentTooltip = tooltipEl.querySelector('.contentTooltip') as HTMLElement;

    while (contentTooltip.firstChild) {
      contentTooltip.firstChild.remove();
    }

    contentTooltip.appendChild($tooltipDate);
    contentTooltip.appendChild($textWrapper);
  }

  const {
    offsetLeft: positionX,
    offsetTop: positionY,
  } = chart.canvas;

  tooltipEl.style.opacity = '1';
  tooltipEl.style.left = positionX + caretX + 'px';
  tooltipEl.style.top = positionY + caretY + 'px';
  tooltipEl.style.padding = padding + 'px ' + padding + 'px';
};

const displayXAxisFormats = (startDate: string, endDate: string) => {
  const startDateMonth = new Date(startDate).getMonth();
  const endDateMonth = new Date(endDate).getMonth();
  const startDateYear = new Date(startDate).getFullYear();
  const endDateYear = new Date(endDate).getFullYear();

  if (startDateMonth === endDateMonth && startDateYear === endDateYear) {
    return 'M/d/yyyy';
  }

  return 'MMM yyyy';
};

export const setTimeUnit = (startDate: string, endDate: string) => {
  const startDateMonth = new Date(startDate).getMonth();
  const endDateMonth = new Date(endDate).getMonth();
  const startDateYear = new Date(startDate).getFullYear();
  const endDateYear = new Date(endDate).getFullYear();
  const oneDay =
    new Date(startDate).toLocaleDateString() === new Date(endDate).toLocaleDateString();

  if (oneDay) {
    return 'second';
  }

  if (startDateMonth === endDateMonth && startDateYear === endDateYear) {
    return 'day';
  }

  return 'month';
};

export const getBoundChart = (multiplier: number, value: number): BoundChart =>
  ({
    type: 'line',
    borderColor: '#470A68',
    borderWidth: multiplier,
    borderDash: [ 5 * multiplier, 5 * multiplier ],
    scaleID: 'y',
    value,
  });

export const getBoundsFill = ({ min: yMin = 0, max: yMax = 0, key }: GetBoundsFill): BoundsFill =>
  ({
    type: 'box',
    backgroundColor: BOUNDS_FILL_COLORS[key],
    borderWidth: 0,
    yMin,
    yMax,
  });

export const getRangesByKey = ({ data, key }: GetRangesByKey): RangeValue[] =>
  data?.datasets?.map(({ ranges }) => {
    const currentRange = ranges?.find(range =>
      range.key === key)?.value || {} as RangeValue;

    return { ...currentRange, key };
  });

export const getAnnotations = ({
  multiplier, ranges, maxValue, minValue,
}: GetAnnotations):
  { [key: string]: BoundChart | BoundsFill } =>
  ranges?.flat().reduce(((acc, { min, max, key }, index) =>
    ({
      ...acc,
      [`boundsFill${index}`]: getBoundsFill({ min, max, key }),
      [`minBound${index}`]: minValue !== min && getBoundChart(multiplier, min),
      [`maxBound${index}`]: maxValue !== max && getBoundChart(multiplier, max),
    })), {});

export const generateLabel = (year: number, month: number, day = 1): ChartPoint =>
  ({ x: `${year}-${addZeroIfOneDigit(month)}-${addZeroIfOneDigit(day)}`, y: NaN });

export const getAdditionalLabels = (
  startDate: string,
  endDate: string
): ChartPoint[] => {
  const startDateYear = new Date(startDate).getFullYear();
  const endDateYear = new Date(endDate).getFullYear();
  const endDateMonth = new Date(endDate).getMonth() + 1;
  const startDateMonth = new Date(startDate).getMonth() + 1;
  const startDateDay = new Date(startDate).getDate();
  const endDateDay = new Date(endDate).getDate();
  const yearDuration = calculateYears(endDate, startDate);
  const yearDiff = startDateYear - endDateYear;
  const monthDiff = startDateMonth - endDateMonth;
  const dayDiff = startDateDay - endDateDay;

  if (monthDiff === 0 && yearDiff === 0) {
    return dayDiff >= 1
      ? [ ...new Array(dayDiff + 1) ].map((_, index) =>
        generateLabel(endDateYear, startDateMonth, endDateDay + index))
      : [];
  } 

  if (monthDiff !== 0 && yearDiff === 0) {
    return [ ...new Array(monthDiff + 1) ].map((_, index) => 
      generateLabel(endDateYear, endDateMonth + index));
  }

  if (yearDuration >= 2) {
    return [ ...new Array(yearDuration + 1) ].map((_, index) => 
      generateLabel(endDateYear + index, endDateMonth));
  }

  return [ ...new Array(yearDiff + 1) ]
    .flatMap((_, year) =>
      [ ...new Array(12) ].map((_, month) => {
        if ((year === 0 && month + 1 < endDateMonth) ||
              (year + endDateYear === startDateYear && month + 1 > startDateMonth)) {
          return { x: '', y: NaN };
        }
        return generateLabel(endDateYear + year, month + 1);
      }))
    .filter(({ x }) => 
      !!x);
};

export const getChartDataWithAdditionalLabels = (
  rawChartData: ChartsMockData,
  additionalLabels: ChartPoint[]
): ChartsMockData => {
  const chartData = cloneDeep(rawChartData);

  const datasetsWithAdditionalLabels = chartData?.datasets?.map((dataset) => {
    const { data: { values } } = dataset;

    const formattedValues = values.map(({ x, y }) =>
      ({ x: formatMMDDYYYYDate(x), y }));

    const valuesWithAdditionalLabels = formattedValues
      .concat(Object.keys(groupDateByDays(values)).length > 1 ? additionalLabels : []);
        
    return { ...dataset, data: { ...dataset.data, values: valuesWithAdditionalLabels } };
  });
    
  return { ...chartData, datasets: datasetsWithAdditionalLabels };
};

export const SetChartOptions = ({
  multiplier,
  data,
  zoomOnPinch = false,
  zoomOnWheel = false,
  zoomMode = 'x',
  panMode = 'x',
  enableXAxisOffset = true,
  yAxisPosition = 'left',
  xAxisType = 'time',
  enablePan = false,
  timeUnitStepSize = 1,
  timeUnit = 'day',
  startDate,
  endDate,
}: ChartOptions) => {
  const maxValue = data?.chartOptions?.chartScales?.yAxis?.max;
  const minValue = data?.chartOptions?.chartScales?.yAxis?.min;
  const selectedFontSize = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE?.fontSize);

  const ranges =
    Object.values(RangeKeys)
      .map(key =>
        getRangesByKey({ data, key }));

  const annotations = getAnnotations({
    multiplier, ranges, maxValue, minValue, 
  });

  const changeFontSize = ()=>{
    if(selectedFontSize === 'font-small'){
      return ((16 * multiplier) + 0);
    }else if(selectedFontSize === 'font-medium'){
      return ((16 * multiplier) + 4);
    }else if(selectedFontSize === 'font-large'){
      return ((16 * multiplier) + 8);
    }
  };

  const fontStyleX = {
    family: 'Inter',
    size: changeFontSize(),
    weight: '600',
  };
  const fontStyleY = {
    family: 'Inter',
    size: changeFontSize(),
    weight: '400',
  };

  const chartOptions = {
    interaction: {
      intersect: true,
      mode: 'index',
    },
    plugins: {
      legend: { display: false },
      tooltip: {
        enabled: false,
        position: 'nearest',
        external: externalTooltipHandler,
      },
      zoom: {
        zoom: {
          mode: zoomMode,
          pinch: { enabled: zoomOnPinch },
          wheel: { enabled: zoomOnWheel },
        },
        pan: {
          enabled: enablePan,
          mode: panMode,
        },
      },
      annotation: { annotations },
    },
    maintainAspectRatio: false,
    scales: {
      x: {
        display: true,
        type: xAxisType,
        time: {
          unit: timeUnit,
          stepSize: timeUnitStepSize,
          displayFormats: {
            second: displayXAxisFormats(startDate, endDate),
            day: displayXAxisFormats(startDate, endDate),
            month: displayXAxisFormats(startDate, endDate),
          },
        },
        ticks: {
          color: '#a8afb9',
          font: fontStyleX,
          source: 'data',
        },
        grid: {
          display: true,
          lineWidth: multiplier,
          tickWidth: 0,
        },
        offset: enableXAxisOffset,
      },
      y: {
        position: yAxisPosition,
        display: true,
        max: maxValue,
        min: minValue,
        ticks: {
          padding: 10 * multiplier,
          stepSize: data?.chartOptions?.chartScales?.yAxis?.stepSize,
          color: '#a8afb9',
          font: fontStyleY,
        },
        grid: {
          display: true,
          borderDash: [ 10 * multiplier, 10 * multiplier ],
          color: '#a8afb9',
          lineWidth: multiplier,
          tickWidth: 0,
          borderWidth: 0,
        },
        beginAtZero: true,
      },
    },
  };

  return chartOptions;
};
