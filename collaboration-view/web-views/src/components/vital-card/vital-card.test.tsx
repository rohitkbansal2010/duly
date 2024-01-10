import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { Measurements } from '@components/vital-card/measurements';
import { VitalCard, VitalCardPropType } from '@components/vital-card/vital-card';
import { vitalsCardsMap } from '@components/vital-card/vitals-cards-map';
import { CardType, MeasurementType, VitalType } from '@enums';

describe('VitalCard', () => {
  let Component: ReturnType<typeof shallow>;
  const cardType = CardType.HEART_RATE;
  const props: VitalCardPropType = {
    isActiveCard: false,
    onClickVitalCard: jest.fn(),
    cardType,
    vitals: [
      {
        vitalType: VitalType.HEART_RATE,
        measurements: [
          {
            measurementType: MeasurementType.HEART_RATE,
            value: 1,
            measured: '',
          },
        ],
      },
      {
        vitalType: VitalType.PAIN_LEVEL,
        measurements: [
          {
            measurementType: MeasurementType.PAIN_LEVEL,
            value: 1,
            measured: '',
          },
        ],
      },
    ],
  };

  beforeEach(() => {
    Component = shallow(<VitalCard {...props} />);
  });

  it('should render correctly', () => {
    expect(toJson(Component)).toMatchSnapshot();
  });

  it('should handle click on vital card', () => {
    const container = Component.find({ 'data-testid': 'vital-card__container' });
    container.simulate('click');

    expect(props.onClickVitalCard).toHaveBeenCalled();
  });

  it('should render measurements for each vital', () => {
    expect(Component.find(Measurements)).toHaveLength(props.vitals.length);
  });

  it('should render no data text if there are no vitals', () => {
    const wrapper = shallow(<VitalCard {...props} vitals={[]}/>);

    expect(wrapper.find({ 'data-testid': 'vital-card__no-data' }).exists()).toBe(true);
    expect(wrapper.find(Measurements)).toHaveLength(0);
  });

  it('should render only one measurement if card type is BMI', () => {
    const wrapper = shallow(<VitalCard {...props} cardType={CardType.BODY_MASS_INDEX} />);
    expect(wrapper.find(Measurements)).toHaveLength(1);
  });

  it('should render vital height no data text', () => {
    const vitals = [
      {
        vitalType: VitalType.WEIGHT,
        measurements: [],
      },
    ];

    const wrapper = shallow(
      <VitalCard
        {...props}
        cardType={CardType.WEIGHT_HEIGHT}
        vitals={vitals}
      />
    );

    expect(wrapper.find({ 'data-testid': 'vital-card__height__no-data' }).exists()).toBe(true);
  });

  it('should render vital weight no data text', () => {
    const vitals = [
      {
        vitalType: VitalType.HEIGHT,
        measurements: [],
      },
    ];

    const wrapper = shallow(
      <VitalCard
        {...props}
        cardType={CardType.WEIGHT_HEIGHT}
        vitals={vitals}
      />
    );

    expect(wrapper.find({ 'data-testid': 'vital-card__weight__no-data' }).exists()).toBe(true);
  });

  it('should render normal icon if vital card is not active', () => {
    const vitalCardData = vitalsCardsMap[cardType];
    expect(Component.find({ 'data-testid': 'vital-card__icon' }).props().src).toBe(vitalCardData.icon);
  });

  it('should render active icon if vital card is active', () => {
    const wrapper = shallow(<VitalCard {...props} isActiveCard />);
    const vitalCardData = vitalsCardsMap[cardType];

    expect(wrapper.find({ 'data-testid': 'vital-card__icon' }).props().src).toBe(vitalCardData.activeIcon);
  });
});

describe('vitalCardsMap', () => {
  const vitalCardTypes = Object.keys(vitalsCardsMap);
  test.each(vitalCardTypes)('should render correctly "%s" vital card', (vitalCardType) => {
    const props: VitalCardPropType = {
      isActiveCard: false,
      onClickVitalCard: jest.fn(),
      cardType: CardType.HEART_RATE,
      vitals: [],
    };
    const wrapper = shallow(<VitalCard {...props} cardType={vitalCardType as CardType} />);

    const vitalCardData = vitalsCardsMap[vitalCardType as CardType];
    expect(wrapper.find({ 'data-testid': 'vital-card__title' }).text()).toBe(vitalCardData.title);
    expect(wrapper.find({ 'data-testid': 'vital-card__icon' }).props().src).toBe(vitalCardData.icon);
  });
});
