import { Accordion, Card } from 'react-bootstrap';

import { Avatar } from '@components/avatar';
import { CustomAccordionToggle } from '@components/custom-accordion-toggle';
import { PatientAppointmentStatus } from '@enums';
import {
  minusIcon, plusIcon, telehealthTimelineIcon, chatOrange
} from '@icons';
import { Practitioner } from '@types';
import { getSrcAvatar, getUserRole, showFirstPrefixItem } from '@utils';

import { formatDateAppointment, formatTimeAppointment } from '../helpers';

import styles from './timeline-appointments-group-items.scss';

export type TimelineAppointmentsGroupItemPropsType = {
  eventKey: string;
  activeKey: string | null;
  startTime: string;
  practitioner: Practitioner;
  reason: string;
  minutesDuration?: number;
  status: PatientAppointmentStatus;
  isTelehealth?: boolean;
  handleClickAppointment: () => void;
  showNoShowStatusCount: boolean;
  showNoShowCancelledCount: boolean;
};

export const TimelineAppointmentsGroupItem = ({
  eventKey,
  activeKey,
  startTime,
  practitioner: {
    photo,
    humanName: {
      prefixes,
      familyName,
      givenNames,
    },
    role,
    speciality,
  },
  reason,
  minutesDuration,
  status,
  isTelehealth = false,
  handleClickAppointment,
  showNoShowStatusCount,
  showNoShowCancelledCount,
}: TimelineAppointmentsGroupItemPropsType) => {
  const isRecentAppointmentsStatuses = [
    PatientAppointmentStatus.COMPLETED,
    PatientAppointmentStatus.CANCELLED,
    PatientAppointmentStatus.NO_SHOW,
  ];

  return (
    <Accordion
      className={styles.timelineAppointmentsGroupItem}
      activeKey={activeKey as string}
    >
      <Card className={styles.timelineAppointmentsGroupItemCard}>
        <CustomAccordionToggle
          eventKey={eventKey}
          callback={handleClickAppointment}
        >
          {(decoratedOnClick, isCurrentEventKey) =>
          (
            <Card.Header
              onClick={decoratedOnClick}
              className={styles.timelineAppointmentsGroupItemCardHeader}
            >

              <div className={styles.timelineAppointmentsGroupItemInfo}>
                <div className={styles.timelineAppointmentsGroupItemInfoDateTime}>
                  <span
                    className={styles.timelineAppointmentsGroupItemInfoDate}
                    data-test="appointment-date"
                  >
                    {`${formatDateAppointment(new Date(startTime))} - `}
                  </span>
                  <span
                    className={styles.timelineAppointmentsGroupItemInfoTime}
                    data-test="appointment-time"
                  >
                    {formatTimeAppointment(new Date(startTime))}
                  </span>
                  <div className={styles.timelineAppointmentsGroupItemInfoStatus}>
                    {PatientAppointmentStatus.NO_SHOW === status && showNoShowStatusCount ? (
                      <>
                        <img src={chatOrange} alt="status-icon" />
                      </>
                    ) : null}
                    {PatientAppointmentStatus.CANCELLED === status && showNoShowCancelledCount ? (
                      <>
                        <img src={chatOrange} alt="status-icon" />
                      </>
                    ) : null}
                  </div>
                </div>

              </div>

              <div className={styles.timelineAppointmentsGroupRightBlock}>
                <div className={styles.timelineAppointmentsGroupItemInfoPractitioner}>
                  <Avatar
                    width={2.125}
                    src={getSrcAvatar(photo)}
                    alt={`${showFirstPrefixItem(givenNames)}${familyName}`.trim()}
                    role={getUserRole({ role, prefixes })}
                  />
                  <div>
                    <Card.Text
                      className={styles.timelineAppointmentsGroupItemInfoPractitionerName}
                      data-test="appointment-practitioner-name"
                    >
                      {prefixes && `${prefixes.join(' ')} `}
                      {familyName}
                    </Card.Text>
                    <Card.Text
                      className={styles.timelineAppointmentsGroupItemInfoPractitionerSpeciality}
                    >
                      {speciality?.join(', ')}
                    </Card.Text>
                  </div>
                </div>
                <button
                  className={styles.timelineAppointmentsGroupItemAccordionToggle}
                  data-test="button-toggle-accordion"
                >
                  <img
                    src={isCurrentEventKey ? minusIcon : plusIcon}
                    alt={`${isCurrentEventKey ? 'minus' : 'plus'} icon`}
                  />
                </button>
              </div>

            </Card.Header>
          )
          }
        </CustomAccordionToggle>
        <Accordion.Collapse eventKey={eventKey}>
          <Card.Body className={styles.timelineAppointmentsGroupItemCardBody}>
            <div className={styles.timelineAppointmentsGroupItemCardBodyTable}>
              <div className={styles.timelineAppointmentsGroupItemCardBodyTableHead}>
                <div className={styles.timelineAppointmentsGroupItemCardBodyTableHeadReason}>
                  Reason For Visit
                </div>
                <div className={styles.timelineAppointmentsGroupItemCardBodyTableHeadDuration}>
                  Duration
                </div>
                <div className={styles.timelineAppointmentsGroupItemCardBodyTableHeadStatus}>
                  Status
                </div>
              </div>
              <div className={styles.timelineAppointmentsGroupItemCardBodyTableBody}>
                <div
                  className={styles.timelineAppointmentsGroupItemCardBodyTableBodyReason}
                  data-test="appointment-reason-visit"
                >
                  {reason}
                  {isTelehealth && (
                    <span
                      className={
                        styles.timelineAppointmentsGroupItemCardBodyTableBodyReasonTelehealth
                      }
                      data-test="appointment-indicator-telehealth"
                    >
                      <span data-test="appointment-indicator-telehealth-text">
                        {` (telehealth)`}
                      </span>
                      <img
                        src={telehealthTimelineIcon}
                        alt="telehealth icon"
                        data-test="appointment-indicator-telehealth-icon"
                      />
                    </span>
                  )}
                </div>
                <div
                  className={styles.timelineAppointmentsGroupItemCardBodyTableBodyDuration}
                  data-test="appointment-duration"
                >
                  {minutesDuration ? `${minutesDuration} Minutes` : ''}
                </div>
                <div
                  className={styles.timelineAppointmentsGroupItemCardBodyTableBodyStatus}
                  data-test="appointment-status"
                >
                  {status}
                  {isRecentAppointmentsStatuses.includes(status) && (
                    <div
                      className={
                        styles[`timelineAppointmentsGroupItemCardBodyTableBodyStatusIndicator${status}`]
                      }
                      data-test="appointment-status-indicator"
                    />
                  )}
                </div>
              </div>
            </div>
          </Card.Body>
        </Accordion.Collapse>
      </Card>
    </Accordion>
  );
};
