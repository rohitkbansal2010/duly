import { mount, ReactWrapper } from 'enzyme';
import toJson from 'enzyme-to-json';
import { ReactChild } from 'react';
import { useSelector } from 'react-redux';

import { patientMockData } from '@mock-data';
import {
  calculateYears,
  getSrcAvatar,
  writeHumanName
} from '@utils';

import { PatientViewModal } from './patient-view-modal';
import styles from './patient-view-modal.scss';
import { MockAvatarProps } from './testMockData';

jest.mock('./patient-view-modal.scss', () => 
  require('./testMockData').mockStyles);

jest.mock('@constants', () => 
  require('./testMockData').mockConstants);

jest.mock('@enums', () => {
  const originalModule = jest.requireActual('@enums');

  return {
    __esModule: true,
    ...originalModule,
    AvatarByRole: { NON_EMPLOYEE: 'mockRole' }, 
  };
});

jest.mock('react-redux', () => 
  ({ useSelector: jest.fn() }));

  
jest.mock('react-router-dom', () => 
  ({
    ...jest.requireActual('react-router-dom'), // use actual for all non-hook parts
    useParams: () => 
      ({
        appointmentId: 'appointment-id',
        patientId: 'patient-id',
        practitionerId: 'practitioner-id',
      }),
  }));

jest.mock('@utils', () => 
  ({
    getSrcAvatar: jest.fn(),
    calculateYears: jest.fn(),
    writeHumanName: jest.fn(),
  }));
	
const mockUsePatientViewModal = jest.fn();
const mockLogoutAzureADRedirect = jest.fn();
jest.mock('@hooks', () => 
  ({
    useAzureADAuth: () =>
      ({ logoutAzureADRedirect: mockLogoutAzureADRedirect }),
    usePatientViewModal: () => 
      ({
        onHidePatientViewModal: mockUsePatientViewModal,
        isShowPatientViewModal: true, 
      }), 
  }));

const mockSideModal = jest.fn();
interface MockSideModalProps {
	show: boolean;
	onHide: () => void;
	children: ReactChild
}
jest.mock('@components/side-modal', () => 
  ({
    SideModal: (props: MockSideModalProps) => {
      mockSideModal(props);
      return props.children;
    }, 
  }));

const mockModalHeader = jest.fn();
const mockModalBody = jest.fn();
interface MockModalBSProps {
	className: string;
	children: ReactChild
}
jest.mock('react-bootstrap', () => 
  ({
    Modal: {
      Header: (props: MockModalBSProps) => {
        mockModalHeader(props);
        return props.children;
      },
      Body: (props: MockModalBSProps) => {
        mockModalBody(props);
        return props.children;
      },
    },
  }));

const mockAvatar = jest.fn();
jest.mock('@components/avatar', () => 
  ({
    Avatar: (props: MockAvatarProps) => {
      mockAvatar(props);
      return null;
    }, 
  }));

describe('PatientViewModal with normal data', () => {
  let wrapper: ReactWrapper;
  beforeEach(() => {
    (useSelector as jest.Mock).mockImplementationOnce(() => 
      patientMockData);

    const { mockUtilsReturnValues } = require('./testMockData');
    (getSrcAvatar as jest.Mock).mockImplementationOnce(() => 
      mockUtilsReturnValues.getSrcAvatar);
    (calculateYears as jest.Mock).mockImplementationOnce(() => 
      mockUtilsReturnValues.calculateYears);
    (writeHumanName as jest.Mock).mockImplementationOnce(() => 
      mockUtilsReturnValues.writeHumanName);
		
    wrapper = mount(<PatientViewModal />);
  });

  it('should match snapshot', () => {
    expect(toJson(wrapper)).toMatchSnapshot();
  });

  it('should pass correct show and onHide props in nested SideModal', () => {
    expect(mockSideModal).toHaveBeenCalledWith(
      expect.objectContaining({
        show: true,
        onHide: mockUsePatientViewModal,
      })
    );
  });

  it('should pass correct className prop in nested Modal.Header', () => {
    expect(mockModalHeader).toHaveBeenCalledWith(
      expect.objectContaining({ className: styles.patientViewModalHeader })
    );
  });

  it('should pass correct className prop in nested Modal.Body', () => {
    expect(mockModalBody).toHaveBeenCalledWith(
      expect.objectContaining({ className: styles.patientViewModalBody })
    );
  });

  it('should pass correct width, src, role and hasBorder props in nested Avatar', () => {
    const { mockAvatarProps } = require('./testMockData');

    expect(mockAvatar).toHaveBeenCalledWith(
      expect.objectContaining(mockAvatarProps)
    );
  });

  it('should pass photoData into getSrcAvatar', () => {
    expect(getSrcAvatar).toHaveBeenCalledWith(patientMockData.photo);
  });

  it('should pass givenNames and familyName into writeHumanName', () => {
    const { givenNames, familyName } = patientMockData.generalInfo.humanName;

    expect(writeHumanName).toHaveBeenCalledWith(givenNames, familyName);
  });

  it('should render correct name in specific element', () => {
    const text = wrapper.find({ 'data-testid': 'humanName' }).text();
    const mockHumanName = require('./testMockData').mockUtilsReturnValues.writeHumanName;
		
    expect(text).toEqual(mockHumanName);
  });

  it('should render correct gender in specific element', () => {
    const elementText = wrapper.find({ 'data-testid': 'gender' }).text();
		
    expect(elementText).toEqual(patientMockData.gender);
  });

  it('should pass birthDate into calculateYears', () => {
    expect(calculateYears).toHaveBeenCalledWith(patientMockData.birthDate);
  });

  it('should render correct age in specific element', () => {
    const elementText = wrapper.find({ 'data-testid': 'age' }).text();
    const mockAge = require('./testMockData').mockUtilsReturnValues.calculateYears;
		
    expect(elementText).toEqual(`${mockAge} years old`);
  });
});

describe('PatientViewModal with undefined data', () => {
  let wrapper: ReactWrapper;
  beforeEach(() => {
    (useSelector as jest.Mock).mockImplementationOnce(() => 
      ({ patientMockData }));
		
    wrapper = mount(<PatientViewModal />);
  });
	
  it('should pass empty string in src prop in nested Avatar', () => {
    (getSrcAvatar as jest.Mock).mockReturnValueOnce('');
    mount(<PatientViewModal />);

    expect(mockAvatar).toHaveBeenCalledWith(
      expect.objectContaining({ src: '' })
    );
  });

  it('should pass undefined instead of photoData into getSrcAvatar', () => {
    expect(getSrcAvatar).toHaveBeenCalledWith(undefined);
  });

  it('should pass ([], "") into writeHumanName if there is no givenNames and familyName', () => {
    expect(writeHumanName).toHaveBeenCalledWith([], '');
  });

  it('should render nothing in specific element if there is no gender', () => {
    const elementText = wrapper.find({ 'data-testid': 'gender' }).text();
		
    expect(elementText).toEqual('');
  });

  it('should render nothing in specific element if there is no birthDate', () => {
    const elementText = wrapper.find({ 'data-testid': 'age' }).text();

    expect(elementText).toEqual('');
  });
});
