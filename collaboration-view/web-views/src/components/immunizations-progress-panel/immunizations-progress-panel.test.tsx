import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { PROGRESS_PANEL } from '@constants';
import { immunizationsWidgetMockData } from '@mock-data';
import { ImmunizationsProgress } from '@types';
import { PercentageDonutPieChart } from '@ui-kit/percentage-donut-pie-chart';

import { ImmunizationsProgressPanel } from './immunizations-progress-panel';

const mockProgress: ImmunizationsProgress =
  immunizationsWidgetMockData.progress;

describe('ImmunizationsProgressPanel', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = shallow(
      <ImmunizationsProgressPanel {...mockProgress} />
    );
  });

  it('should render component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should contains right titles and icon', () => {
    const { title, additional, icon } = PROGRESS_PANEL[mockProgress.completionStatus];

    expect(component.text().includes(title)).toBeTruthy();
    expect(component.text().includes(additional)).toBeTruthy();
    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === icon)).toHaveLength(1);
  });

  it('should contains right ending for recommended immunizations', () => {
    expect(component.text().includes('immunizations')).toBeTruthy();

    component = shallow(
      <ImmunizationsProgressPanel
        {...mockProgress}
        recommendedGroupNumber={1}
      />
    );

    expect(component.text().includes('immunizations')).toBeFalsy();
    expect(component.text().includes('immunization')).toBeTruthy();
  });

  it('should contains right props for donut chart', () => {
    expect(component.find(PercentageDonutPieChart)
      .props().percent === mockProgress.percentageCompletion).toBeTruthy();
  });
});
