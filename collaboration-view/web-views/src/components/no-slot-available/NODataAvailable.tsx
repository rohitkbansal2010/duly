import { FadedScroll } from '@components/faded-scroll-2';

import styles from './no-data-available.scss';

type NODataAvailableProps = {
  height: number;
  isMessage: boolean;
  isMessageDetail: boolean;
  message?: string;
  icon: string;
  iconHeight: string;
  iconWidth: string;
  messageDetail?: string;

}
const NODataAvailable = ({
  height, message, iconHeight, iconWidth, icon, isMessage, isMessageDetail, messageDetail,
}: NODataAvailableProps) =>
  (
    <FadedScroll height={`${height}rem`} className={styles.wrapper} >
      <div className={styles.wrappernoSlotContainer} >
        <div className={styles.wrappernoSlotIconWrapper}>
          <img src={icon} alt="no medications" style={{ height: iconHeight, width: iconWidth }} />
        </div>
        <div className={styles.wrappernoSlotMessage}>
          {isMessage && <div className={styles.wrappernoSlotMessageTitle}>{message}</div>}
          {isMessageDetail && (
          <div className=
            {styles.wrappernoSlotMessageDetail}
          >
            {messageDetail}</div>
        )}
        </div>
      </div>
    </FadedScroll>
  );

export default NODataAvailable;
