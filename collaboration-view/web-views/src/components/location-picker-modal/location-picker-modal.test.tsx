import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';
import { Modal } from 'react-bootstrap';
import { act } from 'react-dom/test-utils';
import { useSelector } from 'react-redux';

import { LocationPickerModal } from '@components/location-picker-modal';
import { SITE_ID } from '@constants';
import { hideLocationPickerModal, setCurrentSite } from '@redux/actions';
import { formatAddress, setLocalStorageItem } from '@utils';


const mockDispatch = jest.fn();
jest.mock('react-redux', () =>
  ({
    ...jest.requireActual('react-redux'),
    useDispatch: () =>
      mockDispatch,
    useSelector: jest.fn(),
  }));

jest.mock('../../common/utils/local-storage');

const mockSites = [
  {
    id: '1',
    address: {
      city: 'test city 1',
      line: 'test line 1',
      postalCode: 'test postalCode 1',
      state: 'test state 1',
    },
  },
  {
    id: '2',
    address: {
      city: 'test city 2',
      line: 'test line 2',
      postalCode: 'test postalCode 2',
      state: 'test state 2',
    },
  },
  {
    id: '3',
    address: {
      city: 'test city 3',
      line: 'test line 3',
      postalCode: 'test postalCode 3',
      state: 'test state 3',
    },
  },
];

describe('LocationPickerModal', () => {
  let Component: ReturnType<typeof mount>;
  const mockState = { isLocationPickerModalShown: true, sites: mockSites, currentSite: null };

  beforeEach(() => {
    (useSelector as jest.Mock).mockImplementation((selector: (state: unknown) => unknown) =>
      selector({ SITE: mockState }));
    Component = mount(
      <LocationPickerModal />
    );
  });

  it ('should render correctly', () => {
    expect(toJson(Component)).toMatchSnapshot();
  });

  it('should have default title without location', () => {
    const dropdownButton = Component.find('.locationPickerModalDropdownButton').find('button');

    expect(dropdownButton.text()).toBe('Selection');
  });

  it('should render list of sites', async () => {
    const dropdownButton = Component.find('.locationPickerModalDropdownButton').find('button');

    dropdownButton.simulate('click');

    const dropdownItems = Component.find('.locationPickerModalDropdownButton').find({ 'data-testid': 'location-picker-modal__dropdown__dropdown-item' });

    expect(dropdownItems.length).toBe(mockSites.length);

    await act(async () =>
      undefined);
  });

  it('should not dispatch close modal action if no site was selected', () => {
    const ModalWrapper = Component.find(Modal);

    ModalWrapper.props().onHide();
    expect(mockDispatch).not.toHaveBeenCalled();
  });

  it('should render correct title if site is present', () => {
    (useSelector as jest.Mock).mockImplementation((selector: (state: unknown) => unknown) =>
      selector({ SITE: { ...mockState, currentSite: mockSites[0] } }));

    const wrapper = mount(<LocationPickerModal />);

    const dropdownButton = wrapper.find('.locationPickerModalDropdownButton').find('button');

    expect(dropdownButton.text()).toBe(formatAddress(mockSites[0].address));
  });

  it('should dispatch correct action if site is present', () => {
    (useSelector as jest.Mock).mockImplementation((selector: (state: unknown) => unknown) =>
      selector({ SITE: { ...mockState, currentSite: mockSites[0] } }));

    const wrapper = mount(<LocationPickerModal />);

    const ModalWrapper = wrapper.find(Modal);

    ModalWrapper.props().onHide();
    expect(mockDispatch).toHaveBeenCalledWith(hideLocationPickerModal());
  });
});


describe('should handle click', () => {
  const mockState = { isLocationPickerModalShown: true, sites: mockSites, currentSite: null };

  beforeEach(() => {
    (useSelector as jest.Mock).mockImplementation((selector: (state: unknown) => unknown) =>
      selector({ SITE: mockState }));
  });

  test.each(mockSites.map((site, index) =>
    ({ index, site: { ...site } })))('should handle click for test site $site.id', async ({ index, site }) => {
    const formattedAddress = formatAddress(site.address);

    const Component = mount(<LocationPickerModal />);

    const dropdownButton = Component.find('.locationPickerModalDropdownButton').find('button');

    dropdownButton.simulate('click');

    const dropdownItems = Component.find('.locationPickerModalDropdownButton').find({ 'data-testid': 'location-picker-modal__dropdown__dropdown-item' });
    expect(dropdownItems.at(index).text()).toBe(formattedAddress);

    dropdownItems.at(index).simulate('click');

    expect(setLocalStorageItem).toHaveBeenCalledWith(SITE_ID, site.id);
    expect(mockDispatch).toHaveBeenCalledWith(setCurrentSite(site));
    expect(mockDispatch).toHaveBeenCalledWith(hideLocationPickerModal());

    Component.update();

    const updatedDropdownButton = Component.find('.locationPickerModalDropdownButton').find('button');
    expect(updatedDropdownButton.text()).toBe(formattedAddress);

    await act(async () =>
      undefined);
  }, 0);
});
