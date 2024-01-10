import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { CardType } from '@enums';
import { conditionMagentaIcon } from '@icons';
import { formatMDYYYYDate } from '@utils';

import { VitalPanelMeasurement, VitalPanelMeasurementPropsType } from './vital-panel-measurement';

const mockProps: VitalPanelMeasurementPropsType = {
  cardType: CardType.BLOOD_PRESSURE,
  isVisible: true,
  text: 'MOST RECENTLY',
  value: 130,
  style: { width: '44%' },
  unit: 'mmHg',
  maxScaleValue: 10,
  additionalValue: 80,
  icon: conditionMagentaIcon,
  date: '2021-12-01T11:52:37.5832382+00:00',
};

const setUp = (props: VitalPanelMeasurementPropsType) =>
  shallow(<VitalPanelMeasurement {...props} />);

describe('VitalPanelMeasurement', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = setUp(mockProps);
  });

  it('should render correctly', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should not render the VitalPanelMeasurement component, if the isVisible flag be falsy', () => {
    const component = setUp({ ...mockProps, isVisible: false });

    expect(toJson(component)).toMatchSnapshot();
    expect(component.type()).toBeNull();
  });

  it('should render the icon', () => {
    const iconElement = component.find('[date-test="vital-panel-measurement-icon"]');

    expect(iconElement).toHaveLength(1);
    expect(iconElement.props().src?.includes(mockProps.icon as string)).toBeTruthy();
  });

  it('should not render the icon, if the icon does not exist', () => {
    const component = setUp({ ...mockProps, icon: '' });

    expect(toJson(component)).toMatchSnapshot();

    const iconElement = component.find('[date-test="vital-panel-measurement-icon"]');

    expect(iconElement).toHaveLength(0);
  });

  it('should render the text', () => {
    const textElement = component.find('[date-test="vital-panel-measurement-text"]');

    expect(textElement).toHaveLength(1);
    expect(textElement.text().includes(mockProps.text as string)).toBeTruthy();
  });

  it('should render the date', () => {
    const dateElement = component.find('[date-test="vital-panel-measurement-date"]');
    const expectedDate = formatMDYYYYDate(new Date(mockProps.date as string));

    expect(dateElement).toHaveLength(1);
    expect(dateElement.text().includes(expectedDate)).toBeTruthy();
  });

  it('should not render the date, if the date does not exist', () => {
    const component = setUp({ ...mockProps, date: '' });

    expect(toJson(component)).toMatchSnapshot();

    const dateElement = component.find('[date-test="vital-panel-measurement-date"]');

    expect(dateElement).toHaveLength(0);
  });

  it('should render the value', () => {
    const valueElement = component.find('[date-test="vital-panel-measurement-value"]');

    expect(valueElement).toHaveLength(1);
    expect(valueElement.text().includes(String(mockProps.value))).toBeTruthy();
  });

  it('should render the maxScaleValue', () => {
    const maxScaleValueElement = component.find('[date-test="vital-panel-measurement-max-scale-value"]');
    const expectedMaxScaleValue = `/ ${mockProps.maxScaleValue}`;

    expect(maxScaleValueElement).toHaveLength(1);
    expect(maxScaleValueElement.text().includes(expectedMaxScaleValue)).toBeTruthy();
  });

  it('should not render the maxScaleValue, if the maxScaleValue does not exist', () => {
    const component = setUp({ ...mockProps, maxScaleValue: undefined });

    expect(toJson(component)).toMatchSnapshot();

    const maxScaleValueElement = component.find('[date-test="vital-panel-measurement-max-scale-value"]');

    expect(maxScaleValueElement).toHaveLength(0);
  });

  it('should render the unit', () => {
    const component = setUp({ ...mockProps, additionalValue: undefined });

    expect(toJson(component)).toMatchSnapshot();

    const unitElement = component.find('[date-test="vital-panel-measurement-unit"]');

    expect(unitElement).toHaveLength(1);
    expect(unitElement.text().includes(mockProps.unit as string)).toBeTruthy();
  });

  it('should not render the unit, if the unit does not exist', () => {
    const component = setUp({ ...mockProps, unit: '' });

    expect(toJson(component)).toMatchSnapshot();

    const unitElement = component.find('[date-test="vital-panel-measurement-unit"]');

    expect(unitElement).toHaveLength(0);
  });

  it('should render the additionalValue', () => {
    const additionalValueElement = component.find('[date-test="vital-panel-measurement-additional-value"]');
    const expectedAdditionalValue = `/ ${mockProps.additionalValue}`;

    expect(additionalValueElement).toHaveLength(1);
    expect(additionalValueElement.text().includes(expectedAdditionalValue)).toBeTruthy();
  });

  it('should not render the additionalValue, if the additionalValue does not exist', () => {
    const component = setUp({ ...mockProps, additionalValue: undefined });

    expect(toJson(component)).toMatchSnapshot();

    const additionalValueElement = component.find('[date-test="vital-panel-measurement-additional-value"]');

    expect(additionalValueElement).toHaveLength(0);
  });
});
