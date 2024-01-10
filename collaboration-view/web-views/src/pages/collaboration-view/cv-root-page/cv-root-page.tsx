import { Route, Switch } from 'react-router-dom';

import { AuthRoute } from '@components/auth-route';
import { ExceptionsModal } from '@components/exceptions-modal';
import { CollaborationViewRoutes } from '@enums';
import { StartCheckout } from '@pages/collaboration-view/start-checkout';
import { PageNotFound } from '@pages/page-not-found';
import { cvRoutes } from '@routes';

import { WelcomePage } from '../welcome-page';

export const CvRootPage = () => 
  (
    <>
      <Switch>
        {cvRoutes.map(({ path, ...props }) => 
(
  <AuthRoute key={path} path={path} exact {...props} />
      ))}
        <Route
          exact
          path={`${CollaborationViewRoutes.welcome}/:appointmentId/:patientId/:practitionerId`}
          component={WelcomePage}
        />
        <Route exact path={`${CollaborationViewRoutes.startCheckout}`} component={StartCheckout} />
        {/* TODO comment: refactor within DPGECLOF-713 */}
        <Route component={PageNotFound} />
      </Switch>
      <ExceptionsModal />
    </>
  );
