import { Button, Modal } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { warningCircleMagentaIcon } from '@icons';
import { hideBTGModal } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';

import styles from './btg-modal.scss';

export const BTGModal = () => {
  const dispatch: AppDispatch = useDispatch();

  const isShowBtgModal = useSelector(({ TODAYS_APPOINTMENTS }: RootState) =>
    TODAYS_APPOINTMENTS.isShowBtgModal);

  const closeModal = () =>
    dispatch(hideBTGModal());

  return (
    <Modal
      animation={false}
      show={isShowBtgModal}
      onHide={closeModal}
      size="lg"
      centered
      dialogClassName={styles.customWidthModal}
      backdropClassName={styles.customBackDropModal}
      className={styles.btgModal}
    >
      <Modal.Header className={styles.btgModalHeader}>
        <div className={styles.btgModalHeaderIconContainer}>
          <div
            className={styles.btgModalHeaderIconWrapper}
          >
            <img
              src={warningCircleMagentaIcon}
              alt="restricted patient record icon"
            />
          </div>
        </div>
      </Modal.Header>
      <Modal.Body>
        <Modal.Title className={styles.btgModalTitle}>
          This is a restricted patient record!
        </Modal.Title>
      </Modal.Body>
      <Modal.Footer className={styles.btgModalFooter}>
        <div className={styles.btgModalFooterButtonContainer}>
          <Button
            variant="primary"
            onClick={closeModal}
            className={styles.btgModalFooterButton}
          >
            Close
          </Button>
        </div>
      </Modal.Footer>
    </Modal>
  );
};
