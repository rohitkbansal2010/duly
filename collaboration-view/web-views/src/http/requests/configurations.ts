import { AppPart, HttpMethod } from '@enums';
import { ResourceServices } from '@http/api';
import { httpRequest } from '@utils';

type ParamsType = {
  appPart: AppPart;
  siteId?: string;
  patientId: string;
};

export const getConfigurations = (params: ParamsType) =>
  httpRequest({
    url: ResourceServices.CONFIGURATIONS,
    method: HttpMethod.GET,
    params,
  });
