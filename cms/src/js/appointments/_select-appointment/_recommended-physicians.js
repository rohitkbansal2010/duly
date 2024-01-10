'use strict';

import moment from 'moment';
import { initLottie } from '../../utils';

/**
 * Manages the recommended physicians widget
 *
 * @param {*} selectAppointmentInstance - a running instance of the "parent" SelectAppointment class
 */
export default class RecommendedPhysicians {
    constructor(selectAppointmentInstance) {
        this.container = document.querySelector('.recommended-physicians');
        this.selectAppointmentInstance = selectAppointmentInstance;

        if (!this.container) {
            return;
        }

        this.recommendedPhysiciansPhysicianBlockTemplate = this.container.querySelector("#recommended-physicians-physician-block");
    }

    /**
     * Resets the widget
     *
     * @param {*} data - used to poppulate the widget
     * @param {*} chosenDate - a widget shows recommended physicians for this specific date
     */
    resetWithData(data, chosenDate) {
        if (!this.container) {
            return;
        }
        this.container = document.querySelector('.recommended-physicians');
        this.renderPhysicians(data);

        if (data.length == 0) {
            this.container.parentNode.classList.remove('showRecommended');
            this.container.classList.add('collapsed');
            return;
        } else {
            this.container.parentNode.classList.add('showRecommended');
            this.container.classList.remove('collapsed');
        }
    }

    /**
     * Generate the list of recommended physicians.
     *
     * @param {*} data - used to poppulate the list of physicians
     * @param {*} chosenDate - a widget shows recommended physicians for this specific date
     */
    renderPhysicians(data) {
        const physiciansContainer = this.container.querySelector('.physicians');
        physiciansContainer.innerHTML = "";

        this.currentlyShown = 0;

        data.forEach(physician => {
            const physicianBlock = this.recommendedPhysiciansPhysicianBlockTemplate.content.cloneNode(true).querySelector(".physician");
            physicianBlock.querySelector('.title').innerText = physician.title;
            physicianBlock.querySelector('.services').innerText = physician.services.join(', ');
            physicianBlock.querySelector('.address .line-one').innerText = physician.address.addressLine;
            physicianBlock.querySelector('.address .line-two').innerText = physician.address.cityStateAndZip;
            physicianBlock.querySelector('.image').innerHTML = physician.image;
            physicianBlock.querySelector('.appointment-times .availability-date').innerText = moment(physician.nextAppointmentDate).format('MMMM D, YYYY');
            physicianBlock.dataset.physicianId = physician.id;
            physicianBlock.dataset.externalDepartmentId = physician.externalDepartmentId;
            physicianBlock.dataset.nextAppointmentDate = physician.nextAppointmentDate;

            if (!physician.nextAppointmentDate) {
                return;
            }

            if (physician.ratings) {
                physicianBlock.querySelector('.ratings').innerHTML = physician.ratings;
            } else {
                physicianBlock.querySelector('.ratings').classList.add('hidden');
            }

            this.currentlyShown++;

            if (this.currentlyShown > 3) {
                physicianBlock.classList.add('hidden');
                this.currentlyShown--;
            }

            physiciansContainer.appendChild(physicianBlock);
        });

        if (data.length > 3) {
            this.container.querySelector('.expand.show-more').classList.remove('hidden');
            this.container.querySelector('.expand.show-more').addEventListener('click', _ => {
                const nextLimit = this.currentlyShown + 3;
                this.currentlyShown = 0;

                this.container.querySelectorAll('.physician').forEach((v, k) => {
                    if (this.currentlyShown < nextLimit) {
                        v.classList.remove('hidden');
                        this.currentlyShown++;
                    }
                })

                if (this.currentlyShown >= this.container.querySelectorAll('.physician').length) {
                    this.container.querySelector('.expand.show-more').classList.add('hidden');
                }
            });
        }

        physiciansContainer.querySelectorAll('.appointment-times .cta').forEach(cta => cta.addEventListener('click', this.physicianAvailabilityClickListener.bind(this)));
        initLottie();
    }

