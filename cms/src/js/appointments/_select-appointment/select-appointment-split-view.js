'use strict';

import moment from 'moment';
import DateSelector from './_date-selector';
import RecommendedPhysicians from './_recommended-physicians';
import { InitializeMap, AddMarkers } from '../../mapbox';
import { initLottie } from '../../utils';
import { SelectAppointment } from './select-appointment';

const BY_PROVIDER_MODE = 0;
const BY_TIME_MODE = 1;
export class SelectAppointmentSplitView extends SelectAppointment {

    constructor(physicianId, daysInASlice, selectedServiceId, newPatient, establishedPatient, followUpVisit, startDate, veinClinicVisit, onlyRecommendedProviders, fullBodySkinExamVisit) {
        // call constructor of the parent class
        super(physicianId, daysInASlice, selectedServiceId, newPatient, establishedPatient, followUpVisit, startDate, veinClinicVisit, onlyRecommendedProviders, fullBodySkinExamVisit);

        this.additionalProviderIdsPerDate = JSON.parse(document.querySelector("#physiciansWithVideoVisitsPerServicePerDate").dataset.physicians)[selectedServiceId];
        this.configureSplitViewSelector();

        this.mode = BY_PROVIDER_MODE;
        this.requestAppointmentTimes();
    }

    configureSplitViewSelector() {
        this.splitViewSelectors = document.querySelector(".select-appointment .split-view");

        this.splitViewSelectors.querySelector('.option.by-provider').addEventListener('click', ({ target }) => {
            this.mode = BY_PROVIDER_MODE;
            this.splitViewSelectors.querySelector('.option.by-provider').classList.add('active');
            this.splitViewSelectors.querySelector('.option.by-time').classList.remove('active');
            this.renderAvailableTimeForSelectedDate(null, true);
            document.querySelector(".times-container").classList.remove('by-provider');
            document.querySelector(".times-container").classList.add('by-provider');
        });

        this.splitViewSelectors.querySelector('.option.by-time').addEventListener('click', ({ target }) => {
            this.mode = BY_TIME_MODE;
            this.splitViewSelectors.querySelector('.option.by-provider').classList.remove('active');
            this.splitViewSelectors.querySelector('.option.by-time').classList.add('active');
            this.renderAvailableTimeForSelectedDate();
            document.querySelector(".times-container").classList.add('by-provider');
            document.querySelector(".times-container").classList.remove('by-provider');
        });
    }

    /**
     * Shows the loading spinner, requests new appointment data, and hides the spinner when ready.
     * Once the data is available, sets the date picker witht valid dates (if any) based on the received data.
     */
    async requestAppointmentTimes(date = null) {
        // clear previous times
        this.container.querySelector(".times-container .times").innerHTML = "";
        this.container.querySelector(".error").classList.add('hidden');

        // show loading spinner
        this.loadingSpinner.classList.remove("hidden");
        this.cta.disabled = true;

        // record currently selected start date
        const startDate = date ? date : moment(this.dateSelector.getFirstDate()).format(this.dateFormat);

        let promises = [
            // "by time"
            this.requestAvailabilityTimes(this.physicianId)
        ];

        this.physiciansCountOffset = 0;
        this.physiciansCountLimit = 6;

        if (this.additionalProviderIdsPerDate[startDate]) {
            promises = promises.concat(
                // "by provider"
                this
                    .additionalProviderIdsPerDate[startDate]
                    .slice(this.physiciansCountOffset, this.physiciansCountOffset + this.physiciansCountLimit)
                    .map(async pid => this.requestAvailabilityTimes(pid.toString()))
            );
        }

        // wait for all; the order is preserved
        let data = await Promise.all(promises);
        let byTimeData = data[0].currentAppointments;

        // record the date of the latest (current) request to avoid hiding the loading indicator too soon
        this.latestTiemsRequestDate = startDate;

        // if this current request is the last request fired, hide the loading spinner
        if (this.latestTiemsRequestDate === startDate) {
            this.loadingSpinner.classList.add("hidden");

            // update the date picker with new valid dates
            if (byTimeData) {
                if (Object.keys(byTimeData).length !== 0) {
                    const validDates = Object.keys(byTimeData)
                        .map(date => {
                            const validDate = new Date(date);
                            return validDate.toUTCString();
                        });
                    this.dateSelector.setValidDates(validDates);
                } else {
                    this.container.querySelector(".error").classList.remove('hidden');
                }
            }
        }
    }

