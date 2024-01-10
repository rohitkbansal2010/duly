import { ReactNode } from 'react';

import { ColorSchemeCharts } from '@enums';

import styles from './donut-pie-chart.scss';
import { LinearGradients } from './linear-gradients';

type PieChartType = {
  currentValue: number;
  maxValue: number;
  width?: number;
  colorScheme: ColorSchemeCharts;
  children: ReactNode;
  className: string;
  strokeWidth?: number;
  isBackwardsFromRight?: boolean;
};

const RADIUS_INNER_CIRCLE = 30;
const RADIUS_OUTER_CIRCLE = 36;
const DIAMETER_INNER_CIRCLE = RADIUS_INNER_CIRCLE * 2;
const DIAMETER_OUTER_CIRCLE = RADIUS_OUTER_CIRCLE * 2;

export const DonutPieChart = ({
  currentValue,
  maxValue,
  width,
  colorScheme,
  children,
  className = '',
  strokeWidth = 12,
  isBackwardsFromRight = false,
}: PieChartType) => {
  const dashesCount = (((currentValue * 100) / maxValue) * (Math.PI * DIAMETER_INNER_CIRCLE)) / 100;

  const transform = isBackwardsFromRight ? '' : `rotate(-90) translate(-${DIAMETER_OUTER_CIRCLE})`;
  const styleTransform = isBackwardsFromRight ? 'scaleY(-1) translateY(-100%)' : '';
  const strokeLinecap = currentValue ? 'round' : undefined;

  return (
    <div className={`${className} ${styles.pieChart}`} style={{ width: `${width}rem` }}>
      <svg width="100%" height="100%" viewBox="0 0 72 72">
        <circle
          r={RADIUS_INNER_CIRCLE}
          cx={RADIUS_OUTER_CIRCLE}
          cy={RADIUS_OUTER_CIRCLE}
          fill="#fff"
          stroke="#f3f6fa"
          strokeWidth={strokeWidth}
        />
        <circle
          r={RADIUS_INNER_CIRCLE}
          cx={RADIUS_OUTER_CIRCLE}
          cy={RADIUS_OUTER_CIRCLE}
          fill="transparent"
          stroke={`url('#${colorScheme}')`}
          strokeWidth={strokeWidth}
          strokeDasharray={`${dashesCount} ${Math.PI * DIAMETER_INNER_CIRCLE}`}
          strokeLinecap={strokeLinecap}
          transform={transform}
          style={{ transform: styleTransform }}
        />
        <LinearGradients />
      </svg>
      <div className={styles.pieChartContent}>{children}</div>
    </div>
  );
};
