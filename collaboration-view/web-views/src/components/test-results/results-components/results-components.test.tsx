import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { FadedScroll } from '@components/faded-scroll-2';
import { testReportsResultMockData } from '@mock-data';

import { ResultsComponents } from './results-components';

describe('ResultsComponents', () => {
  let component: ReturnType<typeof shallow>;

  const {
    results,
    id,
  } = testReportsResultMockData;

  const mockProps = {
    results,
    currentTestResultsId: id,
  };

  beforeEach(() => {
    jest
      .spyOn(redux, 'useDispatch')
      .mockReturnValue(jest.fn());

    jest
      .spyOn(redux, 'useSelector')
      .mockReturnValue({ accordions: {} });

    component = shallow(<ResultsComponents {...mockProps} />);
  });

  it('component should match snapshot', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('component should contain invisible scroll', () => {
    expect(component.find(FadedScroll)).toHaveLength(1);
  });

  it('component should contain right titles', () => {
    expect(component.text().includes('component')).toBeTruthy();
    expect(component.text().includes('your value')).toBeTruthy();
    expect(component.text().includes('standard range')).toBeTruthy();
  });

  it('component should work with empty data', () => {
    component = shallow(<ResultsComponents />);

    expect(component.text().includes('undefined')).toBeFalsy();
    expect(toJson(component)).toMatchSnapshot();
  });
});
