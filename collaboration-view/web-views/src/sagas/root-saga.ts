import { all, fork } from 'redux-saga/effects';

import { activeUserSaga } from './active-user-saga';
import { careTeamMemberstSaga } from './care-team-members-saga';
import { currentAppointmentSaga } from './current-appointment-saga';
import { currentAppointmentWidgetsSaga } from './current-appointment-widgets-saga';
import { currentSiteSaga } from './current-site-saga';
import { 
  checkoutFollowUpSaga, 
  checkoutLabTestSaga, 
  checkoutLocationsSaga, 
  checkoutPrescriptionSaga, 
  checkoutReferralSaga, 
  getReferralProviderDetails, 
  getScheduledAppointmentsSaga, 
  saveFollowUpAppointmentSaga, 
  saveLabTestSaga, 
  saveReferralAppointmentSaga,
  saveImagingAppointmentSaga,
  checkoutTimeSlotSaga,
  getLabsLocationSaga,
  checkoutPharmacyDetailsSaga
} from './cv-checkout-sagas';
import { checkoutImagingSaga } from './cv-checkout-sagas/imaging-saga';
import { patientDataSaga } from './patient-data-saga';
import { sitesSaga } from './sites-saga';
import { testReportsResultsSaga } from './test-reports-results-saga';
import { testReportsSaga } from './test-reports-saga';
import { todaysAppointmentsSaga } from './todays-appointments-saga';
import {
  medicationsWidgetSaga,
  allergiesWidgetSaga,
  todaysVitalsWidgetSaga,
  vitalChartSaga,
  conditionsWidgetSaga,
  immunizationsWidgetSaga,
  patientAppointmentsWidgetSaga
} from './widgets-sagas';

// TODO: refactor sagas and workers within DPGECLOF-1216
export function* rootSaga() {
  yield all([
    fork(todaysAppointmentsSaga),
    fork(currentAppointmentSaga),
    fork(medicationsWidgetSaga),
    fork(allergiesWidgetSaga),
    fork(conditionsWidgetSaga),
    fork(todaysVitalsWidgetSaga),
    fork(vitalChartSaga),
    fork(patientAppointmentsWidgetSaga),
    fork(immunizationsWidgetSaga),
    fork(currentAppointmentWidgetsSaga),
    fork(testReportsSaga),
    fork(testReportsResultsSaga),
    fork(sitesSaga),
    fork(currentSiteSaga),
    fork(careTeamMemberstSaga),
    fork(patientDataSaga),
    fork(activeUserSaga),
    fork(checkoutFollowUpSaga),
    fork(saveFollowUpAppointmentSaga),
    fork(checkoutReferralSaga),
    fork(saveReferralAppointmentSaga),
    fork(getReferralProviderDetails),
    fork(checkoutLabTestSaga),
    fork(saveLabTestSaga),
    fork(checkoutPrescriptionSaga),
    fork(getScheduledAppointmentsSaga),
    fork(checkoutImagingSaga),
    fork(checkoutLocationsSaga),
    fork(saveImagingAppointmentSaga),
    fork(checkoutTimeSlotSaga),
    fork(getLabsLocationSaga),
    fork(checkoutPharmacyDetailsSaga),
  ]);
}
