import { Button, Modal } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { Avatar } from '@components/avatar';
import { PATIENT_GIVEN_NAMES_KEY, PRACTICIONER_KEY, WELCOME_SUFFIX_KEY } from '@constants';
import { CollaborationViewRoutes } from '@enums';
import { useAzureADAuth } from '@hooks';
import { clockVioletIcon, supervisedUserIcon } from '@icons';
import { setPickedAppointmentId } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { AppointmentData } from '@types';
import {
  getUserRole,
  getSrcAvatar,
  showFirstPrefixItem,
  writeHumanName,
  writeTimeInterval,
  encodeURLParams,
  isTitleNotEmpty
} from '@utils';

import styles from './picked-appointment-modal.scss';

export const PickedAppointmentModal = () => {
  const dispatch: AppDispatch = useDispatch();

  const appointments = useSelector(({ TODAYS_APPOINTMENTS }: RootState) =>
    TODAYS_APPOINTMENTS.appointments);
  const pickedAppointmentId = useSelector(({ TODAYS_APPOINTMENTS }: RootState) =>
    TODAYS_APPOINTMENTS.pickedAppointmentId);

  const { logoutAzureADRedirect } = useAzureADAuth();

  // @TODO Fix typing on destructuring of probably undefined value
  const {
    patient: {
      patientGeneralInfo: {
        id: patientId,
        humanName: { familyName: patientFamilyName, givenNames },
      },
      isNewPatient,
    },
    timeSlot: { startTime, endTime },
    practitioner: {
      id: practitionerId,
      humanName: {
        familyName: practitionerFamilyName,
        givenNames: practitionerGivenNames,
        prefixes = [],
      },
      photo,
      role,
    },
    title,
  } = appointments.find(({ id }: AppointmentData) =>
    id === pickedAppointmentId);

  const launchAppointment = () => {
    sessionStorage.setItem(WELCOME_SUFFIX_KEY, isNewPatient ? 'to Duly' : 'Back');
    sessionStorage.setItem(PATIENT_GIVEN_NAMES_KEY, givenNames.join(' '));
    sessionStorage.setItem(PRACTICIONER_KEY, `${showFirstPrefixItem(prefixes)}${practitionerFamilyName}`);
    dispatch(setPickedAppointmentId(''));

    const [
      encodedAppointmentId,
      encodedPatientId,
      encodedPractitionerId,
    ] = encodeURLParams(pickedAppointmentId, patientId, practitionerId);

    logoutAzureADRedirect(`${CollaborationViewRoutes.welcome}/${encodedAppointmentId}/${encodedPatientId}/${encodedPractitionerId}`);
  };

  return (
    <Modal
      animation={false}
      show={true}
      onHide={() =>
        dispatch(setPickedAppointmentId(''))}
      className={styles.encounterModal}
      size="lg"
      centered
      dialogClassName={styles.customWidthModal}
      backdropClassName={styles.customBackDropModal}
    >
      <Modal.Header className={styles.encounterModalHeader}>
        <Modal.Title className={styles.encounterModalHeaderTitle}>
          {writeHumanName(givenNames, patientFamilyName)}
        </Modal.Title>
      </Modal.Header>
      <Modal.Body className={styles.encounterModalBody}>
        <Modal.Title className={`${styles.encounterModalBodyTitle} text-nowrap`}>
          Are you sure you want to launch this appointment?
        </Modal.Title>
        <ul className={styles.encounterModalBodyList}>
          {practitionerFamilyName && (
            <li className={styles.encounterModalBodyListItem}>
              <Avatar
                src={getSrcAvatar(photo)}
                alt={`${showFirstPrefixItem(practitionerGivenNames)}${practitionerFamilyName}`.trim()}
                role={getUserRole({ role, prefixes })}
                width={1.69}
              />
              <span>{`${prefixes} ${practitionerFamilyName}`}</span>
            </li>
          )}
          {isTitleNotEmpty(title) && (
            <li className={styles.encounterModalBodyListItem}>
              <img src={supervisedUserIcon} alt="" />
              <span>{title}</span>
            </li>
          )}
          {startTime && endTime && (
            <li className={styles.encounterModalBodyListItem}>
              <img src={clockVioletIcon} alt="" />
              <span>{writeTimeInterval(new Date(startTime), new Date(endTime))}</span>
            </li>
          )}
        </ul>
      </Modal.Body>
      <Modal.Footer className={styles.encounterModalFooter}>
        <Button
          variant="secondary"
          onClick={() =>
            dispatch(setPickedAppointmentId(''))}
          className={styles.encounterModalFooterCancel}
        >
          Cancel
        </Button>
        <Button
          variant="primary"
          onClick={launchAppointment}
          className={styles.encounterModalFooterLaunch}
        >
          Launch
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
