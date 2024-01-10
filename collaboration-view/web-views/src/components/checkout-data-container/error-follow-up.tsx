
import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';

import { StepperTitles } from '@enums';
import { useSiteId } from '@hooks';
import { Calendar } from '@icons';
import { getFollowUpDetails, setStepsList } from '@redux/actions/cv-checkout-appointments';

import styles from './shedule-follow-up.scss';

export const ErrorFollowUp = () => {
  const { patientId, practitionerId } = useParams<{ 
    patientId: string, 
    appointmentId: string, 
    practitionerId: string 
  }>();
  const dispatch = useDispatch();
  const { siteId } = useSiteId();
  const handleClick = ()=>{
    dispatch(setStepsList([ { alias: StepperTitles.FOLLOW_UP, isSkeletonShown: true } ]));
    dispatch(getFollowUpDetails({ 
      patientId, 
      siteId, 
      practitionerId, 
      notInitialLoad: true, 
    }));
  };
  return (
    <div className = {styles.followUpErrorWrapper}>
      <div className={styles.followUpErrorIconWrapper}>
        <img src={Calendar} alt="Follow up error" />
      </div>
      <div className={styles.followUpErrorTitle}>Couldn&apos;t fetch the data!</div>
      <div
        className=
          {styles.followUpErrorSubTitle}
      >
        Unable to fetch the data. Please try again.</div>
      <button className={styles.followUpErrorTryButton} onClick = {handleClick}>Try Again</button>
    </div>  
  );};
