import { CheckoutMenu } from '@components/checkout-menu';
import { dulyLargeIcon } from '@icons';

import styles from './footer.scss';

export const Footer = () => {
  const onStateChange = () => {
    console.log('State Changed');
  };

  return (
    <footer className={styles.cvAppointmentPageFooterWrapper}>
      <button
        className={`${styles.refreshButton} ${styles.refreshButtonActive}`}
        onClick={()=>
window.location.reload()}
      >
        <img src={dulyLargeIcon} alt="Logo" />
      </button>
      <div className={styles.navPrimaryWrapper}>
        <CheckoutMenu onOpenStateChange={onStateChange} />
      </div>
    </footer>
  );
};
