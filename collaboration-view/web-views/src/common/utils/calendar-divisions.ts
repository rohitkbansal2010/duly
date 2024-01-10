export const formatHourToStringPmAm = (hour: number, minutes: string | number = '00'): string => {
  let hourString = '';

  const mins = `${minutes}`.length > 1 ? minutes : `0${minutes}`;

  if (hour < 12) {
    hourString = `${hour}:${mins} am`;
  }
  if (hour === 12) {
    hourString = `${hour}:${mins} pm`;
  }
  if (hour > 12) {
    hourString = `${hour - 12}:${mins} pm`;
  }
  if (hour === 24) {
    hourString = `${hour - 12}:${mins} am`;
  }

  return hourString;
};

export const calculateCalendarDivisions = (startHour: number, endHour: number): string[] => {
  if (startHour === endHour) return [];

  const calendarDivisionsArray: string[] = [];
  for (let i = startHour; i <= endHour; i++) {
    calendarDivisionsArray.push(formatHourToStringPmAm(i));
  }

  return calendarDivisionsArray;
};
