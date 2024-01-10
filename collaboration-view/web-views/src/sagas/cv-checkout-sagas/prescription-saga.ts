import { call, put, takeLatest } from 'redux-saga/effects';
  
import { ErrorData } from '@constants';
import { StepperTitles } from '@enums';
import { getPreferredPharmacy, getPrescribedMedicineDetails } from '@http/requests';
import { 
  hideSpinner, 
  setMedicationsWidgetData, 
  showSpinner
} from '@redux/actions';
import { 
  CVCheckoutAppointmentsType, 
  GetPreferredPharmacyDetails, 
  GetPrescriptionDetails,
  setPreferredPharmacyDetails,
  setStepsList
} from '@redux/actions/cv-checkout-appointments';
import { MedicationsWidgetDataType, PreferredPharmacyDetailsType } from '@types';
import { ApiExceptionParamType, ApiExceptions } from '@utils';

function* workerGetPrescriptionDetails({ payload }: GetPrescriptionDetails) {
  try {
    yield put(showSpinner());
    const medicationData: MedicationsWidgetDataType = 
      yield call(getPrescribedMedicineDetails, payload.patientId, payload.appointmentId);
    yield put(setMedicationsWidgetData(medicationData));
    const preferredPharmacyData: PreferredPharmacyDetailsType = 
      yield call(getPreferredPharmacy, payload.patientId);
    yield put(setPreferredPharmacyDetails(preferredPharmacyData));
    yield put(setStepsList([ { alias: StepperTitles.PRESCRIPTION, isSkeletonShown: false } ]));
  } catch (err) {
    if(!payload.notInitialLoad) {
      yield ApiExceptions(
        { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
      );
    }
    yield put(setStepsList([ { alias: StepperTitles.PRESCRIPTION, isSkeletonShown: false } ]));
  } finally {
    yield put(hideSpinner());
  }
}

function* workerGetPharmacyDetails({ payload }: GetPreferredPharmacyDetails) {
  try {
    yield put(setStepsList([ { alias: StepperTitles.PREFERRED_PHARMACY, isSkeletonShown: true } ]));
    const preferredPharmacyData: PreferredPharmacyDetailsType = 
      yield call(getPreferredPharmacy, payload.patientId);
    yield put(setPreferredPharmacyDetails(preferredPharmacyData));
    yield put(setStepsList(
      [ { alias: StepperTitles.PREFERRED_PHARMACY, isSkeletonShown: false } ]
    ));
  } catch (err) {
    console.log(err);
    if(!payload.notInitialLoad) {
      yield ApiExceptions(
        { error:err, errorData:ErrorData.UnableToFetchData } as ApiExceptionParamType
      );
    }
    yield put(setStepsList(
      [ { alias: StepperTitles.PREFERRED_PHARMACY, isSkeletonShown: false } ]
    ));
  }
}

export function* checkoutPrescriptionSaga () {
  yield takeLatest(
    CVCheckoutAppointmentsType.GET_PRESCRIPTION_DETAILS,
    workerGetPrescriptionDetails
  );   
}

export function* checkoutPharmacyDetailsSaga () {
  yield takeLatest(
    CVCheckoutAppointmentsType.GET_PREFERRED_PHARMACY_DETAILS,
    workerGetPharmacyDetails
  );   
}
