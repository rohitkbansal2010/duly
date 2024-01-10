import { appointmentModules } from '@constants';
import { AppointmentModuleType } from '@types';

export const getAppointmentModule = (appointmentModuleRoute: string): React.ReactNode => {
  const currentModule = appointmentModules.find(({ route }) => 
    route === `/${appointmentModuleRoute}`) as AppointmentModuleType;
  const Component = currentModule?.module;

  return <>{Component && <Component/>}</>;
};

export const getLocationByModule = (moduleRoute: string) => 
  `${window.location.pathname.split(moduleRoute)[0]}${moduleRoute}`;
