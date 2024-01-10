import { Modal } from 'react-bootstrap';

import { Avatar } from '@components/avatar';

import styles from './start-checkout-modal.scss';

type StartCheckoutModalProps = {
  show: boolean;
  handleClose: () => void;
  handleShow: () => void;
  date: Date | undefined;
  time: string;
  providerImage?: string;
  providerName?: string;
  providerType?: string;
  providerStream?: string;
  doctorType?: string;
  heading?: string;
  location?: string;
  pincode?:string;
  buttonName: string;
  scheduleDetails: {
    imageIcon: string,
    topLine: string,
    middleLine?: string,
    bottomLine?: string
  }[]
  handleScheduleFollowUp?: () => void;
};

export const StartCheckoutModal = (props: StartCheckoutModalProps) => 
  (
    <Modal
      show={props.show}
      centered
      onHide={props.handleClose}
      dialogClassName={styles.modalParentDiv}
    >
      <Modal.Header closeButton style={{ borderBottom: 'none' }}>
        <Modal.Title className={styles.heading}>
          {props.heading ? props.heading : 'Confirm Follow Up'}
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {props.scheduleDetails.map((item, index) =>
        (
          <div key={index} className={styles.parentContainer}>
            <div className={styles.imageIcon}>
              <Avatar src={item.imageIcon} alt={item.topLine} width={6.25} />
              <div className={styles.modalItems}>
                <div className={styles.doctorStream}>{item.topLine}</div>
                <div className={styles.doctorName}>{item.middleLine}</div>
                <div className={styles.data}>{item.bottomLine}</div>
              </div>
            </div>
          </div>
        ))}
        {props.handleScheduleFollowUp && (
        <button
          className={styles.modalButton}
          data-automation="schedule-follow-up-modal-button"
          onClick={() => {
              props.handleClose();
              props.handleScheduleFollowUp
              && props.handleScheduleFollowUp();
          }}
        >
          {props.buttonName}
        </button>
        )}
      </Modal.Body>
    </Modal>
  );
