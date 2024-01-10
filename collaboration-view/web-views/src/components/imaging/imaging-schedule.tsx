import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router';

import { ChooseProvider } from '@components/choose-provider-modal/ChooseProvider';
import { HeadCalender } from '@components/head-calendar';
import { StartCheckoutModal } from '@components/start-checkout-modal/start-checkout-modal';
import StartCheckoutNavbar from '@components/start-checkout-navbar/start-checkout-navbar';
import { followUpAppointment } from '@icons';
import { appointment, location } from '@images';
import { getLocationsList, saveImagingTestDetails } from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import { 
  LabOrImagingTestDetailsType, 
  Location, 
  ProviderDetailsType 
} from '@types';
import { 
  formatAddressBottomLine, 
  getImagingTestByStep, 
  getImagingTestDate
} from '@utils';

type ImagingScheduleProps = {
  setImagingKey: React.Dispatch<React.SetStateAction<string>>;
  imagingKey: string;
  step: number;
  setStep: React.Dispatch<React.SetStateAction<number>>;
  provider: any;
  setProvider: React.Dispatch<any>;
  heading: string;
  skip: (...args: any[]) => any;
  pincode?: string;
  setLocation?: React.Dispatch<React.SetStateAction<string>>;
  setPincode?: React.Dispatch<React.SetStateAction<string>>;
  setImagingProviderData?: React.Dispatch<React.SetStateAction<ProviderDetailsType>>;
  ImagingProviderData?: Array<ProviderDetailsType[]>;
  stepsCompleted: any;
  setStepsCompleted: React.Dispatch<React.SetStateAction<any>>;
  setTestsCompleted: React.Dispatch<React.SetStateAction<any>>;
  testsCompleted: any;
  activeTest: number;
  setActiveTest: React.Dispatch<React.SetStateAction<number>>
  renderImagingType: any;
  imagingData: LabOrImagingTestDetailsType;
  allTests?: string[];
  handleSkip: () => void;
  activeImagingLocation: Location;
  scheduledTests: string[] | undefined;
  reset: boolean;
  setActiveImagingLocation: React.Dispatch<React.SetStateAction<Location>>;
  fromResultScreen: boolean;
};

export const ImagingSchedule = (props: ImagingScheduleProps) => {
  const [ time, setTime ] = useState('');
  const [ date, setDate ] = useState<Date>(new Date());
  const [ showSchedule, setShowSchedule ] = useState(false);
  const [ show, setShow ] = useState(false);
  const {
    patientId, 
    appointmentId,
  }: {
    patientId: string,
    appointmentId: string,
  } = useParams();
  const dispatch = useDispatch();

  const { followUpOrderDetails } = useSelector(({ CHECKOUT_APPOINTMENTS }:RootState)=>
    CHECKOUT_APPOINTMENTS);

  useEffect(() => {
    if(props.reset && props.allTests){
      dispatch(getLocationsList('Imaging', 'Oak', [ props.allTests[props.step] ]));
    }
  }, [ props.reset, dispatch ]);

  const handleScheduleClose = () => {
    setShowSchedule(false);
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

  const handleClose = () => {
    setShow(false);
  };

 

  

  const handleChangeLocation = () => {
    setShow(!show);
  };

  return followUpOrderDetails && (
    <>
      <ChooseProvider
        title = "Change Location"
        isOpen = {show}
        toggleModal = {toggleModal}
        handleClose = {handleClose}
        allTests = {props.allTests}
        imagingStep = {props.step}
        setActiveImagingLocation = {props.setActiveImagingLocation}
      />
      <StartCheckoutModal
        show={showSchedule}
        handleClose={handleScheduleClose}
        handleShow={handleScheduleOpen}
        date={date}
        time={time}
        heading="Confirm Appointment"
        handleScheduleFollowUp={() =>
          dispatch(saveImagingTestDetails({
            provider_ID: '11',
            patient_ID: patientId,
            location_ID: 'locationId',
            appointment_ID: appointmentId,
            imagingType: getImagingTestByStep(props.step, props.allTests),
            imagingLocation: `${props.activeImagingLocation.location.address.addressLine}, ${props.activeImagingLocation.location.address.city}, ${props.activeImagingLocation.location.address.state}, ${props.activeImagingLocation.location.address.zipCode}`,
            bookingSlot: time,
            aptScheduleDate: getImagingTestDate(date),
            skipped: false,
          }))
        }
        buttonName="Schedule Appointment"
        scheduleDetails = {[
          {
            imageIcon: followUpAppointment,
            topLine: 'APPOINTMENT TYPE',
            middleLine: props.allTests && props.allTests[props.step],
            bottomLine: 'Imaging',
          },
          {
            imageIcon: location,
            topLine: 'LOCATION',
            middleLine: props.activeImagingLocation.location.address.addressLine,
            bottomLine: formatAddressBottomLine(
              {
                city: props.activeImagingLocation.location.address.city, 
                postalCode: props.activeImagingLocation.location.address.zipCode, 
                state: props.activeImagingLocation.location.address.state,
              }
            ),
          },
          {
            imageIcon: appointment,
            topLine: 'APPOINTMENT DETAILS',
            middleLine: date?.toDateString(),
            bottomLine: time,
          },
        ]}
      />
      <StartCheckoutNavbar
        heading={props.heading}
        skipButtonText="Skip"
        actionButtonText="Schedule"
        IconImage={followUpAppointment}
        isDetails={true}
        detailspage="Imaging"
        onAction={() =>
          handleScheduleOpen()}
        handleDetailsProviderName={handleShow}
        time={time}
        onSkip={() => {
          dispatch(saveImagingTestDetails({
            provider_ID: '',
            patient_ID: patientId,
            location_ID: 'locationId',
            appointment_ID: appointmentId,
            imagingType: props.allTests ? props.allTests[props.step] : '',
            imagingLocation: `${props.activeImagingLocation.location.address.addressLine}, ${props.activeImagingLocation.location.address.city}, ${props.activeImagingLocation.location.address.state}, ${props.activeImagingLocation.location.address.zipCode}`,
            bookingSlot: '',
            aptScheduleDate: new Date().toISOString(),
            skipped: true,
          }));
        }}
        detailsData = {[
            {
              topLine: 'Imaging Location',
              middleLine: `${props.activeImagingLocation.location.address.addressLine}, ${formatAddressBottomLine({
                      city: props.activeImagingLocation.location.address.city, 
                      postalCode: props.activeImagingLocation.location.address.zipCode, 
                      state: props.activeImagingLocation.location.address.state,
                    })}`,
              bottomLine: 'Change Location',
            },
            {
              topLine: 'Imaging Type',
              middleLine: getImagingTestByStep(props.step, props.allTests),
              bottomLine: '',
            },
        ]}
        handleChangeLocation={handleChangeLocation}
      />
      {props.reset && (
        <HeadCalender 
          stepType="I"
          setDate={setDate} 
          setTime={setTime} 
          date={date} 
          reset={props.reset} 
        />
      )}
    </>
  );
};
