import {
  call, 
  put, 
  SagaReturnType, 
  takeLatest
} from 'redux-saga/effects';

import { getActiveUser } from '@http/requests';
import {
  setUserData,
  setUserRole,
  UserType
} from '@redux/actions';
import {
  ActiveUser,
  Practitioner
} from '@types';
import {
  catchExceptions,
  CatchExceptionsParamsType,
  getUserRole
} from '@utils';

const cacheForInitialLoad = new WeakMap();

type RequestType<P> = () => Promise<P>;

function* requestWithCache<T>(request: RequestType<T>, ...params: Parameters<RequestType<T>>) {
  if (!cacheForInitialLoad.has(request)) {
    const result: SagaReturnType<typeof request> = yield call<RequestType<T>>(request, ...params);
    cacheForInitialLoad.set(request, result);
  }

  return cacheForInitialLoad.get(request);
}

function* workerActiveUser() {
  try {
    const activeUser: ActiveUser | undefined
		= yield requestWithCache<ActiveUser | undefined>(getActiveUser);

    const userData: Practitioner | null = activeUser ? {
      id: activeUser.id,
      humanName: activeUser.name,
      role: activeUser.role,
      photo: activeUser.photo,
    } : null;

    const userRole: string | null = userData ? getUserRole({
      role: userData.role,
      prefixes: userData.humanName.prefixes,
    }) : null;

    yield put(setUserData(userData));
    yield put(setUserRole(userRole));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  }
}

export function* activeUserSaga() {
  yield takeLatest(UserType.GET_ACTIVE_USER, workerActiveUser);
}
