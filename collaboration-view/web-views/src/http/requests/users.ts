import { HttpMethod } from '@enums';
import { EncouterServices } from '@http/api';
import { ActiveUser } from '@types';
import { httpRequest } from '@utils';

export const getActiveUser = (): Promise<ActiveUser | undefined> =>
  httpRequest({
    url: `${EncouterServices.USERS}/active`,
    method: HttpMethod.GET,
  });
