import { PropsWithChildren } from 'react';
import { Modal } from 'react-bootstrap';

import styles from './side-modal.scss';

type SideModalProps = {
  show: boolean;
  onHide: () => void;
  backgroundContentColor?: string;
};

export const SideModal = ({
  show,
  onHide,
  backgroundContentColor = 'Blue',
  children,
}: PropsWithChildren<SideModalProps>) =>
  (
    <Modal
      animation={false}
      show={show}
      onHide={onHide}
      className={styles.sideModal}
      backdropClassName={styles.sideModalBackgrop}
      dialogClassName={styles.sideModalDialog}
      contentClassName={`${styles.sideModalContent} ${styles[`sideModalContentBackground${backgroundContentColor}`]}`}
    >
      {children}
    </Modal>
  );
