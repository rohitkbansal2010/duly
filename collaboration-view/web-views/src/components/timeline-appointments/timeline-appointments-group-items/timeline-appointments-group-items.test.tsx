import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { PractitionerRole, PatientAppointmentStatus, PatientAppointmentAlias } from '@enums';

import * as helpers from '../helpers';

import {
  TimelineAppointmentsGroupItems,
  TimelineAppointmentsGroupItemsPropsType
} from './timeline-appointments-group-items';

const mockDate = '2021-01-05T14:30:00Z';

const mockProps: TimelineAppointmentsGroupItemsPropsType = {
  groupId: `${PatientAppointmentAlias.RECENT}-0`,
  appointments: [
    {
      startTime: mockDate,
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
    {
      startTime: mockDate,
      practitioner: {
        id: '1',
        humanName: {
          familyName: 'Ling',
          givenNames: [],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.MA,
      },
      reason: 'Hip check - Physical Therapy',
      minutesDuration: 60,
      status: PatientAppointmentStatus.NO_SHOW,
    },
  ],
};
const mockcAcordions = { [mockProps.groupId]: [ '0', '1', '2' ] };

describe('TimelineAppointmentsGroupItems', () => {
  let component: ReturnType<typeof mount>;

  beforeEach(() => {
    jest
      .spyOn(helpers, 'formatTimeAppointment')
      .mockReturnValue('2:30 PM');
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({ CURRENT_APPOINTMENT: { accordions: mockcAcordions } }));
    jest
      .spyOn(redux, 'useDispatch')
      .mockReturnValue(jest.fn());

    component = mount(<TimelineAppointmentsGroupItems {...mockProps} />);
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('should render the TimelineAppointmentsGroupItems component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it(`should render ${mockProps.appointments.length} the TimelineAppointmentsGroupItem components`, () => {
    const appointmentsGroupItemsComponents = component.find('TimelineAppointmentsGroupItem');

    expect(appointmentsGroupItemsComponents).toHaveLength(mockProps.appointments.length);
    appointmentsGroupItemsComponents.forEach((appointmentsGroupItemComponent, index) => {
      const mockActiveKey = mockcAcordions[mockProps.groupId]?.find((accordionId: string) =>
        accordionId === String(index)) || null;

      const expectedProps = {
        eventKey: String(index),
        activeKey: mockActiveKey,
        handleClickAppointment: expect.any(Function),
        ...mockProps.appointments[index],
      };

      expect(appointmentsGroupItemComponent.props()).toEqual(expectedProps);
    });
  });
});
