import { mount, shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { healthConditionsMockData } from '@mock-data';
import { ConditionPlate } from '@ui-kit/condition-plate';

import { ConditionsWidget } from './conditions-widget';
import { NoConditionsWidget } from './no-conditions';

const { previousHealthConditions, currentHealthConditions } = healthConditionsMockData;
const commonCount = previousHealthConditions.length + currentHealthConditions.length;

describe('HealthConditionsWidget', () => {
  let component: ReturnType<typeof mount | typeof shallow>;

  beforeEach(() => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({ OVERVIEW_WIDGETS: { conditions: healthConditionsMockData } }));

    component = mount(<ConditionsWidget />);
  });

  it('component should match to snapshot', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('no conditions widget should match to snapshot', () => {
    component = shallow(<NoConditionsWidget isCurrent={true}/>);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('component should contain right count of conditions', () => {
    expect(component.find(ConditionPlate)).toHaveLength(commonCount);
    expect(toJson(component)).toMatchSnapshot();
  });

  it('component should contain render no conditions widget if there is no conditions', () => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({
          OVERVIEW_WIDGETS: {
            conditions: {
              previousHealthConditions: [],
              currentHealthConditions: [],
            },
          },
        }));

    component = mount(<ConditionsWidget />);

    expect(component.find(ConditionPlate)).toHaveLength(0);
    expect(component.find(NoConditionsWidget)).toHaveLength(2);
  });
});
