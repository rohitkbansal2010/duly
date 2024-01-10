import { CheckoutHeaderData } from '@components/checkout-header-data';

import { HeaderPropType } from '../checkout-page';

import styles from './header.scss';

export const Header = ({ data, setCurrentStep }: HeaderPropType) =>
  (
    <header className={styles.cvCheckoutPageHeaderWrapper}>
      <CheckoutHeaderData data={data} setCurrentStep={setCurrentStep} />
    </header>
  );
