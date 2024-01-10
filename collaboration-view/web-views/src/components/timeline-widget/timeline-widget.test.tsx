import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { PatientAppointmentAlias } from '@enums';

import { TimelineWidget } from './timeline-widget';

const countTimelineAppointmentsComponents = Object.values(PatientAppointmentAlias).length;

const aliases = [ PatientAppointmentAlias.RECENT, PatientAppointmentAlias.UPCOMING ];

describe('TimelineWidget', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = shallow(<TimelineWidget />);
  });

  it('should render the TimelineWidget component', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it(`should render ${countTimelineAppointmentsComponents} the TimelineAppointments components`, () => {
    const timelineAppointmentsComponents = component.find('TimelineAppointments');

    expect(timelineAppointmentsComponents).toHaveLength(countTimelineAppointmentsComponents);
    timelineAppointmentsComponents.forEach((timelineAppointmentComponent, index) => {
      const expectedProps = { alias: aliases[index] };

      expect(timelineAppointmentComponent.props()).toEqual(expectedProps);
    });
  });
});
