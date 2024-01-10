export const mockUtilsReturnValues = { 
  getSrcAvatar: 'mockSrc', 
  calculateYears: 100, 
  writeHumanName: 'mockWriteHumanName', 
};

export const mockStyles = {
  patientViewModalHeader: 'mockClassNameHeader',
  patientViewModalBody: 'mockClassNameBody',
  patientViewModalBodyAvatar: 'mockClassNameAvatar',
  patientViewModalBodyHumanName: 'mockClassNameHumanName',
  patientViewModalBodyAdditionalInfo: 'mockClassNameAdditionalInfo',
};

export const mockConstants = { PATIENT_VIEW_MODAL_AVATAR_SIZE: 50 };

export interface MockAvatarProps {
	width: number;
	src: string;
	role: string;
	hasBorder: boolean;
}

export const mockAvatarProps: MockAvatarProps = {
  width: 50,
  src: mockUtilsReturnValues.getSrcAvatar, 
  role: 'mockRole', 
  hasBorder: true, 
};
