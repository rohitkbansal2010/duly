export type MedicationsData = {
  id: string;
  scheduleType: string;
  title?: string;
  reason?: string;
  startDate?: string;
  provider?: {
    familyName: string;
    givenNames?: string[];
    prefixes?: string[];
    suffixes?: string[];
  },
  instructions?: string;
}

export type MedicationsWidgetDataType = {
  [key: string]: MedicationsData[];
};
