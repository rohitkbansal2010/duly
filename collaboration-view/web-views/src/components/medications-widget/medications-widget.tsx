import { useSelector } from 'react-redux';

import { FadedScroll } from '@components/faded-scroll-2';
import { MedicationsPlate } from '@components/medications-plate';
import { MedicationsType, AppointmentModuleWidgetsAlias } from '@enums';
import { useWidgetSkeleton } from '@hooks';
import { RootState } from '@redux/reducers';

import { MedicationsWidgetSkeleton } from './medications-widget-skeleton';
import { NoMedicationsWidget } from './no-medications-widget';

const MEDICATIONS_ORDER = [ MedicationsType.REGULAR, MedicationsType.OTHER ];

export const MedicationsWidget = () => {
  const medications = useSelector(({ OVERVIEW_WIDGETS }: RootState) =>
    OVERVIEW_WIDGETS.medications);

  const isSkeletonShown = useWidgetSkeleton(AppointmentModuleWidgetsAlias.MEDICATIONS);

  if (isSkeletonShown) {
    return <MedicationsWidgetSkeleton/>;
  }

  if (!medications || !Object.values(medications).flat()?.length) {
    return <NoMedicationsWidget/>;
  }

  return (
    <FadedScroll height="100%">
      {MEDICATIONS_ORDER.map((type) => {
        const formattedType = type.toLocaleLowerCase();
        const medicationsByType = medications?.[formattedType];
        const hasMedications = !!medicationsByType?.length;

        return hasMedications && (
        <MedicationsPlate
          key={type}
          type={type}
          medications={medications[formattedType]}
        />
        );
      })}
    </FadedScroll>

  );
};
