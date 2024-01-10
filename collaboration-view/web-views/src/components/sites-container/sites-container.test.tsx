import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';

import { SitesContainer } from './sites-container';

const mockDispatch = jest.fn();
jest.mock('react-redux', () =>
  ({
    ...jest.requireActual('react-redux'),
    useDispatch: () =>
      mockDispatch,
  }));


describe('SitesContainer', () => {
  let Component: ReturnType<typeof mount>;
  
  const TestComponent = () => 
    (
      <div data-testid="test-component">
        test
      </div>
    );
  
  beforeEach(() => {
    Component = mount(
      <SitesContainer>
        <TestComponent />
      </SitesContainer>
    );
  });

  it('should render correctly', () => {
    expect(toJson(Component)).toMatchSnapshot();
  });

  it('should render passed children', () => {
    expect(Component.contains(<TestComponent />)).toBe(true);
  });
});
