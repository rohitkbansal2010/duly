import { useState } from 'react';

import { NavSecondary } from './nav-secondary';

export default {
  title: 'UI-KIT / NavSecondary',
  component: NavSecondary,
};

export const Example = () => {
  const [ item, setItem ] = useState('vitals');

  const handleItemSelected = (item: string) => 
    setItem(item);

  return (
    <NavSecondary
      items={[
        'questions',
        'vitals',
        'goals',
        'conditions',
        'appointments',
        'medications',
        'allergies',
        'immunizations',
      ]}
      activeItem={item}
      onItemSelected={handleItemSelected}
      fade={false}
    />
  );
};
