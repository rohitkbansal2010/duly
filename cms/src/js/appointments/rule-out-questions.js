'use strict';

import { enableFocusTrap } from '../utils';

export class RuleOutQuestions {
    constructor() {
        this.container = document.querySelector("section.schedule-an-appointment-rule-out-questions-container");
        if (!this.container) {
            return;
        }

        const modal = document.querySelector('.warning-modal');
        const closeBtn = modal.querySelector('.close');

        if (!modal.classList.contains('hidden')) {
            // focus trap if the modal is active
            enableFocusTrap('.warning-modal button, .warning-modal span.material-icons.close');
        }

        ['click', 'keydown'].forEach(event => {
            closeBtn.addEventListener(event, (e) => {
                if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
                    modal.classList.add('hidden');
                }
            });
        });
    }
}