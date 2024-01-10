import { Location } from '@types';
import { formatAddressBottomLine } from '@utils';

import styles from './location-card.scss';

type LocationCardProps = {
  title: string;
  location: Location;
};

export const LocationCard = ({ title, location }: LocationCardProps) => {
  const location1 = location ? location.location.address.addressLine : '';
  const location2 = location ? formatAddressBottomLine({
    city: location.location.address.city,
    postalCode: location.location.address.zipCode,
    state: location.location.address.state,
  }) : '';
  const titleForId = title.toLowerCase().replaceAll(' ', '-');
  return(
    <>{location && (
      <div className={styles.LocationCardWrapper}>
        <div className={styles.LocationCardWrapperHeading}>
          {title}
        </div>
        <div className={styles.LocationCardWrapperData}>
          <div className={styles.LocationCardWrapperDataDetails} data-automation={`${titleForId}-card-saved-location`}>
            <div className={styles.LocationCardWrapperDataDetailsName} data-automation={`${titleForId}-card-saved-location-city`}>
              {location.name}
            </div>
            <div className={styles.LocationCardWrapperDataDetailsLocation} data-automation={`${titleForId}-card-saved-location-address`}>
              {location1}<br/>{location2}
            </div>
          </div>
          <div className={styles.LocationCardWrapperDataInfo} data-automation={`${titleForId}-card-saved-location-info`}>
            <div className={styles.LocationCardWrapperDataInfoPhone} data-automation={`${titleForId}-card-saved-location-contact-number`}>
              {location.location.phoneNumber}
            </div>
            {location.location.workingHours.length ? (
              <><div className={styles.LocationCardWrapperDataInfoText}>Working Hours</div><div className={styles.LocationCardWrapperDataInfoHours} data-automation={`${titleForId}-card-saved-location-working-hours`}>
                {location.location.workingHours}
              </div></>
            ) : <></>}
          </div>
        </div>
      </div> 
    )}</>
  );};
