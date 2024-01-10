import { noop } from 'lodash';
import { useEffect, useState } from 'react';
import { Modal, Button } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { Avatar } from '@components/avatar';
import { DulyLoader } from '@components/duly-loader';
import GoogleLocationAutocomplete from '@components/google-maps/google-location-autocomplete';
import { NoDataAvailable } from '@components/no-data-available/no-data-available';
import { CHECKOUT_PROVIDER_MODAL_LOADER_WIDTH, CHOOSE_PROVIDER_MODAL_IMAGE_WIDTH } from '@constants';
import {
  noProvider, purpalLocationIcon, crossIcon, groupIcon 
} from '@icons';
import { location as locationIcon } from '@images';
import {
  getLocationsList, 
  getReferralProviderDetails, 
  resetLoading, 
  setSelectedProviderInReferral, 
  stopLoading 
} from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import { ChooseProviderDetailsType, Location, ProviderDetailsType } from '@types';
import { firstLetterCapital, formatAddressBottomLine, getLatLngFromAddress } from '@utils';

import styles from './ChooseProvider.scss';


type ChooseProviderProps = {
  title: string;
  titleImage?: string;
  isOpen: boolean;
  toggleModal: () => void;
  ChooseProviderData?: any;
  setReferralKey?: React.Dispatch<React.SetStateAction<string>>;
  setProviderDetails?: React.Dispatch<React.SetStateAction<any>>;
  setLocation?: React.Dispatch<React.SetStateAction<string>>;
  setPincode?: React.Dispatch<React.SetStateAction<string>>;
  providerType?: string;
  step?: string;
  handleClose?: () => void;
  allTests?: string[],
  imagingStep?: number,
  setActiveImagingLocation?: React.Dispatch<React.SetStateAction<Location>>
};

