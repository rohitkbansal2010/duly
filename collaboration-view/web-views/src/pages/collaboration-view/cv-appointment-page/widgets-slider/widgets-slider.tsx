/* eslint-disable react/display-name*/
import noop from 'lodash/noop';
import {
  useCallback,
  useEffect,
  useMemo,
  useState,
  SyntheticEvent
} from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
//eslint-disable-next-line import/no-unresolved
import { Swiper, SwiperSlide } from 'swiper/react';
import SwiperClass from 'swiper/types/swiper-class';

import { appointmentModules, appointmentModulesWidgets } from '@constants';
import { AppointmentModuleWidgetsHeaderTitles } from '@enums';
import { WidgetTitle } from '@pages/collaboration-view/cv-appointment-page/widgets-slider/widget-title';
import { getWidgetsData, setWidgetsList } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { WidgetDataType } from '@types';

import { WidgetsMap } from './widgets-map';
import styles from './widgets-slider.scss';

const WidgetTitlesOverrides: Record<string, string> = { [AppointmentModuleWidgetsHeaderTitles.CONDITIONS]: 'Health Conditions' };

type SlideRendererProps = {
  isActive: boolean;
  isPrev: boolean;
  isNext: boolean;
};

const getSlideRenderer = (widget: WidgetDataType, onClickHandler: (e: SyntheticEvent) => void) =>
  ({ isActive, isPrev, isNext }: SlideRendererProps) => {
    const bodyClassname = []
      .concat(isActive ? styles.cvWidgetBodyActive : styles.cvWidgetBodyInactive)
      .concat(isPrev ? styles.cvWidgetBodyPrev : [])
      .concat(isNext ? styles.cvWidgetBodyNext : [])
      .concat(styles.cvWidgetBody)
      .join(' ');

    const Widget = WidgetsMap[widget.route];

    return (
      <div className={bodyClassname}>
        <WidgetTitle>
          {isActive
            ? (WidgetTitlesOverrides[widget.headerTitle] || widget.headerTitle)
            : <>&nbsp;</>}
        </WidgetTitle>
        <div
          className={styles.cvWidgetContentWrapper}
          role="none"
          onClickCapture={isActive ? noop : onClickHandler}
        >
          <Widget />
        </div>
      </div>
    );
  };

export type ContentPropsType = {
  widgets: WidgetDataType[];
  onWidgetChange: (widget: WidgetDataType) => void;
};

export const WidgetsSlider = ({ widgets = [], onWidgetChange = noop }: ContentPropsType) => {
  const dispatch: AppDispatch = useDispatch();
  const {
    appointmentWidgetRoute,
    appointmentModuleRoute,
    appointmentId,
  } = useParams<{
    appointmentWidgetRoute: string;
    appointmentModuleRoute: string;
    appointmentId: string;
  }>();

  const widgetsList = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.widgetsList);
  const navigation = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.navigation);
  const patientId = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientId);

  const [ swiper, setSwiper ] = useState<SwiperClass | null>(null);

  const onSwiper = (swiperInstance: SwiperClass) =>
    setSwiper(swiperInstance);

  const changeWidget = useMemo(
    () =>
      (widgetIndex?: number) => {
        if (widgetIndex == undefined) return;

        const foundWidget = widgets?.[widgetIndex];

        if (foundWidget) {
          onWidgetChange(foundWidget);
        }
      },
    [ onWidgetChange, widgets ]
  );

  const onSlideClick = useCallback(
    (widgetIndex: number) =>
      (e: SyntheticEvent) => {
        e.stopPropagation();
        swiper?.slideTo(widgetIndex);
        changeWidget(widgetIndex);
      },
    [ changeWidget, swiper ]
  );

  useEffect(() => {
    if (swiper) {
      if (appointmentWidgetRoute) {
        const index = widgets.findIndex(widget =>
          widget.route === appointmentWidgetRoute);

        swiper.slideTo(index);
      } else {
        onWidgetChange(widgets[0]);
      }
    }
  }, [ appointmentWidgetRoute, swiper, widgets, onWidgetChange ]);

  useEffect(() => {
    if (appointmentModuleRoute) {
      const currentModuleAlias = appointmentModules.find(({ route }) =>
        `/${appointmentModuleRoute}` === route)?.alias || appointmentModules[0]?.alias;

      const currentWidgetAlias = appointmentModulesWidgets[currentModuleAlias]
        .find(({ route }) =>
          route === appointmentWidgetRoute)?.alias ||
            appointmentModulesWidgets[currentModuleAlias][0]?.alias;

      dispatch(setWidgetsList(currentModuleAlias, currentWidgetAlias));
    }
  }, [ dispatch, appointmentModuleRoute ]);

  useEffect(() => {
    if (widgetsList && navigation) {
      dispatch(getWidgetsData({
        widgetsList,
        navigation,
        patientId,
        appointmentId,
      }));
    }
  }, [ dispatch, navigation, patientId, appointmentId ]);

  const onSlideChange = useCallback(() => {
    changeWidget(swiper?.activeIndex);
  }, [ changeWidget, swiper?.activeIndex ]);

  return (
    <main className={styles.cvAppointmentPageContentWrapper}>
      <div className={styles.cvAppointmentPageSwiperWrapper}>
        <Swiper
          onSwiper={onSwiper}
          onSlideChange={onSlideChange}
          className={styles.cvAppointmentPageSwiper}
          slidesPerView={2}
          spaceBetween={0}
          centeredSlides={true}
          shortSwipes={false}
          threshold={100}
          watchSlidesProgress={true}
        >
          {widgets.map((widget, widgetIndex) =>
          (
            <SwiperSlide key={widget.route}>
              {getSlideRenderer(widget, onSlideClick(widgetIndex))}
            </SwiperSlide>
          ))}
        </Swiper>
      </div>
    </main>
  );
};
