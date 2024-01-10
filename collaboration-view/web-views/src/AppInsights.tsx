import { ReactPlugin } from '@microsoft/applicationinsights-react-js';
import { ApplicationInsights, ITelemetryItem } from '@microsoft/applicationinsights-web';
import { createBrowserHistory } from 'history';

const browserHistory = createBrowserHistory({ basename: '' });
const appInsightsPlugin = new ReactPlugin();
const appInsights = new ApplicationInsights({
  config: {
    instrumentationKey: window?.env?.APP_INSIGHTS_INSTRUMENTATION_KEY || '',
    extensions: [ appInsightsPlugin ],
    extensionConfig: { [appInsightsPlugin.identifier]: { history: browserHistory } },
  },
});

const filteringFunction = (envelope: ITelemetryItem) => {
  const telemetryItem = envelope.baseData || {};

  telemetryItem.properties = telemetryItem.properties || {};
  telemetryItem.properties['ApplicationName'] = 'Duly.CollaborationView.WebViews';
  telemetryItem.properties['ApplicationVersion'] = window?.env?.BUILD_NUMBER || '';
};

appInsights.loadAppInsights();
appInsights.addTelemetryInitializer(filteringFunction);

export { appInsightsPlugin, appInsights };
