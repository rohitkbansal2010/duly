import { PropsWithChildren, useRef } from 'react';

import { useScrollFades } from '@hooks';

import styles from './faded-scroll.module.scss';

type Props = PropsWithChildren<{
    height: string;
    className?: string;
    view?: 'default' | 'bottom-white';
    oversize?: boolean
}>

export const FadedScroll = (props: Props) => {
  const {
    children, height, className, view = 'default', oversize = false,
  } = props;
  const containerRef = useRef<HTMLDivElement>(null);

  const { topFadeClassname, bottomFadeClassname } = useScrollFades(containerRef);

  const topScrollFade = ([] as string[])
    .concat(styles.topScrollFade)
    .concat(styles[topFadeClassname])
    .concat(styles[view])
    .join(' ');

  const bottomScrollFade = ([] as string[])
    .concat(styles.bottomScrollFade)
    .concat(styles[bottomFadeClassname])
    .concat(styles[view])
    .join(' ');

  const wrapperClasses = [ styles.scrollWrapper ];
  className && wrapperClasses.push(className);
  oversize && wrapperClasses.push(styles.oversize);

  return (
    <div className={wrapperClasses.join(' ')} style={{ height }}>
      <div className={topScrollFade} />
      <div ref={containerRef} className={styles.scrollContent}>
        {children}
      </div>
      <div className={bottomScrollFade} />
    </div>
  );
};
