import { useSelector } from 'react-redux';

import { AppointmentModuleWidgetsAlias } from '@enums';
import { RootState } from '@redux/reducers';

export const useWidgetSkeleton = (currentWidgetAlias: AppointmentModuleWidgetsAlias) => {
  const isSkeletonShown = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT?.widgetsList?.find(({ alias }) =>
      alias === currentWidgetAlias)?.isSkeletonShown);

  return !!isSkeletonShown;
};
