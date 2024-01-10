import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Practitioners } from '@components/practitioners';
import { getActiveUser } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { Practitioner } from '@types';
import { getPractitionersWithoutUser } from '@utils';

export const TodaysAppointmentPractitioners = () => {
  const practitioners = useSelector(({ TODAYS_APPOINTMENTS }: RootState) =>
    TODAYS_APPOINTMENTS.practitioners);

  const userData = useSelector(({ USER }: RootState) =>
    USER.userData);

  const dispatch: AppDispatch = useDispatch();

  useEffect(() => {
    dispatch(getActiveUser());
  }, [ dispatch ]);

  const practitionersWithoutActiveUser =
		userData && getPractitionersWithoutUser(practitioners, userData);

  const combined = userData ?
    [
      { ...userData, isCurrentUser: true },
      ...practitionersWithoutActiveUser as Practitioner[],
    ] : practitioners;

  return (
    <Practitioners
      practitioners={combined}
      isTodaysAppointment
    />
  );
};
