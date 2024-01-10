import 'jest-localstorage-mock';
import { shallow, ShallowWrapper } from 'enzyme';
import toJson from 'enzyme-to-json';
import { Modal } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import { Avatar } from '@components/avatar';
import { SideModal } from '@components/side-modal';
import { AvatarByRole } from '@enums';
import { dulyLargeWhiteDarkBlueIcon, dulyLargeWhiteVioletIcon } from '@icons';
import { getSrcAvatar, showFirstPrefixItem } from '@utils';

import { LogoutModal } from './logout-modal';

const mockUserData = {
  userData: {
    humanName: {
      familyName: 'Morris',
      givenNames: [ 'Jane' ],
      prefixes: [ 'Ms', 'Dr' ],
    },
    photo: {
      contentType: 'image/x-png',
      title: 'Photo',
      size: 0,
      data: 'data',
    },
  },
  userRole: AvatarByRole.REGULAR_EMPLOYEE,
};


jest.mock('react-redux', () =>
  ({
    useSelector: jest.fn((selector: (state: unknown) => unknown) => {
      const state = { USER: mockUserData };
      return selector(state);
    }),
  }));

const mockLogoutAzureADRedirect = jest.fn();
const mockOnHideLogoutModal = jest.fn();
jest.mock('@hooks', () =>
  ({
    useAzureADAuth: () =>
      ({ logoutAzureADRedirect: mockLogoutAzureADRedirect }),
    useLogoutModal: () =>
      ({
        isLogoutModalShown: true,
        onHideLogoutModal: mockOnHideLogoutModal,
      }),
  }));

jest.mock('@utils', () =>
  ({
    getSrcAvatar: jest.fn(),
    showFirstPrefixItem: jest.fn(),
  }));

let wrapper: ShallowWrapper;
beforeEach(() => {
  (getSrcAvatar as jest.Mock).mockImplementation(() =>
    'mockSrc');
  (showFirstPrefixItem as jest.Mock).mockImplementation(() =>
    'mockFirstPrefixItem');

  wrapper = shallow(<LogoutModal />);
});

describe('LogoutModal', () => {
  it('should match snapshot', () => {
    expect(toJson(wrapper)).toMatchSnapshot();
  });

  it('should call logoutAzureADRedirect on logout button click', () => {
    const logoutButton = wrapper.find({ 'data-testid': 'logout-button' });
    logoutButton.simulate('click');

    expect(mockLogoutAzureADRedirect).toHaveBeenCalledTimes(1);
  });

  it('should call onHideLogoutModal on cancel button click', () => {
    const cancelButton = wrapper.find({ 'data-testid': 'cancel-button' });
    cancelButton.simulate('click');

    expect(mockOnHideLogoutModal).toHaveBeenCalledTimes(1);
  });

  it('should call showFirstPrefixItem twice with correct params', () => {
    const { givenNames, prefixes } = mockUserData.userData.humanName;

    expect(showFirstPrefixItem).toHaveBeenNthCalledWith(1, givenNames);
    expect(showFirstPrefixItem).toHaveBeenNthCalledWith(2, prefixes);
  });

  it('should call getSrcAvatar with correct params', () => {
    const { photo } = mockUserData.userData;

    expect(getSrcAvatar).toHaveBeenCalledWith(photo);
  });

  it('should render SideModal with correct props', () => {
    const sideModal = wrapper.find(SideModal);
    const expectedProps = {
      show: true,
      onHide: mockOnHideLogoutModal,
      backgroundContentColor: 'Magenta',
    };

    expect(sideModal.props()).toEqual(expect.objectContaining(expectedProps));
  });

  it('should render SideModal with backgroundContentColor = Blue if userRole will be not AvatarByRole.REGULAR_EMPLOYEE', () => {
    const mockUseSelector = (selector: (state: unknown) => unknown) => {
      const state = { USER: { ...mockUserData, userRole: AvatarByRole.MEDICAL_DOCTOR } };
      return selector(state);
    };
    (useSelector as jest.Mock).mockImplementationOnce(mockUseSelector)
      .mockImplementationOnce(mockUseSelector);
    const wrapper = shallow(<LogoutModal />);
    const sideModal = wrapper.find(SideModal);
    const expectedProps = { backgroundContentColor: 'Blue' };

    expect(sideModal.props()).toEqual(expect.objectContaining(expectedProps));
  });

  it('should render img nested in Modal.Header with dulyLargeWhiteVioletIcon src props if userRole = AvatarByRole.REGULAR_EMPLOYEE', () => {
    const sideModal = wrapper.find(Modal.Header);
    const expectedProps = { children: <img alt="Large duly icon" src={dulyLargeWhiteVioletIcon} /> };

    expect(sideModal.props()).toEqual(expect.objectContaining(expectedProps));
  });

  it('should render img nested in Modal.Header with dulyLargeWhiteDarkBlueIcon src props if userRole != AvatarByRole.REGULAR_EMPLOYEE', () => {
    const mockUseSelector = (selector: (state: unknown) => unknown) => {
      const state = { USER: { ...mockUserData, userRole: AvatarByRole.MEDICAL_DOCTOR } };
      return selector(state);
    };
    (useSelector as jest.Mock).mockImplementationOnce(mockUseSelector)
      .mockImplementationOnce(mockUseSelector);
    const wrapper = shallow(<LogoutModal />);
    const sideModal = wrapper.find(Modal.Header);
    const expectedProps = { children: <img alt="Large duly icon" src={dulyLargeWhiteDarkBlueIcon} /> };

    expect(sideModal.props()).toEqual(expect.objectContaining(expectedProps));
  });

  it('should render Avatar with correct props', () => {
    const avatar = wrapper.find(Avatar);
    const {
      userRole,
      userData: { humanName: { familyName } },
    } = mockUserData;

    const expectedProps = {
      width: 12.31,
      src: 'mockSrc',
      alt: `mockFirstPrefixItem${familyName}`,
      role: userRole as AvatarByRole,
      hasBorder: true,
    };

    expect(avatar.props()).toEqual(expect.objectContaining(expectedProps));
  });

  it('should render element with correct human name', () => {
    const element = wrapper.find({ 'data-testid': 'human-name' });
    const familyName = mockUserData.userData.humanName.familyName;

    expect(element.text()).toEqual(`mockFirstPrefixItem${familyName}`);
  });

  it('should render element with only givenNames without prefixes if there are no prefixes', () => {
    const mockUseSelector = (selector: (state: unknown) => unknown) => {
      const state = {
        USER: {
          ...mockUserData,
          userData: {
            ...mockUserData.userData,
            humanName: {
              ...mockUserData.userData.humanName,
              prefixes: [],
            },
          },
        },
      };
      return selector(state);
    };
    (useSelector as jest.Mock).mockImplementationOnce(mockUseSelector)
      .mockImplementationOnce(mockUseSelector);
    const wrapper = shallow(<LogoutModal />);

    const element = wrapper.find({ 'data-testid': 'human-name' });
    const givenNames = mockUserData.userData.humanName.givenNames;

    expect(element.text()).toEqual(givenNames.join(' '));
  });
});
