'use strict';
import { enableFocusTrap } from '../utils';

/**
 * Manages the list of reasons for visit when scheduling an appointment.
 */
export class ReasonsForVisit {
    constructor() {
        this.container = document.querySelector("section.schedule-an-appointment-reasons-for-visit-container");
        if (!this.container) {
            return;
        }

        this.setupRadioListeners();
        this.setupExplanationListeners();
        this.alertModal = document.querySelector('.other-reason-selected-alert-modal');
        this.invalidPhysician = document.querySelector('.physician-reason-for-visit-error-modal');

        if (!this.invalidPhysician.classList.contains('hidden')) {
            // focus on modal while open
            enableFocusTrap('.physician-reason-for-visit-error-modal button');
        }
        this.closeModalListener();
    }

    /**
     * Disables/Enables the main CTA when radio input changes happen.
     * Enables the ability to select radio radio inputs with the "enter" key.
     */
    setupRadioListeners() {
        this.container.querySelectorAll(".reasons .radio input").forEach(radio => radio.addEventListener("change", _ => {
            if (this.container.querySelectorAll(".reasons .radio input:checked").length > 0 && radio.value != 'other') {
                this.container.querySelector(".form-actions button").disabled = false;
            } else {
                this.container.querySelector(".form-actions button").disabled = true;
            }

            if (radio.value == 'other') {
                this.alertModal.classList.remove('hidden');

                // focus on modal while open
                enableFocusTrap('.other-reason-selected-alert-modal button, .other-reason-selected-alert-modal span.material-icons.close');
            }
        }));

        this.container.querySelectorAll(".reasons .radio input").forEach(radio => radio.addEventListener("keyup", e => {
            if (e.keyCode == 13) {
                e.target.checked = true;
                const evt = document.createEvent("HTMLEvents");
                evt.initEvent("change", false, true);
                e.target.dispatchEvent(evt);
            }
        }));
    }

    closeModalListener() {
        const closeBtn = this.alertModal.querySelector('.close');
        const exitBtn = this.alertModal.querySelector('#exit-scheduling-process');
        const invalidPhysicianExitBtn = this.invalidPhysician.querySelector('#new-doctor-reason');

        ['keydown', 'click'].forEach(event => {
            closeBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    this.alertModal.classList.add('hidden');
                }
            });

            exitBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    location.href = "/schedule";
                }
            });

            invalidPhysicianExitBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    location.href = "/schedule/select-physician";
                }
            });
        });
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }

    /**
     * Enables showing/hiding reasons for visit explanations.
     * Triggers include "click" and "enter" key.
     */
    setupExplanationListeners() {
        this.container.querySelectorAll(".reasons .radio .material-icons.caret").forEach(caret => {
            caret.addEventListener("click", _ => {
                if (caret.classList.contains('keyboard_arrow_down')) {
                    this.showExplanation(caret.closest(".radio"));
                } else if (caret.classList.contains('keyboard_arrow_up')) {
                    this.hideExplanation(caret.closest(".radio"));
                }
            });

            caret.addEventListener("keyup", e => {
                if (e.keyCode == 13) {
                    if (caret.classList.contains('keyboard_arrow_down')) {
                        this.showExplanation(caret.closest(".radio"));
                    } else if (caret.classList.contains('keyboard_arrow_up')) {
                        this.hideExplanation(caret.closest(".radio"));
                    }
                }
            });
        });
    }

    /**
     * Helpher method for showing the reason for visit explanation.
     * 
     * @param {*} radio 
     */
    showExplanation(radio) {
        radio.querySelector(".explanation").classList.remove("hidden");
        let caret = radio.querySelector(".material-icons.caret");
        caret.classList.remove('keyboard_arrow_down');
        caret.classList.add('keyboard_arrow_up');
    }

    /**
     * Helpher method for hiding the reason for visit explanation.
     * 
     * @param {*} radio 
     */
    hideExplanation(radio) {
        radio.querySelector(".explanation").classList.add("hidden");
        let caret = radio.querySelector(".material-icons.caret");
        caret.classList.remove('keyboard_arrow_up');
        caret.classList.add('keyboard_arrow_down');
    }
}