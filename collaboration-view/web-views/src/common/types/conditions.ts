export type HealthCondition = {
  title: string;
  date?: string;
}

export type HealthConditionsWidgetDataType = {
  previousHealthConditions: HealthCondition[]
  currentHealthConditions: HealthCondition[]
}
