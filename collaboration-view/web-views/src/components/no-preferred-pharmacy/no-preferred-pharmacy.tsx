import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';

import { Avatar } from '@components/avatar';
import { PharmacySkeleton } from '@components/prescriptions/PharmacySkeleton';
import { StepperTitles } from '@enums';
import { useStepperSkeleton } from '@hooks';
import { errorTriangular } from '@icons';
import { getPreferredPharmacyDetails } from '@redux/actions/cv-checkout-appointments';

import styles from './no-preferred-pharmacy.scss';

type NoPreferredPharmacyPropType = {
  noDataAvailable?:boolean;
}

export const NoPreferredPharmacy = ({ noDataAvailable }:NoPreferredPharmacyPropType) => {
  const dispatch = useDispatch();
  const { patientId } = useParams<{ 
    patientId: string, 
    appointmentId: string, 
    practitionerId: string 
  }>();
  const handleClick = ()=>{
    dispatch(getPreferredPharmacyDetails(patientId, true));
  };

  const isSkeletonShown = useStepperSkeleton(StepperTitles.PREFERRED_PHARMACY);

  if(isSkeletonShown) {
    return <PharmacySkeleton/>;
  }
  return (
    <>
      <div className={styles.container}>
        <div className={styles.icon}>
          <Avatar src={errorTriangular} alt="errorIcon" width={8} />
        </div>
        <h1 className={styles.heading}>Couldn&apos;t get the data!</h1>
        {!noDataAvailable ? ( 
          <>      
            <h2 className={styles.title}>
              Unable to fetch the preferred pharmacy details.<br/>Please try again.
            </h2>
            <button className={styles.btn} onClick = {handleClick}>Try Again</button>  </>     
        ) :
        (
          <h2 className={styles.title}>
            No information available for your preferred pharmacy.
          </h2>
          )
        }
      </div>
    </>
  );
};
