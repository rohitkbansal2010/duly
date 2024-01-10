import { HttpMethod } from '@enums';
import { EncouterServices } from '@http/api';
import { CareTeamMember } from '@types';
import { httpRequest } from '@utils';

export const getCareTeam = (appointmentId: string, patientId: string):
  Promise<CareTeamMember[] | undefined> =>
  httpRequest({
    url: `${EncouterServices.PATIENTS}/${patientId}/appointments/${appointmentId}/Parties`,
    method: HttpMethod.GET,
  });
