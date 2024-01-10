/**
 * We should use only one instance of LogoutExpirationModal on the page
 */
import { useEffect, useRef, useState } from 'react';
import { Button, Modal } from 'react-bootstrap';
import { useDispatch } from 'react-redux';
import { useInterval } from 'usehooks-ts';

import {
  MINUTES_OF_AUTO_LOGOUT,
  OF_MILLISECONDS,
  SECONDS_PER_MINUTE,
  TIME_OF_AUTO_LOGOUT
} from '@constants';
import { useDocumentEvent, useAzureADAuth } from '@hooks';
import { hideLogoutModal, hidePatientViewModal, setPickedAppointmentId } from '@redux/actions';
import { AppDispatch } from '@redux/store';
import { addZeroIfOneDigit, getLocalStorageItem, setTimeOfAutoLogout } from '@utils';

import styles from './logout-expiration-modal.scss';

const MODAL_SHOW_INTERVAL_MS = MINUTES_OF_AUTO_LOGOUT * SECONDS_PER_MINUTE * OF_MILLISECONDS;

const getDiffDate = (timeAutoLogout: number, currentDate: number) => {
  const diffDate = timeAutoLogout - currentDate;

  return {
    minutes: diffDate > 0 ? Math.floor(diffDate / OF_MILLISECONDS / SECONDS_PER_MINUTE) : 0,
    seconds: diffDate > 0 ? Math.floor(diffDate / OF_MILLISECONDS) : 0,
  };
};

export const LogoutExpirationModal = () => {
  const timeOfAutoLogout = parseInt(JSON.parse(getLocalStorageItem(TIME_OF_AUTO_LOGOUT) || 'null'));
  const diff = timeOfAutoLogout - Date.now();
  const isTimeFinished = diff < 0;
  const isTimeAutoLogout = diff <= MODAL_SHOW_INTERVAL_MS;

  const dispatch: AppDispatch = useDispatch();

  const { logoutAzureADRedirect, inProgress } = useAzureADAuth();
  const [ show, setShow ] = useState(false);
  const [ nowTime, setNowTime ] = useState(Date.now());
  const autoLogoutStarted = useRef<boolean>(false);
  const logoutInProgress = inProgress === 'logout';

  // we are listening for the TIME_OF_AUTO_LOGOUT changes from other tabs
  useDocumentEvent(({ key, newValue }) => {
    if (key === TIME_OF_AUTO_LOGOUT) {
      const newTimeOfAutoLogout = parseInt(JSON.parse(newValue || 'null'));
      if (newTimeOfAutoLogout < timeOfAutoLogout) {
        setNowTime(Date.now()); // is needed to rerender the component and run all hooks
      }
    }
  }, 'storage');

  useDocumentEvent(setTimeOfAutoLogout, show ? null : 'click');
  useDocumentEvent(setTimeOfAutoLogout, show ? null : 'touchend');

  useEffect(() => {
    setTimeOfAutoLogout();
  }, []);

  useEffect(() => {
    if (Number.isNaN(timeOfAutoLogout) && inProgress === 'none') {
      // 1. missing timeOfAutoLogout means that user logged out from another tab
      // the case should be handled in another place
      // 2. method will NOT be triggered on mount (after the login)
      // because of inProgress is equal to "handleRedirect" at startup
      return;
    }
    const timeDifference = timeOfAutoLogout - Date.now() - MODAL_SHOW_INTERVAL_MS;
    if (timeDifference > 0) {
      const timeoutId = setTimeout(() =>
        setNowTime(Date.now()), timeDifference);
      return () =>
        clearTimeout(timeoutId);
    }
  }, [ timeOfAutoLogout, inProgress ]);

  useInterval(
    () =>
      setNowTime(Date.now()),
    isTimeAutoLogout ? OF_MILLISECONDS : null
  );

  useEffect(() => {
    if (!logoutInProgress) {
      if (isTimeAutoLogout) {
        if (!isTimeFinished && !show) {
          setShow(true);
        }
        if (!autoLogoutStarted.current) {
          dispatch(setPickedAppointmentId(''));
          dispatch(hidePatientViewModal());
          dispatch(hideLogoutModal());
          autoLogoutStarted.current = true;
        }
      } else {
        autoLogoutStarted.current = false;
        show && setShow(false);
      }
    }
  }, [ logoutInProgress, isTimeAutoLogout, isTimeFinished, show, dispatch ]);

  useEffect(() => {
    // do not hide the modal, because logout will require some time before redirection
    isTimeFinished && inProgress === 'none' && logoutAzureADRedirect();
  }, [ isTimeFinished, inProgress, logoutAzureADRedirect ]);

  const logoutClickHandler = () => {
    inProgress === 'none' && logoutAzureADRedirect();
  };
  const stayClickHandler = () => {
    setTimeOfAutoLogout();
    setNowTime(Date.now()); // is needed to rerender the component and run all hooks
  };

  const timeDiff = getDiffDate(timeOfAutoLogout, nowTime);

  return (
    <Modal
      animation={false}
      show={show}
      className={styles.logoutExpirationModal}
      backdropClassName={styles.logoutExpirationModalBackDrop}
      dialogClassName={styles.logoutExpirationModalDialog}
      contentClassName={styles.logoutExpirationModalContent}
      centered
    >
      <Modal.Header className={styles.logoutExpirationModalHeader}>
        <Modal.Title className={styles.logoutExpirationModalHeaderTitle}>
          Session Timeout
        </Modal.Title>
      </Modal.Header>
      <Modal.Body className={styles.logoutExpirationModalBody}>
        <div className={styles.logoutExpirationModalBodyText}>
          Due to inactivity, you will be automatically logged out
          {!logoutInProgress && !isTimeFinished && (
            <div className={styles.logoutExpirationModalBodyTextTime}>
              <span data-test="minutes">after {timeDiff.minutes} minutes</span>
              <span data-test="seconds">
                and {addZeroIfOneDigit(timeDiff.seconds % SECONDS_PER_MINUTE)} seconds
              </span>
            </div>
          )}
        </div>
      </Modal.Body>
      <Modal.Footer className={styles.logoutExpirationModalFooter}>
        <Button
          disabled={inProgress !== 'none' || isTimeFinished}
          onClick={logoutClickHandler}
          className={styles.logoutExpirationModalFooterBtnLogout}
        >
          Log out
        </Button>
        <Button
          disabled={logoutInProgress || isTimeFinished}
          onClick={stayClickHandler}
          className={styles.logoutExpirationModalFooterBtnStayLoggedIn}
        >
          Stay logged in
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
