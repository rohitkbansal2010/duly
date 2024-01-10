import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { testReportsResultMockData } from '@mock-data';

import { ResultsComponents } from './results-components';
import { ResultsDetails } from './results-details';
import { ResultsHeader } from './results-header';
import { TestResults } from './test-results';
import { TestResultsSkeleton } from './test-results-skeleton';

describe('TestResults', () => {
  let component: ReturnType<typeof shallow>;

  const {
    id,
    effectiveDate,
    title,
    issued,
    status,
    performers,
    results,
  } = testReportsResultMockData;

  beforeEach(() => {
    jest
      .spyOn(redux, 'useDispatch')
      .mockReturnValue(jest.fn());

    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector(
          {
            CURRENT_APPOINTMENT: {
              testReportsResults: testReportsResultMockData,
              isTestReportResultsSkeletonShown: false,
            },
          }
        ));

    component = shallow(<TestResults />);
  });

  it('component should match snapshot', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('skeleton should match snapshot', () => {
    component = shallow(<TestResultsSkeleton />);
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should contain header with right props', () => {
    const header = component.find(ResultsHeader);
    const expectedProps = {
      title,
      effectiveDate,
    };

    expect(header).toHaveLength(1);
    expect(header.props()).toEqual(expectedProps);
  });

  it('should contain details with right props', () => {
    const details = component.find(ResultsDetails);
    const expectedProps = {
      issued,
      effectiveDate,
      status,
      performers,
    };

    expect(details).toHaveLength(1);
    expect(details.props()).toEqual(expectedProps);
  });

  it('should contain components with right props', () => {
    const components = component.find(ResultsComponents);
    const expectedProps = {
      currentTestResultsId: id,
      results,
    };

    expect(components).toHaveLength(1);
    expect(components.props()).toEqual(expectedProps);
  });

  it('should render skeleton', () => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector(
          {
            CURRENT_APPOINTMENT: {
              testReportsResults: testReportsResultMockData,
              isTestReportResultsSkeletonShown: true,
            },
          }
        ));

    component = shallow(<TestResults />);

    const skeleton = component.find(TestResultsSkeleton);

    expect(skeleton).toHaveLength(1);
  });

  it('should work with empty data', () => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector(
          {
            CURRENT_APPOINTMENT: {
              testReportsResults: {},
              isTestReportResultsSkeletonShown: false,
            },
          }
        ));

    component = shallow(<TestResults />);

    expect(component.find(ResultsHeader)).toHaveLength(1);
    expect(component.find(ResultsDetails)).toHaveLength(1);
    expect(component.find(ResultsComponents)).toHaveLength(1);
    expect(component.text().includes('undefined')).toBeFalsy();
    expect(toJson(component)).toMatchSnapshot();
  });
});
