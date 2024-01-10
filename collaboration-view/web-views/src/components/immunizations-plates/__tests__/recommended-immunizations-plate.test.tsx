import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';

import { GROUP_ID_RECOMMENDED_IMMUNIZATION, IMMUNIZATIONS } from '@constants';
import { ImmunizationsType } from '@enums';
import { plusIcon } from '@icons';
import { immunizationsWidgetMockData } from '@mock-data';

import { ImmunizationsGroup } from '../immunizations-group';
import { ImmunizationsTitle } from '../immunizations-title';
import { ImmunizationsToggle } from '../immunizations-toggle';
import { NoImmunizationsPlate } from '../no-immunizations-plate';
import { RecommendedImmunizationsPlate, ImmunizationsPlateProps } from '../recommended-immunizations-plate';

const mockProps: ImmunizationsPlateProps = {
  immunizations: immunizationsWidgetMockData.recommendedImmunizations,
  immunizationsCount: immunizationsWidgetMockData.recommendedImmunizations.length,
  accordions: { [GROUP_ID_RECOMMENDED_IMMUNIZATION]: [ '1', '2', '3' ] },
  handleClickImmunizationGroupItem: jest.fn(),
};
const type = ImmunizationsType.RECOMMENDED;

describe('RecommendedImmunizationsPlate', () => {
  let component: ReturnType<typeof mount>;

  beforeEach(() => {
    component = mount(
      <RecommendedImmunizationsPlate {...mockProps} />
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

  it('should not contain immunizations count', () => {
    component = mount(
      <RecommendedImmunizationsPlate
        {...mockProps}
        immunizations={[ mockProps.immunizations[0] ]}
        immunizationsCount={1}
      />
    );
    expect(component.find(ImmunizationsTitle)
      .props().immunizationsCount === 1).toBeTruthy();
    expect(component.find(ImmunizationsTitle).text()
      .includes('1')).toBeFalsy();
  });

  it('should contains immunizations groups', () => {
    expect(component.find(ImmunizationsGroup)).toHaveLength(mockProps.immunizations.length);
  });

  it('should contains toggle buttons', () => {
    expect(component.find(ImmunizationsToggle)).toHaveLength(mockProps.immunizations.length);
  });

  it('should contains right toggles icon', () => {
    expect(component.findWhere(n =>
      n.type() === 'img' && n.props().src === plusIcon)).toHaveLength(mockProps.immunizations.length);
  });

  it('should render no available component', () => {
    component = mount(
      <RecommendedImmunizationsPlate
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
