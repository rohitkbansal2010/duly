import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { Avatar } from '@components/avatar';
import { PractitionerRole } from '@enums';

import { Practitioner, PractitionerPropsType } from './practitioner';

const setUp = (props: PractitionerPropsType) =>
  shallow(<Practitioner {...props} />);

const mockProps: PractitionerPropsType = {
  id: 'qwerty1',
  humanName: {
    familyName: 'Morris',
    givenNames: [ 'Jane' ],
  },
  photo: {
    contentType: 'image/x-png',
    title: 'Photo',
    size: 0,
    data: 'data',
  },
  role: PractitionerRole.MA,
  isCurrentUser: false,
  isTodaysAppointment: true,
};

describe('Practitioner', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    jest
      .spyOn(redux, 'useSelector')
      .mockReturnValue(jest.fn());
    jest
      .spyOn(redux, 'useDispatch')
      .mockReturnValue(jest.fn());

    component = setUp(mockProps);
  });

  it('should exist', () => {
    expect(Practitioner).toBeDefined();
  });

  it('should render the "Practitioner" component with role MA', () => {
    expect(toJson(component)).toMatchSnapshot();
    expect(component.find(Avatar)).toHaveLength(1);
    expect(component.text().includes(mockProps.humanName.givenNames.join(' '))).toBeTruthy();
    expect(component.text().includes(mockProps.role)).toBeTruthy();
  });
});
