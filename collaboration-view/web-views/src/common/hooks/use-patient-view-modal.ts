import { useDispatch, useSelector } from 'react-redux';

import { hidePatientViewModal, showPatientViewModal } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';

export const usePatientViewModal = () => {
  const dispatch: AppDispatch = useDispatch();
  const isShowPatientViewModal = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.isShowPatientViewModal);

  const onShowPatientViewModal = () =>
    dispatch(showPatientViewModal());

  const onHidePatientViewModal = () =>
    dispatch(hidePatientViewModal());

  return {
    isShowPatientViewModal,
    onShowPatientViewModal,
    onHidePatientViewModal,
  };
};
