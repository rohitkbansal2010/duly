import { useState, useCallback } from 'react';
import { Dropdown, DropdownButton, Modal } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { SITE_ID } from '@constants';
import { locationPickerIcon } from '@icons';
import { hideLocationPickerModal, setCurrentSite } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { Site } from '@types';
import { formatAddress, setLocalStorageItem } from '@utils';

import styles from './location-picker-modal.scss';

export const LocationPickerModal = () => {
  const { isLocationPickerModalShown, sites, currentSite } = useSelector(({ SITE }: RootState) =>
    SITE);

  const dispatch = useDispatch();

  const [ location, setLocation ] = useState<Site | null>(null);
  const site = currentSite || location;

  const handleSelect = useCallback((location: Site) => {
    setLocation(location);
    setLocalStorageItem(SITE_ID, location.id);
    dispatch(setCurrentSite(location));
    dispatch(hideLocationPickerModal());
  }, [ dispatch, setLocation ]);

  const handleClose = useCallback(() => {
    if (site) {
      dispatch(hideLocationPickerModal());
    }
  }, [ site, dispatch ]);

  const dropdownTitle = site?.address ? formatAddress(site.address) : 'Selection';

  return (
    <Modal
      animation={false}
      show={isLocationPickerModalShown}
      className={styles.locationPickerModal}
      size="lg"
      centered
      dialogClassName={styles.customWidthModal}
      backdropClassName={styles.customBackDropModal}
      onHide={handleClose}
    >
      <Modal.Header className={styles.locationPickerModalHeader}>
        <div className={styles.locationPickerModalHeaderIcon}>
          <img
            src={locationPickerIcon}
            alt="location picker icon"
          />
        </div>
        <h1
          className={styles.locationPickerModalHeaderTitle}
        >
          Please select your location
        </h1>
      </Modal.Header>
      <Modal.Body className={styles.locationPickerModalBody}>
        <DropdownButton
          variant="outline-secondary"
          title={dropdownTitle}
          className={styles.locationPickerModalDropdownButton}
        >
          {sites?.map((item: Site) =>
            (
              <Dropdown.Item key={item?.id}>
                <div
                  role="none"
                  data-testid="location-picker-modal__dropdown__dropdown-item"
                  className={styles.locationPickerModalDropdownItem}
                  onClick={() => { handleSelect(item); }}
                >
                  {!!item.address && formatAddress(item?.address)}
                </div>
              </Dropdown.Item>
            ))}
        </DropdownButton>
      </Modal.Body>
    </Modal>
  );
};
