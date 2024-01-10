import { useEffect, useState } from 'react';
import { Col } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import { DataContainerSkeleton } from '@components/checkout-data-container/data-container-skeleton';
import { FollowUpAppointment } from '@components/follow-up-appintment';
import { NoSuggestedOrder } from '@components/no-suggested-order';
import StartCheckoutNavbar from '@components/start-checkout-navbar/start-checkout-navbar';
import { StepperTitles } from '@enums';
import { useStepperSkeleton } from '@hooks';
import { followUpAppointment, imagingPinkBackground } from '@icons';
import {
  getImagingTestDetails,
  getLocationsList,
  setStepsList,
  stopLoading
} from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import {
  LabOrImagingTestDetailsType,
  Location,
  ScheduledImagingTestDetails
} from '@types';

import { ErrorImaging } from './error-imaging';
import { ImagingSchedule } from './imaging-schedule';
import ImagingTest from './imaging-test';
import styles from './imaging.scss';


type ImagingPropsType = {
  skip: () => void,
  onComplete: () => void,
  activeKey: string,
  reset: boolean,
}

export type ImagingDataType = {
  imagingTestDetails: LabOrImagingTestDetailsType,
  locations: Location[],
}

export const Imaging = ({
  skip,
  activeKey,
  reset,
}: ImagingPropsType) => {
  const {
    imagingTestDetails,
    locationLists,
    scheduledImagingTestDetails,
    isImagingTestScheduled,
    loading,
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);

  const dispatch = useDispatch();


  const [ imagingKey, setImagingKey ] = useState('SelectLocation');
  const [ provider, setProvider ] = useState<any>();
  const [ activeImagingLocation, setActiveImagingLocation ] =
    useState<Location>({
      id: '',
      name: '',
      location: {
        address: {
          addressLine: '',
          city: '',
          state: '',
          zipCode: '',
        },
        phoneNumber: '',
        workingHours: '',
        distance: 0,
      },
    });
  const [ step, setStep ] = useState(0);
  const [ allTests, setAllTests ] = useState<string[]>();
  const [ activeTest, setActiveTest ] = useState(0);
  const [ stepsCompleted, setStepsCompleted ] = useState<any>([]);
  const [ testsCompleted, setTestsCompleted ] = useState<any>([]);
  const [ heading, setHeading ] = useState('');
  const [ imagingData, setImagingData ] = useState<ImagingDataType>();
  const [ scheduledTests, setScheduledTests ] = useState<string[]>();
  const [ fromResultScreen, setFromResultScreen ] = useState(false);

  const { patientId, appointmentId } = useParams<{ patientId: string, appointmentId: string }>();

  useEffect(() => {
    if (reset && allTests) {
      dispatch(getLocationsList('Imaging', 'Oak', [ allTests[step] ]));
    }
  }, [ reset, dispatch ]);

  useEffect(() => {
    if (imagingKey && imagingKey !== 'SelectLocation' && imagingKey !== 'ScheduleImaging') {
      setFromResultScreen(true);
    }
  }, [ imagingKey ]);

  const handleSkip = () => {
    if (fromResultScreen) {
      setImagingKey('complete');
      setStep(0);
    }
    else {
      if (allTests?.length) {
        if (step < allTests.length - 1) {
          setStep(step + 1);
        }
        else {
          setImagingKey('complete');
          setStep(0);
        }
      }
    }
  };

  const renderImagingType = () => {
    setImagingKey('ScheduleImaging');
  };
  const setImagingKeyAndStep = () => {
    if(allTests?.length){
      if(step < allTests.length - 1){
        setImagingKey('SelectLocation');
        setStep(step + 1);
      }
      else{
        setImagingKey('complete');
        setStep(0);
      }
    }
  };

  const handleScheduleFollowUp = () => {
    if(fromResultScreen){
      setImagingKey('complete');
      setStep(0);
    }
    else{
      setImagingKeyAndStep();
    }
  };

  const getFollowUpAppointmentLocation = (location: string) => {
    const locationList = location.split(', ');
    const locations = locationList[0];
    const pincode = `${locationList[1]}, ${locationList[2]} ${locationList[3]}`;

    return {
      location: locations,
      pincode: pincode,
    };
  };

  const checkPendingScreen = () => {
    if (isImagingTestScheduled) {
      setImagingKey('complete');
      setStep(0);
      return;
    }
    if (
      imagingTestDetails
      && scheduledImagingTestDetails
      && imagingTestDetails.orderCount === scheduledImagingTestDetails.length
    ) {
      setImagingKey('complete');
      setStep(0);
      return;
    }
    setImagingKey('SelectLocation');
    setStep(0);
  };

  const pendingScheduleHandler = (id: number) => {
    setImagingKey('SelectLocation');
    setStep(id);
    setFromResultScreen(true);
  };

  useEffect(()=>{
    if(scheduledImagingTestDetails){
      let flag = 0;
      scheduledImagingTestDetails.forEach((detail)=>{
        if(!detail.skipped && !scheduledTests?.includes(detail.imagingType!)){
          flag = 1; 
          handleScheduleFollowUp();
        }
      });
      if(!flag){
        handleSkip();
      }
    }
  }, [ scheduledImagingTestDetails ]);

  useEffect(() => {
    if (imagingTestDetails && locationLists) {
      setImagingData({
        imagingTestDetails: imagingTestDetails,
        locations: locationLists,
      });
    }
  }, [ imagingTestDetails, locationLists ]);

  const getImagingHeading = (allTest: string[]) => {
    if (allTest) {
      const totalTests = allTest.length;
      setHeading(`Imaging (${step + 1}/${totalTests})`);
    }
  };

  useEffect(() => {
    const allTest: string[] = [];
    if (imagingTestDetails) {
      imagingTestDetails?.testOrder?.forEach((test) => {
        allTest.push(test.orderName);
      });
      setAllTests(allTest);
      getImagingHeading(allTest);
    }
  }, [ imagingTestDetails ]);

  useEffect(() => {
    if (allTests) {
      getImagingHeading(allTests);
    }
  }, [ step ]);

  useEffect(() => {
    imagingData && setActiveImagingLocation(imagingData.locations[0]);
  }, [ imagingData ]);

  useEffect(() => {
    checkPendingScreen();
  }, [ activeKey ]);

  useEffect(() => {
    const scheduledData: string[] = [];
    scheduledImagingTestDetails?.map((data) => {
      if (!data.skipped) {
        scheduledData.push(data.imagingType!);
      }
    });
    setScheduledTests(scheduledData);
  }, [ scheduledImagingTestDetails ]);

  useEffect(() => {
    if (reset && !imagingTestDetails) {
      dispatch(setStepsList([ { alias: StepperTitles.IMAGING, isSkeletonShown: true } ]));
      dispatch(getImagingTestDetails(patientId, appointmentId, true));
    }
    dispatch(stopLoading());
  }, [ reset ]);

  const isSkeletonShown = useStepperSkeleton(StepperTitles.IMAGING);

  if (isSkeletonShown || loading) {
    return <DataContainerSkeleton />;
  }

  if (!imagingTestDetails) {
    return <ErrorImaging />;
  }

  if(imagingTestDetails && imagingTestDetails.orderCount === 0){
    return (
      <>
        <StartCheckoutNavbar
          heading="Imaging"
          IconImage={followUpAppointment}
          isDetails={false}
          actionButtonText="Next"
          onAction={skip}
        />
        <NoSuggestedOrder icon={imagingPinkBackground} title="No suggested Imaging Orders" />
      </>
    );
  }

  return (
    imagingTestDetails && (
      <div>
        {(() => {
          switch (imagingKey) {
            case 'SelectLocation':
              return (
                <>
                  {activeImagingLocation && (
                    <ImagingTest
                      reset={reset}
                      imagingData={imagingData}
                      activeImagingLocation={activeImagingLocation}
                      setActiveImagingLocation={setActiveImagingLocation}
                      activeTest={activeTest}
                      setActiveTest={setActiveTest}
                      renderImagingType={renderImagingType}
                      allTests={allTests}
                      step={step}
                      heading={heading}
                      handleSkip={handleSkip}
                    />
                  )}
                </>
              );
            case 'ScheduleImaging':
              return (
                <>
                  <ImagingSchedule
                    setImagingKey={setImagingKey}
                    imagingKey={imagingKey}
                    step={step}
                    setStep={setStep}
                    provider={provider}
                    setProvider={setProvider}
                    skip={skip}
                    setActiveImagingLocation={setActiveImagingLocation}
                    heading={`${heading}: ${allTests && allTests[step]}`}
                    handleSkip={handleSkip}
                    reset={reset}
                    stepsCompleted={stepsCompleted}
                    setStepsCompleted={setStepsCompleted}
                    setTestsCompleted={setTestsCompleted}
                    testsCompleted={testsCompleted}
                    activeTest={activeTest}
                    setActiveTest={setActiveTest}
                    renderImagingType={renderImagingType}
                    imagingData={imagingTestDetails}
                    allTests={allTests}
                    activeImagingLocation={activeImagingLocation}
                    scheduledTests={scheduledTests}
                    fromResultScreen={fromResultScreen}
                  />
                </>
              );
            default:
              return (
                <>
                  <div className={styles.ScrollBar}>
                    <div>
                      <StartCheckoutNavbar
                        heading="Imaging"
                        IconImage={followUpAppointment}
                        isDetails={false}
                        actionButtonText="Next"
                        onAction={skip}
                      />
                    </div>

                    <div className={styles.appointmentsContainer}>
                      {
                        scheduledImagingTestDetails
                        && imagingTestDetails
                        && scheduledImagingTestDetails.length > 0
                        && allTests
                        && allTests.map((appointment, index) => {
                          let imaging: ScheduledImagingTestDetails | undefined = undefined;
                          scheduledImagingTestDetails.forEach((scheduledImaging) => {
                            if (scheduledImaging.imagingType!.toLowerCase()
                              === appointment.toLowerCase()) {
                              imaging = scheduledImaging;
                            }
                          });
                          if (imaging !== undefined && !imaging?.skipped) {
                            return (
                              <Col md={6} key={index}>
                                <FollowUpAppointment
                                  date={imaging.aptScheduleDate}
                                  time={imaging.bookingSlot}
                                  location={
                                    getFollowUpAppointmentLocation(imaging.imagingLocation).location
                                  }
                                  pincode={
                                    getFollowUpAppointmentLocation(imaging.imagingLocation).pincode
                                  }
                                  cardType="imaging"
                                  meetingType={appointment}
                                />
                              </Col>
                            );
                          }
                          else {
                            return (
                              <Col md={6} key={index}>
                                <FollowUpAppointment
                                  key={index}
                                  date=""
                                  time=""
                                  location=""
                                  pincode=""
                                  cardType="imaging"
                                  meetingType={appointment}
                                  pendingScheduleHandler={pendingScheduleHandler}
                                  pending={true}
                                  cardIndex={index}
                                />
                              </Col>
                            );
                          }
                        })
                      }
                    </div>
                  </div>
                </>
              );
          }
        })()}
      </div>
    )
  );
};
