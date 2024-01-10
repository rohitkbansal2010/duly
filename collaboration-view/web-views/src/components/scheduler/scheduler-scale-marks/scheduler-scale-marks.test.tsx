import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { calculateCalendarDivisions } from '@utils';

import { SchedulerScaleMarks, SchedulerScaleMarksPropsType } from './scheduler-scale-marks';

const mockProps: SchedulerScaleMarksPropsType = {
  workDayStart: 7,
  workDayEnd: 19,
};

describe('SchedulerScaleMarks', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = shallow(<SchedulerScaleMarks {...mockProps} />);
  });

  it('should render correctly', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should render the "ListGroup" component from "react-bootstrap"', () => {
    expect(component.find('ListGroup')).toHaveLength(1);
  });

  it('should render time segments to the "ListGroup" component', () => {
    const listGroup = component.find('ListGroup');
    const schedulerDivisions =
      calculateCalendarDivisions(mockProps.workDayStart, mockProps.workDayEnd);

    expect(listGroup.find('ListGroupItem')).toHaveLength(schedulerDivisions.length);
  });
});
