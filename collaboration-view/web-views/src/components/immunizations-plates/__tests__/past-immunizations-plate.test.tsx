import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';

import { GROUP_ID_PAST_IMMUNIZATION, IMMUNIZATIONS } from '@constants';
import { ImmunizationsType } from '@enums';
import { immunizationsWidgetMockData } from '@mock-data';

import { ImmunizationsTitle } from '../immunizations-title';
import { ImmunizationsToggle } from '../immunizations-toggle';
import { NoImmunizationsPlate } from '../no-immunizations-plate';
import { PastImmunizationsPlate, PastImmunizationsPlateProps } from '../past-immunizations-plate';

const mockProps: PastImmunizationsPlateProps = {
  immunizations: immunizationsWidgetMockData.pastImmunizations,
  immunizationsCount: immunizationsWidgetMockData.pastImmunizations.length,
  accordions: { [GROUP_ID_PAST_IMMUNIZATION]: [ '1', '2', '3' ] },
  handleClickImmunizationGroupItem: jest.fn(),
  handleClickPastImmunizationGroup: jest.fn(),
};

const type = ImmunizationsType.PAST;

describe('PastImmunizationsPlate', () => {
  let component: ReturnType<typeof mount>;

  beforeEach(() => {
    component = mount(
      <PastImmunizationsPlate {...mockProps} />
    );
  });

  it('should render component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should contains right titles and icon', () => {
    const { title, additional, icon } = IMMUNIZATIONS[type];

    expect(component.text().includes(title)).toBeTruthy();
    expect(component.text().includes(additional)).toBeTruthy();
    expect(component.findWhere(n =>
      n.type() === 'img' && n.props().src === icon)).toHaveLength(1);
  });

  it('should contains right titles props', () => {
    expect(component.find(ImmunizationsTitle).props().type === type).toBeTruthy();
    expect(component.find(ImmunizationsTitle)
      .props().immunizationsCount === mockProps.immunizations.length).toBeTruthy();
    expect(component.find(ImmunizationsTitle).text()
      .includes(String(mockProps.immunizations.length))).toBeTruthy();
  });

  it('should contains additional toggle button', () => {
    const expectedCount = mockProps.immunizations.length + 1;
    expect(component.find(ImmunizationsToggle)).toHaveLength(expectedCount);
  });

  it('should render no available component', () => {
    component = mount(
      <PastImmunizationsPlate
        {...mockProps}
        immunizations={[]}
        immunizationsCount={0}
      />
    );
    const { noAvailableText } = IMMUNIZATIONS[type];

    expect(component.find(NoImmunizationsPlate)).toHaveLength(1);
    expect(component.text().includes(noAvailableText)).toBeTruthy();
  });
});
