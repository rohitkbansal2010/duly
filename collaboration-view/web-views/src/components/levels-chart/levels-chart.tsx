import annotationPlugin from 'chartjs-plugin-annotation';
import zoomPlugin from 'chartjs-plugin-zoom';
import { useEffect, useState } from 'react';
import { Chart, Line } from 'react-chartjs-2';
import 'chartjs-adapter-date-fns';
import { useSelector } from 'react-redux';

import { FONT_SMALL, FONT_LARGE, FONT_MEDIUM } from '@components/font-size-setting/helper';
import { DAY_STEP_SIZE, VITALS_CHART_HEIGHT_BMI, YEAR_STEP_SIZE } from '@constants';
import { CardType, DulyChartsColors } from '@enums';
import { ChartsMockData } from '@interfaces';
import { RootState } from '@redux/reducers';
import {
  calculateYears,
  getAdditionalLabels,
  getChartDataWithAdditionalLabels,
  getDateKeysFromMultipleValuesDaysForTooltip,
  SetChartOptions,
  setDatasetsWithParamsFromChartData,
  setGradientFill,
  setTimeUnit,
  setXAxisLabelsFromChartData
} from '@utils';

import './tooltip.css';

Chart.register(annotationPlugin, { zoomPlugin });

export type LevelsChartType = {
  chartData: ChartsMockData;
  chartId: string;
  currentCardType: CardType | null;
  height: number;
}

export const LevelsChart = ({
  chartData,
  chartId,
  currentCardType,
  height,
}: LevelsChartType) => {
  const [ gradientViolet, setGradientViolet ] = useState('');
  const [ gradientMagenta, setGradientMagenta ] = useState('');
  const [ sizeHeight, setSizeHeight ] = useState(height);
  const [ bmiHeight, setBmiHeight ] = useState(VITALS_CHART_HEIGHT_BMI);

  const remCoefficient = useSelector(({ UI }: RootState) =>
    UI.remCoefficient);

  const startDate = chartData?.datasets?.at(0)?.data.values.at(0).x;
  const endDate = chartData?.datasets?.at(0)?.data.values.at(-1).x;

  const yearDuration = calculateYears(endDate, startDate);
  const timeUnitStepSize = yearDuration >= 2 ? YEAR_STEP_SIZE : DAY_STEP_SIZE;

  const additionalLabels = getAdditionalLabels(startDate, endDate);
  const chartDataWithAdditionalLabels = getChartDataWithAdditionalLabels(
    chartData,
    additionalLabels
  );
  const activeKey = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE?.fontSize);

  
  const height_ratio = ()=>{
    if(activeKey === FONT_SMALL){
      setSizeHeight(height);
    }else if(activeKey === FONT_MEDIUM){
      setSizeHeight(height / 1.25);
    }else if(activeKey === FONT_LARGE){
      setSizeHeight(height / 1.5);
    }
  }; 
  const bmi_height_ratio = ()=>{
    if(activeKey === FONT_SMALL){
      setBmiHeight(VITALS_CHART_HEIGHT_BMI);
    }else if(activeKey === FONT_MEDIUM){
      setBmiHeight(VITALS_CHART_HEIGHT_BMI / 1.25);
    }else if(activeKey === FONT_LARGE){
      setBmiHeight(VITALS_CHART_HEIGHT_BMI / 1.5);
    }
  };

  const multiplier = remCoefficient / 16 ;
  const currentHeight = currentCardType?.includes(CardType.BODY_MASS_INDEX)
    ? bmiHeight
    : sizeHeight;

  useEffect(() => {
    const gradientFillViolet = setGradientFill(
      chartId,
      DulyChartsColors.VIOLET_40,
      DulyChartsColors.VIOLET_0,
      multiplier
    );
    setGradientViolet(gradientFillViolet);

    const gradientFillMagenta = setGradientFill(
      chartId,
      DulyChartsColors.MAGENTA_80,
      DulyChartsColors.MAGENTA_0,
      multiplier
    );
    setGradientMagenta(gradientFillMagenta);
  }, [ multiplier ]);
  
  useEffect(()=>{
    height_ratio();
    if(currentCardType?.includes(CardType.BODY_MASS_INDEX)){
      bmi_height_ratio();}
  }, [ activeKey ]);


  return (
    <>
      <div
        className="my-4"
        style={{
          height: `${currentHeight * multiplier}px !important`,  
          flexGrow:1,    
        }}
        
      >
        <Line
          id={chartId}
          className="canvas"
          data={{
            labels:
              setXAxisLabelsFromChartData(chartDataWithAdditionalLabels),
            datasets:
              setDatasetsWithParamsFromChartData({
                multiplier,
                data: chartDataWithAdditionalLabels,
                borderColor: [ DulyChartsColors.VIOLET_100, DulyChartsColors.MAGENTA_100 ],
                backgroundColorGradient: [ gradientViolet, gradientMagenta ],
                measurement: chartData.datasets[0].data.dimension,
                repeats: {
                  0: getDateKeysFromMultipleValuesDaysForTooltip(chartData),
                  1: getDateKeysFromMultipleValuesDaysForTooltip(chartData, 1),
                },
              }),
          }}
          options={SetChartOptions(
            {
              multiplier,
              data: chartDataWithAdditionalLabels,
              timeUnitStepSize,
              timeUnit: setTimeUnit(startDate, endDate),
              startDate,
              endDate,
            }
          )}
        />
      </div>
    </>
  );
};
