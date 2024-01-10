import { all, put, takeLatest } from 'redux-saga/effects';

import { appointmentModulesWidgets } from '@constants';
import { WidgetsPriority } from '@enums';
import { CurrentAppointmentType, GetWidgetsDataAction } from '@redux/actions';
import { GetDataType, WidgetItem } from '@types';
import { catchExceptions, CatchExceptionsParamsType } from '@utils';

function* workerCurrentAppointmentWidgets({ payload }: GetWidgetsDataAction) {
  try {
    const { widgetsList, navigation, ...rest } = payload;

    const getWidgetsByPriority = (priorityAlias: WidgetsPriority): WidgetItem[] =>
      widgetsList.filter(({ priority }) =>
        priority === priorityAlias);

    const getWidgetsSagas = (widgets: WidgetItem[]) =>
      widgets
        .map(({ alias }) =>
          appointmentModulesWidgets[navigation]
            .find(widget =>
              widget.alias === alias)?.getData as GetDataType)
        .filter(func =>
          !!func)
        .map(getWidgetData =>
          put(getWidgetData(rest)));

    const primaryWidget =
      getWidgetsByPriority(WidgetsPriority.PRIMARY);
    const secondaryWidgets =
      getWidgetsByPriority(WidgetsPriority.SECONDARY);
    const restWidgets =
      getWidgetsByPriority(WidgetsPriority.REST);

    yield all(getWidgetsSagas(primaryWidget));
    yield all(getWidgetsSagas(secondaryWidgets));
    yield all(getWidgetsSagas(restWidgets));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  }
}

export function* currentAppointmentWidgetsSaga() {
  yield takeLatest(
    CurrentAppointmentType.GET_WIDGETS_DATA,
    workerCurrentAppointmentWidgets
  );
}
