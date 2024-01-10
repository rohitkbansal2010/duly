'use strict';

import moment from 'moment';
import { breakpoints } from '../../variables';

/**
 * Manages a custom date picker.
 *
 * @param {*} onDateSelectedCallback - returns a Date object representing actively selected date
 * @param {*} onFirstDateChanged - returns a Date object representing the first day in the current week slice
 */
export default class DateSelector {
    constructor(onDateSelectedCallback = _ => { }, onFirstDateChanged = _ => { }, daysInASlice = null, startDate = null) {
        this.container = document.querySelector(".date-picker");
        if (!this.container) {
            return;
        }

        this.onDateSelectedCallback = onDateSelectedCallback;
        this.onFirstDateChanged = onFirstDateChanged;

        // 5 on mobile, 7 on non-mobile
        this.daysInASlice = daysInASlice ? daysInASlice : (window.innerWidth < breakpoints.medium ? 5 : 7);

        this.leftMonthPickerArrowElement = this.container.querySelector(".date-and-time-picker .month-picker .material-icons.keyboard_arrow_left");
        this.rightMonthPickerArrowElement = this.container.querySelector(".date-and-time-picker .month-picker .material-icons.keyboard_arrow_right");
        this.leftDayPickerArrowElement = this.container.querySelector(".date-and-time-picker .day-picker .material-icons.keyboard_arrow_left");
        this.rightDayPickerArrowElement = this.container.querySelector(".date-and-time-picker .day-picker .material-icons.keyboard_arrow_right");

        this.monthPickerLabel = this.container.querySelector(".date-and-time-picker .date-picker .month-picker .month");
        this.dayContainers = this.container.querySelectorAll(".date-and-time-picker .day-picker .days .day-container");

        this.dateFormat = 'YYYY-MM-DD';

        // use startDate if provided so times can start on days other than today
        if (moment(startDate).isValid()) {
            this.hasFutureDate = true;
            this.startDate = new Date(startDate);
        } else {
            this.startDate = new Date();
        }

        this.startDate.setHours(0, 0, 0, 0);
        this.setValidDates([]);
        this.setupClickListeners();
    }

    getNumberOfDaysInASlice() {
        return this.daysInASlice;
    }

    getFirstDate() {
        return this.dayContainers[0].dataset.date;
    }

    /**
     * By default, all dates in the current weeek slice are inactive. Passing a dates collection to this function
     * enables those dates to be interactive and clickable.
     *
     * @param {*} dates
     */
    setValidDates(dates = []) {
        this.resetDateSelectors();

        dates = dates.map(date => moment.utc(date).format(this.dateFormat));

        const startDate = JSON.parse(JSON.stringify(this.startDate));
        const cutoff = moment(startDate).add(this.daysInASlice, 'days');

        let foundFirstValidDay = false;
        for (let currentDate = moment(startDate), index = 0; currentDate.isBefore(cutoff); currentDate.add(1, 'days'), index++) {
            this.dayContainers[index].dataset.date = currentDate.set({
                hour: 0,
                minute: 0,
                second: 0
            }).format();

            if (dates.includes(currentDate.format(this.dateFormat))) {
                this.dayContainers[index].classList.remove("invalid");

                if (!foundFirstValidDay) {
                    this.dayContainers[index].classList.add("active");
                    foundFirstValidDay = this.dayContainers[index];
                }
            }
        }

        if (foundFirstValidDay) {
            // trigger click callback, triggers onFirstDateChanged
            this.onDateSelectedCallback(new Date(foundFirstValidDay.dataset.date));
        }
    }

    /**
     * Resets the date picker to the original (all inactive) state.
     */
    resetDateSelectors() {
        // record currently selected start date
        const startDate = JSON.parse(JSON.stringify(this.startDate));

        // iterate through seven days and set each day as "inactive", the default state
        let cutoff = moment(startDate).add(this.daysInASlice, 'days');
        for (let currentDate = moment(startDate), index = 0; currentDate.isBefore(cutoff); currentDate.add(1, 'days'), index++) {
            this.dayContainers[index].querySelector(".day .day-name").innerText = currentDate.format('ddd');
            this.dayContainers[index].querySelector(".day .day-number").innerText = currentDate.format('D');

            this.dayContainers[index].classList.add("invalid");
            this.dayContainers[index].classList.remove("active");

            // first date determines the name of the month shown in the month picker
            if (index == 0) {
                this.monthPickerLabel.innerText = currentDate.format('MMMM YYYY');
            }
        }

        this.updateMonthAndDateSelectArrowsVisibility();
    }

