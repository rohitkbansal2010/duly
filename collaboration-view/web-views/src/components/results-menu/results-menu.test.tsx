import { render } from 'enzyme';
import toJson from 'enzyme-to-json';
import noop from 'lodash/noop';
import * as redux from 'react-redux';
import { MemoryRouter } from 'react-router-dom';

import { testReportsMockData } from '@mock-data';
import { TestReport } from '@types';
import { getTestReportDate } from '@utils';

import { ResultsMenu } from './results-menu';
import { ResultsMenuItem } from './results-menu-item';
import { ResultsMenuSkeleton } from './results-menu-skeleton';

describe('TestResultsMenu', () => {
  let component: ReturnType<typeof render>;
  const getCollection = (tag: string) => 
    component.find(tag).toArray() as cheerio.TagElement[];

  const mockProps = {
    testReports: testReportsMockData,
    isTestReportsSkeletonShown: false,
    isTestReportFetching: false,
    onClick: () =>
      () =>
        noop,
    activeTestReportId: '',
  };

  beforeEach(() => {
    jest
      .spyOn(redux, 'useDispatch')
      .mockReturnValue(jest.fn());
   
    component = render(
      <MemoryRouter>
        <ResultsMenu {...mockProps} />
      </MemoryRouter>
    );
  });

  it('should render component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('skeleton should match snapshot', () => {
    component = render(<ResultsMenuSkeleton />);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('should contain right count of items', () => {
    expect(component.find('.resultsMenuItem')).toHaveLength(testReportsMockData.length);
  });

  it('should contain right icons', () => {
    const icons = getCollection('.resultsMenuItemIcon');
    
    icons.forEach((icon) => {
      const { name, attribs: { src, alt } } = icon;

      expect(
        name === 'img' &&
        src === 'test-results-light-blue.svg' &&
        alt === 'test result icon'
      ).toBeTruthy();
    });
  });

  it('should contain right titles', () => {
    const titles = getCollection('.resultsMenuItemTitlesMain');
    
    titles.forEach((title, index) => {
      const { title: expectedTitle } = testReportsMockData[index];

      expect(title.children[0].data === expectedTitle).toBeTruthy();
    });
  });

  it('should contain right dates', () => {
    const dates = getCollection('.resultsMenuItemTitlesAdditional');
    
    dates.forEach((date, index) => {
      const { date: expecteDate } = testReportsMockData[index];

      expect(date.children[0].data === getTestReportDate(expecteDate)).toBeTruthy();
    });
  });

  it('should contain abnormal icons', () => {
    const expectedCount = testReportsMockData.filter(({ hasAbnormalResults }) =>
      hasAbnormalResults).length;
    const abnormalIcons = getCollection('.resultsMenuItemIconAbnormal');
    
    expect(abnormalIcons).toHaveLength(expectedCount);

    abnormalIcons.forEach((icon) => {
      const { name, attribs: { src, alt } } = icon;

      expect(
        name === 'img' &&
        src === 'speech-bubble-orange.svg' &&
        alt === 'abnormal result icon'
      ).toBeTruthy();
    });
  });

  it('should work with empty data', () => {
    component = render(
      <MemoryRouter>
        <ResultsMenu {...mockProps} testReports={[]} />
      </MemoryRouter>
    );

    expect(component.find('.resultsMenu')).toHaveLength(1);
    expect(component.find('.resultsMenuItem')).toHaveLength(0);
  });

  it('should work with incomplete data', () => {
    const mockIncompleteTestReports: TestReport[] = [
      {
        id: '',
        title: '',
        date: '',
        hasAbnormalResults: false,
      },
      {
        id: '',
        title: '',
        date: '',
        hasAbnormalResults: false,
      },
      {
        id: '',
        title: '',
        date: '',
        hasAbnormalResults: true,
      },
    ];

    component = render(
      <MemoryRouter>
        <ResultsMenu {...mockProps} testReports={mockIncompleteTestReports} />
      </MemoryRouter>
    );
    
    expect(component.find('.resultsMenu')).toHaveLength(1);
    expect(component.find('.resultsMenuItem')).toHaveLength(mockIncompleteTestReports.length);
    expect(component.text().includes('undefined')).toBeFalsy();
    expect(component.text().includes('Invalid Date')).toBeFalsy();
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should contains active class for active item', () => {
    component = render(
      <ResultsMenuItem
        isActive
        onClick={() => 
        null}
        {...testReportsMockData[0]}
      />
    );
          
    expect(component.hasClass('resultsMenuItemActive')).toBeTruthy();
  });
});
