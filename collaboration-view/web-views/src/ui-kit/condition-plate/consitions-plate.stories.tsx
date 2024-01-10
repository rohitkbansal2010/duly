import { ConditionPlate } from './condition-plate';

export default {
  title: 'UI-KIT / ConditionPlate',
  component: ConditionPlate,
};

export const Example = () => 
  (
    <div style={{ background: '#ddd', width: '600px', padding: 40 }}>
      <ConditionPlate isCurrent title="Test condition" date="12/12/12" />
    </div>
  );
