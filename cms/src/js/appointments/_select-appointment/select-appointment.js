'use strict';

import moment from 'moment';
import DateSelector from './_date-selector';
import RecommendedPhysicians from './_recommended-physicians';
import { InitializeMap, AddMarkers } from '../../mapbox';
import { initLottie } from '../../utils';

export class SelectAppointment {
    constructor(physicianId, daysInASlice = null, selectedServiceId = null, newPatient = false, establishedPatient = false, followUpVisit = false, startDate = null, veinClinicVisit = null, onlyRecommendedProviders = false, fullBodySkinExamVisit = false) {
        if (onlyRecommendedProviders) {
            this.availabilityTimes = {};

            this.availableTimesLocationContainerTemplate = document.querySelector("#available-times-location-container");
            this.availableTimesLocationTimeSelectorTemplate = document.querySelector("#available-times-location-time-selector");
            this.availableTimesLocationBlockSingularTemplate = document.querySelector("#available-times-location-block-singular");
            this.physicianId = physicianId;
            this.findOtherRecommendedPhysicians = true;
            this.dateFormat = 'YYYY-MM-DD';
            this.cta = document.querySelector("#continue");
            this.onlyRecommendedProviders = onlyRecommendedProviders;
            this.recommendedPhysicians = new RecommendedPhysicians(this);
            document.querySelector("#single-provider-show-other-providers-cta .cta").addEventListener('click', ({target}) => {
                target.closest("#single-provider-show-other-providers-cta").querySelector("span").innerText = "Providers with soonest availability shown";
                target.closest("#single-provider-show-other-providers-cta").classList.add("active");
                this.requestAppointmentTimes();
            });
            if (!document.querySelector(".recommended-providers-sidebar").classList.contains('hidden')) {
                document.querySelector("#single-provider-show-other-providers-cta").querySelector("span").innerText = "Providers with soonest availability shown";
                document.querySelector("#single-provider-show-other-providers-cta").classList.add("active");
            }
            return;
        }

        this.isForTheProviderWidget = document.querySelector('main.physician-page');

        const physicianDetailsBlock = document.querySelector('.physician-info-container');
        this.hasMapModal = false;

        if (physicianDetailsBlock) {
            if (physicianDetailsBlock.classList.contains('has-modal-map')) {
                this.hasMapModal = true;
            }

            // new designs have the map pop out on modal
            // we do not need to initialize a map under the physician details anymore on the select-appointment page
            if (!this.hasMapModal) {
                this.map = InitializeMap(
                    document.querySelector(".map"),
                    [-88.0084115, 41.8293935],
                    (e) => {
                    },
                    '#map.mapboxgl-map'
                );

                this.resetMapMarkers({
                    type: 'FeatureCollection',
                    features: [{
                        type: 'Feature',
                        geometry: {
                            type: 'Point',
                            coordinates: [physicianDetailsBlock.dataset.lng, physicianDetailsBlock.dataset.lat]
                        },
                        properties: {
                            name: '',
                            address: `${physicianDetailsBlock.dataset.addressLine} ${physicianDetailsBlock.dataset.cityStateZipLine}`
                        }
                    }]
                });
            }
        }

        this.container = document.querySelector("section.select-appointment-content");
        if (!this.container) {
            return;
        }

        this.daysInASlice = daysInASlice;

        // if last menstrual date is shown in view, use that as start date
        if (document.querySelector('#last-menstrual-date-for-patient')) {
            let date = document.querySelector('#last-menstrual-date-for-patient').getAttribute('data-start-date');
            this.startDate = date;
        } else {
            this.startDate = startDate;
        }

        this.recommendedPhysicians = new RecommendedPhysicians(this);
        this.findOtherRecommendedPhysicians = true;

        this.dateSelector = new DateSelector(this.renderAvailableTimeForSelectedDate.bind(this), this.onFirstDateChanged.bind(this), this.daysInASlice, this.startDate);
        this.physicianId = physicianId;

        this.availabilityTimes = {};
        this.requestedAvailabilityTimes = [];
        this.numRequests = 0;

        this.availableTimesLocationContainerTemplate = document.querySelector("#available-times-location-container");
        this.availableTimesLocationTimeSelectorTemplate = document.querySelector("#available-times-location-time-selector");
        this.availableTimesLocationBlockSingularTemplate = document.querySelector("#available-times-location-block-singular");

        this.loadingSpinner = this.container.querySelector(".times-container .loading-spinner");
        this.cta = this.container.querySelector("#continue");

        this.dateFormat = 'YYYY-MM-DD';
        this.selectedServiceId = selectedServiceId;
        this.newPatient = newPatient;
        this.establishedPatient = establishedPatient;
        this.followUpVisit = followUpVisit;
        this.veinClinicVisit = veinClinicVisit;
        this.fullBodySkinExamVisit = fullBodySkinExamVisit;

        if (!document.querySelector("#physiciansWithVideoVisitsPerServicePerDate")) {
            // enable auto load only if not within the Duly Now video visit flow
            // Duly Now video visit flow uses the SelectAppointmentSplitView class instead
            this.requestAppointmentTimes();
        }

        this.submitButton();

        if (!this.hasMapModal) {
            this.revealMapOnLoad();
        }
    }

