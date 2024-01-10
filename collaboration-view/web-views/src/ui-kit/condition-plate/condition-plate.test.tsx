import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { conditionsIcon, currentConditionsIcon } from '@icons';
import { healthConditionsMockData } from '@mock-data';
import { ConditionPlate } from '@ui-kit/condition-plate';
import { formatMDYYYYDate } from '@utils';

const { currentHealthConditions } = healthConditionsMockData;
const mockProps = currentHealthConditions[0];

describe('HealthConditionsPlate', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = shallow(<ConditionPlate {...mockProps} />);
  });

  it('component should match to snapshot', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('component should render right data', () => {
    const { title, date } = mockProps;
  
    expect(component.text().includes(title)).toBeTruthy();
    expect(component.text().includes(formatMDYYYYDate(date!))).toBeTruthy();
  });

  it('component should render right icon', () => {
    component = shallow(<ConditionPlate {...mockProps} isCurrent={true} />);

    expect(component.findWhere(n =>
      n.type() === 'img' && n.props().src === currentConditionsIcon)).toHaveLength(1);

    component = shallow(<ConditionPlate {...mockProps} isCurrent={false} />);

    expect(component.findWhere(n =>
      n.type() === 'img' && n.props().src === conditionsIcon)).toHaveLength(1);
  });
});
