import { UIActions, UIType } from '@redux/actions';
import { getRatio, getRemCoefficient, getWithinSh1Size } from '@utils';

export type UIStateType = {
  width: number | null;
  height: number | null;
  ratio: number | null;
  isWithinSh1Size: boolean;
  remCoefficient: number | null;
  isDataFetched: boolean;
}

const uiState: UIStateType = {
  width: null,
  height: null,
  ratio: null,
  isWithinSh1Size: false,
  remCoefficient:  null,
  isDataFetched: false,
};

export const uiReducer = (
  state = uiState,
  action: UIActions
): UIStateType => {
  switch (action.type) {
    case UIType.SET_VIEWPORT_INTERFACE: {
      const { width, height } = action.payload;
      const ratio = getRatio(width, height);
      const isWithinSh1Size = getWithinSh1Size(width, ratio);
      const remCoefficient = getRemCoefficient(width);

      return {
        ...state, width, height, ratio, isWithinSh1Size, remCoefficient,
      };
    }

    case UIType.START_DATA_FETCH:
      return { ...state, isDataFetched: false };

    case UIType.STOP_DATA_FETCH:
      return { ...state, isDataFetched: true };

    default:
      return state;
  }
};
