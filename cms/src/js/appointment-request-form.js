'use strict';

import { MDCSelect } from '@material/select';
import { AsYouType } from 'libphonenumber-js';
import { executeCAPTCHAToken } from './recaptcha';
import { breakpoints } from './variables';
import moment from 'moment';
import Pikaday from "pikaday";

export class AppointmentRequestForm {
    constructor() {
        if (document.querySelector(".physical-occupational-appointment-request-page")) {
            this.container = document.querySelector(".physical-occupational-appointment-request-page");
            if (!this.container) {
                return;
            }

            // enable only mobile or desktop filters depending on current viewport size
            this.isMobile = window.innerWidth < parseInt(breakpoints["bs-large"]) ? true : false;
            this.submitted = false;

            this.selectedInsuranceProviderIndex = 0;
            this.selectedInsurancePlanIndex = 0;
            this.selectedInsuranceProviderIndexField = document.querySelector("input[name='appointment_insurance_provider_id']");
            this.selectedInsurancePlanIndexField = document.querySelector("input[name='appointment_insurance_plan_id']");

            this.patientInsurance = null;
            this.patientPlan = null;
            this.selectedLocationField = document.querySelector("input[name='location']");
            this.selectedBestAvailableTime = document.querySelector("input[name='best_time']");
            this.selectedDates = {}
            this.selectedWindowOne = document.querySelector("input[name='window1']");
            this.selectedWindowTwo = document.querySelector("input[name='window2']");
            this.selectedWindowThree = document.querySelector("input[name='window3']");
            this.submitBtn = document.querySelector('#request-appointment-submit');

            this.setupDropdowns();
            this.setupRadioListeners();
            this.setupDOBFieldListeners();
            this.setupTextareaListeners();
            this.setupPhoneHandler();
            this.setupInsuranceSelectionClickHandlers();
            this.setupBlurListeners();
            this.initPikaDay();

            window.addEventListener('submit', (e) => {
                e.preventDefault();

                executeCAPTCHAToken(token => {
                    this.recaptchaToken = token;
                    this.createFormObject();

                    if (this.isValidJson(this.bodyObject) && token) {
                        this.submitBtn.disabled = true;
                        this.submitForm();
                    }
                });
            })
        }
        else if (document.querySelector(".cosmetic-dermatology-appointment-request-page")) {
            this.container = document.querySelector(".cosmetic-dermatology-appointment-request-page");
            if (!this.container) {
                return;
            }
            this.selectedLocationField = document.querySelector("input[name='location']");
            this.selectedProcedureField = document.querySelector("input[name='procedure']");
            this.submitBtn = document.querySelector('#request-appointment-submit');

            this.setupDropdownsCD();
            this.setupTextareaListenersCD();
            this.setupBlurListenersCD();
            this.setupPhoneHandler();
            this.setupLocationAndProcedureDropdownListeners();
            this.setupCharacterCountLimit();
            
            window.addEventListener('submit', (e) => {
                e.preventDefault();

                executeCAPTCHAToken(token => {
                    this.recaptchaToken = token;
                    this.createFormObjectCD();

                    if (this.isValidJson(this.bodyObject) && token) {
                        this.submitBtn.disabled = true;
                        this.submitFormCD();
                    }
                });
            })
        }
    }

    isValidJson(item) {
        item = typeof item !== "string"
            ? JSON.stringify(item)
            : item;

        try {
            item = JSON.parse(item);
        } catch (e) {
            return false;
        }

        if (typeof item === "object" && item !== null) {
            return true;
        }

        return false;
    }

    setupTextareaListeners() {
        let symptoms = document.querySelector('#symptoms-field');

        symptoms.addEventListener('keyup', (e) => {
            this.validateForm();
        })
    }

    setupTextareaListenersCD() {
        let comments = document.querySelector('#additional-comments');

        comments.addEventListener('keyup', (e) => {
            if (comments.checkValidity()) {
                comments.parentNode.parentNode.classList.add('mdc-text-field-success');
            } else {
                comments.parentNode.parentNode.classList.remove('mdc-text-field-success');
            }
            this.validateFormCD();
        })
    }

    setupDropdowns() {
        const insuranceProviders = new MDCSelect(this.container.querySelector("#insurance-providers"));
        const insurancePlans = new MDCSelect(this.container.querySelector("#insurance-plans"));
        const bestAvailableTime = new MDCSelect(this.container.querySelector("#request-availability"));
        const location = new MDCSelect(this.container.querySelector("#locations-provided"));
        const windowOne = new MDCSelect(this.container.querySelector("#window-one"));
        const windowTwo = new MDCSelect(this.container.querySelector("#window-two"));
        const windowThree = new MDCSelect(this.container.querySelector("#window-three"));

        insuranceProviders.listen("MDCSelect:change", e => {
            this.validateForm();
        });

        insurancePlans.listen("MDCSelect:change", e => {
            this.validateForm();
        });

        bestAvailableTime.listen("MDCSelect:change", e => {
            this.validateForm();
        });

        location.listen("MDCSelect:change", e => {
            this.validateForm();
        });

        windowOne.listen("MDCSelect:change", e => {
            this.validateForm();
        });

        windowTwo.listen("MDCSelect:change", e => {
            this.validateForm();
        });

        windowThree.listen("MDCSelect:change", e => {
            this.validateForm();
        });
    }

