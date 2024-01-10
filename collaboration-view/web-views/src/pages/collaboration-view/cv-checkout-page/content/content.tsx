import { ReactNode, useEffect, useState } from 'react';

import { AfterVisitSteps } from '@components/after-visit-steps-complete/after-visit-steps';
import ScheduleFollowUp from '@components/checkout-data-container/shedule-follow-up';
import { CheckoutSideNav } from '@components/checkout-side-nav';
import { Imaging } from '@components/imaging/imaging';
import Labs from '@components/labs/labs';
import Prescriptions from '@components/prescriptions/prescriptions';
import { Referrals } from '@components/referrals/referrals';
import Stepper from '@components/stepper/stepper';
import { SideNavMockData } from '@mock-data';
import { ScheduleFollowUpAppointmentData, ReferralData } from '@types';

import { CurrentStepType } from '../checkout-page';

type ContentPropsType = {
  currentStep?: CurrentStepType;
  getStepsData?: (...args: any[]) => any;
};
export const Content = ({ currentStep, getStepsData }: ContentPropsType) => {
  useEffect(() => {
    localStorage.setItem('fromAfterVisit', '1');
  }, []);
  const [ stepData, setStepData ] = useState(SideNavMockData);
  const [ followUpData, setFollowUpData ] = useState<ScheduleFollowUpAppointmentData>();
  const [ referralData, setReferralData ] = useState<Array<ReferralData>>();
  const [ currentStepData, setCurrentStepData ] = useState<CurrentStepType>();

  const [ activeKey, setActiveKey ] = useState('');

  const skipScheduleFollowUp = () => {
    setActiveKey(SideNavMockData.list[1].value);
  };
  const skipLabs = () => {
    setActiveKey(SideNavMockData.list[2].value);
  };

  const skipImaging = () => {
    setActiveKey(SideNavMockData.list[3].value);
  };

  const skipReferral = () => {
    setActiveKey(SideNavMockData.list[4].value);
  };

  const skipPrescription = () => {
    setActiveKey(SideNavMockData.list[5].value);
  };

  useEffect(() => {
    setCurrentStepData(currentStep);
    if (currentStep?.step) setActiveKey(currentStep.step);
    else setActiveKey(stepData.list[0].value);
  }, [ currentStep ]);
  useEffect(() => {
    const data = { ...stepData };
    data.list[0].data = followUpData;
    setStepData(data);
  }, [ followUpData ]);

  useEffect(() => {
    const data = { ...stepData };
    data.list[3].data = referralData;
    setStepData(data);
  }, [ referralData ]);
  useEffect(() => {
    if (getStepsData) getStepsData(stepData);
  }, [ stepData ]);
  const completeScheduleFollowUp = () => {
    const data = { ...stepData };
    data.list[0].completed = true;
    setStepData(data);
  };

  const saveLabs = () => {
    const data = { ...stepData };
    data.list[1].completed = true;
    setStepData(data);
  };

  const completeImaging = () => {
    const data = { ...stepData };
    data.list[2].completed = true;
    setStepData(data);
    skipImaging();
  };

  const completeReferral = () => {
    const data = { ...stepData };
    data.list[3].completed = true;
    setStepData(data);
  };

  const savePrescription = () => {
    const data = { ...stepData };
    data.list[4].completed = true;
    setStepData(data);
  };
  const updateActiveKey = (key: string) => {
    setActiveKey(key);
    if (currentStepData) {
      if (stepData.list[3].value === currentStepData?.step) {
        if (stepData.list[3].data.length <= 1) currentStepData.section = 'ChooseProvider';
      } else {
        currentStepData.section = '';
      }
    }
  };
  const content: Array<ReactNode> = [];
  content.push(
    <ScheduleFollowUp
      defaultActiveKey={
        stepData.list[0].value === currentStepData?.step ? currentStepData?.section : ''
      }
      skip={skipScheduleFollowUp}
      onComplete={completeScheduleFollowUp}
      setFollowUpData={setFollowUpData}
      reset={activeKey === SideNavMockData.list[0].value}
    />
  );

  content.push(
    <Labs
      reset={activeKey === SideNavMockData.list[1].value}
      skip={skipLabs}
      onComplete={saveLabs}
    />
  );

  content.push(
    <Imaging
      skip={skipImaging}
      onComplete={completeImaging}
      activeKey={activeKey}
      reset={activeKey === SideNavMockData.list[2].value}
    />
  );
  content.push(
    <Referrals
      activeSection={
        stepData.list[3].value === currentStepData?.step ? currentStepData?.section : ''
      }
      skip={skipReferral}
      onComplete={completeReferral}
      setReferralData={setReferralData}
      activeKey={activeKey}
      reset={activeKey === SideNavMockData.list[3].value}
    />
  );
  content.push(
    <Prescriptions
      onComplete={savePrescription}
      onSkip={skipPrescription}
      reset={activeKey === SideNavMockData.list[4].value}
    />
  );
  content.push(<AfterVisitSteps />);
  return (
    <>
      <Stepper
        data={stepData}
        activeStep={activeKey}
        setActiveStep={updateActiveKey}
        header={<CheckoutSideNav />}
        content={content}
      />
    </>
  );
};
