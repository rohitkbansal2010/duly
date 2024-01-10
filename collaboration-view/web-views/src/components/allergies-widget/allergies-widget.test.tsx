import { render, mount } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { allergiesMockData } from '@mock-data';
import { AllergiePlate } from '@ui-kit/allergie-plate';

import { AllergiesWidget } from './allergies-widget';
import { AllergiesWidgetSkeleton } from './allergies-widget-skeleton';
import { NoAllergiesWidget } from './no-allergies';

describe('AllergiesWidget', () => {
  let component: ReturnType<typeof mount | typeof render>;

  beforeEach(() => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({ OVERVIEW_WIDGETS: { allergies: allergiesMockData } }));

    component = mount(<AllergiesWidget />);
  });

  it('component should match snapshot', () => {
    component = render(<AllergiesWidget />);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('skeleton should match snapshot', () => {
    component = render(<AllergiesWidgetSkeleton />);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('no allergies widget should match snapshot', () => {
    component = render(<NoAllergiesWidget />);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('component should contain right count of allergies', () => {
    expect(component.find(AllergiePlate)).toHaveLength(allergiesMockData.length);
  });

  it('component should render no allegries widget if there is no allergies', () => {
    jest.spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({ OVERVIEW_WIDGETS: { allergies: [] } }));

    component = mount(<AllergiesWidget />);

    expect(component.find(AllergiePlate)).toHaveLength(0);
    expect(component.find(NoAllergiesWidget)).toHaveLength(1);
  });
});
