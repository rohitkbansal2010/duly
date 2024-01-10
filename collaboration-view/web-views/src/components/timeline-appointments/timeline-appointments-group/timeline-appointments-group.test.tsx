import { shallow, ShallowWrapper } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { PractitionerRole, PatientAppointmentStatus, PatientAppointmentAlias } from '@enums';
import { calendarTwoBlueIcon } from '@icons';
import { getSrcAvatar, getUserRole, showFirstPrefixItem } from '@utils';

import { formatDateAppointment } from '../helpers';
import {
  TimelineApppointmentsGroup,
  TimelineApppointmentsGroupPropsType
} from '../timeline-appointments-group';

const mockProps: TimelineApppointmentsGroupPropsType = {
  eventKey: '0',
  groupId: `${PatientAppointmentAlias.RECENT}-0`,
  icon: calendarTwoBlueIcon,
  backgroundIcon: 'Blue',
  backgroundCount: 'DarkBlue',
  title: 'Physical Therapy',
  nearestAppointmentDate: '2022-01-10',
  farthestAppointmentDate: '2022-01-29',
  nearestAppointmentPractitioner: {
    id: '1',
    humanName: {
      prefixes: [ 'Dr.' ],
      familyName: 'Ling',
      givenNames: [],
    },
    role: PractitionerRole.MA,
  },
  appointments: [
    {
      startTime: '2021-01-05T14:30:00Z',
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
      startTime: '2021-01-05T14:30:00Z',
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

const setUp = (props: TimelineApppointmentsGroupPropsType) =>
  shallow(<TimelineApppointmentsGroup {...props} />);

describe('TimelineApppointmentsGroup', () => {
  let component: ReturnType<typeof shallow>;
  let customAccordionToggle: ShallowWrapper;

  beforeEach(() => {
    jest
      .spyOn(redux, 'useSelector')
      .mockReturnValue({ accordions: mockcAcordions });
    jest
      .spyOn(redux, 'useDispatch')
      .mockReturnValue(jest.fn());

    component = setUp(mockProps);
    customAccordionToggle = component.find('CustomAccordionToggle').renderProp('children')();
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('should render the TimelineApppointmentsGroup component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should render the icon', () => {
    const iconElement = customAccordionToggle.find('[data-test="icon-group"]');
    const { src } = iconElement.props();

    expect(iconElement).toHaveLength(1);
    expect(src?.includes(mockProps.icon)).toBeTruthy();
  });

  it('should render in the header number of the appoinntments if their count more than 1', () => {
    const numberAppointmentsElement = customAccordionToggle.find('[data-test="number-appointments"]');

    expect(numberAppointmentsElement).toHaveLength(1);
    expect(numberAppointmentsElement.text().includes(String(mockProps.appointments.length)))
      .toBeTruthy();
  });

  it('should not render in the header number of the appoinntments if their count equal 1', () => {
    const component = setUp({
      ...mockProps,
      appointments: mockProps.appointments.slice(0, 1),
    });

    expect(toJson(component)).toMatchSnapshot();

    const customAccordionToggle = component.find('CustomAccordionToggle').renderProp('children')();
    const numberAppointmentsElement = customAccordionToggle.find('[data-test="number-appointments"]');

    expect(numberAppointmentsElement).toHaveLength(0);
  });

  it('should render in the header the type of appointment', () => {
    const typeAppointmentElement = customAccordionToggle.find('[data-test="title-group"]');

    expect(typeAppointmentElement).toHaveLength(1);
    expect(typeAppointmentElement.text().includes(mockProps.title)).toBeTruthy();
  });

  it('should render in the header the nearestAppointmentDate and farthestAppointmentDate', () => {
    const dateElement = customAccordionToggle.find('[data-test="date-group"]');
    const dates = [
      formatDateAppointment(new Date(mockProps.nearestAppointmentDate)),
      formatDateAppointment(new Date(mockProps.farthestAppointmentDate as string)),
    ];
    const expectedDate = dates.join(' - ');

    expect(dateElement).toHaveLength(1);
    expect(dateElement.text().includes(expectedDate)).toBeTruthy();
  });

  it('should not render in the header the farthestAppointmentDate if it not', () => {
    const component = setUp({
      ...mockProps,
      farthestAppointmentDate: undefined,
    });

    expect(toJson(component)).toMatchSnapshot();

    const dateElement = customAccordionToggle.find('[data-test="date-group"]');
    const expectedDate = formatDateAppointment(new Date(mockProps.nearestAppointmentDate));

    expect(dateElement.text().includes(expectedDate)).toBeTruthy();
  });

  it('shoul render in the header the Avatar component', () => {
    const {
      humanName: {
        givenNames,
        familyName,
        prefixes,
      },
      role,
    } = mockProps.nearestAppointmentPractitioner;

    const expxctedProps = {
      width: 2.125,
      src: getSrcAvatar(),
      alt: `${showFirstPrefixItem(givenNames)}${familyName}`.trim(),
      role: getUserRole({ role, prefixes }),
    };

    const avatarComponent = customAccordionToggle.find('Avatar');

    expect(avatarComponent).toHaveLength(1);
    expect(avatarComponent.props()).toEqual(expxctedProps);
  });

  it('should render in the header the name practitioner', () => {
    const namePractitionerElement = customAccordionToggle.find('[data-test="practitioner-name"]');

    const {
      humanName: {
        familyName,
        prefixes,
      },
    } = mockProps.nearestAppointmentPractitioner;

    const expectedNamePractioner = `${prefixes?.join(' ')} ${familyName}`;

    expect(namePractitionerElement).toHaveLength(1);
    expect(namePractitionerElement.text().includes(expectedNamePractioner)).toBeTruthy();
  });

  it('should render in the header collapse/expand icon', () => {
    const buttonElement = customAccordionToggle.find('[data-test="button-collapse-expand"]');
    const imgElement = buttonElement.find('img');

    expect(buttonElement).toHaveLength(1);
    expect(imgElement).toHaveLength(1);
    expect(imgElement.prop('alt')?.includes('collapse icon')).toBeTruthy();
  });

  it('should render the TimelineAppointmentsGroupItems component', () => {
    const expectedProps = {
      appointments: mockProps.appointments,
      groupId: mockProps.groupId,
    };

    const timelineAppointmentsGroupItemsComponent = component.find('TimelineAppointmentsGroupItems');

    expect(timelineAppointmentsGroupItemsComponent).toHaveLength(1);
    expect(timelineAppointmentsGroupItemsComponent.props()).toEqual(expectedProps);
  });
});
