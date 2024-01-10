import sortBy from 'lodash/sortBy';
import { useMemo } from 'react';
import { useSelector } from 'react-redux';

import { DulyLoader } from '@components/duly-loader';
import { HEALTH_CONDITIONS_LOADER_WIDTH } from '@constants';
import { AppointmentModuleWidgetsAlias } from '@enums';
import { useWidgetSkeleton } from '@hooks';
import { RootState } from '@redux/reducers';

import { ConditionsWidgetCol, ConditionType } from './conditions-widget-col';
import styles from './conditions-widget.module.scss';


const conditionSorter = (conditions: ConditionType[]) =>
  sortBy(
    conditions,
    (condition: ConditionType) =>
      (condition.date ? -new Date(condition.date) : 1),
    [ 'title' ]
  );

export const ConditionsWidget = () => {
  const conditions = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.conditions);

  const isSkeletonShown = useWidgetSkeleton(AppointmentModuleWidgetsAlias.CONDITIONS);

  const prevConditions = useMemo(() =>
    conditionSorter(
      conditions?.previousHealthConditions || []
    ), [ conditions?.previousHealthConditions ]);

  const currentConditions = useMemo(() =>
    conditionSorter(
      conditions?.currentHealthConditions || []
    ), [ conditions?.currentHealthConditions ]);

  if (isSkeletonShown) {
    return (
      <div className={styles.conditionsWidgetSkeleton}>
        <DulyLoader width={HEALTH_CONDITIONS_LOADER_WIDTH} />
      </div>
    );
  }

  return (
    <div className={styles.conditionsWidget}>
      <ConditionsWidgetCol
        conditions={currentConditions}
        isCurrent
        title="Current Health Conditions"
      />
      <ConditionsWidgetCol
        conditions={prevConditions}
        isCurrent={false}
        title="Previous Health Conditions"
      />
    </div>
  );
};