    setupMapModalValues(modal, address) {
        modal.querySelector('span.title').innerText = address.city;
        modal.querySelector('span.address-line').innerText = address['address-line'];
        modal.querySelector('span.city-state-zip').innerText = `${address.city}, ${address.state} ${address.zip}`;
        modal.querySelector('a').href = `https://www.google.com/maps/search/?api=1&query=${address['address-line']} ${address.city}, ${address.state} ${address.zip}`;
    }

    setupModalMapListeners(modal, details, address) {
        let close = modal.querySelector('.close');

        ['keydown', 'click'].forEach(event => {
            details.addEventListener(event, (e) => {
                modal.classList.remove('hidden');
                this.setupMapModalValues(modal, address);
                this.setupModalMapMarkers(address);
            });

            close.addEventListener(event, (e) => {
                modal.classList.add('hidden');
            });
        });
    }

    setupModalMapMarkers(address) {
        this.resetMapMarkers({
            type: 'FeatureCollection',
            features: [{
                type: 'Feature',
                geometry: {
                    type: 'Point',
                    coordinates: [address.lng, address.lat]
                },
                properties: {
                    name: '',
                    address: `${address['address-line']} ${address.city}, ${address.state} ${address.zip}`
                }
            }]
        });
    }

    setupLocationDetails(parentNode, address) {
        let modal = document.querySelector('.map-modal');
        let details = parentNode.querySelector('.modal-location-details');

        if (!this.map) {
            this.setupModalMapInit();
        }

        this.setupMapModalValues(modal, address);
        this.setupModalMapListeners(modal, details, address);
        this.setupModalMapMarkers(address);
    }

    setupModalMapInit() {
        this.map = InitializeMap(
            document.querySelector("#map"),
            [-88.0084115, 41.8293935],
            (e) => {
            },
            '#map.mapboxgl-map'
        );
    }

    resetMapMarkers(markers) {
        if (!this.map) {
            return;
        }
        this.markers = this.markers || [];
        this.markers.forEach(marker => marker.remove());
        if (markers && markers.features.length > 0) {
            this.markers = AddMarkers(markers, this.map);
        }
        this.map.resize();
    }

