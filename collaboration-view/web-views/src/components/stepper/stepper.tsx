
import { ReactNode, useEffect, useState } from 'react';
import { Tab, Nav } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import { checkCircleWhite } from '@icons';
import { AfterVisitSteps } from '@mock-data';
import { RootState } from '@redux/reducers';
import { CheckoutSideNavType, Stepper as StepperData } from '@types';

import styles from './stepper.scss';

type StepperProps = {
  data: CheckoutSideNavType;
  activeStep?: string;
  header?: ReactNode;
  content: Array<ReactNode>;
  setActiveStep?: (...args: any[]) => any;
};
function Stepper({
  data, activeStep, header, content, setActiveStep,
}: StepperProps) {
  const [ activeKey, setActiveKey ] = useState(activeStep);
  const [ description, setDescription ] = useState<string[]>([]);
  const [ isFollowUpSkipped, setIsFollowUpSkipped ] = useState(false); 
  const [ isReferralSkipped, setIsReferralSkipped ] = useState(false); 
  const [ isImagingTestSkipped, setIsImagingTestSkipped ] = useState(false); 
  const [ isLabTestSkipped, setIsLabTestSkipped ] = useState(false); 
  const {
    referralDetails,
    imagingTestDetails,
    scheduledFollowUpDetails,
    scheduledReferralDetails,
    scheduledImagingTestDetails,
    scheduledLabTestDetails,
    isFollowUpScheduled,
    isLabTestScheduled,
    isImagingTestScheduled,
    isReferralScheduled,
    isPrescriptionsChecked,
    labTestDetails,
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);

  const prescriptionData = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.medications);

  const regularPrescription = 
    prescriptionData && prescriptionData.regular ? prescriptionData?.regular : [];
  const otherPrescription = 
    prescriptionData && prescriptionData.other ? prescriptionData?.other : [];

  const prescriptionDetails = [ ...regularPrescription, ...otherPrescription ].length;

  useEffect(() => {
    setActiveKey(activeStep);
  }, [ activeStep ]);

  useEffect(()=>{
    if(isReferralScheduled){
      setIsReferralSkipped(false);
    }
    else if(scheduledReferralDetails && scheduledReferralDetails.length > 0){
      setIsReferralSkipped(true);
    }
    else{
      setIsReferralSkipped(false);
    }
  }, [ isReferralScheduled, scheduledReferralDetails ]);

  useEffect(()=>{
    if(isImagingTestScheduled){
      setIsImagingTestSkipped(false);
    }
    else if(scheduledImagingTestDetails && scheduledImagingTestDetails.length > 0){
      setIsImagingTestSkipped(true);
    }
    else{
      setIsImagingTestSkipped(false);
    }
  }, [ isImagingTestScheduled, scheduledImagingTestDetails ]);

  useEffect(()=>{
    if(isFollowUpScheduled){
      setIsFollowUpSkipped(false);
    }
    else if(scheduledFollowUpDetails && scheduledFollowUpDetails.length > 0){
      setIsFollowUpSkipped(true);
    }
    else{
      setIsFollowUpSkipped(false);
    }
  }, [ isFollowUpScheduled, scheduledFollowUpDetails ]);

  useEffect(()=>{
    if(isLabTestScheduled){
      setIsLabTestSkipped(false);
    }
    else if(scheduledLabTestDetails){
      setIsLabTestSkipped(true);
    }
    else{
      setIsLabTestSkipped(false);
    }
  }, [ isLabTestScheduled, scheduledLabTestDetails ]);

  const getSrcIcon = (
    step: StepperData | undefined, 
    isScheduled: boolean, 
    partialScheduled = false
  ) => {
    if (partialScheduled)
      return step?.partialIcon;
    else if (isScheduled)
      return step?.completeIcon;
    else return step?.defaultIcon;
  };

  const isPartialScheduled = (type: string) => {
    let totalDetails;
    let scheduledDetails;
    let totalDetailsLength = 0;
    if (type === 'imaging') {
      totalDetails = imagingTestDetails;
      scheduledDetails = scheduledImagingTestDetails;
      totalDetailsLength = totalDetails?.orderCount ? totalDetails.orderCount : 0;
    }
    else {
      totalDetails = referralDetails;
      scheduledDetails = scheduledReferralDetails;
      totalDetailsLength = totalDetails ? totalDetails.length : 0;
    }
    let count = 0;
    if (scheduledDetails) {
      scheduledDetails.forEach((detail)=>{
        if(!detail.skipped) count++;
      });
    }
    if (count < totalDetailsLength && count > 0)
      return true;
    else return false;
  };

  const getSrcValue = (step: StepperData | undefined) => {
    if (step?.value === activeKey) return step?.currentIcon;
    else {
      if (step?.value.includes('follow')) {
        return getSrcIcon(step, isFollowUpScheduled);
      }
      else if (step?.value.includes('lab')) {
        return getSrcIcon(step, isLabTestScheduled);
      }
      else if (step?.value.includes('imaging')) {
        const ImagingTestPartialScheduled = isPartialScheduled('imaging');
        return getSrcIcon(step, isImagingTestScheduled, ImagingTestPartialScheduled);
      }
      else if (step?.value.includes('referrals')) {
        const ReferralPartialScheduled = isPartialScheduled('referral');
        return getSrcIcon(step, isReferralScheduled, ReferralPartialScheduled);
      }
      else if (step?.value.includes('prescriptions')) {
        if(!prescriptionDetails)
          return step.defaultIcon;
        return isPrescriptionsChecked ?
          step.completeIcon
          : step.defaultIcon;
      }
      else {
        return checkCircleWhite;
      }
    }
  };

  const skippedOrNot = (step: StepperData | undefined) => {
    if (step?.value === activeKey) return '';
    switch (step?.value) {
      case AfterVisitSteps.FOLLOW_UP:
        return (isFollowUpSkipped && !isFollowUpScheduled) ? ' (SKIPPED)' : '';
      case AfterVisitSteps.LAB:
        return (isLabTestSkipped && !isLabTestScheduled) ? ' (SKIPPED)' : '';
      case AfterVisitSteps.IMAGING:
        return (isImagingTestSkipped && !isImagingTestScheduled) ? ' (SKIPPED)' : '';
      case AfterVisitSteps.REFERRALS:
        return (isReferralSkipped && !isReferralScheduled) ? ' (SKIPPED)' : '';
      default:
        return '';
    }
  };

  const getImagingStatus = () => {
    const imagingDetailsLength = imagingTestDetails ? imagingTestDetails.orderCount : 0;
    let count = 0;
    if(scheduledImagingTestDetails) {
      scheduledImagingTestDetails.forEach((detail)=>{
        if(!detail.skipped) count++;
      });
    }
    const scheduledImagingTestsLength = count;
    const imagingTestsPending = imagingDetailsLength - scheduledImagingTestsLength > 0 ? `${imagingDetailsLength - scheduledImagingTestsLength} pending` : '';
    const imagingTestsScheduled = scheduledImagingTestsLength > 0 ? `${scheduledImagingTestsLength} scheduled` : '';
    if (imagingTestsPending && imagingTestsScheduled)
      return `(${imagingTestsScheduled}, ${imagingTestsPending})`;
    else if(imagingTestsScheduled)
      return `(${imagingTestsScheduled})`;
    else if(imagingTestsPending)
      return `(${imagingTestsPending})`;
    else
      return '(No Suggestions)';
  };

  const getReferralStatus = () => {
    const referralDetailsLength = referralDetails ? referralDetails.length : 0;
    let count = 0;
    if (scheduledReferralDetails) {
      scheduledReferralDetails.forEach((detail)=>{
        if(!detail.skipped) count++;
      });
    }
    const scheduledReferralDetailsLength = count;
    const referralPending = referralDetailsLength - scheduledReferralDetailsLength > 0 ? `${referralDetailsLength - scheduledReferralDetailsLength} pending` : '';
    const referralScheduled = scheduledReferralDetailsLength > 0 ? `${scheduledReferralDetailsLength} scheduled` : '';
    if (referralPending && referralScheduled)
      return `(${referralScheduled}, ${referralPending})`;
    else if(referralScheduled)
      return `(${referralScheduled})`;
    else if(referralPending)
      return `(${referralPending})`;
    else
      return '(No Suggestions)';
  };

  const getLabTestStatus = () => {
    if(labTestDetails && labTestDetails.testOrder?.length > 0){
      if(isLabTestScheduled){
        return '(1 Location saved)';
      }
      return (labTestDetails?.testOrder.length > 1 ? `(${labTestDetails?.testOrder.length} suggestions)` : `(${labTestDetails?.testOrder.length} suggestion)`);
    }
    return '(No Suggestions)';
  };

  const getPrescriptionStatus = () =>{
    if(!prescriptionDetails){
      return '(No Prescriptions)';
    }
    let prescriptionText = prescriptionDetails <= 1 ? `${prescriptionDetails} prescription` : `${prescriptionDetails} prescriptions`;
    prescriptionText = !isPrescriptionsChecked ? `(${prescriptionText})` : `(${prescriptionText} reviewed)`;
    return prescriptionText;
  };


  useEffect(() => {
    const descData = [];
    descData[0] = isFollowUpScheduled ? '(1 scheduled)' : '(1 pending)';
    descData[1] = getLabTestStatus();
    descData[2] = getImagingStatus();
    descData[3] = getReferralStatus();
    descData[4] = getPrescriptionStatus();
    setDescription([ ...descData ]);
  }, [
    isFollowUpScheduled,
    referralDetails,
    scheduledReferralDetails,
    imagingTestDetails,
    scheduledImagingTestDetails,
    prescriptionDetails,
    labTestDetails,
    isPrescriptionsChecked,
    isLabTestScheduled,
  ]);

  return (
    <>
      <div className={styles.contentWrapper}>
        <Tab.Container
          defaultActiveKey={`${activeStep}`}
          onSelect={(key) => {
            if (key === null) setActiveKey(activeStep);
            else {
              setActiveKey(key);
              if (setActiveStep) setActiveStep(key);
            }
          }}
        >
          <div className={styles.contentLeft}>
            {header}
            <div className={styles.stepsContainer}>
              <Nav className={styles.stepsContainerSteps}>
                {data.list.map((step: StepperData, index: number) =>
                (
                  <Nav.Item
                    className={styles.navbarItem}
                    key={`${step.value}`}
                    data-automation={`${step.value}`}
                  >
                    <Nav.Link
                      className={
                        step.value === activeKey ? styles.navbarItemActive : styles.navbarItemLink
                      }
                      eventKey={`${step.value}`}
                    >
                      <div
                        className={
                          step.value === activeKey
                            ? `${styles.stepsContainerStepsStep} ${styles.activeTab}`
                            : styles.stepsContainerStepsStep
                        }
                      >
                        <div className={styles.stepsContainerStepsStepDiv}>
                          {index === data.list.length - 1 ? (
                            <img
                              className={styles.stepsContainerStepsLastStepIcon}
                              src={getSrcValue(step)}
                              alt="step-icon"
                            />
                          ) : (
                            <img
                              className={styles.stepsContainerStepsStepIcon}
                              src={getSrcValue(step)}
                              alt="step-icon"
                            />
                          )}
                        </div>
                        <div className={styles.stepsContainerStepsStepText}>
                          {index !== data.list.length - 1 ?
                            (
                              <>
                                <div>
                                  <span className={styles.stepsContainerStepsStepTitle}>
                                    {step.text}
                                  </span>
                                  <span>{skippedOrNot(step)}</span>
                                </div>
                                <div className={styles.stepsContainerStepsStepDetail}>
                                  {`${description[index]}`}
                                </div>
                              </>
                            ) : (
                              <div className={styles.stepsContainerStepsStepTitle}>
                                <span>{step.text}</span>
                              </div>
                            )
                          }
                        </div>
                      </div>
                    </Nav.Link>
                  </Nav.Item>
                ))}
              </Nav>
            </div>
          </div>
          <div className={activeKey === 'next-steps-complete' ? styles.contentRightAfterVisit : styles.contentRight}>
            <Tab.Content>
              {content.map((stepContent: ReactNode, index) =>
              (
                data.list[index] &&
                (
                <Tab.Pane
                  eventKey={`${data.list[index].value}`}
                  key={index}
                  active={data.list[index].value === activeKey}
                  data-automation={`${data.list[index].value}-tab-pane`}
                >
                  {stepContent}
                </Tab.Pane>
)
              ))}
            </Tab.Content>
          </div>
        </Tab.Container>
      </div>
    </>
  );
}

export default Stepper;
