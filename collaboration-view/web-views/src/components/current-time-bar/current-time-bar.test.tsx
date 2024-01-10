import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';
import { act } from 'react-dom/test-utils';

import { CurrentTimeBar } from '@components/current-time-bar/current-time-bar';

jest.spyOn(Date.prototype, 'getTimezoneOffset')
  .mockImplementation(() =>
    0);

describe('current-time-bar', () => {
  let Component: ReturnType<typeof mount>;
  const date = '2021-12-05T08:00:00';

  beforeAll(() => {
    jest.useFakeTimers('modern');
  });

  afterAll(() => {
    jest.useRealTimers();
  });

  beforeEach(() => {
    jest.setSystemTime(new Date(date));
    Component = mount(<CurrentTimeBar />);
  });
  
  it('should render correctly', () => {
    expect(toJson(Component)).toMatchSnapshot();
  });

  it('should update time', () => {
    expect(Component.find({ 'data-testid': 'current-time-bar__text' }).text()).toBe('8:00 am');
    const oneHourInMiliseconds = 3600000;

    act(() => {
      jest.advanceTimersByTime(oneHourInMiliseconds);
      Component.update();
    });

    expect(Component.find({ 'data-testid': 'current-time-bar__text' }).text()).toBe('9:00 am');
  });

  it('should not render timebar if current hour is not in working span', () => {
    const newDate = '2021-12-05T20:00:00';
    jest.setSystemTime(new Date(newDate));

    const wrapper = mount(<CurrentTimeBar />);


    expect(wrapper.find({ 'data-testid': 'current-time-bar__container' }).exists()).toBe(false);
  });
});
