import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { PractitionerRole } from '@enums';
import { Practitioner } from '@types';

import { TodaysAppointmentPractitioners } from './todays-appointment-practitioners';

const mockPractitioners: Practitioner[] = [ {
  id: '001',
  humanName: {
    familyName: 'qwe',
    givenNames: [ 'Foo', 'E' ],
    prefixes: [ 'Dr.' ],
  },
  role: PractitionerRole.PCP,
},
{
  id: '002',
  humanName: {
    familyName: 'asd',
    givenNames: [ 'Bar', 'E' ],
    prefixes: [ 'Dr.' ],
  },
  photo: {
    contentType: 'image/jpg',
    title: 'photo',
    size: 0,
    url: 'https://dmgwebprodstorage.blob.core.windows.net/dmgprodweb/physician-headshots/Fitzgerald_Michael_FM-003websize.jpg',
  },
  role: PractitionerRole.PCP,
},
{
  id: '003',
  humanName: {
    familyName: 'zxc',
    givenNames: [ 'Baz', 'E' ],
    prefixes: [ 'Dr.' ],
  },
  role: PractitionerRole.PCP,
} ];

const mockDispatch = jest.fn();
jest.mock('react-redux', () =>
  ({
    ...jest.requireActual('react-redux'),
    useSelector: jest.fn().mockImplementation((selector: (state: unknown) => unknown) =>
      selector({
        TODAYS_APPOINTMENTS: { practitioners: mockPractitioners },
        USER: { userData: mockPractitioners[0] },
      })),
    useDispatch: () =>
      mockDispatch,
  }));

describe('LogoutModal', () => {
  it('should match snapshot', () => {
    const wrapper = shallow(<TodaysAppointmentPractitioners />);

    expect(toJson(wrapper)).toMatchSnapshot();
  });
});
