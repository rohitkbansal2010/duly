import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';

import { StepperTitles } from '@enums';
import { labsFilledIcon } from '@icons';
import { getLabTestDetails, setStepsList } from '@redux/actions/cv-checkout-appointments';

import styles from './labs.scss';

export const ErrorLabs = () => {
  const { patientId, appointmentId } = useParams<{ 
    patientId: string, 
    appointmentId: string, 
    practitionerId: string 
  }>();
  const dispatch = useDispatch();
  const handleClick = ()=>{
    dispatch(setStepsList([ { alias: StepperTitles.LABS, isSkeletonShown: true } ]));
    dispatch(getLabTestDetails(patientId, appointmentId, true));
  };
  return (
    <div className = {styles.labsErrorWrapper}>
      <div className={styles.labsErrorIconWrapper}>
        <img src={labsFilledIcon} alt="Labs error" />
      </div>
      <div className={styles.labsErrorTitle}>Couldn&apos;t fetch the data!</div>
      <div
        className=
          {styles.labsErrorSubTitle}
      >
        Unable to fetch the data. Please try again.</div>
      <button className={styles.labsErrorTryButton} onClick = {handleClick}>Try Again</button>

    </div>
  );
};
