import { mount, ReactWrapper } from 'enzyme';
import toJson from 'enzyme-to-json';
import { useSelector } from 'react-redux';

import { PatientHeaderData } from './patient-header-data';

jest.mock('react-redux', () =>
  ({
    useSelector: jest.fn(),
    useDispatch: jest.fn(),
  }));

export interface MockAvatarProps {
	width: number;
	src: string;
	role: string;
	hasBorder: boolean;
}
const mockAvatar = jest.fn();
jest.mock('@components/avatar', () =>
  ({
    Avatar: (props: MockAvatarProps) => {
      mockAvatar(props);
      return null;
    },
  }));


jest.mock('@utils', () =>
  ({ getSrcAvatar: jest.fn().mockReturnValue('mockSrc') }));

jest.mock('@constants', () =>
  ({ PATIENT_HEADER_AVATAR_SIZE: 55 }));

const mockOnShowPatientViewModal = jest.fn();
jest.mock('@hooks', () =>
  ({
    usePatientViewModal: () =>
      ({ onShowPatientViewModal: mockOnShowPatientViewModal }),
  }));

const patientMockData = {
  patientData: {
    generalInfo: {
      id: 'qwerty1',
      humanName: {
        givenNames: [
          'Ana',
          'Maria',
        ],
      },
    },
    photo: 'mockPhotoData',
  },
};

let wrapper: ReactWrapper;

beforeEach(() => {
  (useSelector as jest.Mock).mockImplementation((selector: (state: unknown) => unknown) =>
    selector({ CURRENT_APPOINTMENT : patientMockData }));

  wrapper = mount(<PatientHeaderData />);
});

describe('PatientHeaderData', () => {
  it('should match snapshot', () => {
    expect(toJson(wrapper)).toMatchSnapshot();
  });

  it('should contain span with patientName', () => {
    expect(wrapper.containsMatchingElement(<span>Ana Maria</span>)).toEqual(true);
  });

  it('should pass photoData into getSrcAvatar', () => {
    const getSrcAvatar = require('@utils').getSrcAvatar;

    expect(getSrcAvatar).toHaveBeenCalledWith(patientMockData.patientData.photo);
  });

  it('should call button clickHandler', () => {
    wrapper.find('button').simulate('click');

    expect(mockOnShowPatientViewModal).toHaveBeenCalled();
  });

  it('should render container if there is patientData', () => {
    expect(wrapper.find({ 'data-testid': 'container' }).exists()).toBe(true);
  });

  it('should render container if there is no patientData', () => {
    (useSelector as jest.Mock).mockImplementation((selector: (state: unknown) => unknown) =>
      selector({ CURRENT_APPOINTMENT: {} }));

    const noPatientDataWrapper = mount(<PatientHeaderData />);

    expect(noPatientDataWrapper.find({ 'data-testid': 'container' }).exists()).toBe(true);
  });

  it('should render "," if there is patientName', () => {
    const text = wrapper.find({ 'data-testid': 'welcome' }).text();

    expect(/,/.test(text)).toBe(true);
  });

  it('should not render "," if there is no patientName', () => {
    patientMockData.patientData.generalInfo.humanName.givenNames = [];

    (useSelector as jest.Mock).mockImplementation((selector: (state: unknown) => unknown) =>
      selector({ CURRENT_APPOINTMENT: patientMockData }));

    const noPatientNameWrapper = mount(<PatientHeaderData />);

    const text = noPatientNameWrapper.find({ 'data-testid': 'welcome' }).text();

    expect(/,/.test(text)).toBe(false);
  });

  it('should pass correct width, src, role and hasBorder props in nested Avatar', () => {
    const mockAvatarProps: MockAvatarProps = {
      width: 55,
      src: 'mockSrc',
      role: 'NonEmployee',
      hasBorder: true,
    };
    expect(mockAvatar).toHaveBeenCalledWith(
      expect.objectContaining(mockAvatarProps)
    );
  });
});
