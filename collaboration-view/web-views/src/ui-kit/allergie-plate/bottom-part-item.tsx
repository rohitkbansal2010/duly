import { PropsWithChildren, ReactNode } from 'react';

import styles from './allergie-plate.module.scss';

type BottomPartItemProps = PropsWithChildren<{ title: ReactNode }>;

export const BottomPartItem = ({ title, children }: BottomPartItemProps) => 
  (
    <div className={styles.allergiePlateBottomPartItem}>
      <div className={styles.allergiePlateBottomPartLabel}>{title}</div>
      <div className={styles.allergiePlateBottomPartValue}>{children}</div>
    </div>
  );
