import { AllergiesWidget } from '@components/allergies-widget';
import { ConditionsWidget } from '@components/conditions-widget';
import { ImmunizationsWidget } from '@components/immunizations-widget';
import { MedicationsWidget } from '@components/medications-widget';
import { TimelineWidget } from '@components/timeline-widget';
import { VitalsWidget } from '@components/vitals-widget';
import { AppointmentModuleWidgetsRoutes } from '@enums';

// TODO: remove mock
const MockWidget = (): JSX.Element =>
  (
    <div style={{ width: '100%', height: '100%', backgroundColor: '#ddd' }} />
  );

export const WidgetsMap: Record<string, () => JSX.Element> = {
  [AppointmentModuleWidgetsRoutes.VITALS]: VitalsWidget,
  [AppointmentModuleWidgetsRoutes.GOALS]: MockWidget,
  [AppointmentModuleWidgetsRoutes.CONDITIONS]: ConditionsWidget,
  [AppointmentModuleWidgetsRoutes.TIMELINE]: TimelineWidget,
  [AppointmentModuleWidgetsRoutes.MEDICATIONS]: MedicationsWidget,
  [AppointmentModuleWidgetsRoutes.ALLERGIES]: AllergiesWidget,
  [AppointmentModuleWidgetsRoutes.IMMUNIZATIONS]: ImmunizationsWidget,
};
