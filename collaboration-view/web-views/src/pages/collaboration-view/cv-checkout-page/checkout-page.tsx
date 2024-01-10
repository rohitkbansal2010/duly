import { useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import ScheduleAppointmentError from '@components/error/schedule-appointment-error/ScheduleAppointmentError';
import FontSizeSetting from '@components/font-size-setting/font-size-setting';
import { LogoutExpirationModal } from '@components/logout-expiration-modal';
import { LogoutModal } from '@components/logout-modal';
import { useSiteId } from '@hooks';
import { HeaderMockData, StepsListData } from '@mock-data';
import {
  getPatientData,
  setCurrentAppointmentId,
  setPractitionerId,
  getConfigurationsSaga,
  setPatientId
} from '@redux/actions';
import { 
  getFollowUpDetails, 
  getImagingTestDetails, 
  getLabTestDetails,
  getPrescriptionDetails, 
  getReferralDetails, 
  getScheduledData,
  setStepsList,
  startLoading
} from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { CheckoutHeaderType, CheckoutSideNavType } from '@types';
import { decodeURLParams, formatDateStringAddComma } from '@utils';

import { Footer } from '../cv-appointment-page/footer';

import styles from './checkout-page.scss';
import { Content } from './content';



export type CvCheckoutPageParams = {
  appointmentWidgetRoute: string;
  appointmentModuleRoute: string;
  appointmentId: string;
  patientId: string;
  practitionerId: string;
};
export type CurrentStepType = {
  step: string;
  section?: string;
};
export type HeaderPropType = {
  data: CheckoutHeaderType[];
  setCurrentStep: React.Dispatch<React.SetStateAction<any>>;
};

export const CheckoutPage = () => {
  const [ currentStep ] = useState<CurrentStepType>();
  const [ headerData, setHeaderData ] = useState(HeaderMockData);
  const dispatch: AppDispatch = useDispatch();
  const { appointmentId, patientId, practitionerId } = useParams<CvCheckoutPageParams>();
  const [ decodedAppointmentId, decodedPatientId, decodedPractitionerId ] = decodeURLParams(
    appointmentId,
    patientId,
    practitionerId
  );
  const { siteId } = useSiteId();
  const isDataFetched = useSelector(({ UI }: RootState) =>
    UI.isDataFetched);
  const updateHeader = (data: CheckoutSideNavType) => {
    const header = [ ...headerData ];
    if (data?.list[0]?.data?.length == 1) {
      header[0].title = formatDateStringAddComma(data?.list[0]?.data[0]?.date.toDateString());
      header[0].postTitle = data?.list[0]?.data[0]?.time;
    } else if (data?.list[0]?.data?.length > 1) {
      header[0].title = data?.list[0]?.data?.length + ' Follow up';
    }

    if (data?.list[3]?.data?.length > 0) {
      header[3].postTitle = 'View Details';
    }
    setHeaderData(header);
  };
  useEffect(() => {
    dispatch(startLoading());
    dispatch(getScheduledData(decodedAppointmentId));
    dispatch(setStepsList(StepsListData(true)));
    dispatch(setCurrentAppointmentId(decodedAppointmentId));
    dispatch(getConfigurationsSaga(decodedPatientId));
    dispatch(setPatientId(decodedPatientId));
    !isDataFetched && patientId && dispatch(getPatientData(patientId));
    dispatch(getFollowUpDetails({
      patientId: decodedPatientId, 
      siteId, 
      practitionerId: decodedPractitionerId,
    }));
    dispatch(getLabTestDetails(decodedPatientId, appointmentId));
    dispatch(getReferralDetails(decodedPatientId));
    dispatch(getPrescriptionDetails(decodedPatientId, decodedAppointmentId));
    dispatch(getImagingTestDetails(decodedPatientId, appointmentId));
  }, [ dispatch, decodedAppointmentId, decodedPatientId, patientId, isDataFetched ]);

  useEffect(() => {
    dispatch(setPractitionerId(decodedPractitionerId));
  }, [ dispatch, decodedPractitionerId ]);
  const setStepsData = (data: CheckoutSideNavType) => {
    updateHeader(data);
  };

  const ScheduleAppointmentErrorData = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.ScheduleAppointmentErrorData);

  return (
    <>
      <div className={styles.cvCheckoutPageWrap}>
        {/* <Header data={headerData} setCurrentStep={setCurrentStep} /> */}
        <Row className={styles.titleRow}>
          <Col md={11}> <div className={styles.cvCheckoutPageName}>After Visit Steps</div></Col>
          <Col md={1} > <div className={styles.fontSettingContainer}><FontSizeSetting /></div></Col>
        </Row>
        <Content currentStep={currentStep} getStepsData={setStepsData} />
        <div className={styles.checkoutFooter} >
          <Footer hideAppointmentMenu={true} />
        </div>
        <LogoutModal />
        <LogoutExpirationModal />
        <ScheduleAppointmentError
          modalErrorData={ScheduleAppointmentErrorData}
        />

      </div >
    </>
  );
};
