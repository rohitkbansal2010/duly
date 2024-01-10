import { useEffect } from 'react';
import { useDispatch } from 'react-redux';

import { SITE_ID } from '@constants';
import { showLocationPickerModal } from '@redux/actions';
import { getLocalStorageItem } from '@utils';

export const useSiteId = () => {
  const dispatch = useDispatch();
  const siteId = getLocalStorageItem(SITE_ID);

  useEffect(() => {
    if (!siteId) {
      dispatch(showLocationPickerModal());
    }
  }, [ dispatch, siteId ]);
  
  return { siteId };
};
