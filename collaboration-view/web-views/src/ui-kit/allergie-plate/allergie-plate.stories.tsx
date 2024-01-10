import { AllergiePlate } from './allergie-plate';

export default {
  title: 'UI-KIT / AllergiePlate',
  component: AllergiePlate,
};

export const Example = () => 
  (
    <div style={{ background: '#ddd', width: '600px', padding: 40 }}>
      <AllergiePlate id="1" title="Peanuts" recorded="11/2/20" categories={[ 'food' ]} reactions={[ { title: 'anaphylaxis', severity: 'severe' } ]} />
    </div>
  );
