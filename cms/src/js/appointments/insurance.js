'use strict';

import { MDCSelect } from '@material/select';
import { enableFocusTrap } from '../utils';
import { MenstrualCycleForm } from '../appointments/_forms/menstrual-cycle-form';
import moment from 'moment';
import { Analytics } from "../analytics.js";

export class AppointmentsInsurance {
    constructor() {
        this.container = document.querySelector("section.schedule-an-appointment-insurance-container");
        if (!this.container) {
            return;
        }

        this.selectedInsuranceProviderIndex = 0;
        this.selectedInsurancePlanIndex = 0;

        this.noProviderValid = false;

        this.monthField = document.querySelector(".mdc-text-field__input[name='date_of_birth_month']");
        this.dayField = document.querySelector(".mdc-text-field__input[name='date_of_birth_day']");
        this.yearField = document.querySelector(".mdc-text-field__input[name='date_of_birth_year']");

        this.setupDobFieldListeners();
        this.setupModalListeners();

        this.modal = document.querySelector('.modal.missing-insurance');
        this.checkbox = this.modal.querySelector('#confirm-no-insurance');
        this.continueBtn = this.modal.querySelector('.continue');
        this.physicianInsuranceRestrictionErrorModal = document.getElementById('physicianInsuranceRestrictionErrorModal');

        this.physician = document.querySelector(".physician-info-container");
        this.physicianOnlyAcceptsMedicareAdvantage = this.physician && this.physician.dataset.onlyAcceptsMedicareAdvantage === '1';
        this.physicianInsuranceRestrictionError = false;

        if (this.physicianInsuranceRestrictionErrorModal) {
            const close = this.physicianInsuranceRestrictionErrorModal.querySelector('span.material-icons.close');
            if (close) {
                close.addEventListener('click', () => {
                    this.physicianInsuranceRestrictionErrorModal.classList.add('hidden');
                    this.physicianInsuranceRestrictionErrorModalDismissed = true;
                });
            }
            const button = this.physicianInsuranceRestrictionErrorModal.querySelector('button');
            const form = document.getElementById('insurance-form');
            if (button && form) {
                button.addEventListener('click', () => {
                    form.submit();
                })
            }
        }

        // if OBGYN, must enter last menstrual cycle date
        if (document.querySelector('.last-menstrual-cycle-prompt')) {
            this.mentrualCycleForm = new MenstrualCycleForm(_ => { });
            this.setupLastMenstrualCycleFieldListeners();
        }

        if (document.querySelector('.congrats-btn')) {
            let btn = document.querySelector('.congrats-btn');
            ['click', 'keydown'].forEach(event => {
                btn.addEventListener(event, (e) => {
                    if (this.keydownOrClick(event, e)) {
                        location.href = "/schedule";
                    }
                });
            });
        }

        if (document.querySelector('.age-restriction-modal')) {
            if (!document.querySelector('.age-restriction-modal').classList.contains('hidden')) {
                enableFocusTrap('.age-restriction-modal a, .age-restriction-modal button');
            }
        }

        if (document.querySelector('.age-restriction-physician-modal')) {
            if (!document.querySelector('.age-restriction-physician-modal').classList.contains('hidden')) {
                enableFocusTrap('.age-restriction-physician-modal a, .age-restriction-physician-modal button');
            }
        }

        if (!document.querySelector('.hide-insurance')) {
            this.selectedInsuranceProviderIndexField = document.querySelector("input[name='appointment_insurance_provider_id']");
            this.selectedInsurancePlanIndexField = document.querySelector("input[name='appointment_insurance_plan_id']");
            this.setupInsuranceSelectionClickHandlers();
        } else {
            this.noProviderValid = true;
        }

        // for authorized users, no insurance fields will show 
        // functions dealing with insurance providers should not be called
        if (document.querySelector('.prepopulated-form')) {
            this.triggerFocusOnFields();
            this.validateForm();
        }

        this.analytics = new Analytics(false);
    }

    triggerFocusOnFields() {
        let fields = [this.monthField, this.dayField, this.yearField];

        fields.forEach(textField => {
            textField.focus();
            textField.blur();
        });
    }

