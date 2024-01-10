import { noop } from 'lodash';
import { useCallback } from 'react';
import { Link } from 'react-router-dom';

import { CollaborationViewRoutes } from '@enums';

import styles from './nav-primary.scss';

type NavPrimaryProps = {
  appointmentId: string;
  patientId: string;
  practitionerId: string;
  title: string;
  route?: string;
  icon?: string;
  isActive?: boolean;
  externalLink?: React.ComponentType;
  onClick: (route?: string) => void;
}

export const NavPrimary = ({
  appointmentId,
  patientId,
  practitionerId,
  route,
  icon,
  title,
  isActive = false,
  externalLink,
  onClick,
}: NavPrimaryProps) => {
  const active = isActive ? 'Active' : '';

  const handleClick = useCallback(() => {
    onClick(route);
  }, [ route, onClick ]);
  
  return (
    <div
      role="none"
      className={styles.navPrimary}
      onClick={active ? noop : handleClick}
      data-automation={`${title.toLowerCase().replaceAll(' ', '-')}-nav-primary`}
    >
      <Link
        className={styles[`navPrimaryLink${active}`]}
        to={`${CollaborationViewRoutes.appointment}/${appointmentId}/${patientId}/${practitionerId}${route}`}
        component={externalLink}
      >
        <div className={styles[`navPrimaryLink${active}IconWrapper`]}>
          <img
            src={icon}
            alt={`appointment ${title} icon`}
          />
        </div>
        {title}
      </Link>
    </div>
  );
};
