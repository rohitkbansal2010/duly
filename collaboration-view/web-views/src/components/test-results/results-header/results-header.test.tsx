import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { testResultsLightBlueIcon } from '@icons';
import { testReportsResultMockData } from '@mock-data';
import { getTestReportDate } from '@utils';

import { ResultsHeader } from './results-header';

describe('ResultsHeader', () => {
  let component: ReturnType<typeof shallow>;

  const {
    title,
    effectiveDate,
  } = testReportsResultMockData;
  
  const mockProps = {
    title,
    effectiveDate,
  };

  beforeEach(() => {
    component = shallow(<ResultsHeader {...mockProps} />);
  });

  it('component should match snapshot', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should contain right data', () => {
    const formattedDate = getTestReportDate(mockProps.effectiveDate);

    expect(component.text().includes(mockProps.title)).toBeTruthy();
    expect(component.text().includes(formattedDate)).toBeTruthy();
    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === testResultsLightBlueIcon)).toBeTruthy();
  });

  it('should work with empty data', () => {
    component = shallow(<ResultsHeader />);

    expect(component.text().includes('Invalid Date')).toBeFalsy();
    expect(component.text().includes('undefined')).toBeFalsy();
    expect(toJson(component)).toMatchSnapshot();
  });
});
