import { WidgetsPriority, AppointmentModuleWidgetsAlias } from '@enums';
import { WidgetDataType, WidgetItem } from '@types';

export const getWidgetsListWithPriority = (
  widgets: WidgetDataType[],
  currentWidgetAlias: AppointmentModuleWidgetsAlias
): WidgetItem[] => {
  const currentWidgetIndex = widgets.findIndex(({ alias }) =>
    alias === currentWidgetAlias);

  return widgets.map(({ alias }, index) => {
    const defaultWidget = { alias, isSkeletonShown: true };
    const indexDif = Math.abs(index - currentWidgetIndex);

    if (indexDif === 0) {
      return { ...defaultWidget, priority: WidgetsPriority.PRIMARY };
    }

    if (indexDif === 1) {
      return { ...defaultWidget, priority: WidgetsPriority.SECONDARY };
    }

    return { ...defaultWidget, priority: WidgetsPriority.REST };
  });
};
