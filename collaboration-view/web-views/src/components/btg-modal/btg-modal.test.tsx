import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { BTGModal } from './btg-modal';

const mockDispatch = jest.fn();
jest.mock('react-redux', () =>
  ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn().mockImplementation(() =>
      true),
    useDispatch: () =>
      mockDispatch,
  }));

describe('BTGModal', () => {
  it('should match snapshot', () => {
    expect(toJson(shallow(<BTGModal />))).toMatchSnapshot();
  });
});
