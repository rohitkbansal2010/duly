export enum UIType {
  SET_VIEWPORT_INTERFACE = 'SET_VIEWPORT_INTERFACE',
  START_DATA_FETCH = 'START_DATA_FETCH',
  STOP_DATA_FETCH = 'STOP_DATA_FETCH',
}

type SetViewportInterface = {
  type: typeof UIType.SET_VIEWPORT_INTERFACE;
  payload: { width: number; height: number };
};

type StartDataFetch = {
  type: typeof UIType.START_DATA_FETCH;
};

type StopDataFetch = {
  type: typeof UIType.STOP_DATA_FETCH;
};

export type UIActions =
  SetViewportInterface |
  StartDataFetch |
  StopDataFetch;

export const setViewportInterface =
  ({ width, height }: { width: number; height: number }): UIActions =>
    ({
      type: UIType.SET_VIEWPORT_INTERFACE,
      payload: { width, height },
    });

export const startDataFetch = (): UIActions =>
  ({ type: UIType.START_DATA_FETCH });

export const stopDataFetch = (): UIActions =>
  ({ type: UIType.STOP_DATA_FETCH });
