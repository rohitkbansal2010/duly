import { Accordion } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import { DataContainerSkeleton } from '@components/checkout-data-container/data-container-skeleton';
import GoogleMaps from '@components/google-maps/google-maps';
import { NoPreferredPharmacy } from '@components/no-preferred-pharmacy/no-preferred-pharmacy';
import { PrescriptionMedication } from '@components/prescription-medication';
import { StepperTitles } from '@enums';
import { useStepperSkeleton } from '@hooks';
import { prescriptionIcon } from '@icons';
import { RootState } from '@redux/reducers';
import { formatAddressBottomLine, getPatientAddressForLabs } from '@utils';

import styles from './prescription-accordian.scss';

export const PrescriptionAccordian = () => {
  const prescriptionData = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.medications);
  const { preferredPharmacyDetails } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);

  const patientData = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientData);
  const patientAddress: string = getPatientAddressForLabs(patientData);

  const isSkeletonShown = useStepperSkeleton(StepperTitles.PRESCRIPTION);

  const regularPrescription =
    prescriptionData && prescriptionData.regular ? prescriptionData?.regular : [];
  const otherPrescription =
    prescriptionData && prescriptionData.other ? prescriptionData?.other : [];

  const prescriptionArray = [ ...regularPrescription, ...otherPrescription ];

  const getAddress = () =>
    formatAddressBottomLine(
      {
        city: preferredPharmacyDetails ? preferredPharmacyDetails.city : '',
        postalCode: preferredPharmacyDetails ? preferredPharmacyDetails.zipCode : '',
        state: preferredPharmacyDetails ? preferredPharmacyDetails.state : '',
      }
    );
  const showNoPharmacyModel = () => {
    if (preferredPharmacyDetails !== null) {
      return <NoPreferredPharmacy noDataAvailable={true} />;
    }
    return <NoPreferredPharmacy />;
  };

  if (isSkeletonShown) {
    return <DataContainerSkeleton />;
  }

  return prescriptionArray
    && (
      <>
        <style type="text/css">
          {`
              .accordion-button:focus{
                box-shadow: none;
              }
              .accordian, .accordian*{
                border:none !important;
              }
              .accordian{
                padding: 1rem !important;
              }
              .accordion-body{
                padding: 1rem 0rem;
              }
              .accordion-item{
                border-radius: 0.625rem !important;
                border: none !important;
              }
              .accordion-button:not(.collapsed){
                color:rgba(15, 23, 42, 1) !important;
                background-color: white;
              }
              .container{
                max-width:100%;
              }
              .row > * {
                padding: 0;
              }
              .accordion-button:not(.collapsed){
                box-shadow: none;
              }
              .accordion-item:last-of-type .accordion-button.collapsed{
                padding-bottom: 0;
              }
              .accordion-item:last-of-type .accordion-button.collapsed{
                padding-bottom:0;
              }
            `}
        </style>
        <div className={styles.topDiv}>
          <div className={styles.accordianOuterDiv}>
            <Accordion defaultActiveKey="0" >
              <Accordion.Item eventKey="0" data-automation="after-steps-accordion-prescriptions">
                <Accordion.Header>
                  <div className={styles.accordianHeader}>
                    <img src={prescriptionIcon} alt="Prescription" />
                    <div>
                      <div className={styles.headerData}>
                        <div className={styles.pageHeading}>Prescriptions</div>
                        {preferredPharmacyDetails && (
                          <div className={styles.message}>
                            {`${prescriptionArray.length}`}&nbsp;
                            New Prescription{prescriptionArray?.length > 1 ? `s` : ``} have been sent to your pharmacy.
                          </div>
                        )}
                      </div>
                    </div>
                  </div>
                </Accordion.Header>
                <Accordion.Body>
                  <div className={styles.mainContainer}>
                    <div className={styles.medicationWrapper} >
                      {
                        prescriptionArray.map((medicationDetails, index) =>
                        (
                          <PrescriptionMedication
                            medicationDetails={medicationDetails}
                            key={index}
                          />
                        ))
                      }
                    </div>
                    <div className={styles.wrapperRightSide}>

                      {preferredPharmacyDetails ?
                        (
                          <><div className={styles.wrapperRightSideMap}>
                            <GoogleMaps
                              from={patientAddress}
                              to={preferredPharmacyDetails?.pharmacyName}
                              id="prescription-accordian-map"
                            />
                          </div><div className={`${styles.wrapperRightSidePharmacy} prescription-map-detail`}>
                            <p className={styles.wrapperRightSidePharmacyHeading}>
                              Your Preferred Pharmacy:
                            </p>
                            <div className={styles.wrapperRightSidePharmacyDetails}>
                              <div className={styles.wrapperRightSidePharmacyDetailsCol}>
                                <p className={styles.wrapperRightSidePharmacyDetailsColText}>
                                  <span
                                    className={styles.wrapperRightSidePharmacyDetailsColTextInner}
                                  >
                                    {preferredPharmacyDetails?.pharmacyName?.split(' ')[0]}<br />
                                  </span> {preferredPharmacyDetails?.addressLine1}<br />
                                  {getAddress()}
                                </p>
                              </div>
                              <div className={`${styles.wrapperRightSidePharmacyDetailsCol} ${styles.rightAlign}`}>
                                <p className={styles.wrapperRightSidePharmacyDetailsColText}>
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
                        ) : (
                          <div className={styles.noPharmacy}>
                            {showNoPharmacyModel()}
                          </div>
                        )}
                    </div>
                  </div>
                </Accordion.Body>
              </Accordion.Item>
            </Accordion>
          </div>
        </div>
      </>
    );
};
