import { noop } from 'lodash';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import { DataContainerSkeleton } from '@components/checkout-data-container/data-container-skeleton';
import GoogleMaps from '@components/google-maps/google-maps';
import { LocationCard } from '@components/location-card/location-card';
import { LocationsContainerSkeleton } from '@components/locations-list/location-skeleton';
import { LocationsList } from '@components/locations-list/locations-list';
import { NoDataAvailable } from '@components/no-data-available/no-data-available';
import { NoSuggestedOrder } from '@components/no-suggested-order';
import StartCheckoutNavbar from '@components/start-checkout-navbar/start-checkout-navbar';
import { TestsList } from '@components/tests-list/tests-list';
import { StepperTitles } from '@enums';
import { useStepperSkeleton } from '@hooks';
import { labsFilledIcon, noProvider, labsPinkBackground } from '@icons';
import {
  getLabsLocation, getLabTestDetails, saveLabTestDetails, 
  setLabsTestNotSkipped, setLabsTestSkipped, setStepsList
} from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import { Location } from '@types';
import {
  formatAddressBottomLine,
  getLatLngFromAddress,
  getPatientAddressForLabs,
  getPatientLineAddressForLabs,
  isLabOrderCountZero,
  showSkeletonForLabs
} from '@utils';

import { ErrorLabs } from './error-labs';
import styles from './labs.scss';

type LabsProps = {
  reset: boolean;
  skip: (...args: any[]) => any;
  onComplete: () => void;
}

