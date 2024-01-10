import { useState } from 'react';
import { Button } from 'react-bootstrap';
import { Redirect, useHistory, useParams } from 'react-router-dom';

import { PATIENT_GIVEN_NAMES_KEY, PRACTICIONER_KEY, WELCOME_SUFFIX_KEY } from '@constants';
import { CollaborationViewRoutes } from '@enums';
import { useAzureADAuth } from '@hooks';
import { checkoutIcon, dulyLargeIcon } from '@icons';
import { StartCheckoutData } from '@mock-data';

import styles from './start-checkout.scss';

type StartCheckoutParams = {
  appointmentId: string;
  patientId: string;
  practitionerId: string;
};
const getWelcomeData = () =>
  ({
    welcomeSuffix: sessionStorage.getItem(WELCOME_SUFFIX_KEY),
    patientGivenNames: sessionStorage.getItem(PATIENT_GIVEN_NAMES_KEY),
    practicioner: sessionStorage.getItem(PRACTICIONER_KEY),
  });

export const StartCheckout = () => {
  const history = useHistory();
  const [ welcomeData ] = useState(getWelcomeData());
  const { appointmentId, patientId, practitionerId } = useParams<StartCheckoutParams>();
  const { loginAzureADPopup } = useAzureADAuth();

  const isWelcomeData = Object.values(welcomeData).every(item =>
    !!item);

  if (!isWelcomeData) {
    return <Redirect to={CollaborationViewRoutes.home} />;
  }
  const getStart = () => {
    loginAzureADPopup(() => {
      history.push(`/exam-room/schedule-follow-up/${appointmentId}/${patientId}/${practitionerId}/checkout`);
    });
  };

  return (
    <div className={styles.cvCheckoutContainerWrap}>
      <div className={styles.cvCheckoutContainer}>
        <div className={styles.cvCheckoutLeftCol}>
          <div className={styles.cvCheckoutLogoRow}>
            <img src={dulyLargeIcon} alt="duly large icon" />
          </div>
          {/* <div className={styles.cvCheckoutNextSteps}>Next Steps</div> */}
          <div className={styles.cvCheckoutContentRow}>
            <div className={styles.cvCheckoutContentTextBlock}>
              <div className={styles.cvCheckoutWelcomeMsg}>
                <span className={styles.cvCheckoutWelcomeMsgName}>
                  {welcomeData.patientGivenNames}
                  {welcomeData.patientGivenNames ? ',' : ''}
                </span>
                <br />
                <span className={styles.cvCheckoutWelcomeMsgText}>Let&apos;s review your</span>
                <br />
                <span className={styles.cvCheckoutWelcomeMsgText}>Next Steps</span>
              </div>
              <Button
                className={styles.cvCheckoutPageStartButton}
                variant="primary"
                onClick={getStart}
                data-automation="get-started-checkout"
              >
                Get Started
              </Button>
              <div className={styles.cvCheckoutDescription}>{StartCheckoutData.description}</div>
              <div className={styles.cvCheckoutUserDetails}>
                <div className={styles.cvCheckoutUserDetailsQR}>
                  <img src={StartCheckoutData.qrCode} alt="qr-code" />
                </div>
              </div>
            </div>
            <div className={styles.cvCheckoutPageContentLogoBlock}>
              <img src={checkoutIcon} alt="duly giant icon" />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
