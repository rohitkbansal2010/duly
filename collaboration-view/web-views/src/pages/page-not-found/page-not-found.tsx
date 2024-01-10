import { Row } from 'react-bootstrap';
import { NavLink } from 'react-router-dom';

import { CollaborationViewRoutes } from '@enums';
import { dulyLargeIcon, warningCircleMagentaIcon } from '@icons';

import styles from './page-not-found.scss';

// TODO: Refactor into a common design component with PageExceptions after DPGECLOF-1213,1215
export const PageNotFound = () =>
  (
    <div className={styles.wrapperPageNotFound}>
      <div className={styles.containerPageNotFound}>
        <Row>
          <div className={styles.containerPageNotFoundIconContainer}>
            <div
              className={styles.containerPageNotFoundIconContainerIconWrapper}
            >
              <img
                src={warningCircleMagentaIcon}
                alt="page not found icon"
              />
            </div>
          </div>
          <h2 className={styles.containerPageNotFoundTitle}>Page Not Found</h2>
          <p className={styles.containerPageNotFoundText}>
            The link you followed may be broken or the page <br/> may have been removed.
          </p>
          <div className={styles.containerPageNotFoundLinkContainer}>
            <NavLink
              to={CollaborationViewRoutes.home}
              className={styles.containerPageNotFoundLinkContainerLink}
            >
              Return to Calendar
            </NavLink>
          </div>
        </Row>
      </div>
      <div className={styles.wrapperPageNotFoundLogo}>
        <img src={dulyLargeIcon} alt="Logo" />
      </div>
    </div>
  );
