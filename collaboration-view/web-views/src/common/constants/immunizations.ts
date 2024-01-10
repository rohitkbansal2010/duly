import { ProgressPanelStatuses, ImmunizationsType } from '@enums';
import {
  immunizationLightBlueIcon,
  immunizationMagentaIcon,
  statusAlmostLightBlueIcon,
  statusLaggingOrangeIcon,
  statusUpToDateGreenIcon
} from '@icons';
import { ProgressPanel, Immunizations } from '@types';

export const PROGRESS_PANEL: ProgressPanel = {
  [ProgressPanelStatuses.COMPLETED]: {
    title: 'You’re all up to date!',
    additional: 'You have no recommended immunizations at the moment!',
    icon: statusUpToDateGreenIcon,
  },
  [ProgressPanelStatuses.ALMOST]: {
    title: 'You’re almost there!',
    additional: 'Few more and you’re all set!',
    icon: statusAlmostLightBlueIcon,
  },
  [ProgressPanelStatuses.LAGGING_BEHIND]: {
    title: 'You’re lagging behind!',
    additional: 'You have some catching up to do on your immunizations...',
    icon: statusLaggingOrangeIcon,
  },
};

export const IMMUNIZATIONS: Immunizations = {
  [ImmunizationsType.RECOMMENDED]: {
    title: 'Recommended Immunizations',
    additional: 'Immunizations recommended based on your age, prior vaccinations, health, lifestyle and other indications',
    icon: immunizationLightBlueIcon,
    noAvailableText: 'no recommended immunizations',
  },
  [ImmunizationsType.PAST]: {
    title: 'Past Immunizations',
    additional: 'All past recorded immunizations',
    icon: immunizationMagentaIcon,
    noAvailableText: 'no past immunizations',
  },
};

export const IMMUNIZATIONS_EVENT_KEY = '0';
export const DEFAULT_IMMUNIZATIONS_VALUE = 'N/A';
export const GROUP_ID_RECOMMENDED_IMMUNIZATION = 'recommendedImmunization';
export const GROUP_ID_PAST_IMMUNIZATION = 'pastImmunization';
