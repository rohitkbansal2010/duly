import { shallow, ShallowWrapper } from 'enzyme';
import toJson from 'enzyme-to-json';
import { useSelector } from 'react-redux';

import { MONTH_NAMES_SHORT, WEEKDAYS_NAMES } from '@constants';
import { mapPinMagenta } from '@icons';
import { showLocationPickerModal } from '@redux/actions';
import { Site } from '@types';

import { AppointmentDailyStatistics } from './appointment-daily-statistics';
import { NewPatients } from './new-patients';

const mockDispatch = jest.fn();
jest.mock('react-redux', () =>
  ({
    ...jest.requireActual('react-redux'),
    useDispatch: () =>
      mockDispatch,
    useSelector: jest.fn(),
  }));

const mockSite: Site = {
  id: '111',
  address: {
    city: 'Lunacity',
    line: 'FirstLine',
    postalCode: 'Lu001',
    state: 'Lunastate',
  },
};

describe('AppointmentDailyStatistics', () => {
  let dateSpy: jest.SpyInstance;
  beforeAll(() => {
    const mockDate = new Date('2022-11-01');
    dateSpy = jest.spyOn(global, 'Date');
    dateSpy.mockImplementation(() =>
      mockDate);
  });
  afterAll(() => {
    dateSpy.mockRestore();
  });

  let wrapper: ShallowWrapper;
  beforeEach(() => {
    (useSelector as jest.Mock).mockImplementation((selector: (state: unknown) => unknown) =>
      selector({
        TODAYS_APPOINTMENTS: { dailyStatistics: { newPatients: 3 } },
        SITE:{ currentSite: mockSite },
      }));
    wrapper = shallow(<AppointmentDailyStatistics />);
  });

  it('should match snapshot', () => {
    expect(toJson(wrapper)).toMatchSnapshot();
  });

  it('should pass correct props in NewPatients component', () => {
    const newPatientsComponent = wrapper.find(NewPatients);

    expect(newPatientsComponent.props()).toEqual({ newPatients: 3 });
  });

  it('should not render NewPatients component if newPatients = 0', () => {
    (useSelector as jest.Mock).mockImplementation((selector: (state: unknown) => unknown) =>
      selector({
        TODAYS_APPOINTMENTS: { dailyStatistics: { newPatients: 0 } },
        SITE:{ currentSite: mockSite },
      }));
    wrapper = shallow(<AppointmentDailyStatistics />);

    expect(wrapper.find(NewPatients).length).toBe(0);
  });

  it('should render correct weekday in dateRowDay element', () => {
    const weekday = WEEKDAYS_NAMES[new Date().getDay()];
    const renderedWeekday = wrapper.find({ 'data-testid': 'dateRowDay' }).text()
      .slice(0, -1);

    expect(renderedWeekday).toEqual(weekday);
  });

  it('should render correct date in dateRowMonth element', () => {
    const date = new Date().getDate();
    const renderedDateMatchArray = wrapper.find({ 'data-testid': 'dateRowMonth' }).text()
      .match(/\d+/);
    const renderedDate = renderedDateMatchArray && renderedDateMatchArray[0];

    expect(renderedDate).toEqual(String(date));
  });

  it('should render correct month in dateRowMonth element', () => {
    const month = MONTH_NAMES_SHORT[new Date().getMonth()];
    const renderedMonth = wrapper.find({ 'data-testid': 'dateRowMonth' }).text()
      .replace(/[\d|\s]+/, '');

    expect(renderedMonth).toEqual(month);
  });

  it('should render correct year in dateRowYear element', () => {
    const year = new Date().getFullYear();
    const renderedYear = wrapper.find({ 'data-testid': 'dateRowYear' }).text();

    expect(renderedYear).toEqual(String(year));
  });

  it('should call showLocationPickerModal on addressRow element click', () => {
    const addressRowElement = wrapper.find({ 'data-testid': 'addressRow' });
    addressRowElement.simulate('click');

    expect(mockDispatch).toHaveBeenCalledWith(showLocationPickerModal());
  });

  it('should render image in addressRowTopPart element with src = mapPinMagenta', () => {
    const addressRowTopPartElement = wrapper.find({ 'data-testid': 'addressRowTopPart' });
    const image = addressRowTopPartElement.find('img');

    expect(image.prop('src')).toEqual(mapPinMagenta);
  });

  it('should render correct address line in addressRowTopPartSpacingSpan element', () => {
    const line = mockSite?.address?.line;
    const renderedLine = wrapper.find({ 'data-testid': 'addressRowTopPartSpacingSpan' }).text();

    expect(renderedLine).toEqual(line);
  });

  it('should render correct address city, state and postalCode in addressRowBottomPart element', () => {
    const renderedText = wrapper.find({ 'data-testid': 'addressRowBottomPart' }).text();

    expect(renderedText)
      .toEqual(`${mockSite?.address?.city}, ${mockSite?.address?.state} ${mockSite?.address?.postalCode}`);
  });
});
