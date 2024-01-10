import { useEffect, useRef, useState } from 'react';
import { Card, Col } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import {
  appointmentItemUI,
  HEIGHT_HOUR_SEGMENT,
  MINUTES_PER_HOUR,
  PADDING_BOTTOM_SCHEDULER_SCALE,
  SEGMENT_IN_MINUTES
} from '@constants';
import { AppointmentType } from '@enums';
import { addUserWhiteIcon, videoCameraFillDarkBlueIcon } from '@icons';
import { setPickedAppointmentId, showBTGModal } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { AppointmentData } from '@types';
import {
  calcDurationInMinutes,
  calcTopOfAppointment,
  isTitleNotEmpty,
  writeHumanName,
  writeTimeInterval
} from '@utils';

import styles from './appointment-item.scss';

export type AppointmentItemProps = AppointmentData & {
  startTimeGroups: number;
};

export const AppointmentItem = ({
  id,
  title,
  type,
  timeSlot: {
    startTime,
    endTime,
  },
  patient: {
    patientGeneralInfo: {
      humanName: {
        familyName: patientFamilyName,
        givenNames,
      },
    },
    isNewPatient,
  },
  practitioner: {
    humanName: {
      familyName: practitionerFamilyName,
      prefixes = [],
    },
  },
  isProtectedByBtg,
  startTimeGroups,
}: AppointmentItemProps) => {
  const [ height, setHeight ] = useState<number>(0);

  const dispatch: AppDispatch = useDispatch();

  const duration = calcDurationInMinutes(new Date(startTime), new Date(endTime));
  const isDurationQuarter = duration === SEGMENT_IN_MINUTES;

  useEffect(() => {
    setHeight((duration / MINUTES_PER_HOUR) * HEIGHT_HOUR_SEGMENT - PADDING_BOTTOM_SCHEDULER_SCALE);
  }, [ startTime, endTime, duration ]);

  const ellipsisParentRef = useRef<HTMLDivElement>(null);
  const ellipsisChildRef = useRef<HTMLSpanElement>(null);
  const newPatientIconRef = useRef<HTMLSpanElement>(null);

  const [ isSpaceForNewPatientIcon, setIsSpaceForNewPatientIcon ] = useState(true);
  const [ isSpaceForText, setIsSpaceForText ] = useState(true);

  useEffect(() => {
    if (ellipsisParentRef.current && ellipsisChildRef.current && newPatientIconRef.current) {
      const diff = ellipsisParentRef.current.offsetWidth - ellipsisChildRef.current.offsetWidth;
      const iconWidth = newPatientIconRef.current.offsetWidth;
      setIsSpaceForNewPatientIcon(diff > iconWidth);
    } else {
      setIsSpaceForNewPatientIcon(true);
    }
  }, [ ellipsisParentRef, ellipsisChildRef, newPatientIconRef ]);

  const currentScreenWidth = useSelector(({ UI }: RootState) =>
    UI.width);

  useEffect(() => {
    if (ellipsisParentRef.current && currentScreenWidth) {
      const minWidth =
        currentScreenWidth / appointmentItemUI.BASIC_SCREEN_WIDTH * appointmentItemUI.MIN_WIDTH;
      setIsSpaceForText(ellipsisParentRef.current.offsetWidth > minWidth);
    } else {
      setIsSpaceForText(true);
    }
  }, [ ellipsisParentRef, currentScreenWidth ]);

  const handleCardClick = () => {
    isProtectedByBtg ? dispatch(showBTGModal()) : dispatch(setPickedAppointmentId(id));
  };

  return (
    <Col className={styles.appointmentContainer}>
      <Card
        className={styles.appointmentCard}
        style={{
          top: `${calcTopOfAppointment(new Date(startTime), new Date(startTimeGroups))}rem`,
          height: `${height}rem`,
        }}
      >
        {isNewPatient && isSpaceForNewPatientIcon && (
        <span ref={newPatientIconRef} className={styles.appointmentCardNewPatient}>
          <img src={addUserWhiteIcon} data-testid="appointment-item__new-patient-icon" alt="White add user" />
        </span>
				)}
        <div
          className={styles.appointmentCardBarLeft}
        />
        <Card.Body
          className={styles.appointmentCardBody}
          onClick={handleCardClick}
        >
          {isSpaceForText ? (
            <div className={styles.appointmentCardBodyContent}>
              <Card.Title
                ref={ellipsisParentRef}
                className={styles.appointmentCardBodyContentTitle}
              >
                <span ref={ellipsisChildRef} className={styles.ellipsis}>
                  <span
                    data-testid="appointment-item__patient-name"
                    className={styles.appointmentCardBodyContentHumanName}
                  >
                    {isProtectedByBtg ?
                      <span className={styles.btgRect} /> :
											writeHumanName(givenNames, patientFamilyName)
										}
                  </span>
                  {isTitleNotEmpty(title) && (
                  <>
                    <span className={styles.appointmentCardBodyContentHumanName}>
                      {isProtectedByBtg || ', '}
                    </span>
                    <span className={styles.appointmentCardBodyContentServiceType}>
                      {title}
                    </span>
                  </>
									)}
                  {type === AppointmentType.TELEHEALTH && (
                  <img
                    src={videoCameraFillDarkBlueIcon}
                    data-testid="appointment-item__telehealth-icon"
                    alt="Filled dark blue videocam"
                  />
									)}
                </span>
              </Card.Title>
              <div className={!isDurationQuarter ? 'd-flex flex-column' : styles.ellipsis}>
                <span
                  data-testid="appointment-item__practitioner-card"
                  className={`${styles.appointmentCardBodyContentPractitioner} ${styles.ellipsis}`}
                >
                  {`${writeHumanName(prefixes, practitionerFamilyName)}`.trim()}
                  {isDurationQuarter ? ', ' : ''}
                </span>
                <span className={`${styles.appointmentCardBodyContentTime} ${styles.ellipsis}`}>
                  {writeTimeInterval(new Date(startTime), new Date(endTime))}
                </span>
              </div>
            </div>
				) : (<div className={styles.dots}>...</div>)}
        </Card.Body>
      </Card>
    </Col>
  );
};
