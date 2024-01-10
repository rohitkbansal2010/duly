import { useSelector } from 'react-redux';

import { StepperTitles } from '@enums';
import { RootState } from '@redux/reducers';

export const useStepperSkeleton = (currentStepAlias: StepperTitles) => {
  const isSkeletonShown = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS.stepsList.find(({ alias }) =>
      alias === currentStepAlias)?.isSkeletonShown);

  return !!isSkeletonShown;
};