    revealMapOnLoad() {
        if (document.querySelector('.physician-map-location')) {
            if (document.querySelector('.map-container')) {
                document.querySelector('.map-container').classList.remove('hidden');
            }

            let locations = document.querySelectorAll('.physician-map-location');
            let features = [];

            for (let i = 0; i < locations.length; i++) {
                let marker = {
                    type: 'Feature',
                    geometry: {
                        type: 'Point',
                        coordinates: [locations[i].dataset.lng, locations[i].dataset.lat]
                    },
                    properties: {
                        name: '',
                        address: `${locations[i].dataset.addressLine} ${locations[i].dataset.cityStateZipLine}`
                    }
                }
                features.push(marker);
            }

            let markers = {
                type: 'FeatureCollect',
                features: features
            }

            this.resetMapMarkers(markers);
        }
    }

    onFirstDateChanged() {
        this.requestAppointmentTimes();
    }

    /**
     * Shows the loading spinner, requests new appointment data, and hides the spinner when ready.
     * Once the data is available, sets the date picker witht valid dates (if any) based on the received data.
     */
    async requestAppointmentTimes() {
        if (this.onlyRecommendedProviders) {
            document.querySelector(".map-container").classList.add('hidden');
            document.querySelector(".recommended-providers-sidebar").classList.remove('hidden');
            initLottie();

            const _data = await this.requestAvailabilityTimes(this.physicianId);

            const firstTimeBlockDate = moment().format(this.dateFormat);
            this.recommendedPhysicianData = _data.recommendedPhysicians[firstTimeBlockDate];
            if (!this.recommendedPhysicianData || (Array.isArray(this.recommendedPhysicianData) && !this.recommendedPhysicianData.length)) {
                document.querySelector(".recommended-providers-sidebar .loading-spinner").classList.add('hidden');
                document.querySelector(".recommended-providers-sidebar #no-recommended-providers-error").classList.remove('hidden');
                return;
            }
            this.chosenDate = moment().format(this.dateFormat);
            this.recommendedPhysicians.resetWithData(this.recommendedPhysicianData, this.chosenDate);
            document.querySelector(".recommended-providers-sidebar .recommended-physicians").classList.remove('collapsed');
            document.querySelector(".recommended-providers-sidebar .loading-spinner").classList.add('hidden');

            return;
        }

        this.numRequests++;
        // clear previous times
        this.container.querySelector(".times-container .times").innerHTML = "";
        this.container.querySelector(".error").classList.add('hidden');

        // show loading spinner
        this.loadingSpinner.classList.remove("hidden");
        this.cta.disabled = true;

        // record currently selected start date
        const startDate = moment(this.dateSelector.getFirstDate());

        const _data = await this.requestAvailabilityTimes(this.physicianId);
        const data = _data.currentAppointments;

        if (this.numRequests === 1) {
            const firstTimeBlockDate = moment(this.dateSelector.getFirstDate()).format(this.dateFormat);
            this.recommendedPhysicianData = _data.recommendedPhysicians[firstTimeBlockDate];
            if (this.isForTheProviderWidget && this.recommendedPhysicianData && Object.keys(this.recommendedPhysicianData).length) {
                document.getElementById('showRecommended').classList.remove('hidden');
            }
            this.findOtherRecommendedPhysicians = false;
        }

        // record the date of the latest (current) request to avoid hiding the loading indicator too soon
        this.latestTiemsRequestDate = startDate.format(this.dateFormat);

        // if this current request is the last request fired, hide the loading spinner
        if (this.latestTiemsRequestDate === startDate.format(this.dateFormat)) {
            this.loadingSpinner.classList.add("hidden");

            // update the date picker with new valid dates
            if (data) {
                if (Object.keys(data).length !== 0) {
                    const validDates = Object.keys(data)
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

    appendBlockTitle(key, address, parentNode, isVideo = false) {
        if (isVideo) {
            parentNode.querySelector('span.title').innerText = "Video Visit";
            parentNode.querySelector('span.address-line').classList.add('hidden');
            parentNode.querySelector('span.city-state-zip').classList.add('hidden');
        } else {
            parentNode.querySelector('span.title').innerText = address.city;
            parentNode.querySelector('span.address-line').innerText = address['address-line'];
            parentNode.querySelector('span.city-state-zip').innerText = `${address.city}, ${address.state} ${address.zip}`;
            parentNode.querySelector('span.title').parentNode.dataset.lat = address.lat;
            parentNode.querySelector('span.title').parentNode.dataset.lng = address.lng;
        }
        parentNode.querySelector('span.title').parentNode.dataset.key = key;

        if (this.hasMapModal && !isVideo) {
            this.setupLocationDetails(parentNode, address);
        } else {
            // hide "Location Details" text
            if (parentNode.querySelector('.modal-location-details')) {
                parentNode.querySelector('.modal-location-details').classList.add('hidden');
            }
        }
    }

    appendTimesPerBlock(timeBlock, times, parentNode) {
        const morningSelector = this.availableTimesLocationTimeSelectorTemplate.content.cloneNode(true);
        const morningTimes = this.findTimesPerDayBlock(times, timeBlock);
        morningSelector.querySelector(".label").innerText = `${timeBlock} (${morningTimes.length})`;
        morningSelector.querySelector(".available-times-location-time-selector").dataset.timeBlock = timeBlock;

        if (morningTimes.length == 0) {
            morningSelector.querySelector(".available-times-location-time-selector").classList.add('hidden');
        }

        parentNode.querySelector(".available-times-location-container").appendChild(morningSelector);

        let daysContainer = document.createElement("div");
        daysContainer.classList.add("day-blocks-container");
        let departmentId;
        morningTimes.forEach(time => {
            const singleTimeBlock = this.availableTimesLocationBlockSingularTemplate.content.cloneNode(true).querySelector(".available-times-location-block-singular");
            singleTimeBlock.innerText = moment(time.time).format('h:mm A');
            singleTimeBlock.dataset.timeBlock = timeBlock;
            singleTimeBlock.dataset.visitTypeId = time.visitTypeId;
            singleTimeBlock.dataset.providerId = time.providerId;
            departmentId = time.departmentId;
            ['click', 'keydown'].forEach((event => {
                singleTimeBlock.addEventListener(event, this.singleTimeBlockClickListener.bind(this));
            }));
            daysContainer.appendChild(singleTimeBlock);
        });
        if (departmentId) {
            parentNode.querySelector(".available-times-location-container").dataset.key = departmentId;
        }
        parentNode.querySelector(".available-times-location-container").appendChild(daysContainer);
    }

    singleTimeBlockClickListener(e) {
        if ((e.type === 'keydown' && e.keyCode == 13) || e.type === 'click') {
            e.preventDefault();
            e.stopPropagation();
            const target = e.target;
            this.container.querySelectorAll(".available-times-location-block-singular").forEach(block => {
                if (block == target) {
                    block.classList.add("clicked");
                } else {
                    block.classList.remove("clicked");
                }
            });

            let availableTimesLocationContainer = target.closest(".available-times-location-container");

            // update map if location data available, or hide the map
            const addressLine = availableTimesLocationContainer.querySelector(".address-line").innerText;
            const cityStateZipLine = availableTimesLocationContainer.querySelector(".city-state-zip").innerText;
            const lat = availableTimesLocationContainer.dataset.lat;
            const lng = availableTimesLocationContainer.dataset.lng;
            const appointmentPhysicianId = document.getElementById('appointment_physician_id');

            if (appointmentPhysicianId) {
                appointmentPhysicianId.value = target.dataset.providerId;
            }

            this.updateMap(addressLine, cityStateZipLine, lat, lng);

            this.container.querySelector("[name='appointment_time']").value = moment(moment(this.chosenDate).format(this.dateFormat) + " " + target.innerText, 'YYYY-MM-DD h:mm A').format();
            this.container.querySelector("[name='appointment_department_id']").value = availableTimesLocationContainer.dataset.key;
            this.container.querySelector("[name='appointment_visit_type_id']").value = target.dataset.visitTypeId;
            this.container.querySelector("[name='appointment_selected_recommended_physician_id']").value = null;
            this.cta.disabled = false;

            const physicianDetailsBlock = document.querySelector('.physician-info-container');
            if (physicianDetailsBlock) {
                const selectedTime = document.querySelector('input[name="appointment_time"]').value;
                const appointmentTimeNode = physicianDetailsBlock.querySelector('.appointment-time');
                appointmentTimeNode.classList.remove('hidden');
                appointmentTimeNode.querySelector('.appointment-time .date').innerText = moment(selectedTime).format('ddd, MMM D, YYYY');
                appointmentTimeNode.querySelector('.appointment-time .time').innerText = moment(selectedTime).format('h:mm A');
            }
        };
    }

    updateMap(addressLine, cityStateZipLine, lat = null, lng = null) {
        if (document.querySelector(".map-container")) {
            if (lat && lng) {
                document.querySelector(".map-container .address-line").innerText = addressLine;
                document.querySelector(".map-container .city-state-zip").innerText = cityStateZipLine;
                document.querySelector(".map-container .directions").href = `http://maps.google.com/?q=${addressLine}+${cityStateZipLine}`;
                document.querySelector(".map-container .directions").classList.remove('hidden');

                document.querySelector(".map-container").classList.remove("hidden");
                let markers = {
                    type: 'FeatureCollection',
                    features: [{
                        type: 'Feature',
                        geometry: {
                            type: 'Point',
                            coordinates: [lng, lat]
                        },
                        properties: {
                            name: '',
                            address: `${addressLine}+${cityStateZipLine}`
                        }
                    }]
                };

                this.resetMapMarkers(markers);
            } else {
                document.querySelector(".map-container").classList.add("hidden");
                this.resetMapMarkers(null);
            }
        }
    }

    renderAvailableTimeForSelectedDate(date) {
        this.cta.disabled = true;

        const firstTimeBlockDate = moment(this.dateSelector.getFirstDate()).format(this.dateFormat);
        this.chosenDate = moment(date).format(this.dateFormat);
        const availabilityTimes = this.availabilityTimes[firstTimeBlockDate].currentAppointments[this.chosenDate];

        const timesContainer = this.container.querySelector(".times-container .times");
        timesContainer.innerHTML = "";

        let totalBlocks = 0;

        Object.keys(availabilityTimes).forEach(key => {
            let times = availabilityTimes[key];

            if (times.length == 0) {
                return;
            }
            totalBlocks++;

            const availableTimesLocationContainer = this.availableTimesLocationContainerTemplate.content.cloneNode(true);
            this.appendBlockTitle(times[0].Department.ID, times[0].Department.address, availableTimesLocationContainer, key == "video");

            if (times.length > 9) {
                // if more than 9 time slots are available, split the times into three groups: morning, afternoon and evening
                this.appendTimesPerBlock("morning", times, availableTimesLocationContainer);
                this.appendTimesPerBlock("afternoon", times, availableTimesLocationContainer);
                this.appendTimesPerBlock("evening", times, availableTimesLocationContainer);

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
                            };
                        });
                    }));
                });
            } else {
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
            }

            timesContainer.appendChild(availableTimesLocationContainer);
        });

        let futureTimes = this.availabilityTimes[firstTimeBlockDate].futureAppointments[this.chosenDate];

        Object.keys(futureTimes).forEach(key => {
            let times = futureTimes[key];

            if (times.length == 0) {
                return;
            }
            totalBlocks++;

            const availableTimesLocationContainer = this.availableTimesLocationContainerTemplate.content.cloneNode(true);
            this.appendBlockTitle(times.Department.ID, times.Department.address, availableTimesLocationContainer, key == "video");

            let nextAvailable = document.querySelector("#next-available-label-template").content.cloneNode(true);
            let date = moment(times.Time.date);
            nextAvailable.querySelector(".next-available-label-date").innerText = `${date.format('ddd, MMMM D')} at ${date.format('h:mm A')}`;

            // new designs show a next available container that should also be clickable when displayed
            if (nextAvailable.querySelector('.next-available-date-container')) {
                ['click', 'keydown'].forEach(event => {
                    nextAvailable.querySelector('.next-available-date-container').addEventListener(event, (e) => {
                        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
                            this.dateSelector.skipToDate(date.toDate());
                        }
                    });
                })
            }

            nextAvailable.querySelector(".next-available-date-row").addEventListener('click', _ => {
                this.dateSelector.skipToDate(date.toDate());
            });

            availableTimesLocationContainer.querySelector(".available-times-location-container").appendChild(nextAvailable);

            timesContainer.appendChild(availableTimesLocationContainer);
        });

        if (totalBlocks == 0) {
            this.container.querySelector(".error").classList.remove('hidden');
        } else {
            this.container.querySelector(".error").classList.add('hidden');
        }

        this.reOrderLocationBlocksAlphabetically();

        this.recommendedPhysicians.resetWithData(this.recommendedPhysicianData, this.chosenDate);
    }

    reOrderLocationBlocksAlphabetically() {
        let locationBlocksListContainer = document.querySelector('.times-container .times');
        let locationBlocks = locationBlocksListContainer.querySelectorAll('.available-times-location-container');
        let orderedLocationBlocks = [].slice.call(locationBlocks).sort(function (a, b) {
            return a.querySelector(".title").innerText > b.querySelector(".title").innerText ? 1 : -1;
        });

        orderedLocationBlocks.forEach(function (p) {
            locationBlocksListContainer.appendChild(p);
        });
    }

    submitButton() {
        const form = document.querySelector('#select-appointment-form');

        form.addEventListener('submit', (e) => {
            // prevent double clicking of button resulting in booking a time that is already submitted
            this.cta.disabled = true;
            this.cta.querySelector('.fa-spin').classList.remove('hidden');
            this.cta.querySelector('div.cta').style.display = 'none';
        })
    }

    findTimesPerDayBlock(times, block) {
        let retval = [];
        times.forEach(currentDateTime => {
            const visitType = currentDateTime.VisitType.ID;
            const providerId = currentDateTime.Provider.ID;
            const departmentId = currentDateTime.Department.ID;
            currentDateTime = currentDateTime.Time;
            currentDateTime = new Date(currentDateTime);
            let minTime = (new Date(currentDateTime));
            let maxTime = (new Date(currentDateTime));
            if (block == "morning") {
                minTime.setHours(0, 0, 0, 0);
                maxTime.setHours(11, 59, 0, 0);
            } else if (block == "afternoon") {
                minTime.setHours(12, 0, 0, 0);
                maxTime.setHours(16, 59, 0, 0);
            } else if (block == "evening") {
                minTime.setHours(17, 0, 0, 0);
                maxTime.setHours(24, 0, 0, 0);
            }

            if (currentDateTime.getTime() >= minTime.getTime() && currentDateTime.getTime() <= maxTime.getTime()) {
                retval.push({
                    time: currentDateTime,
                    visitTypeId: visitType,
                    providerId: providerId,
                    departmentId: departmentId
                });
            }
        });
        return retval;
    }

    async requestAvailabilityTimes(physicianId) {
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
        }

        // this.availabilityTimes[startDate] can be 1) undefined, 2) a Promise, or 3) an array of data
        // 1) request for given start date was not initiatied before; call fetch and store the promise
        // 2) request for given start date was already initiated, but did not yet finish; re-use the promise without starting a new one
        // 3) request for given start date already finished, we received the data, so we can just return it
        this.availabilityTimes[startDate] = fetch(url, {
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


        this.availabilityTimes[startDate] = await this.availabilityTimes[startDate];
        return this.availabilityTimes[startDate];
    }
}