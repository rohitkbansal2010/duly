
import { Avatar } from '@components/avatar';
import { AvatarByRole } from '@enums';
import { MedicationsData } from '@types';

import { DATA } from '../../mock-data/prescription-data';

import styles from './prescription-medication-card.scss';

type PrescriptionMedicationType = {
    medicationDetails : MedicationsData;
    index: number;
}
export const PrescriptionMedication = ({ medicationDetails, index } 
  : PrescriptionMedicationType) => {
  const prescriberDetails = (): string => {
    let prescriberName = '';
    if (medicationDetails.provider) {
      if(medicationDetails.provider.prefixes) {
        prescriberName += medicationDetails.provider.prefixes[0];
      }
      prescriberName += medicationDetails.provider?.familyName;
    }
    else {
      prescriberName = 'Your PCP';
    }
    return prescriberName;
  };

  return (
    <>
      <style type="text/css">
        {`
          sup{
            top: -0.15rem;
          }
        `}
      </style>
      <div className={styles.container} >
        <div className={styles.containerHeader}>
          <div className={styles.containerHeaderText} data-automation={`medication-title-${index}`}>
            {medicationDetails.title}
            <sup className={styles.medicationType} data-automation={`medication-type-${index}`}>{DATA.medicationsType}
            </sup>
          </div>
        </div>
        <div className={styles.containerBody}>
          { medicationDetails.provider ? (
            <div className={styles.containerBodyPrescriber} data-automation={`medication-prescriber-${index}`}>
              <div className={styles.containerBodyHeading}>
                PRESCRIBER
              </div>
              <div className={styles.containerBodyPrescriberDetails}>
                <div className={styles.containerBodyPrescriberImage}>
                  <Avatar role={AvatarByRole.NON_EMPLOYEE} alt="doctor" width = {2.125} />
                </div>
                <div className={styles.containerBodyText}>{prescriberDetails()}</div>
              </div>

            </div>
        ) : (
          <div className={styles.containerBodyPrescriber} data-automation={`medication-prescriber-${index}`} />
)
          }
          <div className={styles.conatinerBodyInstruction} data-automation={`medication-instruction-${index}`}>
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
};
