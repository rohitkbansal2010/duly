'use strict';
import { YouTubeApi } from './youtube-api';
import { SelectAppointment } from './appointments/_select-appointment/select-appointment';
import { MenstrualCycleForm } from './appointments/_forms/menstrual-cycle-form';
import moment from 'moment';

export class PhysiciansDetails {
    constructor() {
        this.container = document.querySelector('main.physician-page');
        if (!this.container) {
            return;
        }

        this.physicianId = this.container.querySelector('section.physician-single').dataset.physicianId;

        this.queryElements();

        this.setupDetailsPartsANavigation();
        this.setupDetailsPartsBNavigation();

        this.setupEditorialShowLessShowMore();
        this.setupCommentsShowLessShowMore();

        this.mentrualCycleForm = new MenstrualCycleForm(_ => { });

        if (document.querySelector('.service-videos-section')) {
            this.player = new YouTubeApi();
        }

        if (document.querySelector('.ratings-container')) {
            this.jumpToRatings();
        }

        this.setupOnlineSchedulingModule();
    }

    queryElements() {
        this.navItemGeneralInfo = this.container.querySelector('.details-navigation .nav-item.general-info');
        this.navItemVideo = this.container.querySelector('.details-navigation .nav-item.video');
        this.navItemHospitalAffiliations = this.container.querySelector('.details-navigation .nav-item.hospital-affiliations');

        this.detailsGeneralInfo = this.container.querySelector('.details.general-info');
        this.detailsVideo = this.container.querySelector('.details.video');
        this.detailsHospitalAffiliations = this.container.querySelector('.details.hospital-affiliations');

        this.navItemEducation = this.container.querySelector('.details-navigation .nav-item.education');
        this.navItemEditorial = this.container.querySelector('.details-navigation .nav-item.editorial');
        this.navItemPublications = this.container.querySelector('.details-navigation .nav-item.publications');

        this.detailsEducation = this.container.querySelector('.details.education');
        this.detailsEditorial = this.container.querySelector('.details.editorial');
        this.detailsPublications = this.container.querySelector('.details.publications');
    }

