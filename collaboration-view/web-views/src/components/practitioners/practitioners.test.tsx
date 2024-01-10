import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { PractitionerRole } from '@enums';
import { Practitioner } from '@types';

import { Practitioners } from './practitioners';

const mockPractinioners: Practitioner[] = [
  {
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
  },
  {
    id: 'qwerty2',
    humanName: {
      familyName: 'Ling',
      givenNames: [ 'Jane' ],
      prefixes: [ 'Dr.' ],
    },
    photo: {
      contentType: 'image/x-png',
      title: 'Photo',
      size: 0,
      data: 'data',
    },
    role: PractitionerRole.PCP,
  },
];

describe('Practitioners', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = shallow(
      <Practitioners
        practitioners={mockPractinioners}
      />
    );
  });

  it('should exist', () => {
    expect(Practitioners).toBeDefined();
  });

  it('should render practitioners', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should contains one practitioner with role MA', () => {
    const practitionerWithRoleMA = mockPractinioners.filter(({ role }) =>
      role === PractitionerRole.MA);

    expect(
      component.children().findWhere(child =>
        child.props().role === PractitionerRole.MA)
    ).toHaveLength(practitionerWithRoleMA.length);
  });

  it('should contains practitioners with role PCP', () => {
    const practitionerWithRolePCP = mockPractinioners.filter(({ role }) =>
      role === PractitionerRole.PCP);

    expect(
      component.children().findWhere(child =>
        child.props().role === PractitionerRole.PCP)
    ).toHaveLength(practitionerWithRolePCP.length);
  });
});
