import { noop } from 'lodash';

import { HeaderPropType } from '@pages/collaboration-view/cv-checkout-page/checkout-page';

import styles from './checkout-header-data.scss';

export const CheckoutHeaderData = ({ data, setCurrentStep }: HeaderPropType) =>
  (
    <>
      <ul className={styles.wrapper}>
        {data.length > 0 &&
        data.map((item, key) =>
        (
          <li
            className={styles.listCard}
            key={key}
          >
            <span
              className={styles.listCardIcon}
              onClick={() =>
                setCurrentStep({ step: item.value })}
              onKeyDown={noop}
              role="button"
              tabIndex={-1}
            >
              <img src={item.img} className={styles.listCardIconImg} alt="" />
            </span>
            <div className={styles.listCardTextDiv}>
              <span
                className={styles.listCardTextDivPreTitle}
                onClick={() =>
                  setCurrentStep({ step: item.value })}
                onKeyDown={noop}
                role="button"
                tabIndex={-1}
              > {item.preTitle} </span>
              <span
                className={styles.listCardTextDivTitle}
                onClick={() =>
                  setCurrentStep({ step: item.value })}
                onKeyDown={noop}
                role="button"
                tabIndex={-1}
              > {item.title} </span>
              <span
                className={
                  item.postTitle === 'View Details'
                    ? styles.listCardTextDivPostTitleLink
                    : styles.listCardTextDivPostTitle
                }
                onClick={() =>
                  item.postTitle === 'View Details'
                    ? setCurrentStep({ step: item.value, section: 'View Details' })
                    : setCurrentStep({ step: item.value })}
                onKeyDown={noop}
                role="button"
                tabIndex={-1}
              >

                {' '}
                {item.postTitle}{' '}
              </span>
            </div>
          </li>
        ))}
      </ul>
    </>
  );