    async physicianAvailabilityClickListener({ target }) {
        const parentContainer = target.closest('.physician');
        parentContainer.querySelector('.times-container').classList.toggle('hidden');
        parentContainer.querySelector('.keyboard_arrow_down').classList.toggle('hidden');
        parentContainer.querySelector('.keyboard_arrow_up').classList.toggle('hidden');

        Array.prototype.forEach.call(parentContainer.querySelectorAll('.times .time'), function (node) {
            node.parentNode.removeChild(node);
        });

        let error = parentContainer.querySelector('.times span');
        if (error) {
            error.parentNode.removeChild(error);
        }

        if (parentContainer.querySelector('.times-container').classList.contains('hidden')) {
            parentContainer.querySelector('.loading-spinner').classList.add('hidden');
            return;
        }

        parentContainer.querySelector('.loading-spinner').classList.remove('hidden');

        let data = await this.requestAvailabilityTimes(parentContainer.dataset.physicianId, parentContainer.dataset.nextAppointmentDate);
        let combinedData = [];
        parentContainer.dataset.externalDepartmentId.split(",").forEach(id => {
            const dataPerDay = data.currentAppointments[moment(this.chosenDate).format('YYYY-MM-DD')][id];
            if (dataPerDay) {
                combinedData = combinedData.concat(dataPerDay);
            }
        });

        Array.prototype.forEach.call(parentContainer.querySelectorAll('.times .time'), function (node) {
            node.parentNode.removeChild(node);
        });

        parentContainer.querySelector('.loading-spinner').classList.add('hidden');

        if (!combinedData || combinedData.length === 0) {
            let error = document.createElement('span');
            error.classList.add('error');
            error.innerText = "Sorry! There is no availability for this date.";
            parentContainer.querySelector('.appointment-times').innerHTML = "";
            parentContainer.querySelector('.appointment-times').appendChild(error);
        } else {
            combinedData.forEach(time => {
                const singleTimeBlock = this.selectAppointmentInstance.availableTimesLocationBlockSingularTemplate.content.cloneNode(true).querySelector(".available-times-location-block-singular");
                singleTimeBlock.classList.add('time');
                singleTimeBlock.innerText = moment(time.Time).format('h:mm A');
                singleTimeBlock.dataset.appointmentDepartmentId = time.Department.ID;
                singleTimeBlock.dataset.time = time.Time;
                singleTimeBlock.dataset.appointmentVisitTypeId = time.VisitType.ID;
                singleTimeBlock.dataset.physicianId = parentContainer.dataset.physicianId;

                parentContainer.querySelector('.times').appendChild(singleTimeBlock);
            });

            parentContainer.querySelectorAll('.times .time').forEach(time => {
                time.addEventListener('click', this.timeBlockListener.bind(this));
            });
        }
    }

    timeBlockListener({ target }) {
        Array.prototype.forEach.call(document.querySelectorAll('.available-times-location-block-singular'), function (node) {
            node.classList.remove('clicked');
        });
        target.classList.add('clicked');

        document.querySelector("[name='appointment_time']").value = target.dataset.time;
        document.querySelector("[name='appointment_department_id']").value = target.dataset.appointmentDepartmentId;
        document.querySelector("[name='appointment_visit_type_id']").value = target.dataset.appointmentVisitTypeId;
        document.querySelector("[name='appointment_selected_recommended_physician_id']").value = target.dataset.physicianId;
        this.selectAppointmentInstance.cta.disabled = false;
        this.selectAppointmentInstance.cta.closest(".cta-form").classList.remove("hidden");
    }

    async requestAvailabilityTimes(physicianId, nextAppointmentDate) {
        let startDate = nextAppointmentDate;
        this.chosenDate = nextAppointmentDate;

        let url = new URL(window.location.href);
        url.searchParams.set('startDate', startDate);
        // recommended physicians module always displays available time slots only for one day
        url.searchParams.set('daysInterval', 1);
        url.pathname = '/schedule/get-appointment-times';

        const csrfToken = document.querySelector("input#csrf-token");
        let body = {
            [csrfToken.name]: csrfToken.value
        };

        body.selectedServiceId = this.selectAppointmentInstance.selectedServiceId;
        body.physicianId = physicianId;
        body.findOtherRecommendedPhysicians = false;
        if (this.selectAppointmentInstance.newPatient || this.selectAppointmentInstance.establishedPatient || this.selectAppointmentInstance.followUpVisit) {
            body.newPatient = this.selectAppointmentInstance.newPatient ? "1" : "0";
            body.establishedPatient = this.selectAppointmentInstance.establishedPatient ? "1" : "0";
            body.followUpVisit = this.selectAppointmentInstance.followUpVisit ? "1" : "0";
            body.veinClinicVisit = this.selectAppointmentInstance.veinClinicVisit ? "1" : "0";
            body.fullBodySkinExamVisit = this.selectAppointmentInstance.fullBodySkinExamVisit ? "1" : "0";
        }

        try {
            const response = await fetch(url, {
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
                body: JSON.stringify(body)
            });
            const data = await response.json();
            return data;
        } catch (err) {
            console.log('error loading times for recommended physician:', err.message);
            return false;
        }
    }
}