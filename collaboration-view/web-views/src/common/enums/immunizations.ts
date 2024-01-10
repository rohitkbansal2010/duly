export enum ImmunizationsType {
  RECOMMENDED = 'RECOMMENDED',
  PAST = 'PAST',
}

export enum ImmunizationStatus {
  ADDRESSED = 'Addressed',
  COMPLETED = 'Completed',
  NOT_DUE = 'NotDue',
  DUE_SOON = 'DueSoon',
  DUE_ON = 'DueOn',
  OVERDUE = 'Overdue',
  POSTPONED = 'Postponed',
}

export enum ImmunizationsToggles {
  GROUP = 'GROUP',
  PLATE = 'PLATE',
}

export enum ProgressPanelStatuses {
  COMPLETED = 'Completed',
  ALMOST = 'Almost',
  LAGGING_BEHIND = 'LaggingBehind',
}
