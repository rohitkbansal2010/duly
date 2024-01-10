'use strict';

import { AsYouType } from 'libphonenumber-js';
import { MDCTextFieldHelperText } from '@material/textfield/helper-text';
import { Analytics } from './analytics.js';
import { executeCAPTCHAToken } from './recaptcha';

export class EventDetails {
    constructor() {
        if (document.querySelector('main.event-details')) {
            this.setup();
        }
    }

    setup() {
        this.analytics = new Analytics(false);
        const cancelRegistrationButton = document.getElementById('cancel-registration');
        const startRegistrationButton = document.querySelectorAll('.register');
        const registrationForm = document.getElementById('registration-form');
        const registerButton = registrationForm.querySelector('#complete-registration');

        // Phone Number Validation
        const phoneNumberField = document.querySelector('[name=phoneNumber]');
        ['keyup', 'focus', 'blur', 'paste'].forEach((event) => {
            phoneNumberField.addEventListener(event, (e) => {
                const formattedNumber = new AsYouType('US').input(e.target.value);
                e.target.value = formattedNumber;
            });
        });

        // Handle changing the registration button disabled state
        registrationForm.querySelectorAll('input').forEach((el) => {
            ['change', 'keyup', 'focus', 'blur'].forEach((event) => {
                el.addEventListener(event, (e) => {
                    this.addGlobalError("");

                    if (registrationForm.checkValidity()) {
                        registerButton.removeAttribute('disabled');
                    } else {
                        registerButton.setAttribute('disabled', 'disabled');
                    }
                });
            });
        });

        ['touchstart', 'click'].forEach((event) => {
            registerButton.addEventListener(event, (e) => {
                e.preventDefault();
                // execute recaptcha
                executeCAPTCHAToken(token => {
                    // after human validation, submit the form
                    this.registerUser(e, token);
                });
            }, true);
        });


        startRegistrationButton.forEach((el) => {
            ['click'].forEach((event) => {
                el.addEventListener(event, (e) => {
                    document.querySelector('.registration-section').classList.remove('hidden');
                    document.querySelector('.form-warnings').classList.add('hidden');
                    document.querySelector('.registration-section').scrollIntoView({
                        behavior: 'smooth'
                    });

                    registrationForm.querySelector("#registration-form input:not([type=hidden])").focus();
                });
            });
        });

        ['touchstart', 'click'].forEach((event) => {
            cancelRegistrationButton.addEventListener(event, (e) => {
                e.preventDefault();
                document.querySelector('.registration-section').classList.add('hidden');

                startRegistrationButton.forEach((el) => {
                    el.removeAttribute('disabled');
                });
            });
        });
    }

    async registerUser(e, token) {
        document.querySelector('i.fa-spinner').classList.remove('hidden');
        document.getElementById('complete-registration').setAttribute('disabled', 'disabled');
        const registrationForm = document.getElementById('registration-form');

        const obj = Object.fromEntries(new FormData(registrationForm));
        obj.recaptchaToken = token;
        const body = JSON.stringify(obj);

        fetch('/event-registration/register', {
            method: 'POST',
            mode: 'same-origin',
            cache: 'no-cache',
            credentials: 'same-origin',
            body: body,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        }).then(async (response) => {
            document.querySelector('i.fa-spinner').classList.add('hidden');

            const json = await response.json();
            return response.ok ? json : Promise.reject(json);
        }).then((response) => {
            document.querySelector('.registered-user-email').innerHTML = obj.emailAddress;
            document.querySelector('.registration-section').classList.add('hidden');
            document.querySelector('.registration-complete').classList.remove('hidden');
            document.querySelector('i.fa-spinner').classList.add('hidden');
            document.getElementById('complete-registration').removeAttribute('disabled');
            document.querySelector('.register').setAttribute('disabled', 'disabled');

            this.analytics.EventRegisterCallback();
        }).catch((response) => {
            this.analytics.EventRegisterFailureCallback(response);
            document.getElementById('complete-registration').removeAttribute('disabled');
            if ('errors' in response) {
                for (const [field, errors] of Object.entries(response.errors)) {
                    const input = document.querySelector('input[name="' + field + '"]');
                    if (input != null && input.type != "hidden") {
                        input.parentNode.classList.add('mdc-text-field-error');
                        input.parentNode.classList.remove('mdc-text-field-success');

                        let helperText = document.createElement('div');
                        helperText.classList.add('mdc-text-field-helper-text');
                        helperText.classList.add('mdc-text-field-helper-text--validation-msg');
                        helperText.classList.add('mdc-text-field-helper-text--persistent');
                        helperText.innerHTML = errors[0];
                        let helperTextLine = document.createElement('div');
                        helperTextLine.classList.add('mdc-text-field-helper-line');
                        helperTextLine.appendChild(helperText);
                        input.parentNode.parentNode.appendChild(helperTextLine);
                    }

                    this.addGlobalError(errors[0]);
                }

                [].map.call(document.querySelectorAll('.mdc-text-field-helper-text'), (el) => {
                    return new MDCTextFieldHelperText(el);
                });
            }

            if ('error' in response && response.error != "" && response.error != null) {
                this.addGlobalError(response.error);
            }
        });
    }

    addGlobalError(text) {
        const formWarnings = document.getElementById('registration-form').querySelector('.form-warnings');

        formWarnings.querySelector('#form-error-msg .form-error-msg--text').innerHTML = text;

        if (text != "") {
            formWarnings.classList.remove('hidden');
            formWarnings.removeAttribute('aria-hidden');
        } else {
            formWarnings.classList.add('hidden');
            formWarnings.setAttribute('aria-hidden', 'true');
        }
    }
}
