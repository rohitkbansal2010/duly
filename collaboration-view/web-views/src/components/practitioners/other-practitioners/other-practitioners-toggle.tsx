import {
  forwardRef,
  LegacyRef,
  PropsWithChildren
} from 'react';

import { usersWhiteIcon } from '@icons';

import styles from './other-practitioners.scss';

type OtherPractitionersToggleProps = PropsWithChildren<{
  onClick: () => void;
}>

export const OtherPractitionersToggle =
  forwardRef((
    { children, onClick }: OtherPractitionersToggleProps,
    ref: LegacyRef<HTMLButtonElement>
  ) =>
    (
      <button
        ref={ref}
        onClick={onClick}
        className={styles.otherPractitionersButton}
      >
        <div className={styles.otherPractitionersIcon}>
          <img
            src={usersWhiteIcon}
            alt="other practitioners"
          />
        </div>
        <div className={styles.otherPractitionersText}>
          +{children}
        </div>
      </button>
    ));

OtherPractitionersToggle.displayName = 'OtherPractitionersToggle';
