import {
  HALF_HEIGHT_TIME_SCALE_MARK,
  HEIGHT_ONE_MINUTE_SEGMENT,
  HEIGHT_QUARTER_SEGMENT,
  MINUTES_PER_HOUR,
  PADDING_BOTTOM_SCHEDULER_SCALE,
  PADDING_TOP_SCHEDULER_SCALE,
  SEGMENT_IN_MINUTES
} from '@constants';
import { AppointmentData, GroupedAppointmentType } from '@types';

import { formatHourToStringPmAm } from './calendar-divisions';
import { calcDurationInMinutes } from './date';

const workDayStart = window?.env?.WORK_DAY_START || '';
const workDayEnd = window?.env?.WORK_DAY_END || '';

export const sortAppointmentsInDescTime = (appointments: AppointmentData[]): AppointmentData[] =>
  [ ...appointments ]
    .sort((a, b) =>
      new Date(a.timeSlot.endTime).getTime() - new Date(b.timeSlot.endTime).getTime())
    .sort((a, b) =>
      new Date(b.timeSlot.startTime).getTime() - new Date(a.timeSlot.startTime).getTime());

export const sortAppointmentsInAscStartTime =
  (appointments: AppointmentData[] = []): AppointmentData[] =>
    [ ...appointments ].sort((a, b) =>
      new Date(a.timeSlot.startTime).getTime() - new Date(b.timeSlot.startTime).getTime());

export const groupAppointments = (appointments: AppointmentData[]): GroupedAppointmentType[] => {
  const groupedAppointments: GroupedAppointmentType[] = [];

  sortAppointmentsInAscStartTime(appointments).forEach((appointment) => {
    let crossingDetected = false;

    const appointmentStartTime = new Date(appointment.timeSlot.startTime).getTime();
    const appointmentEndTime = new Date(appointment.timeSlot.endTime).getTime();

    groupedAppointments.forEach(({ startTime, endTime, appointments }, index) => {
      if (startTime <= appointmentStartTime && appointmentStartTime < endTime) {
        crossingDetected = true;
        appointments.push(appointment);

        if (endTime < appointmentEndTime) {
          groupedAppointments[index].endTime = appointmentEndTime;
        }
      }
    });

    if (!crossingDetected) {
      groupedAppointments.push({
        startTime: appointmentStartTime,
        endTime: appointmentEndTime,
        appointments: [ appointment ],
      });
    }
  });

  return groupedAppointments;
};

export const cropAppointmentsByWorkDay = (
  appointments: AppointmentData[] | null
): AppointmentData[] =>
  appointments?.filter(({ timeSlot: { startTime, endTime } }) => {
    const startHours = new Date(startTime).getHours();
    const endHours = new Date(endTime).getHours();
    const endMinutes = new Date(endTime).getMinutes();

    if (+workDayStart <= startHours && endHours <= +workDayEnd) {
      if (endHours === +workDayEnd && endMinutes !== 0) return false;

      return true;
    }

    return false;
  }) || [];

export const calculateCurrentTimeBarOptions = () => {
  const date = new Date();
  const currentHour = date.getHours();
  const currentMinutes = date.getMinutes();
  const minutesFromCalendarStart = (currentHour - +workDayStart) * MINUTES_PER_HOUR +
    currentMinutes;
  const paddingTopScheduler = PADDING_TOP_SCHEDULER_SCALE + HALF_HEIGHT_TIME_SCALE_MARK;

  const time = formatHourToStringPmAm(currentHour, currentMinutes);
  const position = minutesFromCalendarStart * HEIGHT_ONE_MINUTE_SEGMENT + paddingTopScheduler;

  return { time, position };
};

export const calcTopOfGroupsAppointems = (date: Date) => {
  const timeInMinutes = date.getHours() * MINUTES_PER_HOUR + date.getMinutes();
  const calendarTimeStartInMinutes = +workDayStart * MINUTES_PER_HOUR;
  const countSegments = (timeInMinutes - calendarTimeStartInMinutes) / SEGMENT_IN_MINUTES;
  const heightContainedSegments = countSegments * HEIGHT_QUARTER_SEGMENT;

  return heightContainedSegments + HALF_HEIGHT_TIME_SCALE_MARK + PADDING_BOTTOM_SCHEDULER_SCALE;
};

export const calcTopOfAppointment = (startTimeAppointment: Date, startTimeGroups: Date): number => {
  const duration = calcDurationInMinutes(startTimeGroups, startTimeAppointment);

  return duration / SEGMENT_IN_MINUTES * HEIGHT_QUARTER_SEGMENT;
};

export const isTitleNotEmpty = (title: string | null | undefined): boolean => {
  if (!title || !title.replace(/\s/g, '') || title.replace(/\s/g, '').toLowerCase() === 'n/a') return false;
  return true;
};

export const firstLetterCapital = (text:string) => 
  text.toLowerCase().split(' ')
    .map(s =>
      s.charAt(0).toUpperCase() + s.substring(1))
    .join(' ');
