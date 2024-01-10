import { connectRouter } from 'connected-react-router';
import { createBrowserHistory } from 'history';
import { combineReducers } from 'redux';

import { Namespaces } from '../namespaces';

import { appStateReducer } from './app-state';
import { currentAppointmentReducer } from './current-appointment';
import { cvCheckoutAppointmentsReducer } from './cv-checkout-appointments';
import { localeReducer } from './locale';
import { overviewWidgetsReducer } from './overview-widgets';
import { siteReducer } from './site';
import { todaysAppointmentsReducer } from './todays-appointments';
import { uiReducer } from './ui';
import { userAuthenticationReducer } from './user';

export const rootReducer = combineReducers({
  [Namespaces.ROUTER]: connectRouter(createBrowserHistory()),
  [Namespaces.LOCALE]: localeReducer,
  [Namespaces.USER]: userAuthenticationReducer,
  [Namespaces.UI]: uiReducer,
  [Namespaces.APPSTATE]: appStateReducer,
  [Namespaces.TODAYS_APPOINTMENTS]: todaysAppointmentsReducer,
  [Namespaces.CURRENT_APPOINTMENT]: currentAppointmentReducer,
  [Namespaces.OVERVIEW_WIDGETS]: overviewWidgetsReducer,
  [Namespaces.SITE]: siteReducer,
  [Namespaces.CHECKOUT_APPOINTMENTS]: cvCheckoutAppointmentsReducer,
});

export type RootState = ReturnType<typeof rootReducer>;
