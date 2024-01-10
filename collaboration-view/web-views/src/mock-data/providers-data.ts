import { CareAlliesPD } from '@interfaces';
import { Provider } from '@types';

import { careAlliesMockData } from './personal-data';

export const providersData: Provider[] = careAlliesMockData
  .slice()
  .reverse()
  .map(({
    id, firstName, lastName, avatarSmall = '', role,
  }: CareAlliesPD) => {
    const degree = role === 'Internal medicine' ? 'Dr. ' : '';
    const title = `${degree}${firstName} ${lastName}`;
    const altText = `${firstName} ${lastName}`;

    return ({
      id,
      title,
      content: role,
      avatar: avatarSmall,
      altText,
    });
  });
