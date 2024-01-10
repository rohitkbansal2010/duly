import { Modal } from 'react-bootstrap';

import { ReferralScheduleBody } from './ReferralScheduledBody';
import styles from './ReferralScheduleModal.scss';

type ReferralScheduleModalProps = {
  show: boolean;
  handleClose: () => void;
};

export const ReferralScheduleModal = (props: ReferralScheduleModalProps) => 
  (
    <Modal
      show={props.show}
      centered
      contentClassName={styles.modalContent}
      dialogClassName={styles.setWidth}
      onHide={props.handleClose}
    >
      <Modal.Header closeButton style={{ borderBottom: 'none' }}>
        <Modal.Title className={styles.mainheading}>Referral Scheduled (2/3)</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <ReferralScheduleBody />
      </Modal.Body>
    </Modal>
  );
