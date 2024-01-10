import { useDispatch, useSelector } from 'react-redux';

import { addAccordion, deleteAccordion } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { PatientAppointment } from '@types';

import { TimelineAppointmentsGroupItem } from './timeline-appointments-group-item';

export type TimelineAppointmentsGroupItemsPropsType = {
  groupId: string;
  appointments: PatientAppointment[];
  showNoShowStatusCount: boolean;
  showNoShowCancelledCount: boolean;
};

export const TimelineAppointmentsGroupItems =
  ({
    groupId, appointments, showNoShowStatusCount, showNoShowCancelledCount, 
  }: TimelineAppointmentsGroupItemsPropsType) => {
    const dispatch: AppDispatch = useDispatch();

    const accordions = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
      CURRENT_APPOINTMENT.accordions);

    const handleClickAppointment = (accordionId: string) =>
      () => {
        if (!accordions[groupId]?.includes(accordionId)) {
          dispatch(addAccordion(groupId, accordionId));
        } else {
          dispatch(deleteAccordion(groupId, accordionId));
        }
      };

    const getActiveKey = (eventKey: string) =>
      accordions[groupId]?.find((accordionId: string) =>
        accordionId === eventKey) || null;

    const renderGroupAppointments = (appointment: PatientAppointment, index: number) =>
      (
        <TimelineAppointmentsGroupItem
          key={index}
          eventKey={String(index)}
          activeKey={getActiveKey(String(index))}
          handleClickAppointment={handleClickAppointment(String(index))}
          showNoShowStatusCount={showNoShowStatusCount}
          showNoShowCancelledCount={showNoShowCancelledCount}
          {...appointment}
        />
      );

    return (
      <>{appointments.map(renderGroupAppointments)}</>
    );
  };
