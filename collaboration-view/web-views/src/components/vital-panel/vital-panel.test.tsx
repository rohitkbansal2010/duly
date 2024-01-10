import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';
import * as redux from 'react-redux';

import { CardType, MeasurementType, VitalType } from '@enums';
import { DateTimeDoubleChartDataset } from '@types';

import { getConfigVitalPanelByType } from './config-vital-panels';
import { VitalPanel, VitalPanelPropsType } from './vital-panel';
import { VitalPanelMeasurementPropsType } from './vital-panel-measurement';

const mockProps: VitalPanelPropsType = {
  toggle: 'off',
  cardType: CardType.BLOOD_PRESSURE,
  vitals: [
    {
      vitalType: VitalType.BLOOD_PRESSURE,
      measurements: [
        {
          measurementType: MeasurementType.SYSTOLIC_BLOOD_PRESSURE,
          value: 135.14,
          measured: '2021-12-01T11:52:37.583195+00:00',
          unit: 'mm[Hg]',
        },
        {
          measurementType: MeasurementType.DIASTOLIC_BLOOD_PRESSURE,
          value: 93.07,
          measured: '2021-12-01T11:52:37.583195+00:00',
          unit: 'mm[Hg]',
        },
      ],
    },
  ],
};
const mockPatientData = { birthDate: '1979-09-09' };
const mockChart = { datasets: [ { data: { values: [ { y: 94 }, { y: 95 }, { y: 98 } ] } } ] };

const setUp = (props: VitalPanelPropsType) =>
  shallow(<VitalPanel {...props} />);

describe('VitalPanel', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    jest
      .spyOn(redux, 'useSelector')
      .mockImplementation((selector: (state: unknown)=>unknown) =>
        selector({
          CURRENT_APPOINTMENT: { patientData: mockPatientData },
          OVERVIEW_WIDGETS: { chart: mockChart },
        }));

    component = setUp(mockProps);
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('should render correctly', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('should render the VitalPanelMeasurement components', () => {
    const mockMeasurements = mockProps.vitals.map(({ measurements }) =>
      measurements).flat();
    const { measurementsComponents } =
      getConfigVitalPanelByType({
        datasets: mockChart.datasets as DateTimeDoubleChartDataset[],
        cardType: mockProps.cardType,
        measurements: mockMeasurements,
        birthDate: mockPatientData.birthDate,
      });

    const vitalPanelMeasurementComponents = component.find('VitalPanelMeasurement');

    expect(vitalPanelMeasurementComponents).toHaveLength(measurementsComponents.length);
    vitalPanelMeasurementComponents.forEach((vitalPanelMeasurementComponent, index) => {
      const expectedProps: VitalPanelMeasurementPropsType = {
        toggle: 'off',
        convertedHeightUnit: null,
        convertedWeightUnit: null,
        convertedHeightValue: null,
        convertedWeightValue: null,
        cardType: mockProps.cardType,
        isVisible: !!measurementsComponents[index].value,
        ...measurementsComponents[index],
      };

      expect(vitalPanelMeasurementComponent.props()).toEqual(expectedProps);
    });
  });

  it('should render the VitalPanelGeneralAdvice component, if generalAdviceComponent exists', () => {
    const mockMeasurements = mockProps.vitals.map(({ measurements }) =>
      measurements).flat();
    const { generalAdviceComponent } =
      getConfigVitalPanelByType({
        datasets: mockChart.datasets as DateTimeDoubleChartDataset[],
        cardType: mockProps.cardType,
        measurements: mockMeasurements,
        birthDate: mockPatientData.birthDate,
      });

    const vitalGeneralAdviceComponent = component.find('VitalPanelGeneralAdvice');

    expect(vitalGeneralAdviceComponent).toHaveLength(1);
    expect(vitalGeneralAdviceComponent.props()).toEqual(generalAdviceComponent);
  });

  it('should not render the VitalPanelGeneralAdvice component, if generalAdviceComponent does not exist', () => {
    const component = setUp({
      toggle: 'off',
      cardType: CardType.BLOOD_OXYGEN,
      vitals: [
        {
          vitalType: VitalType.BLOOD_OXYGEN,
          measurements: [
            {
              measurementType: MeasurementType.OXYGEN_SATURATION,
              value: 92.31,
              measured: '2021-12-01T11:52:37.5832382+00:00',
              unit: '%',
            },
          ],
        },
      ],
    });

    expect(toJson(component)).toMatchSnapshot();

    const vitalGeneralAdviceComponent = component.find('VitalPanelGeneralAdvice');
    expect(vitalGeneralAdviceComponent).toHaveLength(0);
  });
});
