import { noop } from 'lodash';
import { useEffect, useState } from 'react';

import { NoDataAvailable } from '@components/no-data-available/no-data-available';
import { noProvider } from '@icons';
import { Location } from '@types';
import { formatAddressBottomLine, getMilesForLocation } from '@utils';

import { LocationsContainerSkeleton } from './location-skeleton';
import styles from './locations-list.scss';

type LocationsListProps = {
  heading: string;
  subHeading: string;
  locations: Location[];
  activeLocation:Location;
  setActiveLocation:React.Dispatch<React.SetStateAction<Location>>;
  reset: boolean;  
  savedLocation?:Location;
  parentName:string;
};

export const LocationsList = ({
  heading, 
  subHeading, 
  locations, 
  activeLocation, 
  setActiveLocation, 
  reset, 
  parentName,
  savedLocation,
}: LocationsListProps) => {
  const errorText = 'No location available';
  locations.sort((a, b) => 
    a.location.distance - b.location.distance);

  const sortedLocations = locations;
    
  const [ shownLocations, setShownLocations ] = useState(
    sortedLocations.slice(
      0, 
      sortedLocations.length > 3 
        ? 3 
        : sortedLocations.length
    )
  );

  useEffect(() => {
    if(savedLocation && savedLocation?.name.length){
      setActiveLocation(savedLocation);
    }else{
      setActiveLocation(sortedLocations[0]);}
  }, [ savedLocation, sortedLocations ]);

  const [ endIndex, setEndIndex ] = useState(3);
  const [ isLastIndex, setIsLastIndex ] = useState(false);

  const moreLocationsTextClassName = isLastIndex ? 
    `${styles.LocationsListMoreLocationText} ${styles.LocationsListMoreLocationDisabledText}` : 
    `${styles.LocationsListMoreLocationText}`;
  
  const moreLocationsClassName = isLastIndex ? 
    `${styles.LocationsListMoreLocation} ${styles.LocationsListMoreLocationDisabled}` : 
    `${styles.LocationsListMoreLocation}`;

  const handleMoreLocations = () =>{
    setShownLocations(sortedLocations.slice(0, Math.min(endIndex + 3, sortedLocations.length)));
    setEndIndex(Math.min(endIndex + 3, sortedLocations.length));
    if(endIndex + 3 >= sortedLocations.length){
      setIsLastIndex(true);
    }
  };

  const moreLocationsClickHandler = () => {
    if(isLastIndex){
      return noop;
    }
    else return handleMoreLocations;
  };

  useEffect(()=>{
    if(!reset){
      setActiveLocation(sortedLocations[0]);
      setShownLocations(sortedLocations.slice(0, Math.min(3, sortedLocations.length)));
      setEndIndex(Math.min(3, sortedLocations.length));
      setIsLastIndex(false);
    }
  }, [ reset ]);

  useEffect(() => {
    const scrollableElement = document.getElementById(`location-list-scroll-${parentName}`);
    scrollableElement && scrollableElement.scroll({ top: scrollableElement.scrollHeight, behavior: 'smooth' });
  }, [ shownLocations ]);

  const parentNameId = parentName.toLowerCase().replaceAll(' ', '-');
  const afterActiveLocation = () => 
    (
      activeLocation ? (
        <div className={styles.LocationsList}>
          <div className={styles.LocationsListHeading}>{heading}</div>
          <div className={styles.LocationsListSubheading}>{subHeading}</div>
          <div id={`location-list-scroll-${parentName}`} className={styles.LocationsListRadio}>
            {
        shownLocations.map((loc, index) => 
        (
          <div key={index}>
            <label className={styles.LocationsListRadioItem}>
              <input
                name="location"
                type="radio"
                data-automation={activeLocation === loc ? `selected-location-list-${index}-${parentNameId}` : `location-list-${index}-${parentNameId}`}
                className={
                  activeLocation &&
                  (loc.location.address.addressLine === activeLocation.location.address.addressLine 
            ? `${styles.LocationsListRadioItemInput} ${styles.LocationsListRadioItemInputSelected}`
            : styles.LocationsListRadioItemInput)}
                onClick={()=>{
          setActiveLocation(loc);         
                }}
              />
              <div className={loc.location.address.addressLine === 
              activeLocation.location.address.addressLine 
              ? `${styles.LocationsListRadioItemLabel} ${styles.LocationsListRadioItemLabelSelected}`
              : styles.LocationsListRadioItemLabel}
              >

                <div className={styles.LocationsListRadioItemLabelLocation}>
                  <div className={styles.LocationsListRadioItemLabelLocationName}>
                    {loc.location.address.city}
                  </div>
                  <div className={styles.LocationsListRadioItemLabelLocationLoc}>
                    {loc.location.address.addressLine}, 
                    {formatAddressBottomLine({
                      city: loc.location.address.city,
                      postalCode: loc.location.address.zipCode,
                      state: loc.location.address.state,
                    })}
                  </div>
                </div>
                <div className={styles.LocationsListRadioItemLabelDistance}>
                  {getMilesForLocation(loc.location.distance)} miles from you
                </div>
              </div>
            </label>
          </div>
        ))}
          </div>
          {sortedLocations.length > 3 && (
          <div
            className={moreLocationsClassName} 
            onClick={moreLocationsClickHandler()}
            onKeyDown={noop}
            role="button"
            tabIndex={-1}
            data-automation={`locations-list-view-more-${parentNameId}`}
          >
            <span className={moreLocationsTextClassName}>View More Locations</span>&nbsp;
            <span className={styles.LocationsListMoreLocationIcon}><i className="bi bi-chevron-down" /></span>
          </div>
    )}
        </div>
      ) : (
        <NoDataAvailable
          height={16}
          isMessage ={true}
          isMessageDetail={false}
          message={errorText}
          icon={noProvider}
          iconHeight="5.25rem"
          iconWidth="5.25rem"
        />
      )
    );

  return(
    <> {
      locations ? 
        afterActiveLocation() : 
        <LocationsContainerSkeleton />
    }</>
  );
};
