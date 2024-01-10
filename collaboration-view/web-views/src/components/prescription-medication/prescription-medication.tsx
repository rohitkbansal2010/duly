import { Avatar } from '@components/avatar';
import { ACCORDIAN_CARD_WIDTH } from '@constants';

import styles from './prescription-medication.scss';

type PrescriptionMedicationType = {
    medicationDetails : any;
}
export const PrescriptionMedication = ({ medicationDetails } : PrescriptionMedicationType) =>
  (
    <>
      <style type="text/css">
        {`
          sup{
            top: -0.15rem;
          }
        `}
      </style>
      <div className={`${styles.container} prescription-card`}>
        <div className={styles.containerHeader}>
          <div className={styles.containerHeaderText}>
            {medicationDetails.title}
            <sup className={styles.medicationType}>RX              
            </sup>
          </div>
        </div>
        <div className={styles.containerBody}>
          {medicationDetails.provider ? (
            <div className={styles.containerBodyPrescriber}>
              <div className={styles.containerBodyHeading}>
                PRESCRIBER
              </div>
              <div className={styles.containerBodyPrescriberDetails}>
                <div className={styles.containerBodyPrescriberImage}>
                  <Avatar src={undefined} alt="doctor" width= {ACCORDIAN_CARD_WIDTH} />
                </div>
                <div className={styles.containerBodyText}>
                  {`${medicationDetails.provider?.prefixes} ${medicationDetails.provider.familyName}`}
                </div>
              </div>

            </div>
          ) : (
            <div className={styles.containerBodyPrescriber}/>
          )}
          <div className={styles.conatinerBodyInstruction}>
            <div className={styles.containerBodyHeading}>
              INSTRUCTIONS
            </div>
            <div className={styles.containerBodyText}>
              {medicationDetails.instructions}
            </div>
                
          </div>
            
        </div>
      </div>
      
    </>
    
  );
