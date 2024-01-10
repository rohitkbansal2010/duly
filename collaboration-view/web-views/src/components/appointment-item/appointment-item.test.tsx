import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';
// eslint-disable-next-line no-use-before-define
import React from 'react';
import { Card } from 'react-bootstrap';

import { AppointmentItem, AppointmentItemProps } from '@components/appointment-item/appointment-item';
import {
  AppointmentStatus, AppointmentType, CareTeamMemberRole, PractitionerRole 
} from '@enums';
import { setPickedAppointmentId } from '@redux/actions';

const mockDispatch = jest.fn();
jest.mock('react-redux', () =>
  ({
    ...jest.requireActual('react-redux'),
    useDispatch: () =>
      mockDispatch,
    useSelector: jest.fn().mockImplementation(() => 
      ({ width: 1920 })),
  }));
	

jest.spyOn(Date.prototype, 'getTimezoneOffset')
  .mockImplementation(() =>
    0);

jest.mock('../../common/utils/appointment', () =>
  ({
    ...jest.requireActual('../../common/utils/appointment'),
    calcTopOfAppointment: jest.fn().mockReturnValue(0),
  }));

jest.mock('../../common/utils/date', () =>
  ({
    ...jest.requireActual('../../common/utils/date'),
    writeTimeInterval: jest.fn().mockReturnValue('0:00 - 0:00 pm'),
  }));

const emptyAppointment = {
  id: '1',
  title: '',
  type: AppointmentType.TELEHEALTH,
  status: AppointmentStatus.SCHEDULED,
  timeSlot: {
    startTime: '',
    endTime: '',
  },
  location: { title: '' },
  patient: {
    patientGeneralInfo: {
      humanName: {
        familyName: '',
        givenNames: [],
      },
      id: '1',
    },
    isNewPatient: true,
  },
  practitioner: {
    humanName: {
      familyName: '',
      givenNames: [],
    },
    id: '1',
    role: CareTeamMemberRole.DOCTOR,
  },
  isProtectedByBtg: false,
  startTimeGroups: 1,
};

describe('AppointmentItem', () => {
  let Component: ReturnType<typeof mount>;
  const props: AppointmentItemProps = {
    id: '1',
    title: 'test appointment',
    type: AppointmentType.TELEHEALTH,
    status: AppointmentStatus.SCHEDULED,
    timeSlot: {
      startTime: '2022-01-13T00:00:00',
      endTime: '2022-01-13T01:00:00',
    },
    patient: {
      patientGeneralInfo: {
        humanName: {
          familyName: 'second name',
          givenNames: [],
        },
        id: '1',
      },
      isNewPatient: true,
    },
    practitioner: {
      humanName: {
        familyName: 'practioner name',
        givenNames: [],
      },
      id: '1',
      role: CareTeamMemberRole.DOCTOR,
    },
    isProtectedByBtg: false,
    startTimeGroups: 1,
  };

  beforeEach(() => {
    jest.spyOn(React, 'useEffect').mockImplementation(jest.fn());
    Component = mount(
      <AppointmentItem {...props} />
    );
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('should render correctly', () => {
    expect(toJson(Component)).toMatchSnapshot();
  });

  it('should render telehealth image if appointment is telehealth', () => {
    expect(Component.find({ 'data-testid': 'appointment-item__telehealth-icon' })
      .exists()).toBe(true);
  });

  it('should render new patient image if patient is new', () => {
    expect(Component.find({ 'data-testid': 'appointment-item__new-patient-icon' })
      .exists()).toBe(true);
  });

  it('should dispatch correct action by clicking appointment', () => {
    const wrapper = Component.find(Card.Body);
    wrapper.simulate('click');
    expect(mockDispatch).toHaveBeenCalledWith(setPickedAppointmentId(props.id));
  });

  it('should not render image if appointment type is not telehealth', () => {
    const wrapper = mount(
      <AppointmentItem {...props} type={AppointmentType.ON_SITE} />
    );

    expect(wrapper.find({ 'data-testid': 'appointment-item__telehealth-icon' })
      .exists()).toBe(false);
  });

  it('should not apply column align class if duration is 15 minutes', () => {
    const timeSlot = {
      startTime: '2022-01-13T00:00:00Z',
      endTime: '2022-01-13T00:15:00Z',
    };

    const wrapper = mount(
      <AppointmentItem {...props} timeSlot={timeSlot} />
    );

    expect(wrapper.find('.d-flex flex-column').exists()).toBe(false);
  });

  it('should render correctly appointment with empty data', () => {
    const wrapper = mount(
      <AppointmentItem {...emptyAppointment} />
    );

    expect(toJson(wrapper)).toMatchSnapshot();
  });

  it('should render correctly given names of the patient', () => {
    const patient = {
      patientGeneralInfo: {
        humanName: {
          familyName: 'patient name',
          givenNames: [ 'patientName1', 'patientName2' ],
        },
        id: '1',
      },
      isNewPatient: true,
    };

    const wrapper = mount(<AppointmentItem {...props} patient={patient}/>);

    const patientName = wrapper.find({ 'data-testid': 'appointment-item__patient-name' }).text();
    expect(patientName.includes(patient.patientGeneralInfo.humanName.familyName)).toBe(true);

    patient.patientGeneralInfo.humanName.givenNames.forEach((name) => {
      expect(patientName.includes(name));
    });
  });

  it('should render correctly given names of the practitioner', () => {
    const practitioner = {
      humanName: {
        familyName: 'practioner name',
        givenNames: [ 'givenname1', 'givenname2' ],
      },
      id: '1',
      role: PractitionerRole.PCP,
    };

    const wrapper = mount(<AppointmentItem {...props} practitioner={practitioner} />);

    const practitionerInfo = wrapper.
      find({ 'data-testid': 'appointment-item__practitioner-card' }).text();
    expect(practitionerInfo.includes(practitioner.humanName.familyName)).toBe(true);

    practitioner.humanName.givenNames.forEach((name) => {
      expect(practitionerInfo.includes(name));
    });
  });
});
