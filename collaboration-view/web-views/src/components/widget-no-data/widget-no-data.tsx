import styles from './widget-no-data.scss';

type Props = {
  icon: JSX.Element,
  title: string,
  view: 'blue' | 'magenta',
  align?: 'center' | 'top'
}

export const WidgetNoData = (props: Props) =>
{
  const {
    icon, title, align = 'center', view = 'blue',
  } = props;
  return (
    <div className={`${styles.widgetNoDataContainer} ${styles[view]}`} style={{ height: '100%' }}>
      <div className={styles.icon}>
        {icon}
      </div>
      <div className={styles.title}>
        {title}
      </div>
      {align === 'top' && <div className={styles.pusher}/>}
    </div>
  );
};
