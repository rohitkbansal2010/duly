import { useEffect, useState } from 'react';
import { Col, Tab } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import { FollowUpAppointment } from '@components/follow-up-appintment';
import { HeadCalender } from '@components/head-calendar';
import { StartCheckoutModal } from '@components/start-checkout-modal/start-checkout-modal';
import StartCheckoutNavbar from '@components/start-checkout-navbar/start-checkout-navbar';
import { StepperTitles } from '@enums';
import { useSiteId, useStepperSkeleton } from '@hooks';
import { followUpAppointment } from '@icons';
import { appointment, location as locationIcon } from '@images';
import { CheckoutDetailsBar, checkoutNavbarData } from '@mock-data';
import {
  getFollowUpDetails,
  getWeekDayTimeSlots,
  saveFollowUpDetails,
  setStepsList
} from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import {
  FollowUpOrderType, ProviderDetailsType, ScheduleFollowUpAppointmentData, Site 
} from '@types';
import {
  formatAddressBottomLine,
  formatDateStringAddComma, 
  getAppointmentType,
  writeHumanName,
  convertTo24HourFormat,
  convertTo12HourFormat,
  getRefVisitType
} from '@utils';

import { DataContainerSkeleton } from './data-container-skeleton';
import { ErrorFollowUp } from './error-follow-up';
import styles from './shedule-follow-up.scss';

type ScheduleFollowUpProps = {
  onComplete: (...args: any[]) => any;
  skip: (...args: any[]) => any;
  defaultActiveKey?: string;
  setFollowUpData: React.Dispatch<React.SetStateAction<any | null>>;
  reset: boolean;
};


