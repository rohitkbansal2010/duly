import { Button, Modal } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { unexpectedErrorMagentaIcon } from '@icons';
import { hideExceptionsModal } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';

import styles from './exceptions-modal.scss';

export const ExceptionsModal = () => {
  {/* TODO: alter the layout within DPGECLOF-1544 */
  }
  const dispatch: AppDispatch = useDispatch();

  const isExceptionsModalShown = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.isExceptionsModalShown);
  const errorDate = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.errorDate);
  const xCorrId = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.xCorrId);

  const closeModal = () =>
    dispatch(hideExceptionsModal());

  return (
    <Modal
      animation={false}
      show={isExceptionsModalShown}
      className={styles.exceptionsModal}
      size="lg"
      centered
      dialogClassName={styles.customWidthModal}
      backdropClassName={styles.customBackDropModal}
      onHide={closeModal}
    >
      <img src={unexpectedErrorMagentaIcon} alt="" className={styles.exceptionsModalErrorIcon}/>
      <Modal.Header className={styles.exceptionsModalHeader}>
        Unexpected system error
      </Modal.Header>
      <div className={styles.exceptionsModalSubtitle}>
        Something went wrong. Please try again later.
      </div>
      {errorDate && (
        <div className={styles.exceptionsModalSubtitle}>
          Error appeared at <b>{errorDate}</b>
        </div>
      )}
      {xCorrId && (
        <div className={styles.exceptionsModalSubtitle}>
          Error id: <b>{xCorrId}</b>
        </div>
      )}
      <Modal.Footer className={styles.exceptionsModalFooter}>
        <Button
          variant="primary"
          onClick={closeModal}
          className={styles.exceptionsModalFooterClose}
        >
          Close
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
