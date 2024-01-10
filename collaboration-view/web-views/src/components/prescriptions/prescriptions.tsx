
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import { DataContainerSkeleton } from '@components/checkout-data-container/data-container-skeleton';
import { FadedScroll } from '@components/faded-scroll-2';
import GoogleMaps from '@components/google-maps/google-maps';
import { NoPreferredPharmacy } from '@components/no-preferred-pharmacy/no-preferred-pharmacy';
import { NoSuggestedOrder } from '@components/no-suggested-order';
import { PrescriptionMedication } from '@components/prescription-medication-card';
import StartCheckoutNavbar from '@components/start-checkout-navbar/start-checkout-navbar';
import { StepperTitles } from '@enums';
import { useStepperSkeleton } from '@hooks';
import { prescriptionIcon, prescriptionsPinkBackground } from '@icons';
import { getPrescriptionDetails, setPrescriptionsChecked, setStepsList } from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import { 
  formatAddressBottomLine, 
  getOtherPrescriptions, 
  getPatientAddressForLabs, 
  getRegularPrescriptions 
} from '@utils';

import { ErrorPrescription } from './error-prescription';
import styles from './prescriptions.scss';



type PrescriptionProps ={
  onComplete: ()=>void;
  onSkip: () => void;
  reset: boolean;
}
const Prescriptions = ({ onComplete, onSkip, reset }:PrescriptionProps) => {
  const prescriptionData = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.medications);
  const patientData = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientData);
  const { 
    preferredPharmacyDetails, 
    loading, 
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);
    
  const regularPrescription = getRegularPrescriptions(prescriptionData);
  const otherPrescription = getOtherPrescriptions(prescriptionData);

  const prescriptionArray = [ ...regularPrescription, ...otherPrescription ];
  const patientAddress: string = getPatientAddressForLabs(patientData);
  const { patientId, appointmentId } = useParams<{ 
    patientId: string,
    appointmentId: string
  }>();

  const dispatch = useDispatch();
  useEffect(()=>{
    if(reset){
      onComplete();
      dispatch(setPrescriptionsChecked());
      if (!prescriptionData) {
        dispatch(setStepsList([ { alias: StepperTitles.PRESCRIPTION, isSkeletonShown: true } ]));
        dispatch(getPrescriptionDetails(patientId, appointmentId, true));
      }
    }
  }, [ reset ]);

  const showNoPharmacyModel = ()=>{
    if(preferredPharmacyDetails !== null){
      return <NoPreferredPharmacy noDataAvailable={true} />;
    }
    return <NoPreferredPharmacy />;
  };

  const getAddress = () => 
    formatAddressBottomLine(
      {
        city: preferredPharmacyDetails ? preferredPharmacyDetails.city : '',
        postalCode: preferredPharmacyDetails ? preferredPharmacyDetails.zipCode : '',
        state: preferredPharmacyDetails ? preferredPharmacyDetails.state : '',
      }
    );

  const getProviderSubtitle = (uniqueProvider :Map<any, any>)=>{
    let providerSubtitle = '';
    let counter = 0;
    const uniqueProviderLength = uniqueProvider.size;
    uniqueProvider.forEach(function(value, key){
      const text = value === 1 ? 'prescription' : 'prescriptions';

      if(counter !== uniqueProviderLength - 1 && uniqueProviderLength !== 1){
        providerSubtitle += `${key} has added ${value} new ${text}, `;
      }
      else if (counter === uniqueProviderLength - 1 && uniqueProviderLength === 1){
        providerSubtitle += `${key} has added ${value} new ${text} `;
      }
      else {
        providerSubtitle += `and ${key} has added ${value} new ${text} `;
      }
      counter += 1;
    });
    return providerSubtitle;
  };


  const getPrescriptionByPrescriber = ()=>{
    const uniqueProvider = new Map();
    
    for(const prescription of prescriptionArray){
      if(prescriptionArray && prescription.provider !== undefined){
        uniqueProvider.set(`${prescription.provider?.prefixes} ${prescription.provider?.familyName}`, 0);
      }
      else if(prescriptionArray && prescription.provider === undefined){
        uniqueProvider.set('Your PCP', 0);
      }
    }
    for(const prescription of prescriptionArray)
    {
      if(prescriptionArray && prescription.provider !== undefined){
        const Name = `${prescription.provider?.prefixes} ${prescription.provider?.familyName}`;
        const k = uniqueProvider.get(Name);
        uniqueProvider.set(Name, k + 1) ;
      }
      else if(prescriptionArray && prescription.provider === undefined){
        const k = uniqueProvider.get('Your PCP');
        uniqueProvider.set('Your PCP', k + 1) ;
      }
    }
    return (
      <span className={styles.wrapperleftSidedetailssubTitle}>
        {getProviderSubtitle(uniqueProvider)} to your record </span>
    );
  };

  const isSkeletonShown = useStepperSkeleton(StepperTitles.PRESCRIPTION);

  if(isSkeletonShown || loading){
    return <DataContainerSkeleton />;
  }

  if (!prescriptionData) {
    return <ErrorPrescription />;
  }



  if(prescriptionArray && prescriptionArray.length === 0){
    return (
      <>
        <StartCheckoutNavbar
          heading="Prescriptions"
          IconImage={prescriptionIcon}
          isDetails={false}
          actionButtonText="Next"
          onAction={() =>
            onSkip()}
        />
        <NoSuggestedOrder title="No suggested Prescription" icon={prescriptionsPinkBackground} />
      </>
    );
  }

  return (
    <>
      <StartCheckoutNavbar
        heading="Prescriptions"
        IconImage={prescriptionIcon}
        referralIcon={true}
        isDetails={true}
        actionButtonText="Next"
        onAction={() =>
          onSkip()}
      />
      <div className={styles.wrapper}>
        <div className={styles.wrapperleftSide}>
          <div className={styles.wrapperleftSidedetails}>
            {preferredPharmacyDetails && preferredPharmacyDetails.pharmacyName !== '' && (
            <p className={styles.wrapperleftSidedetailsTitle} data-automation={`prescriptions-title`}>
              <span className={styles.wrapperleftSidedetailsTitleInner} data-automation={`prescriptions-title-count`}>
                {prescriptionArray?.length} new prescription{prescriptionArray?.length > 1 ? `s` : ``} {' '}</span>
              have been sent to your preferred pharmacy!</p>
)}
            <p className={styles.wrapperleftSidedetailssubTitle} data-automation={`prescriptions-subtitle`}>
              {getPrescriptionByPrescriber()}
              {preferredPharmacyDetails && preferredPharmacyDetails.pharmacyName !== '' && (
              <span className={styles.wrapperleftSidedetailssubTitle} >
                and are available for pickup at
                your preferred pharmacy,{' '}
                <span className={styles.wrapperleftSidedetailssubTitleInner}>
                  {preferredPharmacyDetails?.pharmacyName?.split(' ')[0]} on {preferredPharmacyDetails?.addressLine1}</span>
              </span>
                )}.</p>
          </div>
          <FadedScroll height="34rem" className={styles.wrapperleftSideCards}>
            {prescriptionArray?.map((medicationDetails, index) =>
            (
              <PrescriptionMedication 
                medicationDetails={medicationDetails} 
                key={index} 
                index={index}
              />
              ))}
          </FadedScroll>
        </div>
        
        <div className={styles.wrapperRightSide}>
          {preferredPharmacyDetails ? (
            <><div className={styles.wrapperRightSideMap}>
              <GoogleMaps
                from={patientAddress}
                to={preferredPharmacyDetails?.pharmacyName}
                id="prescription-map"
              />
            </div><div className={styles.wrapperRightSidePharmacy}>
              <p className={styles.wrapperRightSidePharmacyHeading} data-automation={`prescriptions-preferred-pharmacy-heading`}>Your Preferred Pharmacy:</p>
              <div className={styles.wrapperRightSidePharmacyDetails}>
                <div className={styles.wrapperRightSidePharmacyDetailsCol}>
                  <p className={styles.wrapperRightSidePharmacyDetailsColText} data-automation={`prescriptions-preferred-pharmacy-address`}>
                    <span className={styles.wrapperRightSidePharmacyDetailsColTextInner}>
                      {preferredPharmacyDetails?.pharmacyName?.split(' ')[0]}
                      <br /></span>{preferredPharmacyDetails?.addressLine1}<br />
                    {getAddress()}</p>
                </div>
                <div className={`${styles.wrapperRightSidePharmacyDetailsCol}  ${styles.alignRight}`}>
                  <p className={styles.wrapperRightSidePharmacyDetailsColText} data-automation={`prescriptions-preferred-pharmacy-contact-number`}>
                    {preferredPharmacyDetails?.phoneNumber}</p>
                  {preferredPharmacyDetails?.closingTime &&
                      (
                        <p className={styles.wrapperRightSidePharmacyDetailsColText}>Working Hours{' '}
                          <span className={styles.wrapperRightSidePharmacyDetailsColTextInner} data-automation={`prescriptions-preferred-pharmacy-closing-time`}>
                            {preferredPharmacyDetails?.closingTime}</span></p>
                      )}
                </div>
              </div>
            </div></>
      )
 : showNoPharmacyModel()
}
        </div>
      </div>
    </>
  );
};
export default Prescriptions;
