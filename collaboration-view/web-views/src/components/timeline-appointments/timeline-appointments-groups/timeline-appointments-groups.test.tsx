import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { PractitionerRole, PatientAppointmentStatus, PatientAppointmentAlias } from '@enums';
import { calendarTwoBlueIcon } from '@icons';

import {
  TimelineApppointmentsGroups,
  TimelineApppointmentsGroupsPropsType
} from './timeline-appointments-groups';

const mockProps: TimelineApppointmentsGroupsPropsType = {
  icon: calendarTwoBlueIcon,
  backgroundIcon: 'Blue',
  backgroundCount: 'DarkBlue',
  alias: PatientAppointmentAlias.RECENT,
  groups: [
    {
      title: 'Physical Therapy',
      nearestAppointmentDate: '2022-01-10',
      farthestAppointmentDate: '2022-01-29',
      nearestAppointmentPractitioner: {
        id: '1',
        humanName: {
          familyName: 'Ling',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      appointments: [
        {
          startTime: 'Fri, Nov 2, 2021',
          practitioner: {
            id: '1',
            humanName: {
              familyName: 'Ling',
              givenNames: [],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.MA,
          },
          reason: 'Patient fell - Hip problems',
          minutesDuration: 30,
          status: PatientAppointmentStatus.COMPLETED,
        },
      ],
    },
  ],
};

describe('TimelineApppointmentsGroups', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = shallow(<TimelineApppointmentsGroups {...mockProps} />);
  });

  it('should render the TimelineApppointmentsGroups component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it(`should render ${mockProps.groups.length} the TimelineApppointmentsGroup components`, () => {
    const appointmentsGroupsComponents = component.find('TimelineApppointmentsGroup');

    expect(appointmentsGroupsComponents).toHaveLength(mockProps.groups.length);
    appointmentsGroupsComponents.forEach((appointmentsGroupComponent, index) => {
      const expectedProps = {
        eventKey: String(index),
        groupId: `${mockProps.alias}-${index}`,
        icon: mockProps.icon,
        backgroundIcon: mockProps.backgroundIcon,
        backgroundCount: mockProps.backgroundCount,
        ...mockProps.groups[index],
      };

      expect(appointmentsGroupComponent.props()).toEqual(expectedProps);
    });
  });
});
