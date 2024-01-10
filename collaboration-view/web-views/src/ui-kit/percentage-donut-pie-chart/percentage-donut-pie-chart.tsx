import styles from './percentage-donut-pie-chart.scss';

type PieChartType = {
  percent: number;
};

const RADIUS_INNER_CIRCLE = 23;
const RADIUS_OUTER_CIRCLE = 30;
const RADIUS_VIEWBOX = 36;
const INNER_STROKE_WIDTH = 5;
const OUTER_STROKE_WIDTH = INNER_STROKE_WIDTH * 2;
const DIAMETER_OUTER_CIRCLE = RADIUS_OUTER_CIRCLE * 2;
const DIAMETER_VIEWBOX = RADIUS_VIEWBOX * 2;
const MAX_PERCENT = 100;

export const PercentageDonutPieChart = ({ percent }: PieChartType) => {
  const dashCount = ((percent * Math.PI * DIAMETER_OUTER_CIRCLE) / MAX_PERCENT);
  const transform = `rotate(-90) translate(-${DIAMETER_VIEWBOX})`;
  
  return (
    <div className={styles.pieChart}>
      <svg width="100%" height="100%" viewBox={`0 0 ${DIAMETER_VIEWBOX} ${DIAMETER_VIEWBOX}`}>
        <circle
          r={RADIUS_OUTER_CIRCLE}
          cx={RADIUS_VIEWBOX}
          cy={RADIUS_VIEWBOX}
          fill="transparent"
          stroke="#79065a"
          strokeWidth={INNER_STROKE_WIDTH}
        />
        <circle
          r={RADIUS_OUTER_CIRCLE}
          cx={RADIUS_VIEWBOX}
          cy={RADIUS_VIEWBOX}
          fill="transparent"
          stroke="#c5299b"
          strokeWidth={OUTER_STROKE_WIDTH}
          strokeDasharray={`${dashCount} ${Math.PI * DIAMETER_OUTER_CIRCLE}`}
          transform={transform}
        />
        <circle
          r={RADIUS_INNER_CIRCLE}
          cx={RADIUS_VIEWBOX}
          cy={RADIUS_VIEWBOX}
          fill="#79065a"
        />
      </svg>
      <div className={styles.pieChartContentContainer}>
        <span className={styles.pieChartContent}>
          <span className={styles.pieChartContentPercent}>{percent}</span>
        </span>
      </div>
    </div>
  );
};
