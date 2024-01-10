import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Avatar } from '@components/avatar';
import FontSizeSetting from '@components/font-size-setting/font-size-setting';
import { PATIENT_HEADER_AVATAR_SIZE } from '@constants';
import { AvatarByRole } from '@enums';
import { usePatientViewModal } from '@hooks';
import { getPatientData } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { getSrcAvatar } from '@utils';


import styles from './patient-header-data.scss';


export const PatientHeaderData = () => {
  const dispatch: AppDispatch = useDispatch();
  const { onShowPatientViewModal } = usePatientViewModal();
  const patientData = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientData);
  const patientId = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientId);

  const patientName = patientData?.generalInfo?.humanName?.givenNames?.join(' ').trim();
  const photoData = patientData?.photo;

  useEffect(() => {
    patientId && dispatch(getPatientData(patientId));
  }, [ dispatch, patientId ]);

  return (
    <div className={styles.patientHeaderContainer} data-testid="container">
      {patientData && (
        <>
          <h2 className={styles.patientHeaderGreeting} data-testid="welcome">
            Welcome
            {patientName ? ', ' : ''}
            <span className={styles.patientName}>{patientName}</span>
          </h2>
          <button
            className={styles.patientHeaderAvatar}
            onClick={onShowPatientViewModal}
          >
            <Avatar
              src={getSrcAvatar(photoData)}
              width={PATIENT_HEADER_AVATAR_SIZE}
              role={AvatarByRole.NON_EMPLOYEE}
              hasBorder
            />
          </button>
          <div className={styles.verticalLine} />
        </>
      )}
      <div className={styles.fontSettingContainer}>
        <FontSizeSetting/>
      </div>
    </div>
  );
};
