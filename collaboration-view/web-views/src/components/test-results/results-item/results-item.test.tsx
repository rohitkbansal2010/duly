import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { CustomAccordionToggle } from '@components/custom-accordion-toggle';
import { speechBubbleOrangeIcon } from '@icons';
import {
  getMeasurementValue,
  getMeasurementUnit,
  getReferenceRange,
  formattedNotes
} from '@utils';

import { ResultsItem } from './results-item';

describe('ResultsItem', () => {
  let component: ReturnType<typeof shallow>;

  const measurement = {
    value: 150,
    unit: 'mg/dL', 
  };

  const onClickNotes = jest.fn();
    
  const mockProps = {
    componentName: 'Cholesterol, Total',
    isAbnormalResult: true,
    measurement,
    referenceRange: { text: '<200' },
    notes: [
      'Desirable  <200 mg/dL\\r\\nBorderline  200-239 mg/dL\\r\\nHigh      >=240 mg/dL\\r\\n\\r\\n',
    ],
    activeKey: '06cd80cf-7230-4e53-bdd1-6956402b2ef8',
    eventKey: '06cd80cf-7230-4e53-bdd1-6956402b2ef8',
    onClickNotes,
  };

  const mockEmptyProps = {
    componentName: '',
    isAbnormalResult: false,
    activeKey: '06cd80cf-7230-4e53-bdd1-6956402b2ef8',
    eventKey: '06cd80cf-7230-4e53-bdd1-6956402b2ef8',
    onClickNotes,
  };

  beforeEach(() => {
    component = shallow(<ResultsItem {...mockProps} />);
  });

  it('component should match snapshot', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('component should contain right data', () => {
    const value =
      `${getMeasurementValue(mockProps.measurement)} ${getMeasurementUnit(mockProps.measurement)}`;
    const referenceRange = getReferenceRange(mockProps.referenceRange);
    const notes = formattedNotes(mockProps.notes);

    expect(component.text().includes(value)).toBeTruthy();
    expect(component.text().includes(mockProps.componentName)).toBeTruthy();
    expect(component.text().includes(referenceRange)).toBeTruthy();
    expect(component.text().includes(notes)).toBeTruthy();
  });

  it('component should contain notes and past results toggles', () => {
    expect(component.find(CustomAccordionToggle)).toHaveLength(2);
  });

  it('component should contain abnormal icon', () => {
    expect(component.findWhere(n =>
      n.type() === 'img' && n.props().src === speechBubbleOrangeIcon)).toHaveLength(1);
  });

  it('should work with empty data', () => {
    component = shallow(<ResultsItem {...mockEmptyProps}/>);

    expect(component.text().includes('undefined')).toBeFalsy();
    expect(toJson(component)).toMatchSnapshot();
  });
});
