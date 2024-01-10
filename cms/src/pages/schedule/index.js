import "./styles.scss";
import { AppointmentRequestForm } from "../../js/appointment-request-form";
import { AppointmentsInsurance } from "../../js/appointments/insurance";
import { ExitAlert } from "../../js/appointments/exit-alert";
import { LandingPage } from "../../js/appointments/landing-page";
import { LandingPageSearch } from "../../js/appointments/landing-page-search";
import { PatientInfo } from "../../js/appointments/patient-info";
import { PhysicianAppointment } from "../../js/appointments/physician-appointment";
import { ReasonsForVisit } from "../../js/appointments/reasons-for-visit";
import { RuleOutQuestions } from "../../js/appointments/rule-out-questions";
import { ServiceSearch } from "../../js/appointments/service-search";
import { ShareAppointment } from "../../js/appointments/share-appointment";
import { VideoVisit } from "../../js/appointments/video-visit";
import { SelectAppointment } from "../../js/appointments/_select-appointment/select-appointment";
import { LocationsList } from "../../js/locations-list";

new AppointmentRequestForm();
new AppointmentsInsurance();
new ExitAlert();
new LandingPage();
new LandingPageSearch();
new PatientInfo();
new PhysicianAppointment();
new ReasonsForVisit();
new RuleOutQuestions();
new ServiceSearch();
new ShareAppointment();
new VideoVisit();
new LocationsList();

const chosenPhysician = document.querySelector(".physician-info-container");
if (chosenPhysician) {
	const selectAppointment = new SelectAppointment(
		chosenPhysician.dataset.physicianId
	);
}
