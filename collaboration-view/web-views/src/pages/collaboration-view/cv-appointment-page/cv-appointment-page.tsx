import { useCallback, useEffect, useMemo } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router';
import { useHistory } from 'react-router-dom';

import { LogoutExpirationModal } from '@components/logout-expiration-modal';
import { LogoutModal } from '@components/logout-modal';
import { PatientViewModal } from '@components/patient-view-modal';
import { appointmentModules, appointmentModulesWidgets } from '@constants';
import { AppointmentModulesRoutes, AppointmentModulesAlias } from '@enums';
import { PageNotFound } from '@pages/page-not-found';
import {
  setCurrentAppointmentId,
  getConfigurationsSaga,
  setPatientId,
  setPractitionerId
} from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { WidgetDataType } from '@types';
import {
  decodeURLParams,
  getAppointmentModule,
  getLocationByModule
} from '@utils';

import styles from './cv-appointment-page.scss';
import { Footer } from './footer';
import { Header } from './header';
import { WidgetsSlider } from './widgets-slider';

const getNewWidgetUrl = (moduleRoute: string, widgetRoute: string) =>
  `${getLocationByModule(moduleRoute)}/${widgetRoute}`;

export type CvAppointmentPageParams = {
  appointmentWidgetRoute: string;
  appointmentModuleRoute: string;
  appointmentId: string;
  patientId: string;
  practitionerId: string;
};

export const CvAppointmentPage = () => {
  const history = useHistory();
  const dispatch: AppDispatch = useDispatch();

  const { isModuleMenuShown, modules } = useSelector(
    ({ CURRENT_APPOINTMENT }: RootState) =>
      CURRENT_APPOINTMENT
  );

  const {
    appointmentId,
    appointmentModuleRoute,
    patientId,
    practitionerId,
  } = useParams<CvAppointmentPageParams>();

  const [
    decodedAppointmentId,
    decodedPatientId,
    decodedPractitionerId,
  ] = decodeURLParams(appointmentId, patientId, practitionerId);

  useEffect(() => {
    dispatch(setCurrentAppointmentId(decodedAppointmentId));
    dispatch(getConfigurationsSaga(decodedPatientId));
    dispatch(setPatientId(decodedPatientId));
  }, [ dispatch, decodedAppointmentId, decodedPatientId ]);

  useEffect(() => {
    dispatch(setPractitionerId(decodedPractitionerId));
  }, [ dispatch, decodedPractitionerId ]);

  const widgets: WidgetDataType[] = useMemo(() => {
    const currentModuleAlias = appointmentModules.find(
      ({ route }) =>
        route === `/${appointmentModuleRoute}`
    )?.alias as AppointmentModulesAlias;

    const currentWidgetsList = modules.find(({ alias }) =>
      alias === currentModuleAlias)?.widgets || [];

    const currentWidgets = appointmentModulesWidgets[currentModuleAlias]
      .filter(({ alias }) =>
        currentWidgetsList.find(widget =>
          widget.alias === alias));

    return currentWidgets || [];
  }, [ appointmentModuleRoute, modules ]);

  const onWidgetChange = useCallback(
    (widget: WidgetDataType) =>
      history.push(getNewWidgetUrl(appointmentModuleRoute, widget.route)),
    [ appointmentModuleRoute, history ]
  );

  if (!(Object.values(AppointmentModulesRoutes).some(route =>
    route === `/${appointmentModuleRoute}`))) {
    return <PageNotFound />;
  }

  return (
    <div className={styles.cvAppointmentPageWrapper}>
      <Header />
      {isModuleMenuShown && (
        widgets.length
          ? <WidgetsSlider widgets={widgets} onWidgetChange={onWidgetChange} />
          : getAppointmentModule(appointmentModuleRoute)
      )}
      <Footer widgets={widgets} onWidgetChange={onWidgetChange} />
      <LogoutModal />
      <LogoutExpirationModal />
      <PatientViewModal />
    </div>
  );
};