    async requestAvailabilityTimes(physicianId) {
        this.splitViewSelectors.classList.remove('hidden');
        let startDate;
        if (this.onlyRecommendedProviders) {
            startDate = moment().format(this.dateFormat);
        } else {
            startDate = moment(this.dateSelector.getFirstDate()).format(this.dateFormat);
        }

        let url = new URL(window.location.href);
        url.searchParams.set('startDate', startDate);
        if (this.daysInASlice) {
            url.searchParams.set('daysInterval', this.daysInASlice);
        }
        url.pathname = '/schedule/get-appointment-times';

        const csrfToken = document.querySelector("input#csrf-token");
        let body = {
            [csrfToken.name]: csrfToken.value
        };

        body.selectedServiceId = this.selectedServiceId;
        body.physicianId = physicianId;
        body.findOtherRecommendedPhysicians = this.findOtherRecommendedPhysicians;
        if (this.newPatient || this.establishedPatient || this.followUpVisit) {
            body.newPatient = this.newPatient ? "1" : "0";
            body.establishedPatient = this.establishedPatient ? "1" : "0";
            body.followUpVisit = this.followUpVisit ? "1" : "0";
            body.veinClinicVisit = this.veinClinicVisit ? "1" : "0";
            body.fullBodySkinExamVisit = this.fullBodySkinExamVisit ? "1" : "0";
            body.videoVisitTypeOnly = "1";
        }

        this.availabilityTimes[physicianId] = this.availabilityTimes[physicianId] ? this.availabilityTimes[physicianId] : {};

        this.availabilityTimes[physicianId][startDate] = this.availabilityTimes[physicianId][startDate] ? this.availabilityTimes[physicianId][startDate] : fetch(url, {
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
        }).then(async (response) => {
            const json = await response.json();
            if (response.ok) {
                return json;
            } else {
                return Promise.reject(json);
            }
        }).then(async (data) => {
            return data;
        }).catch(_ => {
            return false;
        });


        this.availabilityTimes[physicianId][startDate] = await this.availabilityTimes[physicianId][startDate];
        return this.availabilityTimes[physicianId][startDate];
    }

    async renderAvailableTimeForSelectedDate(date, force = false) {
        this.splitViewSelectors.classList.remove('hidden');
        this.container.querySelector(".error").classList.add('hidden');

        if (date) {
            // on date selection, reset to by provider
            this.mode = BY_PROVIDER_MODE;
            // show the by provider tab
            this.splitViewSelectors.querySelector('.option.by-provider').classList.remove('hidden');
        }

        // we are selecting a different date from the calendar module
        // since the list of providers differs between different days, we need to "reset" the view and start from 0
        if (date && moment(date).format(this.dateFormat) != moment(this.lastDate).format(this.dateFormat)) {
            this.physiciansCountOffset = 0;
            if (this.additionalProviderIdsPerDate[moment(date).format(this.dateFormat)]) {
                const timesContainer = this.container.querySelector(".times-container .times");
                timesContainer.innerHTML = "";
                document.querySelector('.times-container .loading-spinner').classList.remove('hidden');
                const promises = this
                    .additionalProviderIdsPerDate[moment(date).format(this.dateFormat)]
                    .slice(this.physiciansCountOffset, this.physiciansCountOffset + this.physiciansCountLimit)
                    .map(async pid => this.requestAvailabilityTimes(pid.toString()));
                let data = await Promise.all(promises);
                document.querySelector('.times-container .loading-spinner').classList.add('hidden');
            }
            this.lastDate = date ? date : this.lastDate;
        }

        this.lastDate = date ? date : this.lastDate;

        if (this.mode == BY_PROVIDER_MODE) {
            this.splitViewSelectors.querySelector('.option.by-provider').classList.add('active');
            this.splitViewSelectors.querySelector('.option.by-time').classList.remove('active');
            this.renderByProviderTimeModeData(this.lastDate, force);
            document.querySelector('.times-container').classList.add('by-provider');
        }
        if (this.mode == BY_TIME_MODE) {
            this.splitViewSelectors.querySelector('.option.by-provider').classList.remove('active');
            this.splitViewSelectors.querySelector('.option.by-time').classList.add('active');
            this.renderByTimeModeData(this.lastDate);
        }
    }

