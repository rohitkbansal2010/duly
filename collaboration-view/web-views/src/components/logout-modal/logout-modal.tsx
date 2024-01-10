import { Modal } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import { Avatar } from '@components/avatar';
import { SideModal } from '@components/side-modal';
import { AvatarByRole } from '@enums';
import { useAzureADAuth, useLogoutModal } from '@hooks';
import { dulyLargeWhiteDarkBlueIcon, dulyLargeWhiteVioletIcon } from '@icons';
import { RootState } from '@redux/reducers';
import { getSrcAvatar, showFirstPrefixItem } from '@utils';

import styles from './logout-modal.scss';

export const LogoutModal = () => {
  const { logoutAzureADRedirect } = useAzureADAuth();

  const { isLogoutModalShown, onHideLogoutModal } = useLogoutModal();
  
  const userData = useSelector(({ USER }: RootState) =>
    USER.userData);
  const userRole = useSelector(({ USER }: RootState) =>
    USER.userRole);

  const isRegularEmployee = userRole === AvatarByRole.REGULAR_EMPLOYEE;
  const backgroundContentColor = isRegularEmployee ? 'Magenta' : 'Blue';
  const styleButton = isRegularEmployee ? 'Violet' : 'DarkBlue';

  return (
    <SideModal
      show={isLogoutModalShown}
      onHide={onHideLogoutModal}
      backgroundContentColor={backgroundContentColor}
    >
      <Modal.Header className={styles.logoutModalHeader}>
        <img
          src={isRegularEmployee ? dulyLargeWhiteVioletIcon : dulyLargeWhiteDarkBlueIcon}
          alt="Large duly icon"
        />
      </Modal.Header>
      <Modal.Body className={styles.logoutModalBody}>
        <div className={styles.logoutModalBodyPractitionerLogo}>
          <Avatar
            width={12.31}
            src={getSrcAvatar(userData?.photo)}
            alt={`${showFirstPrefixItem(userData?.humanName.givenNames)}${userData?.humanName.familyName}`.trim()}
            role={userRole as AvatarByRole}
            hasBorder
          />
        </div>
        <div className={styles.logoutModalBodyPractitionerHumanName} data-testid="human-name">
          {userData?.humanName.prefixes && userData?.humanName.prefixes.length
            ? `${showFirstPrefixItem(userData?.humanName.prefixes)}${userData?.humanName.familyName}`.trim()
            : userData?.humanName.givenNames.join(' ')
          }
        </div>
        <div className={styles.logoutModalBodyQuestion}>
          Are you sure you want to log out?
        </div>
        <div>
          <button
            onClick={() =>{
              sessionStorage.removeItem('metric');
              logoutAzureADRedirect();}}
            className={
              `${styles.logoutModalBodyButtonLogout} ${styles[`logoutModalBodyButton${styleButton}`]}`
            }
            data-testid="logout-button"
          >
            Logout
          </button>
          <button
            onClick={onHideLogoutModal}
            className={
              `${styles.logoutModalBodyButtonCancel} ${styles[`logoutModalBodyButton${styleButton}`]}`
            }
            data-testid="cancel-button"
          >
            Cancel
          </button>
        </div>
      </Modal.Body>
    </SideModal>
  );
};
