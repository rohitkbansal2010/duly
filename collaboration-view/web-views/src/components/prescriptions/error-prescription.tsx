import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';

import { StepperTitles } from '@enums';
import { prescriptionIcon } from '@icons';
import { setStepsList, getPrescriptionDetails } from '@redux/actions/cv-checkout-appointments';

import styles from './prescriptions.scss';

export const ErrorPrescription = () => {
  const { patientId, appointmentId } = useParams<{ 
    patientId: string, 
    appointmentId: string, 
    practitionerId: string
  }>();
  const dispatch = useDispatch();
  const handleClick = ()=>{
    dispatch(setStepsList([ { alias: StepperTitles.PRESCRIPTION, isSkeletonShown: true } ]));
    dispatch(getPrescriptionDetails(patientId, appointmentId, true));
  };
  return (
    <div className={styles.prescriptionErrorWrapper}>
      <div className={styles.prescriptionErrorIconWrapper}>
        <img src={prescriptionIcon} alt="Prescription error" />
      </div>
      <div className={styles.prescriptionErrorTitle}>Couldn&apos;t fetch the data!</div>
      <div
        className=
          {styles.prescriptionErrorSubTitle}
      >
        Unable to fetch the data. Please try again.</div>
      <button className={styles.prescriptionErrorTryButton} onClick = {handleClick}>Try Again
      </button>
    </div>
  );
};
