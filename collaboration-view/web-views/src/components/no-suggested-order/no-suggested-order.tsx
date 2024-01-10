import styles from './no-suggested-order.scss';

type NoSuggestedOrderProps = {
    icon: string;
    title: string;
}

export const NoSuggestedOrder = ({ icon, title }:NoSuggestedOrderProps) => 
  (
    <div className={styles.noOrderWrapper}>
      <div className={styles.noOrderIconWrapper}>
        <img src={icon} alt="screen-icon" />
      </div>
      <div className={styles.noOrderTitle}>{title}</div>
    </div>
  );
