import { HttpMethod } from '@enums';
import { httpRequest } from '@utils';

export const invalidateToken = (): Promise<void> =>
  httpRequest(
    {
      url: window.env.INVALIDATE_URL,
      method: HttpMethod.POST,
    },
    false
  );
