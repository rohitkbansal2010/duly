import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Practitioners } from '@components/practitioners';
import { getCareTeamMembers } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { setActiveUser } from '@utils';

import styles from './care-team-members.scss';

export const CareTeamMembers = () => {
  const dispatch: AppDispatch = useDispatch();
  const {
    careTeamMembers,
    appointmentId,
    patientId,
    isCareTeamShown,
    isModuleMenuShown,
  } = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT);

  const [ wrapperClass, setWrapperClass ] = useState<string>(styles.careTeamMembersWrapper);
  const [ contentClass, setContentClass ] = useState<string>(styles.careTeamMembers);

  useEffect(() => {
    if (appointmentId && patientId) {
      dispatch(getCareTeamMembers(appointmentId, patientId));
    }
  }, [ dispatch, appointmentId, patientId ]);

  useEffect(() => {
    if (careTeamMembers?.length && isCareTeamShown && isModuleMenuShown) {
      setWrapperClass(initClass =>
        initClass.concat(` ${styles.careTeamMembersWrapperAnimation}`));

      const timer = setTimeout(() => {
        setContentClass(initClass =>
          initClass.concat(` ${styles.careTeamMembersVisible}`));
      }, 600);

      return () =>
        clearTimeout(timer);
    }
  }, [ careTeamMembers, isCareTeamShown, isModuleMenuShown ]);

  return (
    <div className={wrapperClass}>
      <div className={contentClass}>
        <Practitioners
          practitioners={setActiveUser(careTeamMembers)}
        />
      </div>
    </div>
  );
};
