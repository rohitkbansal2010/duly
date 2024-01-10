import { shallow, mount } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { MedicationsPlate } from '@components/medications-plate';
import { mockMedicationsWidgetData } from '@mock-data';

import { MedicationsWidget } from './medications-widget';
import { MedicationsWidgetSkeleton } from './medications-widget-skeleton';
import { NoMedicationsWidget } from './no-medications-widget';

describe('MedicationsWidget', () => {
  let component: ReturnType<typeof mount | typeof shallow>;

  beforeEach(() => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({ OVERVIEW_WIDGETS: { medications: mockMedicationsWidgetData } }));

    component = mount(<MedicationsWidget />);
  });

  it('component should match snapshot', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('skeleton should match snapshot', () => {
    component = shallow(<MedicationsWidgetSkeleton />);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('no medications widget should match snapshot', () => {
    component = shallow(<NoMedicationsWidget />);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('component should contain two types of medications', () => {
    expect(component.find(MedicationsPlate)).toHaveLength(2);
  });

  it('component should render no medications if there is no medications', () => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({ OVERVIEW_WIDGETS: { medications: {} } }));

    component = mount(<MedicationsWidget />);

    expect(component.find(MedicationsPlate)).toHaveLength(0);
    expect(component.find(NoMedicationsWidget)).toHaveLength(1);
  });
});
