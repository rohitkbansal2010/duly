import { FadedScroll } from '@components/faded-scroll-2';
import { PatientAppointmentAlias } from '@enums';
import { PatientAppointmentsGroup } from '@types';

import { TimelineApppointmentsGroup } from '../timeline-appointments-group';

export type TimelineApppointmentsGroupsPropsType = {
  icon: string;
  backgroundIcon: string;
  backgroundCount: string;
  alias: PatientAppointmentAlias;
  groups: PatientAppointmentsGroup[];
  showNoShowStatusCount: boolean;
  showNoShowCancelledCount: boolean;
};

export const TimelineApppointmentsGroups = ({
  icon,
  backgroundIcon,
  backgroundCount,
  alias,
  showNoShowStatusCount,
  showNoShowCancelledCount,
  groups,
}: TimelineApppointmentsGroupsPropsType) =>
  (
    <FadedScroll height="100%">
      {groups.map((group, index) =>
    (
      <TimelineApppointmentsGroup
        key={String(index)}
        eventKey={String(index)}
        groupId={`${alias}-${index}`}
        icon={icon}
        backgroundIcon={backgroundIcon}
        backgroundCount={backgroundCount}
        showNoShowStatusCount={showNoShowStatusCount}
        showNoShowCancelledCount={showNoShowCancelledCount}
        {...group}
      />
    ))}
    </FadedScroll>
  );