    renderByProviderTimeModeData(originalDate, force) {
        document.querySelector(".times-container").classList.add('by-provider');
        let date = moment(originalDate).format(this.dateFormat);
        this.cta.disabled = true;

        const firstTimeBlockDate = moment(this.dateSelector.getFirstDate()).format(this.dateFormat);
        this.chosenDate = moment(date).format(this.dateFormat);

        let data = [];
        if (this.additionalProviderIdsPerDate[this.chosenDate]) {
            this.additionalProviderIdsPerDate[this.chosenDate].forEach(id => {
                if (!this.availabilityTimes[id]) {
                    return;
                }
                if (this.availabilityTimes[id] && 
                    this.availabilityTimes[id][firstTimeBlockDate] && 
                    this.availabilityTimes[id][firstTimeBlockDate].currentAppointments && 
                    this.availabilityTimes[id][firstTimeBlockDate].currentAppointments[this.chosenDate]) {
                    data.push({
                        times: this.availabilityTimes[id][firstTimeBlockDate].currentAppointments[this.chosenDate],
                        physicianDetails: this.availabilityTimes[id][firstTimeBlockDate].physicianDetails,
                        physicianId: id
                    });
                }
            });
        }

        // filter out providers with no data
        data = data.filter(d => d.times && d.times.video && d.times.video.length > 0);

        this.renderPhysicians(data);

        if (data.length > 0) {
            if (this.additionalProviderIdsPerDate[this.chosenDate].length > this.physiciansCountLimit + this.physiciansCountOffset) {
                this.addShowMoreProvidersLink();
            }
        } else {
            this.container.querySelector(".error").classList.remove('hidden');

            if (!force) {
                this.mode = BY_TIME_MODE;
                this.renderAvailableTimeForSelectedDate();

                // hide the by provider tab
                this.splitViewSelectors.querySelector('.option.by-provider').classList.add('hidden');
            }
        }
    }

    addShowMoreProvidersLink() {
        const timesContainer = document.querySelector('.times-container .times');
        const link = document.querySelector('#by-provider-more-physicians-link').content.cloneNode(true).querySelector(".by-provider-more-physicians-link").closest(".by-provider-more-physicians-link-container");
        timesContainer.appendChild(link);

        initLottie();

        document.querySelector(".by-provider-more-physicians-link").addEventListener('click', async _ => {
            document.querySelector('.times-container .times .more-providers.loading-spinner').classList.remove('hidden');
            this.physiciansCountOffset = this.physiciansCountOffset + this.physiciansCountLimit;
            const promises = this.additionalProviderIdsPerDate[this.chosenDate]
                .slice(this.physiciansCountOffset, this.physiciansCountOffset + this.physiciansCountLimit)
                .map(async pid => this.requestAvailabilityTimes(pid.toString()));

            // wait for all; the order is preserved
            await Promise.all(promises);
            document.querySelector('.times-container .times .more-providers.loading-spinner').classList.add('hidden');
            this.renderAvailableTimeForSelectedDate();
        });
    }

