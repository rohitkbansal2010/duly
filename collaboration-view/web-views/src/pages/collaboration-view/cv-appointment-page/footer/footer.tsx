import noop from 'lodash/noop';
import {
  useCallback, useEffect, useRef, useState
} from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import { AppointmentMenu } from '@components/appointment-menu';
import { dulyLargeIcon } from '@icons';
import {
  clearAllAccordions,
  CurrentAppointmentAction,
  startDataFetch,
  stopDataFetch
} from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { WidgetDataType } from '@types';
import { NavSecondary } from '@ui-kit/nav-secondary';

import { CvAppointmentPageParams } from '../cv-appointment-page';

import { getActionByModule } from './config-action';
import styles from './footer.scss';

export type FooterPropsType = {
  widgets?: WidgetDataType[];
  onWidgetChange?: (w: WidgetDataType) => void;
}

export const Footer = ({
  widgets = [],
  onWidgetChange = noop,
}: FooterPropsType) => {
  const initRunWasPerformed = useRef<boolean>(false);

  const { appointmentWidgetRoute, appointmentModuleRoute, appointmentId } =
    useParams<CvAppointmentPageParams>();

  const dispatch: AppDispatch = useDispatch();

  const {
    widgetsList, navigation, patientId, testReports, testReportsResults,
  } = useSelector(
    ({ CURRENT_APPOINTMENT }: RootState) =>
      CURRENT_APPOINTMENT
  );

  const isDataFetched = useSelector(({ UI }: RootState) =>
    UI.isDataFetched);

  const {
    medications,
    allergies,
    todaysVitals,
    chart,
    immunizations,
    conditions,
    patientAppointments,
  } = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS);

  const followUpDetails = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS.followUpOrderDetails);

  const [ activeWidget, setActiveWidget ] = useState<WidgetDataType | undefined>();

  const refreshButtonClass = `${styles.refreshButton} ${isDataFetched ? styles.refreshButtonActive : ''
  }`;

  const makeWidgetActive = useCallback(
    (widget: WidgetDataType) => {
      setActiveWidget(widget);
      onWidgetChange(widget);
    },
    [ onWidgetChange ]
  );

  const onClickLogo = () => {
    if (!followUpDetails && appointmentModuleRoute && appointmentWidgetRoute) dispatch(
      getActionByModule(
        {
          widgetsList,
          navigation,
          patientId,
          appointmentId,
        },
        `/${appointmentModuleRoute}`
      ) as CurrentAppointmentAction
    );
    dispatch(clearAllAccordions());
    dispatch(startDataFetch());
  };

  useEffect(() => {
    let foundWidget = widgets.find(widget =>
      widget.route === appointmentWidgetRoute);

    if (!foundWidget && !initRunWasPerformed.current) {
      foundWidget = widgets[0];
    }

    if (foundWidget) {
      makeWidgetActive(foundWidget);
      initRunWasPerformed.current = true;
    } else {
      setActiveWidget(undefined);
    }
  }, [ widgets, appointmentWidgetRoute, makeWidgetActive ]);

  //TODO: refactor the logic within DPGECLOF-2824
  useEffect(() => {
    if (
      (medications &&
        allergies &&
        todaysVitals &&
        chart &&
        immunizations &&
        conditions &&
        patientAppointments) ||
      (testReports && testReportsResults) ||
      !!followUpDetails
    ) {
      dispatch(stopDataFetch());
    }
  }, [
    dispatch,
    medications,
    allergies,
    todaysVitals,
    chart,
    immunizations,
    conditions,
    patientAppointments,
    testReports,
    testReportsResults,
    followUpDetails,
  ]);

  return (
    <footer className={styles.cvAppointmentPageFooterWrapper}>
      <button className={refreshButtonClass} onClick={isDataFetched ? onClickLogo : noop}>
        <img src={dulyLargeIcon} alt="Logo" />
      </button>
      <div className={styles.navSecondaryWrapper}>
        {Boolean(widgets.length) && (
          <NavSecondary
            widgets={widgets}
            onWidgetSelected={makeWidgetActive}
            activeWidget={activeWidget}
          />
        )}
      </div>
      <div className={styles.navPrimaryWrapper}>
        <AppointmentMenu />
      </div >
    </footer >
  );
};
