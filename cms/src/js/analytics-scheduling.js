'use strict';

export class SchedulingAnalytics {
    constructor(baseAnalytics) {
        this.baseAnalytics = baseAnalytics;
        this.bootstrap();
    }

    bootstrap() {
        this.scheduleAppointmentStart();
        this.scheduleAppointmentComplete();
        this.scheduleAppointmentError();
        this.loginComplete();
        this.loginError();
        this.continueAsGuest();
        this.editAppointmentStart();
        this.editAppointmentComplete();
        this.editAppointmentError();
        this.shareAppointmentStart();
        this.shareAppointmentComplete();
        this.addToCalendarStart();
        this.addToCalendarComplete();
        this.createPatientPortalAccountClick();
        this.scheduleAnotherAppointmentClick();
        this.checkYourSymptomsClick();
        this.videoVisitError();
    }

    scheduleAppointmentStart() {
        const mainLandingPageFeaturedServiceButton = '.schedule-an-appointment-page form.service button';
        const mainLandingPageOtherServiceButton = '.schedule-an-appointment-page form.tile-link button';
        const allServicesPageDuringSchedulingButton = '.schedule-services-page form.service button';
        const physicianDetailsPageScheduleButton = '.physician-page button[name="continue"]';
        const mainLandingPageLoggedInUserScheduleButton = '.previously-seen-physicians-container .tile';
        const locationDetailsPageScheduleButton = '.location-details-container .servicesForm .cta.schedule';
        const serviceDetailsPageScheduleButton = '.services-detail-page form button[type="submit"]';

        document.querySelectorAll(`${mainLandingPageFeaturedServiceButton}, ${mainLandingPageOtherServiceButton}, ${allServicesPageDuringSchedulingButton}, ${physicianDetailsPageScheduleButton}, ${mainLandingPageLoggedInUserScheduleButton}, ${locationDetailsPageScheduleButton}, ${serviceDetailsPageScheduleButton}`).forEach(button => {
            button.addEventListener('click', e => {
                if (this.baseAnalytics.keydownOrClick(e.type, e)) {
                    this.baseAnalytics._sendEvent('Schedule Appointment', 'Schedule Appointment Start');
                }
            });
        });
    }

    scheduleAppointmentComplete() {
        // flash message, appears only once after a successful booking
        if (document.querySelector('[name="make-appointment-success"]')) {
            this.baseAnalytics._sendEvent('Schedule Appointment', 'Schedule Appointment Complete');
        }

        // if a clearstep flow, trigger another vent
        if (document.querySelector('[name="make-appointment-success-clearstep"]')) {
            this.baseAnalytics._sendEvent('Clearstep', 'Clearstep Appointment Complete');
        }

        // if a Duly Now video visit flow, trigger another vent
        if (document.querySelector('[name="make-appointment-success-duly-now-video-visit"]')) {
            this.baseAnalytics._sendEvent('Schedule Appointment', 'Duly Now Video Visit Complete');
        }
    }

    videoVisitError() {
        if (document.querySelector('.videoVisitContainer #patient-information-booking-form .error')) {
            const errorText = document.querySelector('.videoVisitContainer #patient-information-booking-form .error').innerHTML;

            if (errorText) {
                this.baseAnalytics._sendEvent('Schedule Appointment', 'Duly Now Video Visit Error', errorText);
            }
        }
    }

    scheduleAppointmentError() {
        // error message comes from backend
        // element only has innerText if error is present
        if (document.querySelector('.main-scheduling-flow #patient-information-booking-form .error')) {
            const errorText = document.querySelector('.main-scheduling-flow #patient-information-booking-form .error').innerHTML;

            if (errorText) {
                this.baseAnalytics._sendEvent('Schedule Appointment', 'Schedule Appointment Error', errorText);
            }
        }
    }

    loginComplete() {
        // flash message, appears only once after a successful login
        if (document.querySelector('[name="login-success"]')) {
            this.baseAnalytics._sendEvent('Login', 'Login Complete');
        }
    }

    loginError() {
        // flash message, appears only once after a failed login
        if (document.querySelector('[name="login-failure"]')) {
            this.baseAnalytics._sendEvent('Login', 'Login Error', 'We were unable to log you in using the supplied credentials.');
        }
    }

    continueAsGuest() {
        const continueAsGuest = document.querySelector('#continue-as-guest');
        if (continueAsGuest) {
            continueAsGuest.addEventListener('click', e => {
                this.baseAnalytics._sendEvent('Schedule Appointment', 'Continue as Guest');
            });
        }
    }

    editAppointmentStart() {
        const editButton = document.querySelector('.patient-appointment-confirmation-page .detail-content .edit');
        if (editButton) {
            editButton.addEventListener('click', e => {
                this.baseAnalytics._sendEvent('Schedule Appointment', 'Edit Appointment Start');
            });
        }
    }

    editAppointmentComplete() {
        if (document.querySelector('[name="edit-appointment-success"]')) {
            this.baseAnalytics._sendEvent('Schedule Appointment', 'Edit Appointment Complete');
        }
    }

    editAppointmentError() {
        if (document.querySelector('[name="cancel-appointment-failed"]')) {
            this.baseAnalytics._sendEvent('Schedule Appointment', 'Edit Appointment Error');
        }
    }

    shareAppointmentStart() {
        window.addEventListener('shareAppointmentStart', _ => {
            this.baseAnalytics._sendEvent('Schedule Appointment', 'Share Appointment Start');
        });
    }

    shareAppointmentComplete() {
        window.addEventListener('shareAppointmentComplete', e => {
            this.baseAnalytics._sendEvent('Schedule Appointment', 'Share Appointment Complete');
        });
    }

    addToCalendarStart() {
        window.addEventListener('addToCalendarStart', _ => {
            this.baseAnalytics._sendEvent('Schedule Appointment', 'Add to Calendar Start');
        });
    }

    addToCalendarComplete() {
        window.addEventListener('addToCalendarComplete', e => {
            this.baseAnalytics._sendEvent('Schedule Appointment', 'Add to Calendar Complete');
        });
    }

    createPatientPortalAccountClick() {
        const createPatientPortalAccount = document.querySelector('#create-patient-portal-btn');
        if (createPatientPortalAccount) {
            createPatientPortalAccount.addEventListener('click', e => {
                this.baseAnalytics._sendEvent('Schedule Appointment', 'MyChart Click');
            });
        }
    }

    scheduleAnotherAppointmentClick() {
        const scheduleAnotherAppointment = document.querySelector('#schedule-another-appointment-btn');
        if (scheduleAnotherAppointment) {
            scheduleAnotherAppointment.addEventListener('click', e => {
                this.baseAnalytics._sendEvent('Schedule Appointment', 'Schedule Another Appointment Click');
            });
        }
    }

    checkYourSymptomsClick() {
        const tooltipCheckYourSymptomsLinks = document.querySelectorAll("#tip-text a, #tip-text-mobile a");
        tooltipCheckYourSymptomsLinks.forEach(link => {
            link.addEventListener('click', _ => {
                this.baseAnalytics._sendEvent('Clearstep', 'Check Symptoms Tooltip Click');
            });
        });

        const schedulingPageCheckYourSymptomsLink = document.querySelector("#schedule-page-check-symptoms-cta");
        if (schedulingPageCheckYourSymptomsLink) {
            schedulingPageCheckYourSymptomsLink.addEventListener('click', _ => {
                this.baseAnalytics._sendEvent('Clearstep', 'Check Symptoms Schedule Page Click');
            });
        };
    }
}