import { useSelector } from 'react-redux';

import { RootState } from '@redux/reducers';

import styles from './bmi-gradient-block.scss';

export const BmiGradientBlock = () => {
  const { datasets, chartOptions } = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.chart) || { };

  const minValue = chartOptions?.chartScales.yAxis.min;
  const maxValue = chartOptions?.chartScales.yAxis.max;
  const latestValue = datasets?.[0].data.values[0].y;

  const getDotValue = (minValue: number, maxValue: number, latestValue: number) => {
    switch(true) {
      case (latestValue > maxValue):
        return maxValue;
      case (latestValue < minValue):
        return minValue;
      default:
        return latestValue;
    }
  };

  const calculateDotPosition = (minValue: number, maxValue: number, latestValue: number) => {
    const dotValue = getDotValue(minValue, maxValue, latestValue);
    const scale = (maxValue) - minValue;
    const indent = (dotValue - minValue) * 100 / scale;

    return `${indent}% - ${(indent) / 100}rem`;
  };

  return (
    <div className={styles.bmiBlock}>
      <div className={styles.bmiBlockGradient}>
        <div className={styles.bmiBlockGradientWrapper}>
          <div
            className={styles.bmiBlockGradientDot}
            style={{ left: `calc(${calculateDotPosition(minValue, maxValue, latestValue)})` }}
          >
            <div className={styles.bmiBlockGradientDotValue}>{latestValue}</div>
          </div>
        </div>
      </div>
      <div className={styles.bmiBlockText}>
        <span>Low BMI</span> <span>High BMI</span>
      </div>
    </div>
  );
};