    setupDropdownsCD() {
        const location = new MDCSelect(this.container.querySelector("#locations-provided"));
        const procedure = new MDCSelect(this.container.querySelector("#procedures-provided"));

        location.listen("MDCSelect:change", e => {
            this.validateFormCD();
        });

        procedure.listen("MDCSelect:change", e => {
            this.validateFormCD();
        });
    }

    setupRadioListeners() {
        let workOrderQuestions = document.querySelectorAll('.work-order-question input');
        let workCompQuestions = document.querySelectorAll('.work-comp-question input');

        ['click', 'keydown', 'change'].forEach(event => {
            workOrderQuestions.forEach(btn => {
                btn.addEventListener(event, (e) => {
                    if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
                        if (btn.id.includes('yes')) {
                            this.isWorkOrder = "Yes";
                        } else {
                            this.isWorkOrder = "No";
                        }
                        this.validateForm();
                    };
                });
            })

            workCompQuestions.forEach(btn => {
                btn.addEventListener(event, (e) => {
                    if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
                        if (btn.id.includes('yes')) {
                            this.isWorkComp = "Yes";
                        } else {
                            this.isWorkComp = "No";
                        }
                        this.validateForm();
                    };
                });
            })
        });
    }

    setupDOBFieldListeners() {
        let container = document.querySelector('.date-of-birth');
        this.monthField = container.querySelector(".mdc-text-field__input[name='date-of-birth-month']");
        this.dayField = container.querySelector(".mdc-text-field__input[name='date-of-birth-day']");
        this.yearField = container.querySelector(".mdc-text-field__input[name='date-of-birth-year']");

        this.monthField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');

            // skip to the next field
            if (this.monthField.value.length == 2) {
                let value = parseInt(this.monthField.value, 10);

                if (this.between(value, 1, 12)) {
                    this.dayField.select();
                } else {
                    this.monthField.parentNode.classList.add("mdc-text-field--invalid", "mdc-text-field-error");
                }
            }

            this.validateForm();
        });

        this.dayField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');

            // skip to the next field
            if (this.dayField.value.length == 2) {
                let value = parseInt(this.dayField.value, 10);

                if (this.between(value, 1, 31)) {
                    this.yearField.select();
                } else {
                    this.dayField.parentNode.classList.add("mdc-text-field--invalid", "mdc-text-field-error");
                }
            }

            this.validateForm();
        });

        this.yearField.addEventListener("keyup", ({ target }) => {
            target.value = target.value.replace(/\D/g, '');
            this.validateForm();
        });

        this.yearField.addEventListener('blur', (e) => {
            let value = parseInt(this.yearField.value);
            let max = new Date().getFullYear();

            if (!this.between(value, 1900, max)) {
                this.yearField.parentNode.classList.add("mdc-text-field--invalid", "mdc-text-field-error");
            }
        });
    }

    between(value, min, max) {
        return value >= min && value <= max;
    }

    setupPhoneHandler() {
        const phoneNumberField = document.querySelector('[name=phoneNumber]');
        ['keyup', 'focus', 'blur', 'paste'].forEach((event) => {
            phoneNumberField.addEventListener(event, (e) => {
                const formattedNumber = new AsYouType('US').input(e.target.value);
                e.target.value = formattedNumber;
            });
        });
    }

    setupInsuranceSelectionClickHandlers() {
        const insuranceProviders = new MDCSelect(document.querySelector("#insurance-providers"));
        const insurancePlans = new MDCSelect(document.querySelector('#insurance-plans'));

        insurancePlans.disabled = true;

        insuranceProviders.listen("MDCSelect:change", e => {

            if (e.target.querySelector('.mdc-select__selected-text')) {
                this.patientInsurance = e.target.querySelector('.mdc-select__selected-text').innerText;
            }

            this.selectedInsuranceProviderIndex = (e.detail.value);
            this.selectedInsuranceProviderIndexField.value = (e.detail.value);

            this.validateForm();

            // reset plan selection
            insurancePlans.selectedIndex = -1;
            insurancePlans.value = '';

            const pickedProvider = document.querySelectorAll("#insurance-providers .mdc-list li")[e.detail.index];
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
        });

        insurancePlans.listen("MDCSelect:change", e => {
            this.selectedInsurancePlanIndex = parseInt(e.detail.value);
            this.selectedInsurancePlanIndexField.value = parseInt(e.detail.value);

            if (e.target.querySelector('.mdc-select__selected-text') && this.selectedInsurancePlanIndex != 0) {
                this.patientPlan = e.target.querySelector('.mdc-select__selected-text').innerText;
            }

            this.validateForm();
        });
    }

    initPikaDay() {
        let inputIds = ['first_preferred_date', 'second_preferred_date', 'third_preferred_date'];

        inputIds.forEach((selector) => {
            let field = document.getElementById(selector);
            let parent = field.parentNode;

            field.addEventListener('blur', (e) => {
                if (field.value) {
                    let date = moment(new Date(field.value)).toISOString();

                    if (moment(date).isValid()) {
                        parent.querySelector('span.mdc-floating-label').classList.add('mdc-floating-label--float-above');
                        this.selectedDates[selector] = moment(field.value).format('YYYY-MM-DD');
                    } else {
                        this.showErrorState(parent);
                    }
                } else {
                    this.showErrorState(parent);
                }

                this.isValid();
            });

            let options = {
                field: field,
                format: "MM/DD/YYYY",
                toString(date, format) {
                    return moment(date).format('MM/DD/YYYY');
                },
                onSelect: (e) => {
                    parent.querySelector('span.mdc-floating-label').classList.add('mdc-floating-label--float-above');
                    this.selectedDates[selector] = this.formatDate(e);
                }
            }

            if (this.isMobile) {
                let container = field.closest('.date-picker');
                options.container = container;
                options.bound = false;
            }

            new Pikaday(options);
        })
    }

    showErrorState(parent) {
        parent.classList.add("mdc-text-field--invalid", "mdc-text-field-error");
        parent.querySelector('span.mdc-floating-label').classList.remove('mdc-floating-label--float-above');
        parent.querySelector('input').value = '';
    }

    formatDate(date) {
        let d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [year, month, day].join('-');
    }

    setupBlurListeners() {
        const inputFields = document.querySelectorAll('.mdc-text-field__input');

        inputFields.forEach(element => {
            element.addEventListener('blur', (e) => {
                this.validateForm();
            })
        });
    }

    setupBlurListenersCD() {
        const inputFields = document.querySelectorAll('.mdc-text-field__input');

        inputFields.forEach(element => {
            element.addEventListener('blur', (e) => {
                this.validateFormCD();
            })
        });
    }

    createFormObject() {
        const csrfToken = document.querySelector("input#csrf-token");
        const firstName = document.querySelector('#request-form-first-name');
        const lastName = document.querySelector('#request-form-last-name');
        const phone = document.querySelector('#patient-phone');
        const dobValue = this.yearField.value + "-" + this.monthField.value + "-" + this.dayField.value;
        let insuranceValue = this.patientInsurance;

        if (this.patientPlan) {
            insuranceValue = this.patientInsurance + ":" + this.patientPlan;
        }

        this.bodyObject = JSON.stringify({
            [csrfToken.name]: csrfToken.value,
            type: "PTOT",
            communication: "Phone",
            recaptchaToken: this.recaptchaToken,
            location: this.selectedLocationField.value,
            first_name: firstName.value,
            last_name: lastName.value,
            phone_number: phone.value,
            dob: dobValue,
            best_time: this.selectedBestAvailableTime.value,
            date1: this.selectedDates['first_preferred_date'],
            window1: this.selectedWindowOne.value,
            date2: this.selectedDates['second_preferred_date'],
            window2: this.selectedWindowTwo.value,
            date3: this.selectedDates['third_preferred_date'],
            window3: this.selectedWindowThree.value,
            insurance: insuranceValue,
            order: this.isWorkOrder,
            work_comp: this.isWorkComp,
            symptoms: this.symptomsValue
        })
    }

    createFormObjectCD() {
        const csrfToken = document.querySelector("input#csrf-token");
        const firstName = document.querySelector('#request-form-first-name');
        const lastName = document.querySelector('#request-form-last-name');
        const phone = document.querySelector('#patient-phone');
        const email = document.querySelector('#patient-email');
        const locationId = document.querySelector('input[name="locationId"]');


        this.bodyObject = JSON.stringify({
            [csrfToken.name]: csrfToken.value,
            type: "CosmeticDerm",
            communication: "Phone",
            recaptchaToken: this.recaptchaToken,
            locationId: locationId.value,
            location: this.selectedLocationField.value,
            procedure: this.selectedProcedureField.value,
            first_name: firstName.value,
            last_name: lastName.value,
            phone_number: phone.value,
            email: email.value,
            comments: this.commentsValue
        })
    }

    isValid() {
        if (
            // three date pickers present
            this.selectedDates['first_preferred_date'] &&
            this.selectedDates['second_preferred_date'] &&
            this.selectedDates['third_preferred_date'] &&
            this.patientInsurance &&
            this.selectedLocationField.value &&
            this.selectedBestAvailableTime.value &&
            this.selectedWindowOne.value &&
            this.selectedWindowTwo.value &&
            this.selectedWindowThree.value &&
            this.isWorkOrder &&
            this.isWorkComp &&
            this.symptomsValue.length
        ) {
            return true;
        }

        return false;
    }

    isValidCD() {
        if (
            this.selectedLocationField.value &&
            this.selectedProcedureField.value &&
            this.commentsValue.length
        ) {
            return true;
        }

        return false;
    }

    validateForm() {
        this.symptomsValue = document.querySelector('#symptoms-field').value;
        let errors = document.querySelector('.mdc-text-field-error');

        if (!errors && !this.submitted && this.isValid()) {
            this.submitBtn.disabled = false;
        } else {
            this.submitBtn.disabled = true;
        }
    }

    validateFormCD() {
        this.commentsValue = document.querySelector('#additional-comments').value;
        let errors = document.querySelector('.mdc-text-field-error');

        if (!errors && !this.submitted && this.isValidCD()) {
            this.submitBtn.disabled = false;
        } else {
            this.submitBtn.disabled = true;
        }
    }

    submitForm() {
        this.submitted = true;

        fetch(`${window.location.origin}/schedule/physical-occupational-therapy`, {
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
            body: this.bodyObject
        }).then(async (response) => {
            const text = await response.text();
            if (response.ok) {
                return text;
            } else {
                return Promise.reject(text);
            }
        }).then(async (data) => {
            if (document.querySelector('.form-request-success')) {
                const modal = document.querySelector('.form-request-success')
                modal.classList.remove('hidden');
            }
        }).catch(_ => {
            if (document.querySelector('.fetch-error')) {
                let error = document.querySelector('.fetch-error');
                error.classList.remove('hidden');
            }
        });
    }

    submitFormCD() {
        this.submitted = true;

        fetch(`${window.location.origin}/schedule/cosmetic-dermatology`, {
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
            body: this.bodyObject
        }).then(async (response) => {
            const text = await response.text();
            if (response.ok) {
                return text;
            } else {
                return Promise.reject(text);
            }
        }).then(async (data) => {
            if (document.querySelector('.form-request-success')) {
                const modal = document.querySelector('.form-request-success')
                modal.classList.remove('hidden');
            }
        }).catch(_ => {
            if (document.querySelector('.fetch-error')) {
                let error = document.querySelector('.fetch-error');
                error.classList.remove('hidden');
            }
        });
    }

    setupLocationAndProcedureDropdownListeners() {
        const locations = new MDCSelect(document.querySelector("#locations-provided"));
        const procedures = new MDCSelect(document.querySelector("#procedures-provided"));

        procedures.getDefaultFoundation().setDisabled(true);

        locations.listen("MDCSelect:change", e => {
            const pickedLocation = e.detail.value;

            document.querySelectorAll('.locations-container .mdc-list li').forEach(location => {
                if (location.dataset.value === pickedLocation) {
                    document.querySelector("input[name='locationId']").value = location.dataset.locationId;
                }
            });

            document.querySelectorAll("#procedures-provided .mdc-list .mdc-list-item").forEach(procedure => {
                if (procedure.dataset.location !== pickedLocation) {
                    procedure.classList.add("hidden");
                } else {
                    procedure.classList.remove("hidden");
                }
                procedures.getDefaultFoundation().setDisabled(false);
            });
        });
    }

    setupCharacterCountLimit() {
        const characterCountContainer = document.querySelector('#character-count-container');
		const textarea = document.querySelector('#additional-comments');

        if (!characterCountContainer || !textarea) {
            return;
        }

        let charactersLeft = document.querySelector('#characters-left');
        const characterLimit = parseInt(characterCountContainer.dataset.limit);

        textarea.addEventListener('keyup', ({ target }) => {
            if (parseInt(charactersLeft.innerText) <= 0) {
                target.value = target.value.substr(0, characterLimit);
            }

            charactersLeft.innerText = characterLimit - target.value.length;

            if (parseInt(charactersLeft.innerText) <= 50) {
                characterCountContainer.classList.add('warning');
            } else {
                characterCountContainer.classList.remove('warning');
            }
        });

        textarea.addEventListener('paste', event => {
            let paste = (event.clipboardData || window.clipboardData).getData('text');
            textarea.value = paste;

            if (parseInt(charactersLeft.innerText) <= 0) {
                textarea.value = textarea.value.substr(0, characterLimit);
            }

            charactersLeft.innerText = characterLimit - textarea.value.length;

            if (parseInt(charactersLeft.innerText) <= 50) {
                characterCountContainer.classList.add('warning');
            } else {
                characterCountContainer.classList.remove('warning');
            }
        });
    }
}