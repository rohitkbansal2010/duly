import {
  CareAllyRoutes, CollaborationViewRoutes, NonAuthRoutes, PatientRoutes
} from '@enums';

export const checkIfHasNonAuthURL = (url: string): boolean =>
  Object.values(NonAuthRoutes).includes(url as NonAuthRoutes);

export const isPatientLoginPath = (url: string): boolean =>
  url === NonAuthRoutes.login;

export const routeWithParam = (route: string, variableValue: string, variableName: string) =>
  route.replace(`:${variableValue}`, variableName);

export const getRedirect = (url: string): string => {
  switch (url) {
    case NonAuthRoutes.login:
      return PatientRoutes.home;
    case NonAuthRoutes.care_ally_login:
      return CareAllyRoutes.home;
    case NonAuthRoutes.cv_login:
      return CollaborationViewRoutes.home;
    default:
      return NonAuthRoutes.login;
  }
};

export const encodeURLParams = (...params: string[]): string[] =>
  params.map(param =>
    encodeURIComponent(param));

export const decodeURLParams = (...params: string[]): string[] =>
  params.map(param =>
    decodeURIComponent(param));
