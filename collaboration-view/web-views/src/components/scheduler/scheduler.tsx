import { useEffect, useState, useRef } from 'react';
import { Row } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import { AppointmentItem } from '@components/appointment-item';
import { BTGModal } from '@components/btg-modal';
import { CurrentTimeBar } from '@components/current-time-bar';
import {
  HEIGHT_HOUR_SEGMENT,
  MINUTES_PER_HOUR,
  PADDING_BOTTOM_SCHEDULER_SCALE
} from '@constants';
import { RootState } from '@redux/reducers';
import { GroupedAppointmentType } from '@types';
import {
  calcDurationInMinutes,
  calcTopOfGroupsAppointems,
  cropAppointmentsByWorkDay,
  groupAppointments,
  sortAppointmentsInDescTime
} from '@utils';

import { PickedAppointmentModal } from '../picked-appointment-modal';

import { SchedulerScaleMarks } from './scheduler-scale-marks';
import styles from './scheduler.scss';

const workDayStart = window?.env?.WORK_DAY_START || '';
const workDayEnd = window?.env?.WORK_DAY_END || '';

export const Scheduler = () => {
  const appointments = useSelector(({ TODAYS_APPOINTMENTS }: RootState) =>
    TODAYS_APPOINTMENTS.appointments);
  const pickedAppointmentId = useSelector(({ TODAYS_APPOINTMENTS }: RootState) =>
    TODAYS_APPOINTMENTS.pickedAppointmentId);
  const isSpinnerShown = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.isSpinnerShown);

  const schedulerClass = []
    .concat(styles.scheduler)
    .concat(isSpinnerShown ? styles.schedulerTranslucent : '')
    .join(' ');

  const [ groupsAppointments, setGrouspAppointments ] = useState<GroupedAppointmentType[]>([]);

  const schedulerRef = useRef<HTMLDivElement>(null);

  const scrollToScheduler = () => {
    schedulerRef.current?.scrollIntoView({
      behavior: 'smooth',
      block: 'center',
    });
  };

  useEffect(() => {
    scrollToScheduler();
  }, [ groupsAppointments ]);

  const renderGroupsAppointments =
    ({ startTime, endTime, appointments }: GroupedAppointmentType, i: number) => {
      const groupsDuration = calcDurationInMinutes(new Date(startTime), new Date(endTime));
      const groupsHeight =
        (groupsDuration / MINUTES_PER_HOUR) * HEIGHT_HOUR_SEGMENT - PADDING_BOTTOM_SCHEDULER_SCALE;

      return (
        <Row
          ref={i === 0 ? schedulerRef : undefined}
          key={startTime}
          className={styles.schedulerGroupAppointments}
          style={{
            top: `${calcTopOfGroupsAppointems(new Date(startTime))}rem`,
            height: `${groupsHeight}rem`,
          }}
        >
          {appointments.map(({ id, title, ...props }) =>
            (
              <AppointmentItem
                key={id}
                id={id}
                title={title}
                startTimeGroups={startTime}
                {...props}
              />
            ))}
        </Row>
      );
    };

  useEffect(() => {
    const groupedAppointments = groupAppointments(cropAppointmentsByWorkDay(appointments))
      .map(groupedAppointment =>
        ({
          ...groupedAppointment,
          appointments: sortAppointmentsInDescTime(groupedAppointment.appointments),
        }));

    setGrouspAppointments(groupedAppointments);
  }, [ appointments ]);

  return (
    <div className={schedulerClass}>
      {pickedAppointmentId && (
        <PickedAppointmentModal />
      )}
      <BTGModal />
      <SchedulerScaleMarks
        workDayStart={+workDayStart}
        workDayEnd={+workDayEnd}
      />
      <CurrentTimeBar />
      {groupsAppointments.map(renderGroupsAppointments)}
    </div>
  );
};
