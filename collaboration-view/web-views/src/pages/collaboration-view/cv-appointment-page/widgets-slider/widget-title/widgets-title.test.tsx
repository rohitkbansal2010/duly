import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { WidgetTitle } from './widget-title';

describe('WidgetTitle', () => {
  it('should match snapshot', () => {
    const wrapper = shallow(<WidgetTitle>Mock Title</WidgetTitle>);

    expect(toJson(wrapper)).toMatchSnapshot();
  });
});
