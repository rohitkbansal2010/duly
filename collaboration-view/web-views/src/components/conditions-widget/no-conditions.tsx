import { WidgetNoData } from '@components/widget-no-data';
import { conditionsIcon, currentConditionsIcon } from '@icons';

type NoConditionsWidgetProps = {
  isCurrent: boolean;
};

export const NoConditionsWidget = ({ isCurrent }: NoConditionsWidgetProps) =>
  (
    <WidgetNoData
      view={isCurrent ? 'magenta' : 'blue'}
      icon={<img src={isCurrent ? currentConditionsIcon : conditionsIcon} alt="no conditions" />}
      title={`No ${isCurrent ? 'Current' : 'Previous'} Health Conditions`}
      align="top"
    />
  );
