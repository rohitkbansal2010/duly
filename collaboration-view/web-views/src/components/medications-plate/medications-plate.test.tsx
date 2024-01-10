import { shallow } from 'enzyme';
import toJson from 'enzyme-to-json';

import { MedicationsType } from '@enums';
import { mockMedicationsWidgetData } from '@mock-data';

import { MedicationsItem } from './medications-item';
import { MedicationsPlate } from './medications-plate';
import { MedicationsPlateSkeleton } from './medications-plate-skeleton';
import { MedicationsTitle } from './medications-title';

const mockType = MedicationsType.REGULAR;
const mockMedications = mockMedicationsWidgetData[mockType.toLocaleLowerCase()];

describe('MedicationsPlate', () => {
  let component: ReturnType<typeof shallow>;

  beforeEach(() => {
    component = shallow(
      <MedicationsPlate
        type={mockType}
        medications={mockMedications}
      />
    );
  });

  it('component should match snapshot', () => {
    expect(toJson(component)).toMatchSnapshot();
  });

  it('skeleton should match snapshot', () => {
    component = shallow(<MedicationsPlateSkeleton />);

    expect(toJson(component)).toMatchSnapshot();
  });

  it('should contain right count of medications', () => {
    expect(component.find(MedicationsTitle).props().count).toEqual(mockMedications.length);
    expect(component.find(MedicationsItem)).toHaveLength(mockMedications.length);
  });
});
