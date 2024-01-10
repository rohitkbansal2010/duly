import { LocaleAction, LocaleType } from '../actions';

export type InitialStateType = {
  locale: string;
};

const initialState: InitialStateType = { locale: 'en-US' };

export const localeReducer = (
  state = initialState,
  { type, payload }: LocaleAction
): InitialStateType => {
  switch (type) {
    case LocaleType.SWITCH_LOCALE:
      return {
        ...state,
        ...payload,
      };

    default:
      return state;
  }
};
