import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { VitalPanelGeneralAdvice, VitalPanelGeneralAdvicePropsType } from './vital-panel-general-advice';

const mockProps: VitalPanelGeneralAdvicePropsType = {
  title: 'General Advice',
  text: 'Blood pressure numbers of less than 120/80 mm Hg are considered within the normal range. If your results fall into this category, stick with heart-healthy habits.',
  style: { width: '56%' },
};

describe('VitalPanelGeneralAdvice', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = shallow(<VitalPanelGeneralAdvice {...mockProps} />);
  });

  it('should render correctly', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should render title', () => {
    const titleElement = component.find('[data-test="general-advice-title"]');

    expect(titleElement).toHaveLength(1);
    expect(titleElement.text().includes(mockProps.title)).toBeTruthy;
  });

  it('should render text', () => {
    const textElement = component.find('[data-test="general-advice-text"]');

    expect(textElement).toHaveLength(1);
    expect(textElement.text().includes(mockProps.text)).toBeTruthy;
  });
});
