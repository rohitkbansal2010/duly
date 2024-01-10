import { Avatar } from '@components/avatar';
import { ACCORDIAN_CARD_WIDTH } from '@constants';
import { firstLetterCapital, formatDateStringAddComma } from '@utils';

import styles from './follow-up-appointment.scss';

type FollowUpAppointmentProps = {
  date: string;
  time: string;
  location: string;
  pincode?: string;
  appointmentType?: string;
  meetingType?: string;
  cardType: string;
  duration?: string;
  providerName?: string;
  providerImg?: string;
  pendingScheduleHandler?: (id: number) => void;
  pending?: boolean;
  cardIndex?: number;
};

export const FollowUpAppointment = (props: FollowUpAppointmentProps) => {
  const handlePendingScheduler = () => {
    props.pendingScheduleHandler 
    && props.pendingScheduleHandler(
      props.cardIndex !== undefined ? props.cardIndex : 0
    );
  };
  const handlePendingHeader = () => 
    (
      <div className={styles.appointmentContainerHead}>
        {
    props.pending ? 
      (
        <div className={styles.notifyText}>
          Pending
        </div>
      ) : (
        <>
          <div>
            {formatDateStringAddComma(new Date(props.date).toDateString())} - {props.time}
          </div>
          {
            props.providerName && (
            <div className={styles.appointmentContainerHeadProvider}>
              <Avatar
                src={props.providerImg}
                alt={props.providerName}
                width={ACCORDIAN_CARD_WIDTH}
              />
              <p className={styles.appointmentContainerHeadProviderTitle}>
                {`Dr. ${props.providerName}`}
              </p>
            </div>
            )
          }
        </>
      )}
      </div>
    );
  const handleReferralCardType = () => 
    (
      <>
        <div className={styles.appointmentContainerBottomLocation}>
          <div className={styles.appointmentContainerBottomLocationTitle}>Referral visit</div>
          <div className={styles.appointmentContainerBottomLocationAddress}>
            {props.meetingType}
          </div>
        </div>
        {props.pending ? (
          <button 
            className={styles.buttonPrimary}
            onClick={ () => 
                  props.pendingScheduleHandler 
                  && props.pendingScheduleHandler(
                    props.cardIndex !== undefined ? props.cardIndex : 0
            ) }
          >Schedule</button>
        ) : (
          <div className={styles.appointmentContainerBottomLocation}>
            <div className={styles.appointmentContainerBottomLocationTitle}>Duration</div>
            <div className={styles.appointmentContainerBottomLocationAddress}>
              {props.duration}
            </div>
          </div>
        )}
      </>
    );
  const handleScheduleCardType = () => 
    (
      props.cardType == 'schedule' && (
        props.pending ? (
          <button 
            className={styles.buttonPrimary}
            onClick={ () => 
                handlePendingScheduler() }
          >Schedule</button>
        ) : (
          <div className={styles.appointmentContainerBottomLocation}>
            <div className={styles.appointmentContainerBottomLocationTitle}>APPOINTMENT TYPE</div>
            <div className={styles.appointmentContainerBottomLocationAddress}>
              {props.appointmentType}
            </div>
            <div className={styles.appointmentContainerBottomLocationPincode}>
              {props.meetingType}
            </div>
          </div>
        )
      )
    );
  return(
    <div className={styles.appointmentContainer}>
      {handlePendingHeader()}
      <div className={props.cardType == 'imaging' ? styles.appointmentContainerImaging : styles.appointmentContainerBottom}>
        {
          !(props.pending && (props.cardType == 'referral' || props.cardType == 'imaging')) && (
            <div className={styles.appointmentContainerBottomLocation}>
              <div className={styles.appointmentContainerBottomLocationTitle}>LOCATION</div>
              <div className={styles.appointmentContainerBottomLocationAddress}>
                {firstLetterCapital(props.location)}
              </div>
              <div className={styles.appointmentContainerBottomLocationPincode}>
                {props.pincode && firstLetterCapital(props.pincode)}
              </div>
            </div>
          )
        }
        {handleScheduleCardType()}
        {
        props.cardType == 'imaging' && (
          <>
            <div className={styles.appointmentContainerImagingLocation}>
              <div className={styles.appointmentContainerImagingLocationTitle}>
                APPOINTMENT TYPE
              </div>
              <div className={styles.appointmentContainerImagingLocationAddress}>
                {props.meetingType}
              </div>
              <div className={styles.appointmentContainerImagingLocationPincode}>
                Imaging Test
              </div>
            </div>
            {props.pending && (
              <button 
                className={styles.buttonPrimary}
                onClick={ () => 
                  handlePendingScheduler() }
              >Schedule</button>
            ) }
          </>
        )
        }
        {props.cardType == 'referral' && handleReferralCardType()}
      </div>
    </div>
  );
};
