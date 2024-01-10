import { ReactNode } from 'react';

import styles from './widgets-title.scss';

type WidgetTitleProps = {
	children?: ReactNode;
};

export const WidgetTitle = ({ children }: WidgetTitleProps): JSX.Element =>
  (
    <h2 className={styles.cvWidgetTitle}>{children}</h2>
  );
