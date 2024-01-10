import { useEffect, useMemo } from 'react';
import Form from 'react-bootstrap/esm/Form';
import { useDispatch, useSelector } from 'react-redux';

import { BmiGradientBlock } from '@components/bmi-gradient-block';
import { LevelsChart } from '@components/levels-chart';
import { VitalPanel } from '@components/vital-panel';
import { VITALS_CHART_HEIGHT } from '@constants';
import { CardType } from '@enums';
import { setChartDataMetric, setChartDataNonMetric } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { Vital } from '@types';
import { getTitleByCardType } from '@utils';

import { VitalsChartLegend } from './vitals-chart-legend';
import { VitalsChartNoData } from './vitals-chart-no-data';
import { VitalsChartSkeleton } from './vitals-chart-skeleton';
import styles from './vitals-chart.scss';


type VitalsChartPropsType = {
  toggle: string;
  setToggle: React.Dispatch<React.SetStateAction<string>>;
}
export const VitalsChart = ({ toggle, setToggle }: VitalsChartPropsType) => {
  const dispatch: AppDispatch = useDispatch();
  const todaysVitals = useSelector(
    ({ OVERVIEW_WIDGETS }: RootState) =>
      OVERVIEW_WIDGETS.todaysVitals
  );
  const currentCardType = useSelector(
    ({ OVERVIEW_WIDGETS }: RootState) =>
      OVERVIEW_WIDGETS.currentCardType
  );
  const chart = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.chart);

  const patientData = useSelector(
    ({ CURRENT_APPOINTMENT }: RootState) =>
      CURRENT_APPOINTMENT.patientData
  );
  const isSkeletonChartShown = useSelector(
    ({ CURRENT_APPOINTMENT }: RootState) =>
      CURRENT_APPOINTMENT.isSkeletonChartShown
  );

  const humanName = patientData?.generalInfo?.humanName;
  const currentTodayVital = useMemo(
    () =>
      todaysVitals?.find(({ cardType }) =>
        cardType === currentCardType),
    [ todaysVitals, currentCardType ]
  );
  useEffect(() => {
    if (currentCardType === 'WeightAndHeight') {
      if (toggle === 'on' && chart?.datasets[0].data.dimension === 'lbs') {
        dispatch(setChartDataMetric(chart));
      }
      else if (toggle === 'off' && chart?.datasets[0].data.dimension === 'kg') {
        dispatch(setChartDataNonMetric(chart));
      }
    }
  }, [ chart, currentCardType, toggle ]);

  if (isSkeletonChartShown) {
    return <VitalsChartSkeleton />;
  }
  const handleToggleChange = () => {
    if (toggle === 'on') {
      setToggle('off');
      sessionStorage.setItem('metric', 'off');
    }
    else {
      setToggle('on');
      sessionStorage.setItem('metric', 'on');
    }
  };


  return (
    currentCardType && (
      <div className={styles.vitalsWidgetContainerChart}>
        <h4 className={styles.vitalsWidgetContainerChartTitle}>
          <span className={styles.vitalsWidgetContainerChartTitleGivenName}>
            {humanName?.givenNames?.length && `${humanName.givenNames[0]}’s `}
          </span>
          {getTitleByCardType(currentTodayVital?.vitals as Vital[], currentCardType, true)}
        </h4>
        {currentCardType === 'BodyMassIndex' || currentCardType === 'WeightAndHeight' ? (
          <div className={styles.vitalsWidgetContainerChartWrapper}>
            <Form.Check
              type="switch"
              id="custom-switch"
              className={styles.vitalsWidgetContainerChartToggleSwitch}
              defaultChecked={toggle === 'off' ? false : true}
              onClick={() =>
                handleToggleChange()}
            />
            <p className={styles.vitalsWidgetContainerChartToggleSwitchText}>METRIC</p>
          </div>
        ) : (
          ''
        )}
        {Boolean(currentTodayVital?.vitals.length) &&
          chart &&
          chart.datasets[0]?.data.values.length ? (
            <>
              {currentTodayVital && <VitalPanel toggle={toggle} {...currentTodayVital} />}

              {currentTodayVital && currentTodayVital.cardType.includes(CardType.BODY_MASS_INDEX) 
              && (
              <BmiGradientBlock />
            )}

              <LevelsChart
                chartData={chart}
                chartId={currentCardType}
                currentCardType={currentCardType}
                height={VITALS_CHART_HEIGHT}
              />

              <VitalsChartLegend currentCardType={currentCardType} />
            </>
        ) : (
          <VitalsChartNoData />
        )}
      </div>
    )
  );
};
