import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { NewPatients } from './new-patients';

describe('NewPatients', () => {
  it('should match snapshot', () => {
    const wrapper = shallow(<NewPatients newPatients={5}/>);

    expect(toJson(wrapper)).toMatchSnapshot();
  });
});