function Labs({ reset, skip, onComplete }: LabsProps) {
  const patientData = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientData);
  const [ patientAddress, setPatientAddress ] = useState(getPatientAddressForLabs(patientData));
  const [ patientLatLng, setpatientLatLng ] = useState();

  
  const {
    labTestDetails,
    isLabTestScheduled,
    isLabTestSkipped,
    scheduledLabTestDetails,
    labsLocation,
    loading,
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);
  const isLabLocationSkeletonShown = useStepperSkeleton(StepperTitles.LAB_LOCATIONS);

  const { refreshLocation } = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.refreshData);

  const { appointmentId, patientId } = useParams<{ appointmentId: string, patientId: string }>();
  const dispatch = useDispatch();
  const [ savedLocation, setSavedLocation ] = useState<Location>({
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
  const [ activeLabLocation, setActiveLabLocation ] = useState<Location>({
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
  const [ resetLabs, setResetLabs ] = useState(reset);
  const lineAddress = getPatientLineAddressForLabs(patientData);

  const Tests: string[] = [];
  labTestDetails?.testOrder.map((tests) => {
    Tests.push(tests.orderName);
  });

  useEffect(()=>{
    setPatientAddress(getPatientAddressForLabs(patientData));
  }, [ lineAddress ]);

  useEffect(()=>{
    async function fetchLatLngData() {
      const latLng = await getLatLngFromAddress(getPatientAddressForLabs(patientData));
      setpatientLatLng(latLng);
    }
    if(reset){
      fetchLatLngData();
    }    
  }, [ lineAddress, reset ]);

  useEffect(() => {
    setResetLabs(reset);
    if (reset && !labTestDetails) {
      dispatch(setStepsList([ { alias: StepperTitles.LABS, isSkeletonShown: true } ]));
      dispatch(getLabTestDetails(patientId, appointmentId, true));
    }
    if (reset) {
      patientLatLng && dispatch(getLabsLocation(patientLatLng.lat, patientLatLng.lng));
    }
  }, [ reset, patientLatLng ]);



  useEffect(() => {
    if (labsLocation) {
      setActiveLabLocation(labsLocation[0]);
    }
    
    if (isLabTestScheduled) {
      setResetLabs(true);
      labsLocation?.forEach((location) => {
        if (scheduledLabTestDetails?.
          lab_Name.includes(location.location.address.addressLine)) {
          setSavedLocation(location);
          setActiveLabLocation(location);
        }
      });
    }
  }, [ isLabTestScheduled, scheduledLabTestDetails, labTestDetails, labsLocation ]);

  useEffect(()=>{
    dispatch(setLabsTestNotSkipped());
  }, [ reset ]);

  useEffect(()=>{
    if(refreshLocation !== 'saveSkippedLab' && isLabTestSkipped){
      skip();
    }
  }, [ refreshLocation, isLabTestSkipped ]);

  const Title = () =>
    (
      <div className={styles.Title} data-automation="labs-title">
        We&apos;ve ordered &nbsp;
        <span className={styles.TitleInner}>
          {labTestDetails && labTestDetails?.testOrder.length} {labTestDetails && labTestDetails?.testOrder?.length > 1 ? `labs` : `lab`}&nbsp;
        </span>
        for you!
      </div>
    );

  const isSkeletonShown = useStepperSkeleton(StepperTitles.LABS);

  if (showSkeletonForLabs(isSkeletonShown, loading)) {
    return <DataContainerSkeleton />;
  }

  if (!labTestDetails) {
    return <ErrorLabs />;
  }

  if (isLabOrderCountZero(labTestDetails)) {
    return (
      <>
        <StartCheckoutNavbar
          heading={'Labs'}
          IconImage={labsFilledIcon}
          actionButtonText="Next"
          isDetails={false}
          onAction={() =>
            skip()}
        />
        <NoSuggestedOrder title="No suggested Lab Tests" icon={labsPinkBackground} />
      </>
    );
  }

  const labLocationList = () => {
    if (isLabLocationSkeletonShown) {
      return <LocationsContainerSkeleton />;
    }
    return (
      labsLocation ? (
        <LocationsList
          heading={'Select your preferred location.'}
          subHeading={'Walk-In Lab Locations Nearby your home.'}
          locations={labsLocation}
          activeLocation={activeLabLocation}
          setActiveLocation={setActiveLabLocation}
          reset={reset}
          savedLocation={savedLocation}
          parentName={'Labs'}
        />
      ) : (
        <NoDataAvailable
          message="Locations Not Available"
          iconHeight="7.25rem"
          iconWidth="7.25rem"
          height={22}
          isMessage={true}
          isMessageDetail={true}
          messageDetail="Unable to fetch the lab locations. Please try again."
          icon={noProvider}
          isButton={true}
          buttonText="Try Again"
          onClick={() => 
            dispatch(getLabsLocation(patientLatLng?.lat, patientLatLng?.lng))}
        />
      )
    );
  };

  return (
    <div>
      <StartCheckoutNavbar
        heading={'Labs'}
        {...((resetLabs && isLabTestScheduled) ? { actionButtonText: 'Next' } : { actionButtonText: 'Save' })}
        {...((resetLabs && isLabTestScheduled) ? {} : { skipButtonText: 'Skip' })}

        IconImage={labsFilledIcon}
        isDetails={false}
        onAction={(resetLabs && isLabTestScheduled)
          ? () => { skip(); }
          : () => {
            setSavedLocation(activeLabLocation);
            onComplete();
            dispatch(saveLabTestDetails({
              type: 'L',
              lab_ID: activeLabLocation.id,
              lab_Location: formatAddressBottomLine({
                city: activeLabLocation.location.address.city,
                postalCode: activeLabLocation.location.address.zipCode,
                state: activeLabLocation.location.address.state,
              }),
              lab_Name: activeLabLocation.location.address.addressLine,
              createdDate: new Date().toISOString(),
              appointment_ID: appointmentId,
              patient_ID: patientId,
              skipped: false,
            }));
        }
        }
        onSkip={() => {
          scheduledLabTestDetails && dispatch(setLabsTestSkipped());
          !scheduledLabTestDetails && 
          dispatch(saveLabTestDetails({
            type: 'L',
            lab_ID: '',
            lab_Location: '',
            lab_Name: '',
            createdDate: new Date().toISOString(),
            appointment_ID: appointmentId,
            patient_ID: patientId,
            skipped: true,
          }));
        }}
        primaryButtonDisable={!labsLocation}
      />
      <div className={styles.LabsWrapper}>
        <div className={styles.LabsWrapperData}>
          <TestsList
            title={<Title />}
            tests={Tests}
          />
          {(resetLabs && isLabTestScheduled) ?
            (
              <div className={styles.LabsWrapperDataSavedLocation} >
                <p className={styles.LabsWrapperDataSavedLocationHeading}>Saved Location</p>
                <p className={styles.LabsWrapperDataSavedLocationValue} data-automation={`labs-saved-location`}>
                  <span className={styles.LabsWrapperDataSavedLocationValueName}>
                    {scheduledLabTestDetails?.lab_Name} </span>
                  {scheduledLabTestDetails?.lab_Location}
                </p>
                <div
                  className={styles.LabsWrapperDataSavedLocationPrevBtn}
                  onClick={() => {
                    setResetLabs(false);
                  }
                  }
                  onKeyDown={noop}
                  role="button"
                  tabIndex={0}
                  data-automation={`labs-change-location`}
                >Change Location</div>
              </div>
            )
            :
            labLocationList()
          }

        </div>
        
        
        <div className={styles.LabsWrapperLocation}>
          {activeLabLocation && labsLocation && (
            <><div className={styles.LabsWrapperLocationMap}>
              <GoogleMaps
                from={patientAddress}
                to={`${activeLabLocation.name} ${formatAddressBottomLine({
                  city: activeLabLocation.location.address.city,
                  postalCode: activeLabLocation.location.address.zipCode,
                  state: activeLabLocation.location.address.state,
                })}`}
                id="labs-map"
              />
            </div><div className={styles.LabsWrapperLocationData}>
              {labsLocation && labsLocation[0] && (
              <LocationCard
                title={'Labs Location:'}
                location={(resetLabs && isLabTestScheduled) ?
                  labsLocation?.filter(location => 
                    (scheduledLabTestDetails?.
                      lab_Name.includes(location.location.address.addressLine)))[0] :
                activeLabLocation}
              />
                )}
            </div></>    
        )}
        </div>
      </div>
    </div>
  );
}

export default Labs;
