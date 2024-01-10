import { WEEKDAYS_NAMES } from '@constants';
import { MOCK_TIME_SLOTS } from '@mock-data';
import { DateTimeSlotsType, GetTimeSlotsAPIResponseType, TimeSlotType } from '@types';

import { addZeroIfOneDigit } from './date';

export type WeekdayType = {
  nameWeekday: string;
  numberWeekday: number;
  slots: TimeSlotType[];
}

export const getTimeSlots = (startDate: string, endDate: string): WeekdayType[] => {
  const firstDate = new Date(startDate);
  const lastDate = new Date(endDate);
  const nextDate = new Date(firstDate);

  const result: WeekdayType[] = [];
  while (lastDate.getTime() >= nextDate.getTime()) {
    result.push({
      nameWeekday: WEEKDAYS_NAMES[nextDate.getDay()],
      numberWeekday: nextDate.getDate(),
      slots: MOCK_TIME_SLOTS[nextDate.getDay()],
    });
    nextDate.setDate(nextDate.getDate() + 1);
  }
  return result;
};

export const convertTo12HourFormat = (time:string):string => {
  const timeArray = time.split(':');
  const hours = Number(timeArray[0]);
  const minutes = Number(timeArray[1]);

  const ampm = hours >= 12 ? 'PM' : 'AM';
  const newHours = addZeroIfOneDigit(hours % 12);
  const newMinutes = addZeroIfOneDigit(minutes);

  if(newHours === '00') {
    return `12:${newMinutes} ${ampm}`;
  }
  return `${newHours}:${newMinutes} ${ampm}`;
};

export const convertTo24HourFormat = (time:string):string => {
  const timeArray = time.split(':');
  const hours = Number(timeArray[0]);
  const minutes = timeArray[1].split(' ')[0];
  const seconds = '00';
  if(time.includes('PM')) {
    if(hours == 12){
      return `${addZeroIfOneDigit(hours)}:${minutes}:${seconds}`;
    }
    return `${addZeroIfOneDigit((hours + 12) % 24)}:${minutes}:${seconds}`;
  }
  else{
    if(hours == 12){
      return `${addZeroIfOneDigit((hours + 12) % 24)}:${minutes}:${seconds}`;
    }
    return `${addZeroIfOneDigit(hours)}:${minutes}:${seconds}`;
  }
};

export const modifyTimeSlots = (timeSlots:DateTimeSlotsType[]):DateTimeSlotsType[] => 
  (
    timeSlots.map((timeSlot:DateTimeSlotsType) => 
      ({
        ...timeSlot,
        displayTime: convertTo12HourFormat(timeSlot.displayTime),
      }))
  );

export const setTimeSlots = (data: GetTimeSlotsAPIResponseType[])=>{
  if(data.length){
    return modifyTimeSlots(data[0].timeSlots);
  }else{
    return [];
  }  
};
