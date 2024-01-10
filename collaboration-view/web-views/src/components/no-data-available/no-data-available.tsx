import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';

import { FadedScroll } from '@components/faded-scroll-2';
import { CheckoutAppointmentsAction, getWeekDayTimeSlots } from '@redux/actions/cv-checkout-appointments';

import styles from './no-data-available.scss';

type NoDataAvailableProps = {
  height: number;
  isMessage: boolean;
  isMessageDetail: boolean;
  message?: string;
  icon: string;
  iconHeight: string;
  iconWidth: string;
  messageDetail?: string;
  isButton?:boolean;
  buttonText?:string;
  onClick?:() => CheckoutAppointmentsAction;
  contentLocation?:string;
  forSlot?:boolean;
  prescription?:boolean
  stepType?:string;
  selectedMeetingType?:string;
  selectedDate?:Date;
}
export const NoDataAvailable = ({
  height,
  message,
  iconHeight,
  iconWidth,
  icon,
  isMessage,
  isMessageDetail,
  messageDetail,
  isButton,
  buttonText,
  onClick,
  contentLocation,
  forSlot,
  prescription,
  stepType,
  selectedMeetingType,
  selectedDate,
}: NoDataAvailableProps) =>{
  const dispatch = useDispatch();
  const { appointmentId } = useParams<{ patientId: string, appointmentId: string }>();
  return (
    <FadedScroll height={`${height}rem`} className={styles.wrapper} >
      <div className={styles.wrappernoSlotContainer} style={{ justifyContent:contentLocation }} >
        <div className={styles.wrappernoSlotIconWrapper}>
          <img src={icon} alt="no medications" style={{ height: iconHeight, width: iconWidth }} />
        </div>
        <div
          className={styles.wrappernoSlotMessage}
        >
          {isMessage && !prescription ? 
            <div className={styles.wrappernoSlotMessageTitle}>{message}</div> :
            <div className={styles.wrappernoSlotMessageTitlePrescription}>{message}</div>
          }
          {isMessageDetail && (
          <div
            className=
              {styles.wrappernoSlotMessageDetail}
          >
            {messageDetail}</div>
        )}
          {isButton && (
          <button
            className=
              {styles.wrappernoSlotMessageActionButton}
            onClick={()=>{
              if(forSlot){
                dispatch(getWeekDayTimeSlots({
                  date: selectedDate!,
                  providerId:selectedMeetingType ? selectedMeetingType : '8001',
                  appointmentId: appointmentId,
                  stepType: stepType ? stepType : '',
                  meetingType: selectedMeetingType, 
                }));
              }else{
                onClick && onClick();
              }
            }}
          >
            {buttonText}</button>
        )}

        </div>
      </div>
    </FadedScroll>
  );
};
