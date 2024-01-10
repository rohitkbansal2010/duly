import { MedicationsData, MedicationsWidgetDataType } from '../types/medications';

export const getRegularPrescriptions = 
  (prescriptionData: MedicationsWidgetDataType | null): MedicationsData[] =>
    prescriptionData && prescriptionData.regular ? prescriptionData?.regular : [];


export const getOtherPrescriptions = 
  (prescriptionData: MedicationsWidgetDataType | null): MedicationsData[] =>
    prescriptionData && prescriptionData.other ? prescriptionData?.other : [];
