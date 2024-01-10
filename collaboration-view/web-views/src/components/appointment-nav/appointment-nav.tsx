import { noop } from 'lodash';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { useParams, useHistory } from 'react-router-dom';

import { appointmentModules } from '@constants';
import { usePatientViewModal } from '@hooks';
import { AfterVisitLinkIcon } from '@icons';
import { RootState } from '@redux/reducers';
import { AppointmentModuleType } from '@types';
import { NavPrimary } from '@ui-kit/nav-primary';

import styles from './appointment-nav.scss';

type AppointmentNavProps = {
  onClick: (route?: string) => void;
}

export const AppointmentNav = ({ onClick }: AppointmentNavProps) => {
  const history = useHistory();
  const {
    appointmentId,
    modules,
    patientId,
    practitionerId,
    isCareTeamShown,
    isModuleMenuShown,
  } = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT);
  const { appointmentModuleRoute } = useParams<{ appointmentModuleRoute: string }>();
  const [ wrapperClass, setWrapperClass ] = useState<string>(styles.appointmentNavWrapper);
  const [ contentClass, setContentClass ] = useState<string>(styles.appointmentNav);

  const getIsActiveModuleRoute = (moduleRoute?: string): boolean =>
    `/${appointmentModuleRoute}` === moduleRoute;

  useEffect(() => {
    if (modules?.length && isCareTeamShown && isModuleMenuShown) {
      setWrapperClass(initClass =>
        initClass.concat(` ${styles.appointmentNavWrapperAnimation}`));

      const timer = setTimeout(() => {
        setContentClass(initClass =>
          initClass.concat(` ${styles.appointmentNavVisible}`));
      }, 600);

      return () =>
        clearTimeout(timer);
    }
  }, [ modules, isCareTeamShown, isModuleMenuShown ]);

  const { onShowPatientViewModal } = usePatientViewModal();

  const gotoAfterSteps = () => {
    history.push(`/exam-room/schedule-follow-up/${appointmentId}/${patientId}/${practitionerId}/checkout`);
  };

  return (
    <div>
      <div className={wrapperClass}>
        <div className={contentClass}>
          {modules.map(({ alias }) => {
            const {
              id, icon, route, title, externalLink,
            } = appointmentModules.find(m =>
              m.alias === alias) as AppointmentModuleType;
            if (alias !== 'ModuleEducation' && alias !== 'ModuleCarePlan')
              return (
                <>
                  <NavPrimary
                    key={id}
                    appointmentId={appointmentId}
                    patientId={patientId}
                    practitionerId={practitionerId}
                    icon={icon}
                    route={route}
                    title={title}
                    isActive={getIsActiveModuleRoute(route)}
                    externalLink={externalLink}
                    onClick={onClick}
                  />

                </>
              );
          })}
          {
            <div
              role="none"
              className={styles.navPrimary}
            >
              <div
                className={styles[`navPrimaryLink${getIsActiveModuleRoute('/checkout') ? 'Active' : ''}`]}
                onClick={localStorage.getItem('fromAfterVisit') === '1' ?
                  gotoAfterSteps : onShowPatientViewModal
                }
                tabIndex={-1}
                role="button"
                onKeyDown={noop}
              >
                <div className={styles[`navPrimaryLink${getIsActiveModuleRoute('/checkout') ? 'Active' : ''}IconWrapper`]}>
                  <img
                    src={AfterVisitLinkIcon}
                    alt={`after visit`}
                  />
                </div>
                After visit
              </div>
            </div>
          }
        </div>
      </div>
    </div >
  );
};