    setupLastMenstrualCycleFieldListeners() {
        let container = document.querySelector('.last-menstrual-cycle-prompt');
        let monthField = container.querySelector(".mdc-text-field__input[name='last_menstrual_cycle_month']");
        let dayField = container.querySelector(".mdc-text-field__input[name='last_menstrual_cycle_day']");
        let yearField = container.querySelector(".mdc-text-field__input[name='last_menstrual_cycle_year']");
        // toISOString is needed to prevent future deprecation issues
        let todayDate = moment(new Date().toISOString());

        monthField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateCycleForm(monthField, dayField, yearField, todayDate);

            // skip to the next field
            if (monthField.value.length == 2) {
                dayField.select();
            }
        });

        dayField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateCycleForm(monthField, dayField, yearField, todayDate);

            // skip to the next field
            if (dayField.value.length == 2) {
                yearField.select();
            }
        });

        yearField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateCycleForm(monthField, dayField, yearField, todayDate);
        });
    }

    validateCycleForm(monthField, dayField, yearField, todayDate) {
        this.submittedDate = new Date().getFullYear() - 200 && moment(`${monthField.value.padStart(2, '0')}/${dayField.value.padStart(2, '0')}/${yearField.value.padStart(4, '0')}`).toISOString();
        this.timePassed = todayDate.diff(this.submittedDate, 'days');
        this.mentrualCycleForm.validate(this.submittedDate, todayDate, yearField, '.last-menstrual-cycle-prompt');
    }

    validateForm() {
        let dateEntered = moment(`${this.monthField.value.padStart(2, '0')}/${this.dayField.value.padStart(2, '0')}/${this.yearField.value.padStart(4, '0')}`).unix();
        // this gets the current date plus 9 months to allow parents to use their childs due date as a DOB
        let currentDate = Math.floor(new Date().getTime() / 1000) + 23670000;
        let validDate = dateEntered < currentDate && parseInt(this.yearField.value) < new Date().getFullYear() + 2 && this.yearField.value.length === 4 && moment(`${this.monthField.value.padStart(2, '0')}/${this.dayField.value.padStart(2, '0')}/${this.yearField.value.padStart(4, '0')}`, 'MM/DD/YYYY', true).isValid();

        if (this.monthField.dataset.modified === "1" && this.dayField.dataset.modified === "1" && this.yearField.dataset.modified === "1" && !validDate) {
            this.monthField.closest(".mdc-text-field").classList.add("mdc-text-field-error");
            this.dayField.closest(".mdc-text-field").classList.add("mdc-text-field-error");
            this.yearField.closest(".mdc-text-field").classList.add("mdc-text-field-error");
        } else {
            this.monthField.closest(".mdc-text-field").classList.remove("mdc-text-field-error");
            this.dayField.closest(".mdc-text-field").classList.remove("mdc-text-field-error");
            this.yearField.closest(".mdc-text-field").classList.remove("mdc-text-field-error");
        }


        if (validDate && (this.selectedInsurancePlanIndex || this.noProviderValid) && (this.selectedInsuranceProviderIndex !== 0 || this.noProviderValid) && !this.physicianInsuranceRestrictionError) {
            document.querySelector("#find-a-doctor").disabled = false;
        } else {      
            if (this.physicianInsuranceRestrictionError && validDate && !this.physicianInsuranceRestrictionErrorModalDismissed) {
                if (this.physicianInsuranceRestrictionErrorModal) {
                    this.physicianInsuranceRestrictionErrorModal.classList.remove('hidden');
                }
            }
            document.querySelector("#find-a-doctor").disabled = true;
        }
    }

    setupDobFieldListeners() {
        this.monthField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateForm();

            // skip to the next field
            if (this.monthField.value.length == 2) {
                this.dayField.select();
            }
        });

        this.dayField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateForm();

            // skip to the next field
            if (this.dayField.value.length == 2) {
                this.yearField.select();
            }
        });

        this.yearField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateForm();
        });

        [this.monthField, this.dayField, this.yearField].forEach(field => field.addEventListener("blur", ({ target }) => {
            target.dataset.modified = "1";
            this.validateForm();
        }));
    }

    setupInsuranceSelectionClickHandlers() {
        const insuranceProviders = new MDCSelect(document.querySelector("#insurance-providers"));
        const insurancePlans = new MDCSelect(document.querySelector("#insurance-plans"));
        
        insurancePlans.disabled = true;

        insuranceProviders.listen("MDCSelect:change", e => {
            this.selectedInsuranceProviderIndex = parseInt(e.detail.value);

            if (e.detail.value == 'no-provider' || e.detail.value == 'self-pay') {
                this.noProviderValid = true;
            } else {
                this.noProviderValid = false;
            }

            const pickedProvider = document.querySelectorAll("#insurance-providers .mdc-list li")[e.detail.index];
            if (pickedProvider.dataset.missingInsurance) {
                this.showMissingInsuranceModal();
            }

            this.validateForm();

            // reset plan selection
            insurancePlans.selectedIndex = -1;
            insurancePlans.value = '';

            if (pickedProvider.classList.contains("invalid-option")) {
                // disable plan picker until avalid provider is selected
                insurancePlans.disabled = true;
                insurancePlans.valid = true;
            } else {
                // enable plan picker if a avalid provider is selected
                insurancePlans.disabled = false;

                // hide plans not belonging to this provider
                // but keep invalid options (default option, "cash" options, etc.)
                document.querySelectorAll("#insurance-plans .mdc-list .mdc-list-item:not(.invalid-option)").forEach(plan => {
                    if (plan.dataset.providerId !== this.selectedInsuranceProviderIndexField.value) {
                        plan.classList.add("hidden");
                        plan.parentNode.appendChild(plan);
                    } else {
                        plan.classList.remove("hidden");
                    }
                });

                let validPlans = document.querySelectorAll("#insurance-plans .mdc-list .mdc-list-item:not(.invalid-option):not(.hidden)");
                Array.from(validPlans).sort((a, b) => a.innerText > b.innerText ? 1 : -1).forEach(node => {
                    let invalidOptions = node.parentNode.querySelectorAll(".invalid-option");
                    let lastOption = node.parentNode.querySelector('.last-option');
                    node.parentNode.insertBefore(node, invalidOptions[invalidOptions.length - 1].nextSibling);
                    node.parentNode.insertBefore(lastOption, node.nextSibling);
                });
            }

            this.analytics._sendEvent("Schedule Appointment", "Insurance Provider Menu Choice Click", pickedProvider.innerText);
        });

        const medicareAdvantage = document.getElementById('medicareAdvantage');

        insurancePlans.listen("MDCSelect:change", e => {
            this.selectedInsurancePlanIndex = parseInt(e.detail.value);
            this.physicianInsuranceRestrictionError = false;
            this.physicianInsuranceRestrictionErrorModalDismissed = false;

            const pickedPlan = document.querySelectorAll("#insurance-plans .mdc-list li")[e.detail.index];
            
            if (!pickedPlan) {
                return this.validateForm();
            }
            
            if (this.physicianOnlyAcceptsMedicareAdvantage && this.selectedInsurancePlanIndex > 0 && pickedPlan.dataset.medicareAdvantage !== '1') {
                this.physicianInsuranceRestrictionError = true;
            }

            if (pickedPlan.dataset.missingInsurance) {
                this.showMissingInsuranceModal();
            }

            if (pickedPlan.classList.contains("invalid-option")) {
                this.selectedInsurancePlanIndex = 0;
            }

            medicareAdvantage.value = pickedPlan.dataset.medicareAdvantage ?? '0';

            this.validateForm();

            if (pickedPlan.innerText !== "Insurance Plan") {
                this.analytics._sendEvent("Schedule Appointment", "Insurance Plan Menu Choice Click", pickedPlan.innerText);
            }
        });

        document.querySelector(".modal.missing-insurance .material-icons.close").addEventListener("keydown", e => {
            if (e.keyCode == 13) {
                document.querySelector(".modal.missing-insurance").classList.add("hidden");
                this.validateInsuranceDisclaimerCheckbox();
            }
        });

        document.querySelector(".modal.missing-insurance").addEventListener("keydown", e => {
            if (e.keyCode == 27) {
                document.querySelector(".modal.missing-insurance").classList.add("hidden");
                this.validateInsuranceDisclaimerCheckbox();
            }
        });
    }

    validateInsuranceDisclaimerCheckbox() {
        // If the "I understand..." checkbox is not enabled,
        // disable the CTA so the user can't proceed further
        if (this.checkbox.checked) {
            this.continueBtn.disabled = false;
            this.noProviderValid = true;
        } else {
            this.continueBtn.disabled = true;
            this.noProviderValid = false;
        }
        this.validateForm();
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }

    setupModalListeners() {
        const events = ['click', 'keydown', 'change'];
        const modal = document.querySelector('.modal.missing-insurance');
        const ageRestrictionModal = document.querySelector('.modal.age-restriction-modal');
        const ageRestrictionPhysicianModal = document.querySelector('.modal.age-restriction-physician-modal');
        const close = modal && modal.querySelector('.close');
        const checkbox = modal.querySelector('#confirm-no-insurance');
        const redirectBtn = modal.querySelector('.insurance-redirect');
        const continueBtn = modal.querySelector('.continue');

        events.forEach(event => {
            close && close.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    modal.classList.add('hidden');
                    this.validateInsuranceDisclaimerCheckbox();
                }
            })

            redirectBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    e.preventDefault();
                    e.stopPropagation();
                    window.open("/insurance-carriers");
                }
            })

            ageRestrictionModal.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    if (e.target.classList.contains('form-actions') || e.target.classList.contains('action-button')) {
                        e.preventDefault();
                        e.stopPropagation();
                        location.href = "/schedule";
                    }
                }
            })

            ageRestrictionPhysicianModal.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    if (e.target.classList.contains('form-actions') || e.target.classList.contains('action-button')) {
                        e.preventDefault();
                        e.stopPropagation();
                        location.href = "/schedule/insurance";
                    }
                }
            })

            checkbox.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    if (checkbox.checked) {
                        continueBtn.disabled = false;
                        this.noProviderValid = true;
                    } else {
                        continueBtn.disabled = true;
                        this.noProviderValid = false;
                    }
                }
            })

            continueBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    e.preventDefault();
                    e.stopPropagation();
                    modal.classList.add('hidden');
                    this.validateInsuranceDisclaimerCheckbox();
                    this.validateForm();
                }
            })
        })
    }

    showMissingInsuranceModal() {
        // uncheck the checkbox every time the modal is rendered
        this.checkbox.checked = false;
        // render the dislaimer modal
        document.querySelector(".modal.missing-insurance").classList.remove("hidden");
        enableFocusTrap('.missing-insurance .close, .missing-insurance .insurance-redirect, .missing-insurance .insurance-checkbox');
        // At the same time, if the "I understand..." checkbox is not enabled,
        // disable the CTA so the user can't proceed further
        this.validateInsuranceDisclaimerCheckbox();
    }
}