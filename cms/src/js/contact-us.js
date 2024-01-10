'use strict';
import { MDCSelect } from '@material/select';
import { executeCAPTCHAToken } from './recaptcha';
import { setupPhoneHandler } from './utils'; 

export class ContactUs {
    constructor() {
        this.container = document.querySelector(".contact-us-page");
        if (!this.container) {
            return;
        }

        this.requireCAPTCHA = true;

        // intercept form submission
        document.querySelector("#contact-us-form").addEventListener("submit", e => {
            document.querySelector('.button.submit-button').classList.add('hidden');
            document.querySelector('.loading-spinner').classList.remove('hidden');
            if (this.requireCAPTCHA) {
                e.preventDefault();
                // execute recaptcha
                executeCAPTCHAToken(token => {
                    // after human validation, submit the form
                    // reCAPTCHA token will be included in the POST request
                    document.querySelector("input#recaptcha-token").value = token;
                    document.querySelector("#contact-us-form .button").scrollIntoView();
                    this.requireCAPTCHA = false;
                    document.querySelector("#contact-us-form .submit-button").click();
                });
            }
        });

        // handles auto-formatting of phone number
        const phoneNumber = document.getElementById('phone');
        if (phoneNumber) {
            setupPhoneHandler(phoneNumber);
        }
    }
}