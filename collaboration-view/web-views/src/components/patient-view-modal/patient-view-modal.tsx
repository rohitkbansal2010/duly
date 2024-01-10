
import { Modal } from 'react-bootstrap';
import { useSelector } from 'react-redux';
import { useParams, useHistory } from 'react-router-dom';

import { Avatar } from '@components/avatar';
import { SideModal } from '@components/side-modal';
import { PATIENT_VIEW_MODAL_AVATAR_SIZE } from '@constants';
import { AvatarByRole } from '@enums';
import { usePatientViewModal, useAzureADAuth } from '@hooks';
import { dulyLargeWhiteDarkBlueIcon } from '@icons';
import { RootState } from '@redux/reducers';
import { PatientData } from '@types';
import { calculateYears, getSrcAvatar, writeHumanName } from '@utils';

import styles from './patient-view-modal.scss';

type AppointmentParams = {
  appointmentId: string;
  patientId: string;
  practitionerId: string;
};
export const PatientViewModal = () => {
  const history = useHistory();
  const { logoutAzureADRedirect } = useAzureADAuth();
  const { appointmentId, patientId, practitionerId } = useParams<AppointmentParams>();

  const { isShowPatientViewModal, onHidePatientViewModal } = usePatientViewModal();
  const patientData: PatientData | null = useSelector(
    ({ CURRENT_APPOINTMENT }: RootState) =>
      CURRENT_APPOINTMENT.patientData
  );

  const givenNames: string[] = patientData?.generalInfo?.humanName?.givenNames || [];
  const familyName: string = patientData?.generalInfo?.humanName?.familyName || '';
  const gender: string = patientData?.gender || '';
  const birthDate: string | undefined = patientData?.birthDate;
  const yearsOld: number | undefined = calculateYears(birthDate);

  const StartCheckout = () => {
    sessionStorage.removeItem('metric'); // set toggle to off when logout.
    logoutAzureADRedirect(`/start-checkout/${appointmentId}/${patientId}/${practitionerId}`);
  };

  const gotoAfterSteps = () => {
    history.push(`/exam-room/schedule-follow-up/${appointmentId}/${patientId}/${practitionerId}/checkout`);
  };
  return (
    <SideModal show={isShowPatientViewModal} onHide={onHidePatientViewModal}>
      <Modal.Header className={styles.patientViewModalHeader}>
        <img src={dulyLargeWhiteDarkBlueIcon} alt="Large duly icon" />
      </Modal.Header>
      <Modal.Body className={styles.patientViewModalBody}>
        <div className={styles.patientViewModalBodyAvatar}>
          <Avatar
            width={PATIENT_VIEW_MODAL_AVATAR_SIZE}
            src={getSrcAvatar(patientData?.photo)}
            role={AvatarByRole.NON_EMPLOYEE}
            hasBorder
          />
        </div>
        <div className={styles.patientViewModalBodyHumanName} data-testid="humanName">
          {writeHumanName(givenNames, familyName)}
        </div>
        <div className={styles.patientViewModalBodyAdditionalInfo}>
          <div data-testid="gender">{gender}</div>
          <div data-testid="age">{!!yearsOld && `${yearsOld} years old`}</div>
        </div>
        <div className={styles.patientViewModalBodyNextSteps}>
          <div>
            <button
              className={styles.patientViewModalBodyNextStepsButtonPrimary}
              onClick={() => 
                StartCheckout()}
              data-automation="handover-to-ma-button"
            >
              Handover to MA
            </button>
            <button
              className={styles.patientViewModalBodyNextStepsButtonDefault}
              onClick={() =>
                {gotoAfterSteps();
                onHidePatientViewModal();}
              }
              data-automation="continue-to-after-visit-steps-button"

            >
              Continue to After Visit Steps
            </button>
          </div>
        </div>
      </Modal.Body>
    </SideModal>
  );
};
