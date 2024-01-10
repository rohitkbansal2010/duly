import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import {
  AppointmentModuleWidgetsAlias, PatientAppointmentAlias, PatientAppointmentStatus, PractitionerRole
} from '@enums';
import { PatientAppointments } from '@types';

import { TimelineAppointments, TimelineAppointmentsPropsType } from './timeline-appointments';
import { timelineAppointmentsMap } from './timeline-appointments-map';

const mockPatientAppointments: PatientAppointments = {
  [PatientAppointmentAlias.RECENT]: [
    {
      title: 'Physical Therapy',
      appointments: [
        {
          practitioner: {
            id: 'qwerty1',
            humanName: {
              familyName: 'Reyes',
              givenNames: [ 'Ana', 'Maria' ],
              prefixes: [ 'Dr.' ],
            },
            role: PractitionerRole.PCP,
          },
          isTelehealth: true,
          reason: 'Patient fell - Hip problims',
          status: PatientAppointmentStatus.COMPLETED,
          startTime: '2021-11-02T14:45:00Z',
          minutesDuration: 30,
        },
      ],
      nearestAppointmentPractitioner: {
        id: 'qwerty1',
        humanName: {
          familyName: 'Reyes',
          givenNames: [ 'Ana', 'Maria' ],
          prefixes: [ 'Dr.' ],
        },
        role: PractitionerRole.PCP,
      },
      nearestAppointmentDate: '2021-11-02',
      farthestAppointmentDate: '2021-07-15',
    },
  ],
  [PatientAppointmentAlias.UPCOMING]: [],
};
const mockProps: TimelineAppointmentsPropsType = { alias: PatientAppointmentAlias.RECENT };

const setUp = (props: TimelineAppointmentsPropsType) =>
  shallow(<TimelineAppointments {...props} />);

describe('TimelineAppointments', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({ OVERVIEW_WIDGETS: { patientAppointments: mockPatientAppointments } }));
    jest
      .spyOn(redux, 'useDispatch')
      .mockReturnValue(jest.fn());

    component = setUp(mockProps);
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('should render the TimelineAppointments component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should render the title', () => {
    const expectedTitle = timelineAppointmentsMap[mockProps.alias].title;

    expect(component.find('h4').text()
      .includes(expectedTitle)).toBeTruthy();
  });

  it('should render the TimelineApppointmentsGroups component', () => {
    const {
      icon,
      backgroundIcon,
      backgroundCount,
      skeletonCount,
      showNoShowStatusCount,
      showNoShowCancelledCount,
    } = timelineAppointmentsMap[mockProps.alias];

    const expectedProps = {
      icon,
      backgroundIcon,
      alias: mockProps.alias,
      groups: mockPatientAppointments[mockProps.alias],
      backgroundCount,
      skeletonCount,
      showNoShowStatusCount,
      showNoShowCancelledCount,
    };

    const componentTimelineApppointmentsGroups = component.find('TimelineApppointmentsGroups');

    expect(componentTimelineApppointmentsGroups).toHaveLength(1);
    expect(componentTimelineApppointmentsGroups.props()).toEqual(expectedProps);
  });

  it('should render the TimelineApppointmentsGroupsNoData component if no patientAppointments by alias', () => {
    const {
      icon,
      backgroundIcon,
      title,
      colorTitle,
    } = timelineAppointmentsMap[mockProps.alias];

    const expectedProps = {
      icon,
      backgroundIcon,
      title,
      colorTitle,
    };

    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector(
          { OVERVIEW_WIDGETS: { patientAppointments: { [PatientAppointmentAlias.RECENT]: [] } } }
        ));

    const component = setUp(mockProps);

    expect(toJson(component)).toMatchSnapshot();

    const componentTimelineApppointmentsGroupsNoDate = component.find('TimelineApppointmentsGroupsNoData');
    expect(componentTimelineApppointmentsGroupsNoDate).toHaveLength(1);
    expect(componentTimelineApppointmentsGroupsNoDate.props()).toEqual(expectedProps);
  });

  it('should render the TimelineAppointmentsSkeleton component while dispatch array of appointments', () => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({
          OVERVIEW_WIDGETS: { patientAppointments: { [PatientAppointmentAlias.RECENT]: [] } },
          CURRENT_APPOINTMENT: {
            widgetsList: [
              { alias: AppointmentModuleWidgetsAlias.TIMELINE, isSkeletonShown: true },
            ],
          },
        }));

    const component = setUp(mockProps);

    expect(toJson(component)).toMatchSnapshot();
    expect(component.find('TimelineAppointmentsSkeleton')).toHaveLength(1);
    expect(component.find('h4')).toHaveLength(0);
    expect(component.find('TimelineApppointmentsGroups')).toHaveLength(0);
  });
});
