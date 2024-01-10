import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import { DataContainerSkeleton } from '@components/checkout-data-container/data-container-skeleton';
import { NoSuggestedOrder } from '@components/no-suggested-order';
import StartCheckoutNavbar from '@components/start-checkout-navbar/start-checkout-navbar';
import { StepperTitles } from '@enums';
import { useStepperSkeleton } from '@hooks';
import { addMessageIcon, addChat, referralsPinkBackground } from '@icons';
import { 
  getReferralDetails, resetLoading, saveReferralDetails, setStepsList 
} from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import { ProviderDetailsType, ReferralData } from '@types';

import { Referrals_Insider } from '../referrals-screens/referral-choose-provider';
import { ReferralsProvider } from '../referrals-screens/referrals-choose-provider-modal';

import { ErrorReferral } from './error-referral';
import { ReferralResultScreen } from './referral-result-screen';


type ReferralsProps = {
  onComplete: (...args: any[]) => any;
  skip: (...args: any[]) => any;
  setReferralData: (...args: any[]) => any;
  activeSection?: string;
  activeKey: string;
  reset: boolean;
};

export const Referrals = ({
  activeSection,
  skip,
  onComplete,
  setReferralData,
  activeKey,
  reset,
}: ReferralsProps) => {
  const [ referralKey, setReferralKey ] = useState('ChooseProvider');
  const [ step, setStep ] = useState(0);
  const [ stepsCompleted, setStepsCompleted ] = useState<any>([]);
  const [ provider, setProvider ] = useState<ProviderDetailsType>();
  const [ location, setLocation ] = useState('');
  const [ pincode, setPincode ] = useState('');
  const [ referralDataArray, setReferralDataArray ] = useState<Array<ReferralData>>([]);
  const [ fromResultScreen, setFromResultScreen ] = useState(false);
  const [ time, setTime ] = useState('');
  const [ date, setDate ] = useState<Date>(new Date());
  const {
    referralDetails,
    scheduledReferralDetails,
    isReferralScheduled,
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);

  const dispatch = useDispatch();

  const checkPendingScreen = () => {
    if(isReferralScheduled) {
      setReferralKey('');
      return;
    }
    if(
      referralDetails 
      && scheduledReferralDetails
      && scheduledReferralDetails.length === referralDetails.length
    ) {
      setReferralKey('');
      return;
    }
    setReferralKey('ChooseProvider');
    setStep(0);
  };

  const { patientId, appointmentId } = useParams<{ patientId: string, appointmentId: string }>();


  useEffect(() => {
    setReferralData(referralDataArray);
  }, [ referralDataArray ]);
  useEffect(() => {
    const scheduledSteps: number[] = [];
    scheduledReferralDetails && scheduledReferralDetails.forEach((detail) => {
      referralDetails && referralDetails.forEach((refDetail, index) => {
        if (!detail.skipped && refDetail.providerType === detail.refVisitType) {
          scheduledSteps.push(index);
        }
      });
    });
    setStepsCompleted([ ...scheduledSteps ]);
  }, [ scheduledReferralDetails, activeKey ]);  

  useEffect(() => {
    if (activeSection) setReferralKey(activeSection);
  }, [ activeSection ]);

  useEffect(() => {
    setReferralData(referralDataArray);
  }, [ referralDataArray ]);

  useEffect(() => {
    localStorage.setItem('referralData', '');
    checkPendingScreen();
  }, [ activeKey ]);

  useEffect(() => {
    if(reset){
      dispatch(resetLoading());
      if(!referralDetails){
        dispatch(setStepsList([ { alias: StepperTitles.REFERRAL, isSkeletonShown: true } ]));
        dispatch(getReferralDetails(patientId, true));
      }
    }
  }, [ reset ]);

  const skipHandler = () => {
    if(fromResultScreen){
      setReferralKey('');
      setStep(0);
      return;
    }
    if (referralDetails && referralDetails.length > 0) {
      for (let i = step; i < referralDetails.length - 1; i++) {
        if (!stepsCompleted.includes(i + 2)) {
          setReferralKey('ChooseProvider');
          setStep(i + 1);
          return;
        }
      }
      setReferralKey('');
      setStep(0);
    }
  };

  const handleScheduleFollowUp = () => {
    onComplete();
    const _stepsCompleted = [ ...stepsCompleted ];

    if(fromResultScreen){
      setReferralKey('');
    }
    else if (referralDetails && step < referralDetails.length - 1) {
      const temp = step;

      let flag = 0;
      for (let i = temp; i < referralDetails.length - 1; i++) {
        if (!_stepsCompleted.includes(i + 1)) {
          setStep(i + 1);
          setReferralKey('ChooseProvider');
          flag = 1;
          break;
        }
      }
      if (!flag) 
        setReferralKey('');
    } else {
      setReferralKey('EndScreen');
    }


    const completedReferral: ReferralData = {
      providerName: `Dr. ${provider?.humanName.familyName}`,
      time: time,
      date: date,
      img: provider?.photo?.url ? provider.photo?.url : '',
      location: location,
      pincode: pincode,
      duration: '30 Minutes',
      providerType:referralDetails![step].providerType,
    };

    const referral = [ ...referralDataArray ];
    referral.push(completedReferral);
    setReferralData(referral);
  };

  const pendingScheduleHandler = (id: number) =>{
    setReferralKey('ChooseProvider');
    setStep(id);
    setFromResultScreen(true);
  };

  const dispatchSaveAction = () => {
    dispatch(saveReferralDetails({
      provider_ID: '',
      patient_ID: patientId,
      refVisitType: referralDetails ? referralDetails[step].providerType : '',
      location_ID: 0,
      aptScheduleDate: new Date().toISOString(),
      bookingSlot: '',
      created_Date: new Date().toISOString(),
      type: 'R',
      appointment_Id: appointmentId,
      department_Id: '',
      skipped: true,
      visitTypeId: '8001',
    }));
  };

  const getChooseProviderScreen = () =>
    referralDetails
    && (
      <>
        <StartCheckoutNavbar
          heading={`Referral (${step + 1}/${referralDetails.length}): ${referralDetails[step].providerType
          }`}
          skipButtonText="Skip"
          IconImage={addChat}
          isDetails={false}
          onSkip={() =>{
            dispatchSaveAction();
          }}
        />

        <Referrals_Insider
          setReferralKey={setReferralKey}
          referralVisit={referralDetails[step].providerType}
          setProvider={setProvider}
          providerType={referralDetails[step].providerType}
          setLocation={setLocation}
          setPincode={setPincode}
          location={location}
          pincode={pincode}
        />
      </>
    );

  const getScheduleReferralScreen = () =>
    referralDetails && (
      <>
        <ReferralsProvider
          skip={()=>{
            dispatchSaveAction();
          }}
          heading={`Referral (${step + 1}/${referralDetails.length}): ${referralDetails[step].providerType
          }`}
          setReferralData={setReferralDataArray}
          referralData={referralDataArray}
          setReferralKey={setReferralKey}
          step={step}
          setStep={setStep}
          mockDataLength={referralDetails.length}
          provider={provider!}
          providerType={referralDetails[step].providerType}
          setProvider={setProvider}
          location={location}
          setLocation={setLocation}
          setPincode={setPincode}
          pincode={pincode}
          setStepsCompleted={setStepsCompleted}
          stepsCompleted={stepsCompleted}
          reset={reset}
          onComplete={onComplete}
          fromResultScreen={fromResultScreen}
          handleScheduleFollowUp={handleScheduleFollowUp}
          date={date}
          time={time}
          setDate={setDate}
          setTime={setTime}
        />
      </>
    );

  const isSkeletonShown = useStepperSkeleton(StepperTitles.REFERRAL);

  useEffect(()=>{
    let flag = 0;
    scheduledReferralDetails?.forEach((detail)=>{
      if(!detail.skipped){
        referralDetails?.forEach((det, index)=>{
          if(det.providerType === detail.refVisitType){
            if(!stepsCompleted.includes(index)){
              handleScheduleFollowUp();
              flag = 1;
            }
          }
        });
      }
    });
    if(!flag){
      skipHandler();
    }
  }, [ scheduledReferralDetails ]);

  if(isSkeletonShown){
    return <DataContainerSkeleton />;
  }
  
  if (!referralDetails) {
    return <ErrorReferral />;
  }
  

  if(referralDetails && referralDetails.length === 0){
    return (
      <>
        <StartCheckoutNavbar
          heading={`Referrals`}
          actionButtonText="Next"
          IconImage={addChat}
          isDetails={false}
          onAction={skip}
        />
        <NoSuggestedOrder title="No suggested Referral Orders" icon={referralsPinkBackground} />
      </>
    );
  }

  return (
    <div>
      {(() => {
        switch (referralKey) {
          case 'ChooseProvider':
            return getChooseProviderScreen();
          case 'ScheduleReferral':
            return getScheduleReferralScreen();
          default:
            return scheduledReferralDetails && (
              <>
                <div>
                  <StartCheckoutNavbar
                    heading="Referral Appointments"
                    actionButtonText="Next"
                    IconImage={addMessageIcon}
                    referralIcon={true}
                    isDetails={true}
                    onAction={skip}
                  />
                </div>
                <ReferralResultScreen 
                  setFromResultScreen={setFromResultScreen}
                  pendingScheduleHandler={pendingScheduleHandler}  
                />
              </>
            );
        }
      })()}
    </div>
  );
};
