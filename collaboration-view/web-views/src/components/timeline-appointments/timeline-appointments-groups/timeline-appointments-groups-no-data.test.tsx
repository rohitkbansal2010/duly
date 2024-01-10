import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { calendarTwoBlueIcon } from '@icons';

import {
  TimelineApppointmentsGroupsNoData,
  TimelineApppointmentsGroupsNoDataPropsType
} from './timeline-appointments-groups-no-data';

const mockProps: TimelineApppointmentsGroupsNoDataPropsType = {
  icon: calendarTwoBlueIcon,
  backgroundIcon: 'Blue',
  title: 'Recent Appointments',
  colorTitle: 'LightBlue',
};

describe('TimelineApppointmentsGroupsNoData', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = shallow(<TimelineApppointmentsGroupsNoData {...mockProps} />);
  });

  it('should render the TimelineApppointmentsGroupsNoData component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should render icon', () => {
    const iconElement = component.find('[data-test="appointment-icon"]');

    const expectedProps = {
      src: mockProps.icon,
      alt: 'Calendar icon',
      'data-test': 'appointment-icon',
    };

    expect(iconElement).toHaveLength(1);
    expect(iconElement.props()).toEqual(expectedProps);
  });

  it('should render title', () => {
    const titleElement = component.find('[data-test="appointment-title"]');

    const expectedTitle = `No ${mockProps.title}`;

    expect(titleElement).toHaveLength(1);
    expect(titleElement.text().includes(expectedTitle)).toBeTruthy();
  });
});
