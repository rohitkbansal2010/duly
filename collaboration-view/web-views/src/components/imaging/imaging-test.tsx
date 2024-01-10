
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import { DulyLoader } from '@components/duly-loader';
import GoogleMaps from '@components/google-maps/google-maps';
import { LocationCard } from '@components/location-card/location-card';
import { LocationsList } from '@components/locations-list/locations-list';
import StartCheckoutNavbar from '@components/start-checkout-navbar/start-checkout-navbar';
import { TestsList } from '@components/tests-list/tests-list';
import { followUpAppointment } from '@icons';
import { saveImagingTestDetails } from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import { Location } from '@types';

import { ImagingDataType } from './imaging';
import styles from './imaging.scss';


type ImagingTestProps = {
    reset: boolean;
    imagingData?: ImagingDataType;
    activeImagingLocation: Location;
    setActiveImagingLocation: React.Dispatch<React.SetStateAction<Location>>;
    renderImagingType: () => void;
    allTests: string[] | undefined;
    step: number;
    activeTest: number;
    setActiveTest: React.Dispatch<React.SetStateAction<number>>
    heading: string;
    handleSkip: () => void;
}

const ImagingTest = ({
  reset,
  imagingData,
  activeImagingLocation,
  setActiveImagingLocation,
  renderImagingType,
  step,
  allTests,
  heading,
}: ImagingTestProps) => {
  const patientData = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientData);
  const patientAddress: string = patientData?.address || '400 S.Eagle Street, Naperville, IL 60302';

  const { appointmentId, patientId } = useParams<{ patientId: string, appointmentId: string }>();
  const dispatch = useDispatch();


  const Title = () => 
    (
      <div className={styles.Title}>
        Let&apos;s schedule your&nbsp;
        <span className={styles.TitleInner}>
          Imaging Tests&nbsp;!
        </span>       
      </div>
    );

  return (
    <div className={styles.inProgress}>
      <StartCheckoutNavbar
        heading= {heading}
        actionButtonText="Continue"
        skipButtonText="Skip"
        IconImage={followUpAppointment}
        isDetails={false}
        onAction={renderImagingType}
        onSkip={() =>{
          dispatch(saveImagingTestDetails({
            provider_ID: '',
            patient_ID: patientId,
            location_ID: 'locationId',
            appointment_ID: appointmentId,
            imagingType: allTests ? allTests[step] : '',
            imagingLocation: `${activeImagingLocation.location.address.addressLine}, ${activeImagingLocation.location.address.city}, ${activeImagingLocation.location.address.state}, ${activeImagingLocation.location.address.zipCode}`,
            bookingSlot: '',
            aptScheduleDate: new Date().toISOString(),
            skipped: true,
          }));}
        }
        primaryButtonDisable={!imagingData?.locations}
      />
      <div className={styles.ImagingWrapper}>
        <div className={styles.ImagingWrapperData}>
          {imagingData ?
          (
            <>
              {allTests && (
              <TestsList
                title={<Title />}
                step= {step}
                tests={allTests}
              />
            )}
              <LocationsList
                heading={'Imaging Locations Nearby'}
                subHeading={'Select a location that is closest to your home.'}
                locations={imagingData.locations}
                activeLocation={activeImagingLocation}
                setActiveLocation={setActiveImagingLocation}
                reset={reset}
                parentName={'Imaging'}
              />
            </>
          ) : <DulyLoader width={8}/>
          }

        </div>
        <div className={styles.ImagingWrapperLocation}>
          <div className={styles.ImagingWrapperLocationMap} >
            {activeImagingLocation &&
            <GoogleMaps from={patientAddress} to={activeImagingLocation.name} id={`imaging-map`}/> }
          </div>
          <div className={styles.ImagingWrapperLocationData} >
            <LocationCard title={'Imaging Location:'} location={activeImagingLocation} />
          </div>
        </div>
      </div>
    </div>
  );
};

export default ImagingTest;
