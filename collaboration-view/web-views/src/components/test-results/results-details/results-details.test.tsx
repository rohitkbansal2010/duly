import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { Avatar } from '@components/avatar';
import { testReportsResultMockData } from '@mock-data';
import {
  getPerformerInfo,
  getSrcAvatar,
  getTestReportDate,
  getTestReportTime,
  getUserRole,
  showFirstPrefixItem
} from '@utils';

import { ResultsDetails } from './results-details';

describe('ResultsDetails', () => {
  let component: ReturnType<typeof shallow>;

  const {
    effectiveDate,
    issued,
    status,
    performers,
  } = testReportsResultMockData;
  
  const mockProps = {
    effectiveDate,
    issued,
    status,
    performers,
  };
  
  const {
    familyName,
    givenNames,
    prefixes,
    photo,
    role,
  } = getPerformerInfo(performers);

  beforeEach(() => {
    jest
      .spyOn(Date.prototype, 'toLocaleTimeString')
      .mockImplementation(() =>
        '10:17 AM');

    component = shallow(<ResultsDetails {...mockProps} />);
  });

  it('component should match snapshot', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('component should contain right titles', () => {
    expect(component.text().includes('ordered by')).toBeTruthy();
    expect(component.text().includes('collected on')).toBeTruthy();
    expect(component.text().includes('resulted on')).toBeTruthy();
    expect(component.text().includes('status')).toBeTruthy();
  });

  it('component should contain avatar', () => {
    const avatar = component.find(Avatar);

    const expectedProps = {
      width: 2.125,
      src: getSrcAvatar(photo),
      alt: `${showFirstPrefixItem(givenNames)}${familyName}`.trim(),
      role: getUserRole({ role, prefixes }),
    };
    
    expect(avatar).toBeTruthy();
    expect(avatar.props()).toEqual(expectedProps);
  });

  it('component should contain right data', () => {
    const formattedEffectiveDate = `${getTestReportDate(effectiveDate)} - ${getTestReportTime(effectiveDate)}`;
    const formattedIssuedDate = `${getTestReportDate(issued!)} - ${getTestReportTime(issued!)}`;

    expect(component.text().includes(mockProps.status)).toBeTruthy();
    expect(component.text().includes(formattedEffectiveDate)).toBeTruthy();
    expect(component.text().includes(formattedIssuedDate)).toBeTruthy();
  });

  it('component should work with empty data', () => {
    component = shallow(<ResultsDetails />);

    expect(component.text().includes('Invalid Date')).toBeFalsy();
    expect(component.text().includes('undefined')).toBeFalsy();
    expect(toJson(component)).toMatchSnapshot();
  });
});
