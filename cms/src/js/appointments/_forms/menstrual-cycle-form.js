'use strict';

import moment from 'moment';

export class MenstrualCycleForm {
    validate(submittedDate, todayDate, yearField, containerSelector) {
        let fullYearEntered = yearField.value.length == 4;
        let validDate = fullYearEntered && moment(submittedDate, moment.ISO_8601).isValid();
        let timePassed = todayDate.diff(submittedDate, 'days');
        // tomorrow's date results in 0
        // any day after tomorrow is negative
        let isFutureDate = timePassed <= 0;

        const container = document.querySelector(containerSelector);
        const submitBtn = container.querySelector('button');
        const errorMessage = container.querySelector('.future-date-error');
        
        if (validDate && fullYearEntered && !isFutureDate) {
            submitBtn.disabled = false;
        } else {
            submitBtn.disabled = true;
        }

        if (isFutureDate && fullYearEntered)  {
            errorMessage.classList.remove('hidden');
        } else {
            errorMessage.classList.add('hidden');
        }
    }
}