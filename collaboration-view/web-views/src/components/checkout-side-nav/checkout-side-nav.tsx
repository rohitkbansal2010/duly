import { useSelector } from 'react-redux';

import { Avatar } from '@components/avatar';
import { PATIENT_HEADER_AVATAR_SIZE } from '@constants';
import { AvatarByRole } from '@enums';
import { RootState } from '@redux/reducers';
import { calculateYears, getSrcAvatar, writeHumanName } from '@utils';

import styles from './checkout-side-nav.scss';

export const CheckoutSideNav = () => {
  const patientData = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientData);
  const givenNames: string[] = patientData?.generalInfo?.humanName?.givenNames || [];
  const familyName: string = patientData?.generalInfo?.humanName?.familyName || '';
  const gender: string = patientData?.gender || '';
  const birthDate: string | undefined = patientData?.birthDate;
  const yearsOld: number | undefined = calculateYears(birthDate);
  return (
    <>
      <div className={styles.userInfo}>
        <Avatar
          src={getSrcAvatar(patientData?.photo)}
          width={PATIENT_HEADER_AVATAR_SIZE}
          role={AvatarByRole.NON_EMPLOYEE}
          hasBorder
        />
        <div className={styles.userInfoDiv}>
          <div className={styles.userInfoDivPreTitle}>PATIENT</div>
          <div className={styles.userInfoDivTitle}>{writeHumanName(givenNames, familyName)}</div>
          <div className={styles.userInfoDivPostTitle}>{gender}{yearsOld ? `, ${yearsOld} years old` : ''}</div>
        </div>
      </div>
    </>
  );
};
