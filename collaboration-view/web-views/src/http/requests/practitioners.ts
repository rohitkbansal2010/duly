import { HttpMethod } from '@enums';
import { EncouterServices, PatientsService } from '@http/api';
import { Practitioner } from '@types';
import { httpRequest } from '@utils';

export const getBrief = (patientId: number): Promise<Practitioner | undefined> =>
  httpRequest({
    url: `${EncouterServices.PRACTITIONERS}/${patientId}${PatientsService.BRIEF}`,
    method: HttpMethod.GET,
  });
