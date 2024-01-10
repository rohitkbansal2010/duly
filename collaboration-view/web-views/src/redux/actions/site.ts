import { SiteStateType, SiteStateAction } from '@redux/actions';
import { Site } from '@types';

export const showLocationPickerModal = (): SiteStateAction =>
  ({ type: SiteStateType.SHOW_LOCATION_PICKER_MODAL });

export const hideLocationPickerModal = (): SiteStateAction =>
  ({ type: SiteStateType.HIDE_LOCATION_PICKER_MODAL });

export const getSites = (): SiteStateAction =>
  ({ type: SiteStateType.GET_SITES });

export const setSites = (sites: Site[]): SiteStateAction =>
  ({ type: SiteStateType.SET_SITES, payload: { sites } });

export const getCurrentSite = (siteId: string): SiteStateAction =>
  ({ type: SiteStateType.GET_CURRENT_SITE, payload: { siteId } });

export const setCurrentSite = (site: Site): SiteStateAction =>
  ({ type: SiteStateType.SET_CURRENT_SITE, payload: { currentSite: site } });
