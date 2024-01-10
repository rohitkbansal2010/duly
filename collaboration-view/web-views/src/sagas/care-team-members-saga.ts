import { call, put, takeLatest } from 'redux-saga/effects';

import { getCareTeam } from '@http/requests/appointments';
import {
  CurrentAppointmentType,
  GetCareTeamMembers,
  setCareTeamMembers,
  setUserData,
  setUserRole,
  showCareTeam
} from '@redux/actions';
import { CareTeamMember, UserData } from '@types';
import {
  catchExceptions,
  CatchExceptionsParamsType,
  getUserRole
} from '@utils';

function* workerCareTeamMembers({ payload }: GetCareTeamMembers) {
  try {
    const { patientId, appointmentId } = payload;
    yield put(showCareTeam(false));

    const careTeamArray: CareTeamMember[] = yield call(getCareTeam, appointmentId, patientId);
    const tempUserWorkaround = ({
      memberType,
      ...temporaryUserData
    }: CareTeamMember): UserData =>
      ({
        ...temporaryUserData,
        role: memberType,
      });

    if (careTeamArray.length) {
      const careTeamMember = tempUserWorkaround(careTeamArray[0]);

      yield put(setUserData(careTeamMember));
      yield put(setUserRole(getUserRole({
        role: careTeamMember.role,
        prefixes: careTeamMember.humanName.prefixes,
      })));
    }

    yield put(setCareTeamMembers(careTeamArray));
  } catch ({ status, headers: { 'x-correlation-id': xCorrId } }) {
    yield catchExceptions({ status, xCorrId } as CatchExceptionsParamsType);
  } finally {
    yield put(showCareTeam(true));
  }
}

export function* careTeamMemberstSaga() {
  yield takeLatest(CurrentAppointmentType.GET_CARE_TEAM_MEMBERS, workerCareTeamMembers);
}
