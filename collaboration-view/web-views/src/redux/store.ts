import { routerMiddleware } from 'connected-react-router';
import { createBrowserHistory } from 'history';
import { applyMiddleware, createStore } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import createSagaMiddleware from 'redux-saga';

import { rootSaga } from '@sagas';
import { getCurrentLocale, setCurrentLocale } from '@utils';

import { Namespaces } from './namespaces';
import { rootReducer } from './reducers';

export const history = createBrowserHistory();
const sagaMiddleware = createSagaMiddleware();
const middlewares = [ sagaMiddleware, routerMiddleware(history) ];
const middlewareEnhancer = applyMiddleware(...middlewares);

const persistedState = { [Namespaces.LOCALE]: { locale: getCurrentLocale() || 'en-US' } };

export const store = createStore(
  rootReducer,
  persistedState,
  composeWithDevTools(middlewareEnhancer)
);

store.subscribe(() => {
  setCurrentLocale(store.getState().LOCALE.locale);
});

sagaMiddleware.run(rootSaga);

export type AppDispatch = typeof store.dispatch;
