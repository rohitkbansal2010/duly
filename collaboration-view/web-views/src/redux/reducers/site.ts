import { SiteStateAction, SiteStateType } from '@redux/actions';
import { Site } from '@types';

type SiteState = {
  isLocationPickerModalShown: boolean;
  sites: Site[],
  currentSite: Site | null;
};

const siteState: SiteState = {
  isLocationPickerModalShown: false,
  sites: [],
  currentSite: null,
};

export const siteReducer = (
  state = siteState,
  action: SiteStateAction
) => {
  switch (action.type) {
    case SiteStateType.SHOW_LOCATION_PICKER_MODAL:
      return {
        ...state,
        isLocationPickerModalShown: true,
      };
    case SiteStateType.HIDE_LOCATION_PICKER_MODAL:
      return {
        ...state,
        isLocationPickerModalShown: false,
      };

    case SiteStateType.SET_SITES:
      return {
        ...state,
        sites: action.payload.sites,
      };
    case SiteStateType.SET_CURRENT_SITE:
      return {
        ...state,
        currentSite: action.payload.currentSite,
      };
    default:
      return state;
  }
};
