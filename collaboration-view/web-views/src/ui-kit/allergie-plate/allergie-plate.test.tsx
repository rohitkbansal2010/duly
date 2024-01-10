import { shallow, mount } from 'enzyme';
import toJson from 'enzyme-to-json';
import capitalize from 'lodash/capitalize';

import {
  foodAllergieIcon,
  medicalAllergieIcon,
  biologicAllergieIcon,
  environmentAllergieIcon,
  noAllergiesIcon,
  severeLevelWarning
} from '@icons';
import { allergiesMockData } from '@mock-data';
import { formatMDYYYYDate } from '@utils';

import { AllergiePlate } from './allergie-plate';
import { AllergiePlateSkeleton } from './allergie-plate-skeleton';

describe('AllergiesPlate', () => {
  let component: ReturnType<typeof mount | typeof shallow>;
  const mockProps = allergiesMockData[0];

  it('component should match snapshot', () => {
    component = shallow(<AllergiePlate {...mockProps} />);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('skeleton should match snapshot', () => {
    component = shallow(<AllergiePlateSkeleton />);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('should render right data', () => {
    component = mount(<AllergiePlate {...mockProps} />);

    const {
      title,
      categories,
      reactions: [ { title: reactionsTitle } ],
      recorded,
    } = mockProps;

    expect(component.text().includes(capitalize(title!))).toBeTruthy();
    expect(component.text().includes(capitalize(reactionsTitle))).toBeTruthy();
    expect(component.text().includes(categories.join(''))).toBeTruthy();
    expect(component.text().includes(formatMDYYYYDate(recorded))).toBeTruthy();
  });

  it('should render severity icon', () => {
    component = shallow(<AllergiePlate {...mockProps} />);

    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === severeLevelWarning)).toHaveLength(1);
  });

  it('should not render severity icon', () => {
    component = shallow(<AllergiePlate {...allergiesMockData[1]} />);

    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === severeLevelWarning)).toHaveLength(0);
  });

  it('should render food category icon', () => {
    component = shallow(<AllergiePlate {...{ ...mockProps, categories: [ 'Food' ] } } />);

    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === foodAllergieIcon)).toHaveLength(1);
  });

  it('should render medication category icon', () => {
    component = shallow(<AllergiePlate {...{ ...mockProps, categories: [ 'Medication' ] } } />);

    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === medicalAllergieIcon)).toHaveLength(1);
  });

  it('should render biologic category icon', () => {
    component = shallow(<AllergiePlate {...{ ...mockProps, categories: [ 'Biologic' ] } } />);

    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === biologicAllergieIcon)).toHaveLength(1);
  });

  it('should render environment category icon', () => {
    component = shallow(<AllergiePlate {...{ ...mockProps, categories: [ 'Environment' ] } } />);

    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === environmentAllergieIcon)).toHaveLength(1);
  });

  it('should render common icon', () => {
    component = shallow(<AllergiePlate {...{ ...mockProps, categories: [ 'Environment', 'Biologic' ] } } />);

    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === noAllergiesIcon)).toHaveLength(1);

    component = shallow(<AllergiePlate { ...{ ...mockProps, categories: [] } } />);

    expect(component.findWhere(n => 
      n.type() === 'img' && n.props().src === noAllergiesIcon)).toHaveLength(1);
  });
});
