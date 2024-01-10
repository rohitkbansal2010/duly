import noop from 'lodash/noop';
import {
  useCallback, useEffect, useState, useRef
} from 'react';
import { useDetectClickOutside } from 'react-detect-click-outside';

import { CareTeamMembers } from '@components/care-team-members';
import { CheckoutNav } from '@components/checkout-nav';
import { useDetectTouchOutside } from '@hooks';

import styles from './checkout-menu.scss';

type AppointmentMenuProps = {
  onOpenStateChange: (isOpen: boolean) => void;
};

export const CheckoutMenu = ({ onOpenStateChange = noop }: AppointmentMenuProps) => {
  const [ isOpen, setIsOpen ] = useState(true);
  const setTimeoutRef: { current: NodeJS.Timeout | null | undefined } = useRef();

  const openMenu = useCallback(() => {
    if (!isOpen) {
      setIsOpen(true);
    }
  }, [ isOpen ]);

  const closeMenu = () => {
    console.log('close Menu function');
  };

  useEffect(() => {
    onOpenStateChange(isOpen);
  }, [ isOpen, onOpenStateChange ]);

  useEffect(() => {
    const timer = setTimeoutRef.current;

    if (timer) {
      return () =>
        clearTimeout(timer);
    }
  }, []);

  const outerClassName = []
    .concat(isOpen ? styles.appointmentMenuIsOpen : styles.appointmentMenuIsCollapsed)
    .concat(styles.appointmentMenuOuter)
    .join(' ');

  const ref = useDetectTouchOutside<HTMLDivElement>({
    onTriggered: closeMenu,
    ref: useDetectClickOutside({ onTriggered: closeMenu }),
  });

  return (
    <div className={outerClassName}>
      <div
        className={styles.appointmentMenuInner}
        tabIndex={-1}
        role="button"
        onKeyDown={noop}
        onClick={openMenu}
        ref={ref}
      >
        <CheckoutNav />
        <CareTeamMembers />
      </div>
    </div>
  );
};
