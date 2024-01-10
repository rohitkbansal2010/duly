import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';

import { StepperTitles } from '@enums';
import { addChat } from '@icons';
import { getReferralDetails, setStepsList } from '@redux/actions/cv-checkout-appointments';

import styles from './referrals.scss';

export const ErrorReferral = () => {
  const { patientId } = useParams<{ 
    patientId: string, 
    appointmentId: string, 
    practitionerId: string 
  }>();
  const dispatch = useDispatch();
  const handleClick = ()=>{
    dispatch(setStepsList([ { alias: StepperTitles.REFERRAL, isSkeletonShown: true } ]));
    dispatch(getReferralDetails(patientId, true));
  };

  return (
    <div className={styles.referralErrorWrapper}>
      <div className={styles.referralErrorIconWrapper}>
        <img src={addChat} alt="Referral error" />
      </div>
      <div className={styles.referralErrorTitle}>Couldn&apos;t fetch the data!</div>
      <div
        className=
          {styles.referralErrorSubTitle}
      >
        Unable to fetch the data. Please try again.</div>
      <button className={styles.referralErrorTryButton} onClick = {handleClick}>Try Again</button>
    </div>
  );
};
