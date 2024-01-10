import { ErrorDataType, RefreshSlotsType } from '@redux/actions';

import { ErrorData } from '../constants/errors-mock-data';


export const errorDataForSchedulingFail = (data: RefreshSlotsType): ErrorDataType =>
  ({
    ...ErrorData.UnableToProcessAppointment,
    refreshSlots: {
      date: data.date,
      providerId: data.providerId,
      meetingType: data.meetingType,
      appointmentId: data.appointmentId,
      stepType: data.stepType,
      departmentId: data.departmentId,
    },
  });

export const getRefVisitType = (meetingType: string): string => {
  if(meetingType === 'In-Person') {
    return '8001';
  }
  return '2990';
};  
