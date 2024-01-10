import { CSSProperties } from 'react';

import styles from './vital-panel-general-advice.scss';

export type VitalPanelGeneralAdvicePropsType = {
  title: string;
  text: string;
  style: CSSProperties;
};

export const VitalPanelGeneralAdvice = ({
  title,
  text,
  style,
}: VitalPanelGeneralAdvicePropsType) =>
  (
    <div className={styles.vitalPanelGeneralAdviceContainer} style={style}>
      <h2
        className={styles.vitalPanelGeneralAdviceContainerTitle}
        data-test="general-advice-title"
      >
        {title}
      </h2>
      <p
        className={styles.vitalPanelGeneralAdviceContainerText}
        data-test="general-advice-text"
      >
        {text}
      </p>
    </div>
  );
