import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import { ModalTitle, Button } from 'react-bootstrap';
import * as redux from 'react-redux';

import { appointmentsMockData } from '@mock-data';
import { writeHumanName, writeTimeInterval } from '@utils';

import { PickedAppointmentModal } from './picked-appointment-modal';

describe('PickedAppointmentModal', () => {
  let component: ReturnType<typeof shallow>;
  const mockPickedAppointment = appointmentsMockData[6];
  const {
    title: expectedServiceType,
    patient: {
      patientGeneralInfo: {
        humanName: {
          givenNames: patientGivenNames,
          familyName: patientFamilyName,
        },
      },
    },
    practitioner: {
      humanName: {
        familyName: providerFamilyName,
        prefixes: providerPrefixes,
      },
    },
    timeSlot: { startTime, endTime },
  } = mockPickedAppointment;
  beforeEach(() => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown) => unknown) =>
        selector({
          TODAYS_APPOINTMENTS: {
            appointments: appointmentsMockData,
            pickedAppointmentId: mockPickedAppointment.id,
          },
        }));
    jest
      .spyOn(redux, 'useDispatch')
      .mockReturnValue(jest.fn());
    component = shallow(<PickedAppointmentModal />);
  });
  afterEach(() => {
    jest.restoreAllMocks();
  });
  it('should exist', () => {
    expect(PickedAppointmentModal).toBeDefined();
  });
  it('should render PickedAppointmentModal', () => {
    expect(toJson(component)).toMatchSnapshot();
  });
  it('should contains title', () => {
    const expectedTitle = 'Are you sure you want to launch this appointment?';
    expect(component.find(ModalTitle)).toHaveLength(2);
    expect(component.text().includes(expectedTitle)).toBeTruthy();
  });
  it('should contains patient name', () => {
    const expectedPatientName = writeHumanName(patientGivenNames || [], patientFamilyName);
    expect(component.text().includes(expectedPatientName)).toBeTruthy();
  });
  it('should contains service type of appointment', () => {
    expect(component.text().includes(expectedServiceType)).toBeTruthy();
  });
  it('should contains encounter start and end time', () => {
    const expectedTime = writeTimeInterval(new Date(startTime), new Date(endTime));
    expect(component.text().includes(expectedTime)).toBeTruthy();
  });
  it('should contains provider name', () => {
    const expectedProviderName = `${providerPrefixes} ${providerFamilyName}`;
    expect(component.text().includes(expectedProviderName)).toBeTruthy();
  });
  it('should contains cancel and launch buttons', () => {
    expect(component.find(Button)).toHaveLength(2);
    expect(component.find(Button).filterWhere(n =>
      n.text() === 'Cancel')).toHaveLength(1);
    expect(component.find(Button).filterWhere(n =>
      n.text() === 'Launch')).toHaveLength(1);
  });
});
