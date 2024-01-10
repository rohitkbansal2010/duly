import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { v4 } from 'uuid';

import { ACCESS_TOKEN } from '@constants';
import { HttpMethod } from '@enums';

import { getLocalStorageItem } from './local-storage';

export const httpRequest = async <T>(
  options: AxiosRequestConfig,
  isSubscriptionKey = true
): Promise<T | undefined> => {
  try {
    const isMethodGet = options.method === HttpMethod.GET;

    const accessToken = getLocalStorageItem(ACCESS_TOKEN);
    const authorization = accessToken ? `Bearer ${accessToken}` : '';
    const isOcpApimTrace = (window?.env?.OCP_APIM_TRACE || '').toLowerCase() === 'true';
    const headers = {
      'x-correlation-id': v4(),
      ...(isOcpApimTrace && { 'Ocp-Apim-Trace': 'True' }),
      Authorization: authorization,
    };

    const response: AxiosResponse<T> = await axios({
      ...options,
      params: isMethodGet
        ? { ...options.params, timestamp: Date.now() }
        : options.params,
      headers: isSubscriptionKey
        ? {
          ...headers,
          'subscription-key': window?.env?.SUBSCRIPTION_KEY,
        }
        : headers,
    });

    return response.data;
  } catch (error) {
    const { response } = error as AxiosError<T>;

    return Promise.reject(response);
  }
};
