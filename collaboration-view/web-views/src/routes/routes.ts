import { FC } from 'react';
import { RouteComponentProps } from 'react-router-dom';

import { CollaborationViewRoutes } from '@enums';
import { CvAppointmentPage } from '@pages/collaboration-view/cv-appointment-page';
import { CheckoutPage } from '@pages/collaboration-view/cv-checkout-page';
import { CvHomePage } from '@pages/collaboration-view/cv-home-page';

export type AuthRoute = {
  path: string;
  Component: FC<RouteComponentProps>;
};

export const cvRoutes: AuthRoute[] = [
  {
    path: CollaborationViewRoutes.home,
    Component: CvHomePage,
  },
  {
    path: `${CollaborationViewRoutes.appointment}/:appointmentId/:patientId/:practitionerId/:appointmentModuleRoute/:appointmentWidgetRoute?`,
    Component: CvAppointmentPage,
  },
  {
    path: `${CollaborationViewRoutes.examRoom1}/:appointmentId/:patientId/:practitionerId/:appointmentModuleRoute`,
    Component: CheckoutPage,
  },
];