    /**
     * Potentially disables left arrows, as we do not allow iterating before today's date
     */
    updateMonthAndDateSelectArrowsVisibility() {
        if (this.startDate.getTime() < (new Date()).getTime() || this.hasFutureDate) {
            this.leftMonthPickerArrowElement.classList.add("invisible");
            this.leftDayPickerArrowElement.classList.add("invisible");
        } else {
            this.leftMonthPickerArrowElement.classList.remove("invisible");
            this.leftDayPickerArrowElement.classList.remove("invisible");
        }
    }

    skipToDate(date) {
        this.startDate = date;
        this.startDate.setHours(0, 0, 0, 0);
        this.setValidDates();
        this.onFirstDateChanged(this.startDate);
    }

    setupClickListeners() {
        // on the left month arrow, if currently selected the first of the month, go to the first of the previous month, but not before today's date
        // on the left month arrow, if currently not selected the first of the month, go to the first of the current month, but not before today's date
        ['click', 'keydown'].forEach((event => {
            this.leftMonthPickerArrowElement.addEventListener(event, (e) => {
                if ((e.type === 'keydown' && e.keyCode == 13) || e.type === 'click') {
                    if (this.startDate.getDate() == 1) {
                        this.startDate.setMonth(this.startDate.getMonth() - 1, 1);
                    } else {
                        this.startDate.setMonth(this.startDate.getMonth() - 0, 1);
                    }
                    if (this.startDate < (new Date())) {
                        this.startDate = new Date();
                    }
                    this.startDate.setHours(0, 0, 0, 0);
                    this.setValidDates();
                    this.onFirstDateChanged(this.startDate);
                };
            });
            // on the right month arrow, go to the first of the next month
            this.rightMonthPickerArrowElement.addEventListener(event, (e) => {
                if ((e.type === 'keydown' && e.keyCode == 13) || e.type === 'click') {
                    this.startDate.setMonth(this.startDate.getMonth() + 1, 1);
                    this.setValidDates();
                    this.onFirstDateChanged(this.startDate);
                };
            });
            // on the left day arrow, go back a week
            this.leftDayPickerArrowElement.addEventListener(event, (e) => {
                if ((e.type === 'keydown' && e.keyCode == 13) || e.type === 'click') {
                    this.startDate.setDate(this.startDate.getDate() - this.daysInASlice);
                    if (this.startDate < (new Date())) {
                        this.startDate = new Date();
                    }
                    this.startDate.setHours(0, 0, 0, 0);
                    this.setValidDates();
                    this.onFirstDateChanged(this.startDate);
                };
            });
            // on the right day arrow, go forward a week
            this.rightDayPickerArrowElement.addEventListener(event, (e) => {
                if ((e.type === 'keydown' && e.keyCode == 13) || e.type === 'click') {
                    this.startDate.setDate(this.startDate.getDate() + this.daysInASlice);
                    this.setValidDates();
                    this.onFirstDateChanged(this.startDate);
                };
            });

            this.dayContainers.forEach(dayContainer => dayContainer.addEventListener(event, (e) => {
                if ((e.type === 'keydown' && e.keyCode == 13) || e.type === 'click') {
                    const selectedDayContainer = e.target.closest('.day-container');

                    // update selected day container
                    this.dayContainers.forEach(dayContainer => {
                        if (selectedDayContainer == dayContainer) {
                            dayContainer.classList.add("active");
                        } else {
                            dayContainer.classList.remove("active");
                        }
                    });
                    
                    this.newSelectedDate = new Date(selectedDayContainer.dataset.date);

                    this.onDateSelectedCallback(this.newSelectedDate);
                };
            }));
        }));
    }
}