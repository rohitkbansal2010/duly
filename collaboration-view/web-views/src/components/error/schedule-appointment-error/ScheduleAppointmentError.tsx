import { Modal } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { ErrorData } from '@constants';
import { ErrorDataType, refreshDataAction } from '@redux/actions';
import { getWeekDayTimeSlots } from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';

import styles from './ScheduleAppointmentError.scss';



type ScheduleAppointmentErrorProps ={
  modalErrorData:ErrorDataType;
}
const ScheduleAppointmentError = (
  { modalErrorData }: ScheduleAppointmentErrorProps
) => {
  const dispatch = useDispatch();
  const { refreshLocation } = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.refreshData);
  const { errorMessage } = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.ScheduleAppointmentErrorData);

  const handleBadNetworkException = ()=>{
    dispatch(modalErrorData.onAction());
    if((refreshLocation === 'saveSchedule' || errorMessage === ErrorData.sameSlotBooked.errorMessage) && modalErrorData.refreshSlots){
      dispatch(refreshDataAction({
        isRefreshRequired:false,
        refreshLocation:'', 
      }));
      if (modalErrorData.refreshSlots.stepType === 'F')
        dispatch(getWeekDayTimeSlots({
          date: modalErrorData.refreshSlots.date,
          providerId: modalErrorData.refreshSlots.providerId,
          meetingType: modalErrorData.refreshSlots.meetingType,
          appointmentId: modalErrorData.refreshSlots.appointmentId,
          stepType: 'F',
        }));
      else if (modalErrorData.refreshSlots.stepType === 'R')
        dispatch(getWeekDayTimeSlots({
          date: modalErrorData.refreshSlots.date,
          providerId: modalErrorData.refreshSlots.providerId,
          meetingType: '',
          appointmentId: modalErrorData.refreshSlots.appointmentId,
          stepType: 'R',
          departmentId: modalErrorData.refreshSlots.departmentId,
        }));
      else
        dispatch(getWeekDayTimeSlots({
          date: modalErrorData.refreshSlots.date,
          providerId: modalErrorData.refreshSlots.providerId,
          meetingType: '',
          appointmentId: modalErrorData.refreshSlots.appointmentId,
          stepType: 'I',
        })); }
    if(refreshLocation === 'timeSlots'){
      dispatch(refreshDataAction({
        isRefreshRequired:false,
        refreshLocation:'', 
      })); 
    }
  };
  return (
    
    <Modal
      show={modalErrorData.isScheduleAppointmentErrorShown}
      centered
      onHide={() => 
        handleBadNetworkException()
      }
      dialogClassName={styles.modalParentDiv}
    >
      <Modal.Header closeButton className={styles.modalHeader}>
        <Modal.Title className={styles.heading}>
          {modalErrorData.errorHeader}
        </Modal.Title>
      </Modal.Header>
      <Modal.Body className={styles.modalBody}>
        <div className={styles.parentContainer}>
          <div className={styles.error}>
            <img src={modalErrorData.icon} alt="error" className={styles.errorIcon}/>
            <div className={styles.errorData}>
              <p className={styles.errorDataHeading}>{modalErrorData.errorTitle}</p>
              <p
                className={styles.errorDataMessage} 
                dangerouslySetInnerHTML={{ __html: modalErrorData.errorMessage }}
              />
            </div>      
          
            <button
              className={styles.modalButton}
              data-automation="error-button"
              onClick={() => {
                handleBadNetworkException();
              }}
            >
              {modalErrorData.errorButtonText}
            </button>
          </div>
        </div>
      </Modal.Body>
    </Modal>
  );
};

export default ScheduleAppointmentError;
