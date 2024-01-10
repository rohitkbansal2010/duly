import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { LocationPickerModal } from '@components/location-picker-modal';
import { LogoutExpirationModal } from '@components/logout-expiration-modal';
import { LogoutModal } from '@components/logout-modal';
import { useSiteId } from '@hooks';
import {
  getCurrentSite,
  getSites,
  getTodaysAppointmentsSaga
} from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';

import { Content } from './content';
import styles from './cv-home-page.scss';
import { Sidebar } from './sidebar';

export const CvHomePage = () => {
  const currentSite = useSelector(({ SITE }: RootState) =>
    SITE.currentSite);
  const dispatch: AppDispatch = useDispatch();
  const { siteId } = useSiteId();

  useEffect(() => {
    dispatch(getSites());
  }, [ dispatch ]);

  useEffect(() => {
    if (currentSite) {
      dispatch(getTodaysAppointmentsSaga());
    }
  }, [ dispatch, currentSite ]);

  useEffect(() => {
    if (!currentSite && siteId) {
      dispatch(getCurrentSite(siteId));
    }
  }, [ currentSite, dispatch, siteId ]);

  return (
    <div className={styles.cvWrapper}>
      <Sidebar />
      <Content />
      <LocationPickerModal/>
      <LogoutModal />
      <LogoutExpirationModal />
    </div>
  );
};
