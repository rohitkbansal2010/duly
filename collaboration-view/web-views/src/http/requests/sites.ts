import { HttpMethod } from '@enums';
import { EncouterServices, SitesService } from '@http/api';
import { GetAppointments } from '@types';
import { httpRequest } from '@utils';

// TODO: generate x-correlation-id

export const getSites = () => 
  httpRequest({
    url: `${EncouterServices.SITES}`,
    method: HttpMethod.GET,
  });

export const getSiteById = ({ siteId }: { siteId: string }) =>
  httpRequest({
    url: `${EncouterServices.SITES}/${siteId}`,
    method: HttpMethod.GET,
  });


export const getPractitioners = (sitesId: string) =>
  httpRequest({
    url: `${EncouterServices.SITES}/${sitesId}${SitesService.PRACTITIONERS}`,
    method: HttpMethod.GET,
  });

export const getAppointments = ({ siteId, startDate, endDate }: GetAppointments) =>
  httpRequest({
    url: `${EncouterServices.SITES}/${siteId}${SitesService.APPOINTMENTS}`,
    params: { startDate, endDate },
    method: HttpMethod.GET,
  });
