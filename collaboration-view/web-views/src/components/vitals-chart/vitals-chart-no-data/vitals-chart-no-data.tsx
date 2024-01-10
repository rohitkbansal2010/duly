import { useSelector } from 'react-redux';

import { CardType } from '@enums';
import { RootState } from '@redux/reducers';

import { noDataIconsMap } from './no-data-icons-map';
import styles from './vitals-chart-no-data.scss';

export const VitalsChartNoData = () =>{
  const currentCardType = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.currentCardType || CardType.BLOOD_PRESSURE) ;

  return (
    <div className={styles.vitalsChartNoDataWrapper}>
      <div className={styles.vitalsChartNoDataIconWrapper}>
        {currentCardType && (
          <img
            src={noDataIconsMap[currentCardType]}
            alt="vital icon"
          />
        )}
      </div>
      <span>No Data Available</span>
    </div>
  );};
