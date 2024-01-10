import { LEGEND } from '@constants';
import { CardType } from '@enums';

import { VitalsChartLegendItem } from './vitals-chart-legend-item';
import styles from './vitals-chart-legend.scss';
 
type VitalsChartLegendProps = {
  currentCardType: CardType;
}

export const VitalsChartLegend = ({ currentCardType }: VitalsChartLegendProps) =>
  (
    <ul className={styles.vitalsChartLegend}>
      {LEGEND[currentCardType].map(legend =>
        (
          <VitalsChartLegendItem
            key={legend.title}
            {...legend}
          />
        ))}
    </ul>
  );
