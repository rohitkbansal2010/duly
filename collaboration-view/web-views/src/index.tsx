import { MsalProvider } from '@azure/msal-react';
import { AppInsightsContext } from '@microsoft/applicationinsights-react-js';
import { ConnectedRouter } from 'connected-react-router';
import { render } from 'react-dom';
import { Provider } from 'react-redux';

import { history, store } from '@redux/store';
import { msalClient } from '@utils';

import { App } from './App';
import { appInsightsPlugin } from './AppInsights';

import './common/fonts/Nexa/Nexa-Heavy.otf';
import './common/fonts/Nexa/Nexa-Bold.otf';
import './common/fonts/Nexa/Nexa-ExtraBold.otf';
import './common/fonts/Nexa/Nexa-Regular.otf';

import './common/styles/main.scss';
import './common/styles/themes.scss';

import 'bootstrap/dist/css/bootstrap.css';
//eslint-disable-next-line import/no-unresolved
import 'swiper/css';

const root = document.getElementById('root');

render(
  <Provider store={store}>
    <MsalProvider instance={msalClient}>
      <ConnectedRouter history={history}>
        <AppInsightsContext.Provider value={appInsightsPlugin}>
          <App />
        </AppInsightsContext.Provider>
      </ConnectedRouter>
    </MsalProvider>
  </Provider>,
  root
);
