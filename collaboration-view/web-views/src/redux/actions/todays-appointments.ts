import { DailyStatistics, AppointmentData, Practitioner } from '@types';

export enum TodaysAppointmentType {
  GET_TODAYS_APPOINTMENTS_SAGA = 'GET_TODAYS_APPOINTMENTS_SAGA',
  SET_PRACTITIONERS = 'SET_PRACTITIONERS',
  SET_APPOINTMENTS = 'SET_APPOINTMENTS',
  SET_PICKED_APPOINTMENT_ID = 'SET_PICKED_APPOINTMENT_ID',
  SET_DAILY_STATISTICS = 'SET_DAILY_STATISTICS',
	SHOW_BTG_MODAL = 'SHOW_BTG_MODAL',
  HIDE_BTG_MODAL = 'HIDE_BTG_MODAL',
}

type GetTodaysAppointmentsSaga = {
  type: typeof TodaysAppointmentType.GET_TODAYS_APPOINTMENTS_SAGA;
};

type SetPractitionersAction = {
  type: typeof TodaysAppointmentType.SET_PRACTITIONERS;
  payload: { practitioners: Practitioner[] };
};

type SetAppointmentsAction = {
  type: typeof TodaysAppointmentType.SET_APPOINTMENTS;
  payload: { appointments: AppointmentData[] };
};

type SetPickedAppointmentIdAction = {
  type: typeof TodaysAppointmentType.SET_PICKED_APPOINTMENT_ID;
  payload: { pickedAppointmentId: string };
};

type SetDailyStatisticsAction = {
  type: typeof TodaysAppointmentType.SET_DAILY_STATISTICS;
  payload: {
    dailyStatistics: DailyStatistics;
  };
};

type ShowBtgModal = {
  type: typeof TodaysAppointmentType.SHOW_BTG_MODAL;
};

type HideBtgModal = {
  type: typeof TodaysAppointmentType.HIDE_BTG_MODAL;
};

export type TodaysAppointmentAction =
  GetTodaysAppointmentsSaga
  | SetPractitionersAction
  | SetAppointmentsAction
  | SetDailyStatisticsAction
  | SetPickedAppointmentIdAction
	| ShowBtgModal 
	| HideBtgModal;

export const getTodaysAppointmentsSaga = (): TodaysAppointmentAction =>
  ({ type: TodaysAppointmentType.GET_TODAYS_APPOINTMENTS_SAGA });

export const setPractitioners = (
  practitioners: Practitioner[]
): TodaysAppointmentAction =>
  ({
    type: TodaysAppointmentType.SET_PRACTITIONERS,
    payload: { practitioners },
  });

export const setAppointments = (appointments: AppointmentData[]): TodaysAppointmentAction =>
  ({
    type: TodaysAppointmentType.SET_APPOINTMENTS,
    payload: { appointments },
  });

export const setPickedAppointmentId = (
  pickedAppointmentId: string
): TodaysAppointmentAction =>
  ({
    type: TodaysAppointmentType.SET_PICKED_APPOINTMENT_ID,
    payload: { pickedAppointmentId },
  });

export const setDailyStatistics = (dailyStatistics: DailyStatistics): TodaysAppointmentAction =>
  ({
    type: TodaysAppointmentType.SET_DAILY_STATISTICS,
    payload: { dailyStatistics },
  });

export const showBTGModal = (): TodaysAppointmentAction =>
  ({ type: TodaysAppointmentType.SHOW_BTG_MODAL });

export const hideBTGModal = (): TodaysAppointmentAction =>
  ({ type: TodaysAppointmentType.HIDE_BTG_MODAL });