    setupLastMenstrualCycleFieldListeners() {
        let container = document.querySelector('.last-menstrual-cycle-prompt');
        this.monthField = container.querySelector(".mdc-text-field__input[name='last_menstrual_cycle_month']");
        this.dayField = container.querySelector(".mdc-text-field__input[name='last_menstrual_cycle_day']");
        this.yearField = container.querySelector(".mdc-text-field__input[name='last_menstrual_cycle_year']");
        // toISOString is needed to prevent future deprecation issues
        let todayDate = moment(new Date().toISOString());

        this.monthField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateCycleForm(this.monthField, this.dayField, this.yearField, todayDate);

            // skip to the next field
            if (this.monthField.value.length == 2) {
                this.dayField.select();
            }
        });

        this.dayField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateCycleForm(this.monthField, this.dayField, this.yearField, todayDate);

            // skip to the next field
            if (this.dayField.value.length == 2) {
                this.yearField.select();
            }
        });

        this.yearField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateCycleForm(this.monthField, this.dayField, this.yearField, todayDate);
        });
    }

    validateCycleForm(monthField, dayField, yearField, todayDate) {
        this.submittedDate = new Date().getFullYear() - 200 && moment(`${monthField.value.padStart(2, '0')}/${dayField.value.padStart(2, '0')}/${yearField.value.padStart(4, '0')}`).toISOString();
        this.timePassed = todayDate.diff(this.submittedDate, 'days');
        this.mentrualCycleForm.validate(this.submittedDate, todayDate, yearField, '.last-menstrual-cycle-prompt');
    }

    setupOnlineSchedulingModule() {
        this.newPatient = null;
        this.establishedPatient = null;
        this.followUpVisit = null;
        this.hospitalFollowUpVisit = null;
        this.isPregnant = null;

        this.setupSelectServicePrompt();
        this.setupNewPatientPrompt();
        this.setupFollowUpPrompt();
        this.setupVeinClinicPrompt();
        this.setupFullBodySkinExamPrompt();
        this.setupRecommendedPhysicians();

        if (this.container.querySelector('.select-appointment [name="continue"')) {
            // submit selected date and time
            this.container.querySelector('.select-appointment [name="continue"]').addEventListener('click', e => {
                e.preventDefault();

                this.submitOnlineSchedulingPrep(
                    this.container.querySelector('[name="appointment_department_id"]').value,
                    this.container.querySelector('[name="appointment_time"]').value,
                    this.container.querySelector('[name="appointment_visit_type_id"]').value
                );
            });
        }

        // on desktop, the scheduling right rail is set an position: absolute
        // this can cause a UI issue if the rain height is > than the rest of the physician page and overlap the footer
        // we want to dynamically increase the height of the page so that the right rail never overlaps the footer
        this.appointmentsRightRailElement = document.querySelector(".next-available-appointments-container");
        this.mainContainerElement = document.querySelector(".physician-single");
        let lastMargin = 0;
        if (this.appointmentsRightRailElement !== null) {
            // as the users interact with the right rail, it will change its height asynchronously (e.g. when loading data)
            // periodically resize the page if needed
            setInterval(_ => {
                const diff = this.mainContainerElement.offsetHeight - this.appointmentsRightRailElement.offsetHeight;

                if (diff < 0) {
                    // skip re-paint if no changes are detected
                    if (Math.abs(diff) !== lastMargin) {
                        lastMargin = Math.abs(diff);
                        this.mainContainerElement.style.marginBottom = `${lastMargin}px`;
                    }
                }
            }, 50);
        }
    }

    setupRecommendedPhysicians() {
        const selectAppointmentContent = document.querySelector('section.select-appointment-content');
        const showRecommendedCta = document.getElementById('showRecommended');
        const dateAndTimePickerContainer = document.getElementById('dateAndTimePickerContainer');
        const recommendedProvidersContainer = document.getElementById('recommendedProvidersContainer');
        showRecommendedCta.addEventListener('click', () => {
            dateAndTimePickerContainer.style.position = 'absolute';
            recommendedProvidersContainer.style.position = 'relative';
            showRecommendedCta.classList.add('hidden');
            const submitButton = document.querySelector('#recommendedScheduleCta #continue');
            submitButton.addEventListener('click', e => {
                e.preventDefault();
                this.physicianId = document.querySelector('[name="appointment_selected_recommended_physician_id"]').value;
                this.appointmentSelectedRecommendedPhysicianId = document.querySelector('[name="appointment_selected_recommended_physician_id"]').value;
                this.submitOnlineSchedulingPrep(
                    this.container.querySelector('[name="appointment_department_id"]').value,
                    this.container.querySelector('[name="appointment_time"]').value,
                    this.container.querySelector('[name="appointment_visit_type_id"]').value
                );
            });
            this.selectAppointment.cta = submitButton
            selectAppointmentContent.classList.add('showRecommended');
        })
        const recommendedBackArrow = document.getElementById('recommendedBackArrow');
        recommendedBackArrow.addEventListener('click', () => {
            dateAndTimePickerContainer.style.position = 'relative';
            recommendedProvidersContainer.style.position = 'absolute';
            showRecommendedCta.classList.remove('hidden');
            this.selectAppointment.cta = document.querySelector('#select-appointment-form #continue');
            this.physicianId = this.container.querySelector('section.physician-single').dataset.physicianId;
            this.appointmentSelectedRecommendedPhysicianId = null;
            selectAppointmentContent.classList.remove('showRecommended');  
        })
    }

    setupSelectServicePrompt() {
        const serviceButtons = this.container.querySelectorAll('.select-service-prompt button');

        serviceButtons.forEach(button => {
            button.addEventListener('click', ({ target }) => {
                this.selectedServiceId = target.dataset.serviceId;

                this.container.querySelector('.select-service-prompt').classList.add('hidden');
                this.container.querySelector('.new-patient-prompt').classList.remove('hidden');
            })
        });

        // if only one service is schedulable, click it for the user to reduce the number of steps they need to take
        if (serviceButtons.length == 1) {
            serviceButtons[0].click();
        }
    }

    setupPregnancyPrompt() {
        const isPregnantBtn = this.container.querySelector("#patient-is-pregnant");
        const isNotPregnantBtn = this.container.querySelector("#patient-is-not-pregnant");
        if (isPregnantBtn != undefined && isNotPregnantBtn != undefined) {
            [isPregnantBtn, isNotPregnantBtn].forEach(button => button.addEventListener('click', ({ target }) => {
                if (target === isPregnantBtn) {
                    this.isPregnant = true;
                    this.showMenstralCyclePrompt();
                } else if (target === isNotPregnantBtn) {
                    this.isPregnant = false;
                    this.showSchedulingPrompt();
                }

                this.container.querySelector('.ask-if-pregnant-prompt').classList.add('hidden');
            }));
        }
    }

    showMenstralCyclePrompt() {
        if (this.container.querySelector('.last-menstrual-cycle-prompt')) {
            this.setupLastMenstrualCycleFieldListeners();
            const form = this.container.querySelector('.last-menstrual-cycle-prompt');
            const btn = this.container.querySelector('#submit-last-menstrual-date');
            form.classList.remove('hidden');

            btn.addEventListener('click', (e) => {
                this.determineIfValidDate(form);
            })
        }
    }

    determineIfValidDate(form) {
        if (this.timePassed && this.submittedDate) {
            form.classList.add('hidden');
            // 224 days == 32 weeks
            if (this.timePassed > 224) {
                this.showCongratsMessage();
            } else if (this.timePassed < 56) {
                // 56 days == 8 weeks
                let startAvailableTimesOnDate = moment(this.submittedDate).add(56, 'days');
                document.querySelector(".select-appointment-content").classList.remove('hidden');
                new SelectAppointment(this.physicianId, 5, this.selectedServiceId, this.newPatient, this.establishedPatient, this.followUpVisit, startAvailableTimesOnDate);
            } else {
                this.showSchedulingPrompt();
            }
        }
    }

    showCongratsMessage() {
        if (document.querySelector('.pregnancy-congrats-message')) {
            document.querySelector('.last-menstrual-cycle-prompt').classList.add('hidden');
            document.querySelector('.pregnancy-congrats-message').classList.remove('hidden');
        }
    }

    setupPrimaryCarePrompt() {
        const isHospitalFollowUp = this.container.querySelector("#hospital-follow-up-visit");
        const notHospitalFollowUp = this.container.querySelector("#not-hospital-follow-up-visit");

        if (isHospitalFollowUp != undefined && notHospitalFollowUp != undefined) {
            [isHospitalFollowUp, notHospitalFollowUp].forEach(button => button.addEventListener('click', ({ target }) => {
                if (target === isHospitalFollowUp) {
                    this.hospitalFollowUpVisit = true;
                    this.showFollowUpDisclaimer();
                } else if (target === notHospitalFollowUp) {
                    this.hospitalFollowUpVisit = false;
                    this.showSchedulingPrompt();
                }

                this.container.querySelector('.hospital-followup-prompt').classList.add('hidden');
            }));
        }
    }

    showFollowUpDisclaimer() {
        if (this.container.querySelector('.hospital-followup-disclaimer')) {
            const message = this.container.querySelector('.hospital-followup-disclaimer');
            const btn = this.container.querySelector('#confirm-disclaimer');
            message.classList.remove('hidden');

            btn.addEventListener('click', (e) => {
                message.classList.add('hidden');
                this.showSchedulingPrompt();
            })
        }
    }

    setupNewPatientPrompt() {
        const newPatientButton = this.container.querySelector("#new-patient-appointment");
        const establishedPatientButton = this.container.querySelector("#established-patient-appointment");

        if (newPatientButton != undefined && establishedPatientButton != undefined) {
            [newPatientButton, establishedPatientButton].forEach(button => button.addEventListener('click', ({ target }) => {
                if (target === newPatientButton) {
                    this.newPatient = true;
                    this.establishedPatient = false;
                } else if (target === establishedPatientButton) {
                    this.newPatient = false;
                    this.establishedPatient = true;
                }

                this.container.querySelector('.new-patient-prompt').classList.add('hidden');

                this.checkOtherPromptTypes();
            }));
        }
    }

    checkOtherPromptTypes() {
        if (this.veinClinicVisit !== undefined && this.veinClinicVisit === null) {
            // show the vein clinic prompt
            this.container.querySelector('.vein-clinic-prompt').classList.remove('hidden');
            return;
        }
        if (this.fullBodySkinExamVisit !== undefined && this.fullBodySkinExamVisit === null) {
            // show the vein clinic prompt
            this.container.querySelector('.full-body-skin-exam-prompt').classList.remove('hidden');
            return;
        }

        if (this.veinClinicVisit === true || this.fullBodySkinExamVisit === true) {
            // skip all other prompts
            this.showSchedulingPrompt();
            return;
        } else {
            if (this.container.querySelector(`.select-service-prompt input[data-service-id="${this.selectedServiceId}"]`) && this.followUpVisit === null) {
                this.container.querySelector('.follow-up-visit-prompt').classList.remove('hidden');
            } else if (this.container.querySelector('.hospital-followup-prompt') && this.establishedPatient && !this.followUpVisit) {
                this.container.querySelector('.hospital-followup-prompt').classList.remove('hidden');
                this.setupPrimaryCarePrompt();
            } else if (this.container.querySelector('.ask-if-pregnant-prompt') && (!this.establishedPatient || this.establishedPatient && !this.followUpVisit)) {
                this.container.querySelector('.ask-if-pregnant-prompt').classList.remove('hidden');
                this.setupPregnancyPrompt();
            } else {
                this.showSchedulingPrompt();
            }
        }
    }

    showSchedulingPrompt() {
        this.container.querySelector(".select-appointment-content").classList.remove('hidden');
        this.selectAppointment = new SelectAppointment(this.physicianId, 5, this.selectedServiceId, this.newPatient, this.establishedPatient, this.followUpVisit, null, this.veinClinicVisit, false, this.fullBodySkinExamVisit);
    }

    setupFollowUpPrompt() {
        const yesFollowupVisitButton = this.container.querySelector("#follow-up-visit");
        const notFollowupVisitButton = this.container.querySelector("#not-follow-up-visit");

        if (yesFollowupVisitButton != undefined && notFollowupVisitButton != undefined) {
            [yesFollowupVisitButton, notFollowupVisitButton].forEach(button => button.addEventListener('click', ({ target }) => {
                if (target === yesFollowupVisitButton) {
                    this.followUpVisit = true;
                } else if (target === notFollowupVisitButton) {
                    this.followUpVisit = false;
                }

                this.container.querySelector('.follow-up-visit-prompt').classList.add('hidden');
                this.checkOtherPromptTypes();
            }));
        }
    }

    setupVeinClinicPrompt() {
        const yesVeinClinicVisitButton = this.container.querySelector("#vein-clinic-visit");
        const notVeinClinicVisitButton = this.container.querySelector("#not-vein-clinic-visit");

        if (yesVeinClinicVisitButton != undefined && notVeinClinicVisitButton != undefined) {
            this.veinClinicVisit = null;
            [yesVeinClinicVisitButton, notVeinClinicVisitButton].forEach(button => button.addEventListener('click', ({ target }) => {
                if (target === yesVeinClinicVisitButton) {
                    this.veinClinicVisit = true;
                } else if (target === notVeinClinicVisitButton) {
                    this.veinClinicVisit = false;
                }

                this.container.querySelector('.vein-clinic-prompt').classList.add('hidden');
                this.checkOtherPromptTypes();
            }));
        }
    }

    setupFullBodySkinExamPrompt() {
        const yesFullBodySkinExamVisitButton = this.container.querySelector("#full-body-skin-exam-visit");
        const notFullBodySkinExamVisitButton = this.container.querySelector("#not-full-body-skin-exam-visit");

        if (yesFullBodySkinExamVisitButton != undefined && notFullBodySkinExamVisitButton != undefined) {
            this.fullBodySkinExamVisit = null;
            [yesFullBodySkinExamVisitButton, notFullBodySkinExamVisitButton].forEach(button => button.addEventListener('click', ({ target }) => {
                if (target === yesFullBodySkinExamVisitButton) {
                    this.fullBodySkinExamVisit = true;
                } else if (target === notFullBodySkinExamVisitButton) {
                    this.fullBodySkinExamVisit = false;
                }

                this.container.querySelector('.full-body-skin-exam-prompt').classList.add('hidden');
                this.checkOtherPromptTypes();
            }));
        }
    }

    jumpToRatings() {
        const count = document.querySelector('.stars .count');
        const ratingsContainer = document.querySelector('.ratings-container');

        count.addEventListener('click', (e) => {
            if (window.innerWidth >= 800) {
                ratingsContainer.scrollIntoView({ behavior: 'smooth', block: 'center' });
            } else {
                ratingsContainer.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    }

    submitOnlineSchedulingPrep(appointmentDepartmentId, appointmentTime, appointmentVisitTypeId) {
        const csrfToken = document.querySelector("input#csrf-token");

        fetch(`${window.location.origin}/schedule/scheduling-from-physician-page`, {
            method: 'POST',
            mode: 'same-origin',
            cache: 'no-cache',
            follow: true,
            async: true,
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
                'x-isAjax': true
            },
            body: JSON.stringify({
                [csrfToken.name]: csrfToken.value,
                appointment_department_id: appointmentDepartmentId,
                appointment_time: appointmentTime,
                appointment_visit_type_id: appointmentVisitTypeId,
                appointment_selected_recommended_physician_id: this.appointmentSelectedRecommendedPhysicianId,
                appointment_physician_id: this.physicianId,
                appointment_new_patient_visit: this.newPatient ? '1' : '0',
                appointment_follow_up_visit: this.followUpVisit ? '1' : '0',
                appointment_service_ids: JSON.stringify([this.selectedServiceId])
            })
        }).then(async (response) => {
            const json = await response.json();
            if (response.ok) {
                return json;
            } else {
                return Promise.reject(json);
            }
        }).then(async (data) => {
            if (data.success) {
                if (document.querySelector('.choose-patient-prompt')) {
                    const modal = document.querySelector('.choose-patient-prompt')
                    modal.classList.remove('hidden');
                } else {
                    window.location = `${window.location.origin}/login-portal`;
                }
            }
        }).catch(_ => {
            return false;
        });
    }

    setupDetailsPartsANavigation() {
        [
            this.navItemGeneralInfo,
            this.navItemVideo,
            this.navItemHospitalAffiliations
        ].forEach(navItem => {
            if (!navItem) {
                return;
            }
            navItem.addEventListener("click", this.detailsANavigationHandler.bind(this));
            navItem.addEventListener("keyup", this.detailsANavigationHandler.bind(this));
        });
    }

    setupDetailsPartsBNavigation() {
        [
            this.navItemEducation,
            this.navItemEditorial,
            this.navItemPublications
        ].forEach(navItem => {
            if (!navItem) {
                return;
            }
            navItem.addEventListener("click", this.detailsBNavigationHandler.bind(this));
            navItem.addEventListener("keyup", this.detailsBNavigationHandler.bind(this));

            if (navItem.classList.contains('active')) {
                navItem.click();
            }
        });
    }

    detailsANavigationHandler(e) {
        // skip if a non-enter key was pressed
        if (e.keyCode !== undefined && e.keyCode !== 13) {
            return;
        }

        let target = e.target;

        this.navItemGeneralInfo && this.navItemGeneralInfo.classList.remove('active');
        this.navItemVideo && this.navItemVideo.classList.remove('active');
        this.navItemHospitalAffiliations && this.navItemHospitalAffiliations.classList.remove('active');

        this.detailsGeneralInfo && this.detailsGeneralInfo.classList.remove('active');
        this.detailsVideo && this.detailsVideo.classList.remove('active');
        this.detailsHospitalAffiliations && this.detailsHospitalAffiliations.classList.remove('active');

        if (target.classList.contains("general-info")) {
            this.navItemGeneralInfo && this.navItemGeneralInfo.classList.add('active');
            this.detailsGeneralInfo && this.detailsGeneralInfo.classList.add('active');
        }

        if (target.classList.contains("video")) {
            this.navItemVideo && this.navItemVideo.classList.add('active');
            this.detailsVideo && this.detailsVideo.classList.add('active');

        } else {
            if (this.player) {
                this.player.pauseVideo();
            }
        }

        if (target.classList.contains("hospital-affiliations")) {
            this.detailsHospitalAffiliations && this.detailsHospitalAffiliations.classList.add('active');
            this.navItemHospitalAffiliations && this.navItemHospitalAffiliations.classList.add('active');
        }
    }

    detailsBNavigationHandler(e) {
        // skip if a non-enter key was pressed
        if (e.keyCode !== undefined && e.keyCode !== 13) {
            return;
        }

        let target = e.target;

        this.navItemEducation && this.navItemEducation.classList.remove('active');
        this.navItemEditorial && this.navItemEditorial.classList.remove('active');
        this.navItemPublications && this.navItemPublications.classList.remove('active');

        this.detailsEducation && this.detailsEducation.classList.remove('active');
        this.detailsEditorial && this.detailsEditorial.classList.remove('active');
        this.detailsPublications && this.detailsPublications.classList.remove('active');

        if (target.classList.contains("education")) {
            this.navItemEducation && this.navItemEducation.classList.add('active');
            this.detailsEducation && this.detailsEducation.classList.add('active');
        }
        if (target.classList.contains("editorial")) {
            this.navItemEditorial && this.navItemEditorial.classList.add('active');
            this.detailsEditorial && this.detailsEditorial.classList.add('active');
        }
        if (target.classList.contains("publications")) {
            this.detailsPublications && this.detailsPublications.classList.add('active');
            this.navItemPublications && this.navItemPublications.classList.add('active');
        }
    }

    setupEditorialShowLessShowMore() {
        if (!document.querySelector(".editorial .expand.show-more")) {
            return;
        }
        document.querySelector(".editorial .expand.show-more").addEventListener("click", _ => {
            document.querySelector(".editorial.physician-contributions").classList.remove("show-less");
            document.querySelector(".editorial .expand.show-more").classList.add("hidden");
            document.querySelector(".editorial .expand.show-less").classList.remove("hidden");
        });
        document.querySelector(".editorial .expand.show-less").addEventListener("click", _ => {
            document.querySelector(".editorial.physician-contributions").classList.add("show-less");
            document.querySelector(".editorial .expand.show-more").classList.remove("hidden");
            document.querySelector(".editorial .expand.show-less").classList.add("hidden");
        });
    }

    setupCommentsShowLessShowMore() {
        if (!document.querySelector(".comments-container .expand.show-more")) {
            return;
        }
        document.querySelector(".comments-container .expand.show-more").addEventListener("click", _ => {
            document.querySelector(".comments-container .comments").classList.remove("show-less");
            document.querySelector(".comments-container .expand.show-more").classList.add("hidden");
            document.querySelector(".comments-container .expand.show-less").classList.remove("hidden");
        });
        document.querySelector(".comments-container .expand.show-less").addEventListener("click", _ => {
            document.querySelector(".comments-container .comments").classList.add("show-less");
            document.querySelector(".comments-container .expand.show-more").classList.remove("hidden");
            document.querySelector(".comments-container .expand.show-less").classList.add("hidden");
        });
    }
}
