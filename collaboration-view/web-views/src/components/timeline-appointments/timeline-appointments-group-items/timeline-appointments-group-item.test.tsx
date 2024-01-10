import { shallow, ShallowWrapper } from 'enzyme';
import toJson from 'enzyme-to-json';

import { PractitionerRole, PatientAppointmentStatus } from '@enums';
import { telehealthTimelineIcon } from '@icons';
import { getSrcAvatar, getUserRole, showFirstPrefixItem } from '@utils';

import { formatDateAppointment, formatTimeAppointment } from '../helpers';

import {
  TimelineAppointmentsGroupItem,
  TimelineAppointmentsGroupItemPropsType
} from './timeline-appointments-group-item';

const mockProps: TimelineAppointmentsGroupItemPropsType = {
  eventKey: '0',
  accordionsGroup: [ '0', '1', '2' ],
  startTime: '2020-01-02T17:15:00Z',
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
  isTelehealth: true,
  handleClickAppointment: jest.fn(),
};

const setUp = (props: TimelineAppointmentsGroupItemPropsType) =>
  shallow(<TimelineAppointmentsGroupItem {...props} />);

describe('TimelineAppointmentsGroupItem', () => {
  let component: ReturnType<typeof shallow>;
  let customAccordionToggle: ShallowWrapper;

  beforeEach(() => {
    component = setUp(mockProps);
    customAccordionToggle = component.find('CustomAccordionToggle').renderProp('children')();
  });

  it('should render the TimelineAppointmentsGroupItem component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should render in the header the date', () => {
    const dateElement = customAccordionToggle.find('[data-test="appointment-date"]');

    const expectedDate = `${formatDateAppointment(new Date(mockProps.startTime))} - `;

    expect(dateElement).toHaveLength(1);
    expect(dateElement.text().includes(expectedDate)).toBeTruthy();
  });

  it('should render in the header the time', () => {
    const timeElement = customAccordionToggle.find('[data-test="appointment-time"]');

    const exptectedTime = formatTimeAppointment(new Date(mockProps.startTime));

    expect(timeElement).toHaveLength(1);
    expect(timeElement.text().includes(exptectedTime)).toBeTruthy();
  });

  it('shoul render in the header the Avatar component', () => {
    const {
      humanName: {
        givenNames,
        familyName,
        prefixes,
      },
      role,
    } = mockProps.practitioner;

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

  it('should render in the header the practitioner name', () => {
    const practitionerNameElement = customAccordionToggle.find('[data-test="appointment-practitioner-name"]');

    const {
      humanName: {
        familyName,
        prefixes,
      },
    } = mockProps.practitioner;

    const expectedPractitionerName = `${prefixes?.join(' ')} ${familyName}`;

    expect(practitionerNameElement).toHaveLength(1);
    expect(practitionerNameElement.text().includes(expectedPractitionerName)).toBeTruthy();
  });

  it('should render in the header button toggle', () => {
    expect(customAccordionToggle.find('[data-test="button-toggle-accordion"]')).toHaveLength(1);
  });

  it('should render in the body the reason for visit', () => {
    const elementResonVisit = component.find('[data-test="appointment-reason-visit"]');

    expect(elementResonVisit).toHaveLength(1);
    expect(elementResonVisit.text().includes(mockProps.reason)).toBeTruthy();
  });

  it('should render in the body after reason for visit indicator telehealth if the appointment is telehealth', () => {
    const elementIndicatorTelehealth = component.find('[data-test="appointment-indicator-telehealth"]');
    const elementIndicatorTelehealthText = component.find('[data-test="appointment-indicator-telehealth-text"]');
    const elementIndicatorTelehealthIcon = component.find('[data-test="appointment-indicator-telehealth-icon"]');
    const expectedText = ` (telehealth)`;
    const expectedIconProps = {
      src: telehealthTimelineIcon,
      alt: 'telehealth icon',
      'data-test': 'appointment-indicator-telehealth-icon',
    };

    expect(elementIndicatorTelehealth).toHaveLength(1);
    expect(elementIndicatorTelehealthText.text().includes(expectedText)).toBeTruthy();
    expect(elementIndicatorTelehealthIcon.props()).toEqual(expectedIconProps);
  });

  it('should not render in the body after reason for visit indicator telehealth if the appointment is not telehealth', () => {
    const component = setUp({ ...mockProps, isTelehealth: false });

    expect(toJson(component)).toMatchSnapshot();
    expect(component.find('[data-test="appointment-indicator-telehealth"]')).toHaveLength(0);
  });

  it('should render in the body the duration', () => {
    const elementDuration = component.find('[data-test="appointment-duration"]');
    const expectedDuration = `${mockProps.minutesDuration} Minutes`;

    expect(elementDuration).toHaveLength(1);
    expect(elementDuration.text().includes(expectedDuration)).toBeTruthy();
  });

  it('should not render in the body the duration if it not', () => {
    const component = setUp({ ...mockProps, minutesDuration: undefined });

    expect(toJson(component)).toMatchSnapshot();
    expect(component.find('[data-test="appointment-duration"]').text()).toBeFalsy();
  });

  it('should render in the body the status', () => {
    const elementStatus = component.find('[data-test="appointment-status"]');

    expect(elementStatus).toHaveLength(1);
    expect(elementStatus.text().includes(mockProps.status)).toBeTruthy();
  });

  it('should render in the body after status indicator if the status are completed, canceled or no-show', () => {
    expect(component.find('[data-test="appointment-status-indicator"]')).toHaveLength(1);
  });

  it('should not render in the body after status indicator if the status is schedule', () => {
    const component = setUp({ ...mockProps, status: PatientAppointmentStatus.SCHEDULED });
    expect(toJson(component)).toMatchSnapshot();

    expect(component.find('[data-test="appointment-status-indicator"]')).toHaveLength(0);
  });
});
