import {
  ProgressPanelStatuses,
  ImmunizationsType,
  ImmunizationStatus
} from '@enums';

export type ProgressPanel = {
  [key in ProgressPanelStatuses]: {
    title: string;
    additional: string;
    icon: string;
  }
};

export type Immunizations = {
  [key in ImmunizationsType]: {
    title: string;
    additional: string;
    icon: string;
    noAvailableText: string;
  }
};

export type ImmunizationsProgress = {
  percentageCompletion: number;
  recommendedGroupNumber: number;
  completionStatus: ProgressPanelStatuses;
}

export type DoseType = {
  amount: number;
  unit?: string;
}

export type VaccinationType = {
  title: string;
  dateTitle: string;
  status?: ImmunizationStatus;
  date?: string;
  notes?: string;
  dose?: DoseType;
}

export type ImmunizationsGroupType = {
  title: string;
  vaccinations: VaccinationType[];
}

export type ImmunizationsWidgetDataType = {
  progress: ImmunizationsProgress;
  recommendedImmunizations: ImmunizationsGroupType[];
  pastImmunizations: ImmunizationsGroupType[];
}
