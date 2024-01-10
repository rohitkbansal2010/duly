import { useDispatch, useSelector } from 'react-redux';

import { hideLogoutModal, showLogoutModal } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';

export const useLogoutModal = () => {
  const dispatch: AppDispatch = useDispatch();
  const isLogoutModalShown = useSelector(({ USER }: RootState) =>
    USER.isLogoutModalShown);

  const onShowLogoutModal = () =>
    dispatch(showLogoutModal());

  const onHideLogoutModal = () =>
    dispatch(hideLogoutModal());

  return {
    isLogoutModalShown,
    onShowLogoutModal,
    onHideLogoutModal,
  };
};
