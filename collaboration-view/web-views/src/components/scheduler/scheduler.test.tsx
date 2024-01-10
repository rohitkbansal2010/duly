import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { AppointmentStatus, AppointmentType, PractitionerRole } from '@enums';
import { AppointmentData } from '@types';

import { Scheduler } from './scheduler';

const mockAppointments: AppointmentData[] = [
  {
    id: 'qwerty2',
    title: 'Check Up',
    type: AppointmentType.TELEHEALTH,
    status: AppointmentStatus.ARRIVED,
    timeSlot: {
      startTime: '2021-12-23T11:00:00',
      endTime: '2021-12-23T11:15:00',
    },
    location: { title: 'Room #3' },
    patient: {
      patientGeneralInfo: {
        id: 'qwerty2',
        humanName: {
          familyName: 'Smith',
          givenNames: [ 'Benjamin' ],
        },
      },
      isNewPatient: false,
    },
    practitioner: {
      id: 'qwerty2',
      humanName: {
        familyName: 'Sussman',
        prefixes: [ 'Dr.' ],
      },
      role: PractitionerRole.PCP,
    },
  },
  {
    id: 'qwerty3',
    title: 'Check Up',
    type: AppointmentType.ON_SITE,
    status: AppointmentStatus.IN_PROGRESS,
    timeSlot: {
      startTime: '2021-12-23T11:00:00',
      endTime: '2021-12-23T11:30:00',
    },
    location: { title: 'Room #2' },
    patient: {
      patientGeneralInfo: {
        id: 'qwerty3',
        humanName: {
          familyName: 'Johnson',
          givenNames: [ 'Mary' ],
        },
      },
      isNewPatient: true,
    },
    practitioner: {
      id: 'qwerty3',
      humanName: { familyName: 'Palmero' },
      role: PractitionerRole.MA,
    },
  },
];

describe('Scheduler', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({
          TODAYS_APPOINTMENTS: { appointments: mockAppointments },
          APPSTATE: {},
        }));
    jest
      .spyOn(redux, 'useDispatch')
      .mockReturnValue(jest.fn());

    component = shallow(<Scheduler />);
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('should exist', () => {
    expect(Scheduler).toBeDefined();
  });

  it('should render the components SchedulerScaleMarks, CurrentTimeBar', () => {
    expect(toJson(component)).toMatchSnapshot();

    expect(component.find('SchedulerScaleMarks')).toHaveLength(1);
    expect(component.find('CurrentTimeBar')).toHaveLength(1);
  });
});
