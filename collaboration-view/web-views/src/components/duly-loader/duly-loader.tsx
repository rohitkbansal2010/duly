import Lottie from 'react-lottie';

import * as animationData from './duly-loader.json';
import styles from './duly-loader.scss';

const defaultOptions = {
  loop: true,
  autoplay: true,
  animationData,
  rendererSettings: { preserveAspectRatio: 'xMidYMid' },
};

type DulyLoaderProps = {
  width: number;
}

export const DulyLoader = ({ width }: DulyLoaderProps) =>
  (
    <div
      className={styles.dulyLoaderWrapper}
      style={{ width: `${width}rem` }}
    >
      <Lottie
        options={defaultOptions}
        isClickToPauseDisabled
      />
    </div>
  );
