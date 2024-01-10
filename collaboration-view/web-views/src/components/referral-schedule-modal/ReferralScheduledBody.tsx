import { Card } from 'react-bootstrap';

import { physcian, beri } from '@images';
import { firstLetterCapital } from '@utils';

import styles from './ReferralScheduleBody.scss';

const ModalData = [
  {
    Date: 'Fri, Mar 3, 2022 - 11:20 AM',
    Image: physcian,
    name: 'Dr. Codo',
    address: '1133 South Blvd',
    secondaddress: 'Oak Park, IL 60302',
    speciality: 'Endocrinologist',
    duration: '30 Minutes',
  },
  {
    Date: 'Fri, Mar 3, 2022 - 11:20 AM',
    Image: beri,
    name: 'Dr. Beri',
    address: '1133 South Blvd',
    secondaddress: 'Oak Park, IL 60302',
    speciality: 'Dermatologist',
    duration: '30 Minutes',
  },
];

export const ReferralScheduleBody = () => 
  (
    <>
      {ModalData.map((item, index) => 
        (
          <Card key={index} className={styles.card}>
            <Card.Text>
              <div className={styles.textData}>
                <div>{item.Date}</div>
                <div className={styles.providerImage}>
                  <img src={item.Image} alt="provider" />
                  <span className={styles.providerName}>{item.name}</span>
                </div>
              </div>
            </Card.Text>
            <Card.Text>
              <div className={styles.lowerBody}>
                <div className={styles.location}>
                  <div className={styles.heading}>LOCATION</div>
                  <div className={styles.address}>{firstLetterCapital(item.address)}</div>
                  <div className={styles.secondAddress}>
                    {firstLetterCapital(item.secondaddress)}
                  </div>
                </div>
                <div className={styles.referralVisit}>
                  <div className={styles.heading}>REFERRAL VISIT</div>
                  <div className={styles.speciality}>{item.speciality}</div>
                </div>
                <div className={styles.duration}>
                  <div className={styles.heading}>DURATION</div>
                  <div className={styles.time}>{item.duration}</div>
                </div>
              </div>
            </Card.Text>
          </Card>
        ))}
    </>
  );
