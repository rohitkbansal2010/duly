import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';

import { StepperTitles } from '@enums';
import { followUpAppointment } from '@icons';
import { getImagingTestDetails, setStepsList } from '@redux/actions/cv-checkout-appointments';

import styles from './imaging.scss';

export const ErrorImaging = () => {
  const { patientId, appointmentId } = useParams<{ 
    patientId: string, 
    appointmentId: string, 
    practitionerId: string 
  }>();
  const dispatch = useDispatch();
  const handleClick = ()=>{
    dispatch(setStepsList([ { alias: StepperTitles.IMAGING, isSkeletonShown: true } ]));
    dispatch(getImagingTestDetails(patientId, appointmentId, true));
  };
  return(
    <div className={styles.imagingErrorWrapper}>
      <div className={styles.imagingErrorIconWrapper}>
        <img src={followUpAppointment} alt="Imaging error" />
      </div>
      <div className={styles.imagingErrorTitle}>Couldn&apos;t fetch the data!</div>
      <div
        className=
          {styles.imagingErrorSubTitle}
      >
        Unable to fetch the data. Please try again.</div>
      <button className={styles.imagingErrorTryButton} onClick = {handleClick}>Try Again</button>
    </div>
  );};
