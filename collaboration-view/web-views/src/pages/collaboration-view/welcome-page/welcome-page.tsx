import { useState } from 'react';
import { Button } from 'react-bootstrap';
import { Redirect, useHistory, useParams } from 'react-router-dom';

import { PATIENT_GIVEN_NAMES_KEY, PRACTICIONER_KEY, WELCOME_SUFFIX_KEY } from '@constants';
import { CollaborationViewRoutes } from '@enums';
import { useAzureADAuth } from '@hooks';
import { dulyGiantIcon, dulyLargeIcon } from '@icons';
import { NavCalendar } from '@ui-kit/nav-calendar/nav-calendar';

import styles from './welcome-page.scss';

type WelcomePageParams = {
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

export const WelcomePage = () => {
  const history = useHistory();

  const { appointmentId, patientId, practitionerId } = useParams<WelcomePageParams>();
  const [ welcomeData ] = useState(getWelcomeData());
  const { loginAzureADPopup } = useAzureADAuth();

  const onStartAppointment = () => 
    loginAzureADPopup(() => {
      history.push(CollaborationViewRoutes.startAppointment(
        ...[ appointmentId, patientId, practitionerId ]
      ));
    });

  const isWelcomeData = Object.values(welcomeData).every(item =>
    !!item);

  if (!isWelcomeData) {
    return (
      <Redirect to={CollaborationViewRoutes.home} />
    );
  }

  return (
    <div className={styles.cvWelcomePageContainerWrap}>
      <div className={styles.cvWelcomePageContainer}>
        <NavCalendar />
        <div className={styles.cvWelcomePageLeftCol}>
          <div className={styles.cvWelcomePageLogoRow}>
            <img src={dulyLargeIcon} alt="duly large icon" />
          </div>
          <div className={styles.cvWelcomePageContentRow}>
            <div className={styles.cvWelcomePageContentTextBlock}>
              <div className={styles.cvWelcomePageWelcomeMsg}>
                <span className={styles.cvWelcomePageWelcomeMsgText}>
                  Welcome{welcomeData.welcomeSuffix ? ` ${welcomeData.welcomeSuffix},` : ','}
                </span>
                <br />
                <span className={styles.cvWelcomePageWelcomeMsgName}>
                  {welcomeData.patientGivenNames}!
                </span>
              </div>
              <div className={styles.cvWelcomePageWelcomeCongrats}>
                We’re excited to see your progress!
              </div>
              <section className={styles.cvWelcomePageWelcomeDoctorStatus}>
                <span className={styles.cvWelcomePageWelcomeDoctorStatusHighlight}>
                  {welcomeData.practicioner}
                </span>
                <span>{' is finishing up charting and will be with you '}</span>
                <span className={styles.cvWelcomePageWelcomeDoctorStatusHighlight}>shortly.</span>
              </section>
              <Button
                onClick={onStartAppointment}
                className={styles.cvWelcomePageStartButton}
                variant="primary"
              >
                Start Appointment
              </Button>
            </div>
            <div className={styles.cvWelcomePageContentLogoBlock}>
              <img src={dulyGiantIcon} alt="duly giant icon" />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