    renderPhysicians(data) {
        const timesContainer = this.container.querySelector(".times-container.by-provider .times");
        timesContainer.innerHTML = "";

        this.byProviderPhysiciansPhysicianBlock = document.querySelector("#by-provider-physicians-physician-block");

        data.forEach(physician => {
            const physicianBlock = this.byProviderPhysiciansPhysicianBlock.content.cloneNode(true).querySelector(".physician");
            const physicianDetails = physician.physicianDetails;

            physicianBlock.querySelector('.title').innerText = physicianDetails.title;
            physicianBlock.querySelector('.services').innerText = physicianDetails.services.join(', ');
            physicianBlock.querySelector('.image').innerHTML = physicianDetails.image;
            physicianBlock.dataset.physicianId = physicianDetails.id;

            if (physicianDetails.ratings.overall) {
                physicianBlock.querySelector('.ratings .rating-number').innerText = physicianDetails.ratings.overall;
                physicianBlock.querySelector('.ratings .count').innerText = `(${physicianDetails.ratings.count})`;
            }

            const availableTimesLocationContainer = this.availableTimesLocationContainerTemplate.content.cloneNode(true);

            // if more than 9 time slots are available, split the times into three groups: morning, afternoon and evening
            this.appendTimesPerBlock("morning", physician.times.video, availableTimesLocationContainer);
            this.appendTimesPerBlock("afternoon", physician.times.video, availableTimesLocationContainer);
            this.appendTimesPerBlock("evening", physician.times.video, availableTimesLocationContainer);

            availableTimesLocationContainer.querySelectorAll(".available-times-location-time-selector").forEach(selector => {
                ['click', 'keydown'].forEach((event => {
                    selector.addEventListener(event, (e) => {
                        if ((e.type === 'keydown' && e.keyCode == 13) || e.type === 'click') {
                            let container = e.target.closest(".available-times-location-container");
                            let selector = e.target.closest(".available-times-location-time-selector");
                            let timeBlock = selector.dataset.timeBlock;

                            let hideAll = false;
                            if (selector.classList.contains("active")) {
                                hideAll = true;
                            }

                            container.querySelectorAll(".available-times-location-time-selector").forEach(selector => {
                                if (selector.dataset.timeBlock == timeBlock && !hideAll) {
                                    selector.classList.add("active");
                                } else {
                                    selector.classList.remove("active");
                                }
                            });

                            container.querySelectorAll(".available-times-location-block-singular").forEach(time => {
                                if (time.dataset.timeBlock == timeBlock && !hideAll) {
                                    time.classList.add("active");
                                } else {
                                    time.classList.remove("active");
                                }
                            });
                        }
                    });
                }));
            });
            physicianBlock.appendChild(availableTimesLocationContainer);

            timesContainer.appendChild(physicianBlock);

            timesContainer.querySelectorAll('.available-times-location-block-singular').forEach(block => {
                block.addEventListener('click', ({ target }) => {
                    target.closest('.select-appointment').querySelector('input[name="appointment_physician_id"]').value = target.closest('.physician').dataset.physicianId;
                });
            });
        });
    }

    renderByTimeModeData(date) {
        document.querySelector(".times-container").classList.remove('by-provider');
        this.cta.disabled = true;

        const firstTimeBlockDate = moment(this.dateSelector.getFirstDate()).format(this.dateFormat);
        this.chosenDate = moment(date).format(this.dateFormat);

        if (!this.availabilityTimes[this.physicianId]) {
            return;
        }

        const availabilityTimes = this.availabilityTimes[this.physicianId][firstTimeBlockDate].currentAppointments[this.chosenDate];

        const timesContainer = this.container.querySelector(".times-container:not(.by-provider) .times");
        timesContainer.innerHTML = "";

        let totalBlocks = 0;

        Object.keys(availabilityTimes).forEach(key => {
            let times = availabilityTimes[key];

            if (times.length == 0) {
                return;
            }
            totalBlocks++;

            const availableTimesLocationContainer = this.availableTimesLocationContainerTemplate.content.cloneNode(true);

            let daysContainer = document.createElement("div");
            daysContainer.classList.add("day-blocks-container");
            times.forEach(time => {
                const singleTimeBlock = this.availableTimesLocationBlockSingularTemplate.content.cloneNode(true).querySelector(".available-times-location-block-singular");
                singleTimeBlock.innerText = moment(time.Time).format('h:mm A');
                singleTimeBlock.classList.add("active");
                singleTimeBlock.dataset.visitTypeId = time.VisitType.ID;
                singleTimeBlock.dataset.providerId = time.Provider.ID;
                ['click', 'keydown'].forEach((event => {
                    singleTimeBlock.addEventListener(event, this.singleTimeBlockClickListener.bind(this));
                }));
                daysContainer.appendChild(singleTimeBlock);
            });
            availableTimesLocationContainer.querySelector(".available-times-location-container").appendChild(daysContainer);

            timesContainer.appendChild(availableTimesLocationContainer);
        });

        timesContainer.querySelectorAll('.available-times-location-block-singular').forEach(block => {
            block.addEventListener('click', ({ target }) => {
                const selectedTile = document.querySelector('.tile-link.selected');
                target.closest('.select-appointment').querySelector('input[name="appointment_physician_id"]').value = selectedTile.querySelector('input[name="appointment_physician_id"]').value;
                target.closest('.select-appointment').querySelector('input[name="appointment_department_id"]').value = selectedTile.querySelector('input[name="appointment_department_id"]').value;
            });
        });

        if (totalBlocks == 0) {
            this.container.querySelector(".error").classList.remove('hidden');
        } else {
            this.container.querySelector(".error").classList.add('hidden');
        }
    }
}