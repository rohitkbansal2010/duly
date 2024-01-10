import { useEffect } from 'react';
import { Col } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import { FollowUpAppointment } from '@components/follow-up-appintment';
import { RootState } from '@redux/reducers';
import { ScheduledFollowUpDetails } from '@types';
import { formatAddressBottomLine } from '@utils';

import styles from './referrals.scss';


type ReferralResultScreenProps = {
  setFromResultScreen: React.Dispatch<React.SetStateAction<boolean>>;
  pendingScheduleHandler: (id: number) => void;
}

export const ReferralResultScreen = ({
  setFromResultScreen,
  pendingScheduleHandler,
}: ReferralResultScreenProps) => {
  const {
    scheduledReferralDetails,
    referralDetails,
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);
  useEffect(() => {
    setFromResultScreen(true);
  }, []);
  return (
    <div className={styles.appointmentsContainer}>

      {scheduledReferralDetails
        && referralDetails
        && scheduledReferralDetails.length > 0 &&
        referralDetails.map((detail, index) => {
          let referral: ScheduledFollowUpDetails | undefined = undefined;
          scheduledReferralDetails.forEach((scheduledReferral) => {
            if (scheduledReferral.refVisitType?.toLowerCase() 
                === detail.providerType.toLowerCase()) {
              referral = scheduledReferral;
            }
          });
          if (referral !== undefined && !referral?.skipped) {
            return (
              <Col key={index} md={6}>
                {detail.providerDetails ? (
                  <FollowUpAppointment
                    date={new Date(referral.aptScheduleDate).toDateString()}
                    time={referral.bookingSlot}
                    location={detail.providerDetails.location.address.addressLine}
                    pincode={formatAddressBottomLine(
                      {
                        city: detail.providerDetails.location.address.city,
                        postalCode: detail.providerDetails.location.address.zipCode,
                        state: detail.providerDetails.location.address.state,
                      }
                    )}
                    meetingType={referral?.refVisitType}
                    cardType="referral"
                    duration="30 Minutes"
                    providerName={detail.providerDetails.humanName.familyName}
                    providerImg={detail.providerDetails.photo.url}
                  />
                  ) : (
                    <FollowUpAppointment 
                      date={new Date(referral.aptScheduleDate).toDateString()}
                      time={referral.bookingSlot}
                      location=""
                      pincode={formatAddressBottomLine(
                        {
                          city: '',
                          postalCode: '',
                          state: '',
                        }
                      )}
                      meetingType={referral?.refVisitType}
                      cardType="referral"
                      duration="30 Minutes"
                      providerName=""
                      providerImg=""
                    />
                  )}
              </Col>
            );
          }
          return (
            <Col key={index} md={6}>
              <FollowUpAppointment
                date=""
                time=""
                location=""
                meetingType={detail.providerType}
                cardType="referral"
                duration=""
                pendingScheduleHandler={pendingScheduleHandler}
                pending={true}
                cardIndex={index}
              />
            </Col>
          );
        })
      }
    </div>
  );
};
