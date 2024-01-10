import { HttpMethod } from '@enums';
import { ProviderService } from '@http/api';
import { httpRequest } from '@utils';


export const getProviderByLatLng = 
  (lat: number | string, lng: number | string, type: number | string) =>
    httpRequest({
      url: `${ProviderService.PROVIDER_BY_LAT_LNG}/${lat}/${lng}/${type}`,
      method: HttpMethod.GET,
    });