function ScheduleFollowUp({
  skip, onComplete, defaultActiveKey, setFollowUpData, reset,
}: ScheduleFollowUpProps) {
  const [ time, setTime ] = useState('');
  const [ date, setDate ] = useState<Date>(new Date());
  const [ show, setShow ] = useState(false);
  const [ lastScreenVisible, setLastScreenVisible ] = useState(false);
  const [ selectedAppType, setSelectedAppType ] = useState(3);
  const [ selectedMeetingType, setSelectedMeetingType ] = useState('In-Person');
  const [ actKey, setActKey ] = useState('1');

  const {
    followUpOrderDetails,
    isFollowUpScheduled,
    scheduledFollowUpDetails,
    loading,
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);
  
  const { appointmentId, patientId, practitionerId } = useParams<{ 
    patientId: string, 
    appointmentId: string, 
    practitionerId: string 
  }>();
  const { siteId } = useSiteId();

  const dispatch = useDispatch();
  
  const handleClose = () => {
    setShow(false);
    followUpOrderDetails && dispatch(getWeekDayTimeSlots({
      date, 
      providerId: practitionerId,
      meetingType: selectedMeetingType,
      appointmentId,
      stepType: 'F',
    }));
  };

  const handleShow = () => {
    setShow(true);
  };

  const getLocation = (location: Site) => 
    ({
      id: location.id,
      address: {
        addressLine: location.address?.line ? location.address?.line : '',
        addressLine2: '',
        city: location.address?.city ? location.address?.city : '',
        state: location.address?.state ? location.address?.state : '',
        zipCode: location.address?.postalCode ? location.address?.postalCode : '',
      },
      geographicCoordinates: {
        latitude: 0,
        longitude: 0,
      },
      phoneNumber: '',
      distance: 0,
    });

  const getPractitionersDetails = (details: FollowUpOrderType): ProviderDetailsType =>
    ({
      id: details.practitioner.id,
      providerId: details.practitioner.id,
      humanName: details.practitioner.humanName,
      location: getLocation(details.location),
      photo: details.practitioner.photo ? details.practitioner.photo : { contentType: '' },
      specialty: 'Physician',
    });

  const skipHandler = () => {
    dispatch(saveFollowUpDetails(
      {
        provider_ID: '',
        patient_ID: patientId,
        aptType: '',
        aptFormat: '',
        location_ID: 0,
        aptScheduleDate: '',
        bookingSlot: '',
        created_Date: new Date().toISOString(),
        type: 'F',
        appointment_Id: appointmentId,
        skipped: true,
      }
    ));
  };

  const pendingScheduleHandler = () => {
    setLastScreenVisible(false);
  };

  useEffect(() => {
    if (scheduledFollowUpDetails?.length) {
      setLastScreenVisible(true);
    }
  }, [ scheduledFollowUpDetails ]);

  useEffect(() => {
    if (defaultActiveKey)
      setActKey(defaultActiveKey);
  }, [ defaultActiveKey ]);

  useEffect(() => {
    if (reset && !followUpOrderDetails) {
      dispatch(setStepsList([ { alias: StepperTitles.FOLLOW_UP, isSkeletonShown: true } ]));
      dispatch(getFollowUpDetails({ 
        patientId, 
        siteId, 
        practitionerId,
        notInitialLoad: true,
      }));
    }
    if (reset) {
      if (scheduledFollowUpDetails?.length) {
        setLastScreenVisible(true);
      }
    }
  }, [ reset ]);

  const Appointments: ScheduleFollowUpAppointmentData[] = [
    {
      date: date,
      time: time,
      location: CheckoutDetailsBar.location,
      pincode: CheckoutDetailsBar.pincode,
      appointmentType: selectedAppType,
      meetingType: selectedMeetingType,
    },
  ];
  const isSkeletonShown = useStepperSkeleton(StepperTitles.FOLLOW_UP);

  if (isSkeletonShown || loading) {
    return <DataContainerSkeleton />;
  }

  if (!followUpOrderDetails) {
    return <ErrorFollowUp />;
  }

  return followUpOrderDetails && (
    <Tab.Container
      defaultActiveKey={`${defaultActiveKey}`}
      onSelect={(key) => {
        if (key === null) setActKey(actKey);
        else setActKey(key);
      }}
    >
      <Tab.Content>
        <Tab.Pane active={!lastScreenVisible}>
          <div>
            <StartCheckoutModal
              show={show}
              handleShow={handleShow}
              handleClose={handleClose}
              date={date}
              time={time}
              scheduleDetails={[
                {
                  imageIcon: followUpOrderDetails.practitioner.photo?.url ? followUpOrderDetails.practitioner.photo?.url : '',
                  topLine: 'Physician',
                  middleLine: `Dr. ${writeHumanName(
                    followUpOrderDetails.practitioner.humanName.givenNames,
                    followUpOrderDetails.practitioner.humanName.familyName
                  )}`,
                  bottomLine: followUpOrderDetails.practitioner.role,
                },
                {
                  imageIcon: locationIcon,
                  topLine: 'LOCATION',
                  middleLine: followUpOrderDetails.location.address?.line,
                  bottomLine: formatAddressBottomLine(
                    {
                      city: followUpOrderDetails.location.address?.city,
                      postalCode: followUpOrderDetails.location.address?.postalCode,
                      state: followUpOrderDetails.location.address?.state,
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
              handleScheduleFollowUp={() => {
                onComplete();
                setActKey('2');
                setFollowUpData(Appointments);
                dispatch(saveFollowUpDetails(
                  {
                    provider_ID: followUpOrderDetails.practitioner.id,
                    patient_ID: patientId,
                    aptType: getAppointmentType(selectedAppType),
                    refVisitType: getRefVisitType(selectedMeetingType),
                    aptFormat: selectedMeetingType,
                    location_ID: parseInt(siteId),
                    aptScheduleDate: date.toISOString(),
                    bookingSlot: convertTo24HourFormat(time),
                    created_Date: new Date().toISOString(),
                    type: 'F',
                    appointment_Id: appointmentId,
                    skipped: false,
                  }
                ));
              }}
              buttonName="Schedule Follow Up"
            />
            <StartCheckoutNavbar
              providerDetails={getPractitionersDetails(followUpOrderDetails)}
              time={time}
              heading={checkoutNavbarData.heading}
              actionButtonText={checkoutNavbarData.buttonName1}
              skipButtonText={checkoutNavbarData.buttonName2}
              IconImage={checkoutNavbarData.IconImage}
              isDetails={checkoutNavbarData.isDetails}
              detailsprofileImage={CheckoutDetailsBar.profileImage}
              detailsdoctorType={CheckoutDetailsBar.doctorType}
              detailsdoctorName={CheckoutDetailsBar.doctorName}
              detailsproviderName={CheckoutDetailsBar.providerName}
              detailslocation={CheckoutDetailsBar.location}
              detailspincode={CheckoutDetailsBar.pincode}
              detailspage={CheckoutDetailsBar.page}
              onAction={handleShow}
              onSkip={skipHandler}
              selectedAppType={selectedAppType}
              setSelectedAppType={setSelectedAppType}
              selectedMeetingType={selectedMeetingType}
              setSelectedMeetingType={setSelectedMeetingType}
            />
          </div>
          <div>
            {(reset || isFollowUpScheduled) && (
              <HeadCalender
                stepType="F"
                setDate={setDate}
                setTime={setTime}
                date={date}
                selectedAppType={selectedAppType}
                selectedMeetingType={selectedMeetingType}
                reset={reset}
                providerId={practitionerId}
              />
            )}
          </div>
        </Tab.Pane>
        <Tab.Pane active={lastScreenVisible}>
          {scheduledFollowUpDetails && (
            <>
              <div>
                <StartCheckoutNavbar
                  heading="Follow-Up Appointments"
                  providerDetails={getPractitionersDetails(followUpOrderDetails)}
                  IconImage={followUpAppointment}
                  resultScreen={true}
                  isDetails={true}
                  detailsprofileImage={CheckoutDetailsBar.profileImage}
                  detailsdoctorType={CheckoutDetailsBar.doctorType}
                  detailsdoctorName="Dr. Susan Ling"
                  detailsproviderName="PCP - Internal Medicine"
                  detailspage="Follow-Up Appointments"
                  actionButtonText="Next"
                  onAction={skip}
                />
              </div>
              <div className={styles.appointmentsContainer}>
                {followUpOrderDetails && scheduledFollowUpDetails.map((followUp, index) => {
                  if (!followUp.skipped) {
                    return (
                      <Col md={6} key={index}>
                        <FollowUpAppointment
                          date={new Date(followUp.aptScheduleDate).toDateString()}
                          time={convertTo12HourFormat(followUp.bookingSlot)}
                          location={followUpOrderDetails.location.address?.line ? followUpOrderDetails.location.address?.line : ''}
                          pincode={formatAddressBottomLine(
                            {
                              city: followUpOrderDetails.location.address?.city,
                              postalCode: followUpOrderDetails.location.address?.postalCode,
                              state: followUpOrderDetails.location.address?.state,
                            }
                          )}
                          appointmentType={followUp.aptType}
                          meetingType={followUp.aptFormat}
                          cardType="schedule"
                        />
                      </Col>
                    );
                  }
                  else {
                    return (
                      <Col md={6} key={index}>
                        <FollowUpAppointment
                          date=""
                          time=""
                          location={followUpOrderDetails.location.address?.line ? followUpOrderDetails.location.address?.line : ''}
                          pincode={formatAddressBottomLine(
                            {
                              city: followUpOrderDetails.location.address?.city,
                              postalCode: followUpOrderDetails.location.address?.postalCode,
                              state: followUpOrderDetails.location.address?.state,
                            }
                          )}
                          cardType="schedule"
                          duration=""
                          pendingScheduleHandler={pendingScheduleHandler}
                          pending={true}
                          cardIndex={index}
                        />
                      </Col>
                    );
                  }
                })}
              </div>
            </>
          )}
        </Tab.Pane>
      </Tab.Content>
    </Tab.Container>
  );
}

export default ScheduleFollowUp;