export const ChooseProvider = ({
  isOpen,
  toggleModal,
  setReferralKey,
  setProviderDetails,
  providerType,
  step,
  title,
  handleClose,
  allTests,
  imagingStep,
  setActiveImagingLocation,
}: ChooseProviderProps) => {
  const errorText = title.includes('Location') ? 'No imaging available at this location. Please change your location' :
    'No provider is available for this location. Please change your location.';

  const patientData = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientData);
  const patientAddress: string = patientData?.address || '400 S.Eagle Street, NaperVille, IL 60302';

  const [ doctors, setDoctors ] = useState<ChooseProviderDetailsType[] | []>([]);


  const { 
    referralProviderDetailsList, 
    loading,
    locationLists,
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);
  const {
    errorButtonText, errorMessage, errorTitle, icon,
  } = useSelector(({ APPSTATE }: RootState) =>

    APPSTATE.ScheduleAppointmentErrorData);  
  const dispatch = useDispatch();
  const getDoctors = (loc: any) => {
    loc && dispatch(getReferralProviderDetails(loc.lat, loc.lng, providerType ? providerType : ''));
  };
  
  const getLocations = (city: any = 'Oak') => {
    if(allTests && imagingStep){
      dispatch(getLocationsList('Imaging', city, [ allTests[imagingStep] ]));
    }
  };

  

  const getDoctorDetails = (doctor: ChooseProviderDetailsType): ProviderDetailsType => 
    ({
      id: doctor.id.toString(),
      providerId: doctor.providerId,
      humanName: {
        familyName: doctor.providerDisplayName,
        givenNames: doctor.providerName.split(', '),
      },
      photo:{
        contentType: '',
        url: doctor.providerPhotoURL,
      },
      specialty: doctor.providerSpecialty,
      location: {
        id: doctor.locationId,
        address: {
          addressLine: doctor.locationAdd_1,
          addressLine2: '',
          city: doctor.city,
          state: doctor.locationState,
          zipCode: doctor.locationZip,
        },
        geographicCoordinates: {
          latitude: parseFloat(doctor.locationLatitudeCoordinate),
          longitude: parseFloat(doctor.locationLongitudeCoordinate),
        },
        phoneNumber: '',
        distance: doctor.distance,
      },
      department_Id: doctor.department_Id,
    });
  const setData = (data: {doctor: ChooseProviderDetailsType, type: string}) => {
    if(step === 'Referral') {
      dispatch(setSelectedProviderInReferral({
        doctor: getDoctorDetails(data.doctor),
        type: providerType ? providerType : '',
      }));
    }
  };
  const dispatchChooseProvider = ()=>{
    const loc:Promise<any> = getLatLngFromAddress(patientAddress);
    return loc && dispatch(getReferralProviderDetails(loc.lat, loc.lng, providerType ? providerType : ''));
  };

  useEffect(() => {
    if (referralProviderDetailsList){
      setDoctors(referralProviderDetailsList);
    }
  }, [ referralProviderDetailsList ]);


  const getMiles = (meters: number) => 
    (Math.round(meters * 0.000621371))
  ;
  const exceptionHandle = ()=>
    (
      <div>
        { errorTitle === 'Could\'nt get the data!' ? ( 
          <NoDataAvailable
            height={26}
            isMessage ={false}
            isMessageDetail={true}
            messageDetail={errorMessage}
            icon={icon}
            iconHeight="9.25rem"
            iconWidth="9.25rem"
            isButton={true}
            buttonText={errorButtonText}
            onClick={dispatchChooseProvider}
            contentLocation="flex-end"
          />                      
         ) : (                      
           <NoDataAvailable
             height={26}
             isMessage ={false}
             isMessageDetail={true}
             messageDetail={errorText}
             icon={noProvider}
             iconHeight="9.25rem"
             iconWidth="9.25rem"
           />
)} 
      </div>
    );

  const referralProviderData = () => 
    (
      doctors.length ?
        (
          <>
            <p className={styles.rpText}>RECOMMENDED PROVIDERS</p>
            <div className={styles.midCard}>
              {doctors.map((doctor, index) => {
          const len = doctors.length;
          return (
            <div key={index}>
              <div className={styles.doctorCard}>
                <div className={styles.doctorCardItem}>
                  <Avatar
                    src={doctor.providerPhotoURL}
                    alt={doctor.providerName}
                    width={CHOOSE_PROVIDER_MODAL_IMAGE_WIDTH}
                  />
                  <div className={styles.doctorInfo}>
                    <p className={styles.docName}>
                      Dr. {doctor.providerName}
                    </p>
                    <p className={styles.docSpecialization}>
                      {firstLetterCapital(doctor.providerSpecialty)}
                    </p>

                    <div className={styles.docLocationDiv}>
                      <img
                        src={purpalLocationIcon}
                        alt=""
                        className={styles.PurpalLocationIcon}
                      />
                      <span className={styles.docLocation}>
                        {doctor.city.toLowerCase()}
                      </span>
                      <span className={styles.locationDistance}>
                        ({getMiles(doctor.distance)}  miles from you)
                      </span>
                    </div>
                  </div>
                </div>

                <div className={styles.buttonDiv}>
                  <Button
                    type="button"
                    size="lg"
                    variant="light"
                    className={styles.selectBtn}
                    onClick={() => {
                      setData({ doctor, type: providerType ? providerType : '' });
                      setProviderDetails && 
                      setProviderDetails(getDoctorDetails(doctor));
                      dispatch(resetLoading());
                      setTimeout(() => {
                        setReferralKey &&
                        setReferralKey('ScheduleReferral');
                        toggleModal();
                      }, 500);
                    }}
                  >
                    <p className={styles.selectText}>Select</p>
                  </Button>
                </div>
              </div>

              <div>
                {len != index + 1 ? <hr className={styles.lineBreak} /> : null}
              </div>
            </div>
          );
        })}</div></>
        )
        : (
          exceptionHandle()
        ));

  const imagingProviderData = () => 
    (locationLists && locationLists?.length > 0 ?
      (
        <>
          <p className={styles.rpText}>RECOMMENDED LOCATIONS</p>
          <div className={styles.midCard}>
            {locationLists && locationLists.map((location, index) => 
                      (index < 3 &&
                        (
                        <div key={index}>
                          <div className={styles.doctorCard}>
                            <div className={styles.doctorCardItem}>
                              <Avatar
                                src={locationIcon}
                                alt="location"
                                width={CHOOSE_PROVIDER_MODAL_IMAGE_WIDTH}
                              />
                              <div className={styles.doctorInfo}>
                                <p className={styles.docName}>
                                  {location.location.address.addressLine}
                                </p>
                                <p className={styles.docSpecialization}>
                                  {formatAddressBottomLine(
                                    {
                                      city: location.location.address.city,
                                      postalCode: location.location.address.zipCode,
                                      state: location.location.address.state,
                                    }
                                  )}
                                </p>
            
                                <div className={styles.docLocationDiv}>
                                  <span className={styles.docLocation}>
                                    {location.location.distance}  miles from you
                                  </span>
                                </div>
                              </div>
                            </div>
            
                            <div>
                              <Button
                                type="button"
                                size="lg"
                                variant="light"
                                className={styles.selectBtn}
                                onClick={() => {
                                  setActiveImagingLocation && 
                                  setActiveImagingLocation(location);
                                  dispatch(stopLoading());
                                  setTimeout(() => {
                                    toggleModal();
                                  }, 500);
                                }}
                              >
                                <p className={styles.selectText}>Select</p>
                              </Button>
                            </div>
                          </div>
            
                          <div>
                            {index < 2 ? <hr className={styles.lineBreak} /> : null}
                          </div>
                        </div>
            )
                      ))}</div></>
      )
      : (
        exceptionHandle()
      ));
   
  return (
    <>
      <Modal
        show={isOpen}
        onExit={()=>{
          if(!title.includes('Location')){
            dispatch(resetLoading());
          }
          handleClose !== undefined && handleClose();
        }}
        dialogClassName={styles.cpModal}
        centered
      >
        <Modal.Header className={styles.modalHeader}>
          
          <div className={styles.subHeader}>
            {!title.includes('Location') ? (
              <>
                <img src={groupIcon} alt="" className={styles.groupIcon} />
                <span className={styles.headerText}>{title}</span>
              </>
            ) : 
              <span className={styles.headerTextImaging}>{title}</span>
            }
          </div>
          <div
            role="button"
            onKeyDown={noop}
            tabIndex={-1}
            onClick={() =>
              toggleModal()}
          >
            <img src={crossIcon} className={styles.headerCloseBtn} alt="" />
          </div>
        </Modal.Header>
        <Modal.Body className={styles.modalBody}>
          {loading && <DulyLoader width={CHECKOUT_PROVIDER_MODAL_LOADER_WIDTH} />}
          <div className={styles.locationTextDiv}>
            <p className={styles.locationText}>LOCATION</p>
            <p className={styles.locationInfo}>
              This is your home address. If you need to change location, tap and search
              by zip-code, city or address
            </p>
          </div>
          {providerType ? (
            <GoogleLocationAutocomplete
              placeholder={'Location'}
              defaultValue={patientAddress}
              onSelected={(value: any) => {
              getDoctors(value);
              }}
            /> 
        ) : (
          <GoogleLocationAutocomplete
            placeholder={'Location'}
            defaultValue={patientAddress}
            onSelected={(value: any) => {
            getLocations(value);
            }}
          />
        )}
          {(() => {
            if (loading === false) {
              if(!title.includes('Location')){
                return(
                  referralProviderData()
                );
              }else{
                return(
                  imagingProviderData()
                );
              }
            }
          })()
          }
        </Modal.Body>
      </Modal>
    </>
  );
};
