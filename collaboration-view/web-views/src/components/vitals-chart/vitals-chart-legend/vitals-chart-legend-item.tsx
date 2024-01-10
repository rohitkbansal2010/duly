import styles from './vitals-chart-legend.scss';

type VitalsChartLegendItemTypeProps = {
  title: string;
  color: string;
}

export const VitalsChartLegendItem = ({
  title,
  color,
}: VitalsChartLegendItemTypeProps) =>
  (
    <li className={`${styles.vitalsChartLegendItem} d-flex align-items-center`}>
      <span className={styles[`vitalsChartLegendItemRect${color}`]} />
      {title}
    </li>
  );
