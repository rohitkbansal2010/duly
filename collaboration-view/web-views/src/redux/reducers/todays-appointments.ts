import {
  TodaysAppointmentAction,
  TodaysAppointmentType,
  UIActions,
  UIType
} from '@redux/actions';
import { DailyStatistics, Practitioner, AppointmentData } from '@types';

type TodaysAppointmentsStateType = {
  practitioners: Practitioner[];
  appointments: AppointmentData[] | null;
  pickedAppointmentId: string;
  dailyStatistics: DailyStatistics | null;
	isShowBtgModal: boolean
};

const todaysAppointmentsState: TodaysAppointmentsStateType = {
  practitioners: [],
  appointments: null,
  pickedAppointmentId: '',
  dailyStatistics: null,
  isShowBtgModal: false,
};

export const todaysAppointmentsReducer = (
  state = todaysAppointmentsState,
  action: TodaysAppointmentAction | UIActions
): TodaysAppointmentsStateType => {
  switch (action.type) {
    case TodaysAppointmentType.SET_PRACTITIONERS:
      return {
        ...state,
        practitioners: action.payload.practitioners,
      };

    case TodaysAppointmentType.SET_APPOINTMENTS:
      return {
        ...state,
        appointments: action.payload.appointments,
      };

    case TodaysAppointmentType.SET_PICKED_APPOINTMENT_ID:
      return {
        ...state,
        pickedAppointmentId: action.payload.pickedAppointmentId,
      };

    case TodaysAppointmentType.SET_DAILY_STATISTICS:
      return {
        ...state,
        dailyStatistics: action.payload.dailyStatistics,
      };

    case TodaysAppointmentType.SHOW_BTG_MODAL:
      return {
        ...state,
        isShowBtgModal: true,
      };

    case TodaysAppointmentType.HIDE_BTG_MODAL:
      return {
        ...state,
        isShowBtgModal: false,
      };

    case UIType.START_DATA_FETCH:
      return {
        ...state,
        appointments: null,
      };

    default:
      return state;
  }
};
