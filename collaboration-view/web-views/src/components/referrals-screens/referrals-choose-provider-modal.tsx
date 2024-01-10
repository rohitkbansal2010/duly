import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import { ChooseProvider } from '@components/choose-provider-modal/ChooseProvider';
import { HeadCalender } from '@components/head-calendar';
import { StartCheckoutModal } from '@components/start-checkout-modal/start-checkout-modal';
import StartCheckoutNavbar from '@components/start-checkout-navbar/start-checkout-navbar';
import { addChat } from '@icons';
import { appointment, location as locationIcon } from '@images';
import { ChooseProviderData } from '@mock-data';
import { getWeekDayTimeSlots, saveReferralDetails } from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import { ProviderDetailsType, ReferralData } from '@types';
import {
  firstLetterCapital, formatAddressBottomLine, formatDateStringAddComma, writeHumanName 
} from '@utils';

type ReferralsProviderProps = {
  setReferralKey: React.Dispatch<React.SetStateAction<string>>;
  step: number;
  setStep: React.Dispatch<React.SetStateAction<number>>;
  mockDataLength: number;
  provider: ProviderDetailsType;
  providerType: string;
  setProvider: React.Dispatch<any>;
  heading: string;
  skip: () => void;
  location: string;
  pincode: string;
  setLocation: React.Dispatch<React.SetStateAction<string>>;
  setPincode: React.Dispatch<React.SetStateAction<string>>;
  setReferralData: React.Dispatch<React.SetStateAction<any>>;
  referralData: Array<ReferralData>;
  setStepsCompleted: React.Dispatch<React.SetStateAction<any>>;
  stepsCompleted: any;
  reset: boolean;
  onComplete: () => void;
  fromResultScreen: boolean;
  handleScheduleFollowUp: () => void;
  date: Date;
  time: string;
  setDate: React.Dispatch<React.SetStateAction<Date>>;
  setTime: React.Dispatch<React.SetStateAction<string>>
};

export const ReferralsProvider = ({
  setReferralKey,  
  provider,
  providerType,
  setProvider,
  heading,
  skip,
  location,
  pincode,
  setLocation,
  setPincode,  
  referralData,  
  reset,
  date, time, setDate, setTime,
}: ReferralsProviderProps) => {
  const [ show, setShow ] = useState(false);
  const [ showSchedule, setShowSchedule ] = useState(false);
  const dispatch = useDispatch();
  
  const { appointmentId, patientId } = useParams<{ patientId: string, appointmentId: string }>();
  const { errorTitle } = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.ScheduleAppointmentErrorData);

  const handleScheduleClose = () => {
    setShowSchedule(false);
    dispatch(getWeekDayTimeSlots({
      date,
      providerId: provider ? provider.providerId : '',
      meetingType: '',
      appointmentId,
      stepType: 'R',
      departmentId: provider.department_Id,
    }));
  };

  const handleScheduleOpen = () => {
    setShowSchedule(true);
  };

  const handleShow = () => {
    setShow(true);
  };

  const toggleModal = () => {
    setShow(!show);
  };
  
  useEffect(()=>{
    if(errorTitle === 'Bad Network!' && show){
      setShow(false);
    }
  }, [ errorTitle ]);
  

  return (
    <>
      <StartCheckoutModal
        show={showSchedule }
        handleClose={handleScheduleClose}
        handleShow={handleScheduleOpen}
        date={date}
        time={time}
        providerImage={provider.photo?.url}
        providerName={writeHumanName(
          provider.humanName?.givenNames, 
          provider.humanName.familyName
        )}
        doctorType="PROVIDER"
        providerType={provider.specialty}
        heading="Confirm Referral"
        handleScheduleFollowUp={() => {
          provider 
            && dispatch(saveReferralDetails({
              provider_ID: provider.providerId,
              patient_ID: patientId,
              refVisitType: providerType,
              location_ID: parseInt(provider.location.id),
              aptScheduleDate: date.toISOString(),
              bookingSlot: time,
              created_Date: new Date().toISOString(),
              type: 'R',
              appointment_Id: appointmentId,
              skipped: false,
              department_Id: provider.department_Id,
              visitTypeId: '8001',
            }));
        }}
        scheduleDetails={[
          {
            imageIcon: 
              provider.photo 
                ? provider.photo?.url 
                : '',
            topLine: 'PROVIDER',
            middleLine: `Dr. ${provider.humanName.givenNames}`,
            bottomLine: providerType,
          },
          {
            imageIcon: locationIcon,
            topLine: 'LOCATION',
            middleLine: firstLetterCapital(provider.location.address.addressLine),
            bottomLine: formatAddressBottomLine(
              {
                city: provider.location.address.city,
                postalCode: provider.location.address.zipCode,
                state: provider.location.address.state,
              }
            ),
          },
          {
            imageIcon: appointment,
            topLine: 'APPOINTMENT DETAILS',
            middleLine: formatDateStringAddComma(date.toDateString()),
            bottomLine: time,
          },
        ]}
        buttonName="Schedule Referral"
        location={location}
        pincode={pincode}
      />
      <StartCheckoutNavbar
        providerDetails={provider}
        heading={heading}
        skipButtonText="Skip"
        actionButtonText="Schedule"
        IconImage={addChat}
        isDetails={true}
        detailsprofileImage={provider.photo?.url}
        detailsdoctorType="Provider"
        detailsdoctorName={provider.humanName.familyName}
        detailsproviderName="Change Provider"
        detailslocation={provider.location.address.addressLine}
        detailspincode={formatAddressBottomLine({
          city: provider.location.address.city,
          postalCode: provider.location.address.zipCode,
          state: provider.location.address.state,
        })}
        detailspage="Referral"
        changeProviderStyle="changeProvider"
        onAction={() =>
          handleScheduleOpen()}
        handleDetailsProviderName={handleShow}
        providerType={providerType}
        time={time}
        onSkip={() =>
          skip()}
      />
      {reset && (
        <HeadCalender 
          providerId={provider.providerId}
          stepType="R"
          setDate={setDate} 
          setTime={setTime} 
          date={date} 
          selectedData={referralData} 
          reset={reset}
          department_Id={provider.department_Id}
        />
      )}
      <ChooseProvider
        title="Choose Provider"
        isOpen={show}
        toggleModal={toggleModal}
        setReferralKey={setReferralKey}
        setProviderDetails={setProvider}
        ChooseProviderData={ChooseProviderData}
        setLocation={setLocation}
        setPincode={setPincode}
        providerType={providerType}
        step="Referral"
      />
    </>
  );
};
