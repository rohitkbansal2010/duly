import { useEffect, useState } from 'react';
import { Accordion, Card } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { Avatar } from '@components/avatar';
import { CustomAccordionToggle } from '@components/custom-accordion-toggle';
import { FONT_LARGE } from '@components/font-size-setting/helper';
import { PatientAppointmentStatus } from '@enums';
import { expandIcon } from '@icons';
import { clearAccordions } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { Practitioner, PatientAppointment } from '@types';
import { getSrcAvatar, getUserRole, showFirstPrefixItem } from '@utils';

import { formatDateAppointment } from '../helpers';
import { TimelineAppointmentsGroupItems } from '../timeline-appointments-group-items';

import styles from './timeline-appointments-group.scss';

export type TimelineApppointmentsGroupPropsType = {
  eventKey: string;
  groupId: string;
  icon: string;
  backgroundIcon: string;
  backgroundCount: string;
  title: string;
  nearestAppointmentDate: string;
  farthestAppointmentDate?: string;
  nearestAppointmentPractitioner: Practitioner;
  appointments: PatientAppointment[];
  showNoShowStatusCount: boolean,
  showNoShowCancelledCount: boolean,
}

export const TimelineApppointmentsGroup = ({
  eventKey,
  groupId,
  icon,
  backgroundIcon,
  backgroundCount,
  title,
  nearestAppointmentDate,
  farthestAppointmentDate,
  nearestAppointmentPractitioner: {
    photo,
    humanName: {
      prefixes,
      familyName,
      givenNames,
    },
    role,
    speciality,
  },
  appointments,
  showNoShowStatusCount,
  showNoShowCancelledCount,
}: TimelineApppointmentsGroupPropsType) => {
  const dispatch: AppDispatch = useDispatch();
  const [ missedCount, setMissedCount ] = useState(0);
  const accordions = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.accordions);
  const activeKey = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE?.fontSize); 

  const classesApppointmentsGroupIcon = [ styles.timelineAppointmentsGroupIcon ]
    .concat(styles[`timelineAppointmentsGroupIcon${backgroundIcon}`])
    .join(' ');

  const classesApppointmentsGroupCount = [ styles.timelineAppointmentsGroupCountsAppointments ]
    .concat(styles[`timelineAppointmentsGroupCountsAppointments${backgroundCount}`])
    .join(' ');

  const getClassesAccordionToggle = (isCurrentEventKey: boolean) =>
    [ styles.timelineAppointmentsGroupAccordionToggleIcon ]
      .concat(styles[`timelineAppointmentsGroupAccordionToggleIcon${isCurrentEventKey ? 'Expand' : 'Collapse'}`])
      .join(' ');

  const handleClickGroup = (isCurrentEventKey: boolean) => {
    if (isCurrentEventKey && accordions[groupId]) {
      dispatch(clearAccordions(groupId));
    }
  };
  const ststusCount = () => {
    const missed = appointments.filter(appointment =>
      (appointment?.status == PatientAppointmentStatus.CANCELLED && showNoShowCancelledCount)
      || (appointment?.status == PatientAppointmentStatus.NO_SHOW && showNoShowStatusCount));
    setMissedCount(missed.length);
  };
  const getDividerClass = ()=>
    activeKey !== FONT_LARGE ? `${styles.timelineAppointmentsGroupStatusDivider}` : `${styles.timelineAppointmentsGroupStatusDivider} ${styles.LargeDivider}`;

  useEffect(() => {
    ststusCount();
  }, [ appointments ]);

  return (
    < Accordion className={styles.timelineAppointmentsGroup} >
      <Card className={styles.timelineAppointmentsGroupCard}>
        <CustomAccordionToggle
          eventKey={eventKey}
          callback={handleClickGroup}
        >
          {(decoratedOnClick, isCurrentEventKey) =>
          (
            <Card.Header
              onClick={decoratedOnClick}
              className={styles.timelineAppointmentsGroupCardHeader}
            >
              <div className={styles.timelineAppointmentsGroupLeftBlock}>
                <div className={classesApppointmentsGroupIcon}>
                  <img src={icon} alt="Calendar icon" data-test="icon-group" />
                  {appointments.length > 1 && (
                    <div
                      className={classesApppointmentsGroupCount}
                      data-test="number-appointments"
                    >
                      {appointments.length}
                    </div>
                  )}
                </div>
                <div className={styles.timelineAppointmentsGroupAppointment}>
                  <Card.Title
                    className={styles.timelineAppointmentsGroupAppointmentTitle}
                    data-test="title-group"
                  >
                    {title}
                  </Card.Title>
                  <Card.Text
                    data-test="date-group"
                  >
                    <div className={activeKey !== FONT_LARGE ? `${styles.timelineAppointmentsGroupAppointmentSubtitleRow}` : `${styles.timelineAppointmentsGroupAppointmentSubtitleRow} ${styles.Large}` }>
                      <div className={styles.timelineAppointmentsGroupAppointmentSubtitleCol}>
                        <div className={styles.timelineAppointmentsGroupAppointmentDate} >
                          {formatDateAppointment(new Date(nearestAppointmentDate))}
                          {farthestAppointmentDate && ` - ${formatDateAppointment(new Date(farthestAppointmentDate))}`}
                        </div>
                      </div>
                      <div className={styles.timelineAppointmentsGroupAppointmentSubtitleCol}>
                        <div className={styles.timelineAppointmentsGroupAppointmentStatus}>
                          {
                            missedCount > 0 ? (
                              <>
                                <div 
                                  className={getDividerClass()}
                                />
                                <div className={styles.timelineAppointmentsGroupStatus}>
                                  Missed: {missedCount}
                                </div>
                              </>
                            ) : null
                          }
                        </div>
                      </div>
                    </div>
                  </Card.Text>
                </div>

              </div>

              <div className={styles.timelineAppointmentsGroupRightBlock}>
                <div className={styles.timelineAppointmentsGroupPractitioner}>
                  <Avatar
                    width={2.125}
                    src={getSrcAvatar(photo)}
                    alt={`${showFirstPrefixItem(givenNames)}${familyName}`.trim()}
                    role={getUserRole({ role, prefixes })}
                  />
                  <div>
                    <Card.Text
                      className={styles.timelineAppointmentsGroupPractitionerName}
                      data-test="practitioner-name"
                    >
                      {prefixes && `${prefixes.join(' ')} `}
                      {familyName}
                    </Card.Text>

                    <Card.Text
                      className={styles.timelineAppointmentsGroupPractitionerSpeciality}
                    >
                      {speciality?.join(', ')}
                    </Card.Text>
                  </div>
                </div>
                <button
                  className={styles.timelineAppointmentsGroupAccordionToggle}
                  data-test="button-collapse-expand"
                >
                  <img
                    src={expandIcon}
                    alt={`${isCurrentEventKey ? 'expand' : 'collapse'} icon`}
                    className={getClassesAccordionToggle(isCurrentEventKey)}
                  />
                </button>
              </div>

            </Card.Header>
          )
          }
        </CustomAccordionToggle>
        <Accordion.Collapse eventKey={eventKey}>
          <Card.Body className={styles.timelineAppointmentsGroupCardBody}>
            <TimelineAppointmentsGroupItems
              groupId={groupId}
              appointments={appointments}
              showNoShowStatusCount={showNoShowStatusCount}
              showNoShowCancelledCount={showNoShowCancelledCount}
            />
          </Card.Body>
        </Accordion.Collapse>
      </Card>
    </Accordion >
  );
};
