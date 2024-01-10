import { HttpMethod, CardType } from '@enums';
import { EncouterServices, PatientsService } from '@http/api';
import { TestReportRequest } from '@types';
import { httpRequest } from '@utils';

export const getAllergies = (patientId: number | string) =>
  httpRequest({
    url: `${EncouterServices.PATIENTS}/${patientId}${PatientsService.ALERGIES}`,
    method: HttpMethod.GET,
  });

export const getHealthConditions = (patientId: number | string) =>
  httpRequest({
    url: `${EncouterServices.PATIENTS}/${patientId}${PatientsService.CONDITIONS}`,
    method: HttpMethod.GET,
  });

export const getPatientData = (patientId: string) =>
  httpRequest({
    url: `${EncouterServices.PATIENTS}/${patientId}`,
    method: HttpMethod.GET,
  });

export const getMedications = (patientId: string) =>
  httpRequest({
    url: `${EncouterServices.PATIENTS}/${patientId}${PatientsService.MEDICATIONS}`,
    method: HttpMethod.GET,
  });

export const getTodaysVitals = (patientId: string) =>
  httpRequest({
    url: `${EncouterServices.PATIENTS}/${patientId}${PatientsService.VITALS}${PatientsService.CARDS}`,
    method: HttpMethod.GET,
  });

export const getChartByVitalsCardType = (patientId: string, cardType: CardType) =>
  httpRequest({
    url: `${EncouterServices.PATIENTS}/${patientId}${PatientsService.VITALS}${PatientsService.CHART}?vitalsCardType=${cardType}`,
    method: HttpMethod.GET,
  });

export const getPatientAppointments = (appointmentId: string) =>
  httpRequest({
    url: `${EncouterServices.APPOINTMENTS_FOR_SAME_PATIENT}/${appointmentId}/forSamePatient`,
    method: HttpMethod.GET,
  });

export const getImmunizations = (patientId: string) =>
  httpRequest({
    url: `${EncouterServices.PATIENTS}/${patientId}${PatientsService.IMMUNIZATIONS}`,
    method: HttpMethod.GET,
  });

export const getTestReportsData = ({
  patientId, startDate, endDate, amount,
}: TestReportRequest) =>
  httpRequest({
    url: `${EncouterServices.PATIENTS}/${patientId}${PatientsService.TEST_REPORTS}`,
    params: { startDate, endDate, amount },
    method: HttpMethod.GET,
  });

export const getTestReportsResultsData = (testReportId: string) =>
  httpRequest({
    url: `${EncouterServices.TEST_REPORTS}/${testReportId}`,
    method: HttpMethod.GET,
  });
