'use strict';
import { enableFocusTrap } from '../utils';
import { InitializeMap, AddMarkers } from './../mapbox';

export class ShareAppointment {
    constructor() {
        this.container = document.querySelector(".patient-appointment-confirmation-page");
        if (!this.container) {
            return;
        }

        this.isVideoVisitFlow = this.container.classList.contains('video-visit');

        this.addEventListeners();
        this.validateShareForm();
        this.shareDetails = 'date-time-only';
        this.visitType = 'in-person';
        this.addToCalendarClicked = false;

        this.map = InitializeMap(
            document.querySelector(".map"),
            [-88.0084115, 41.8293935],
            (e) => {
            },
            '#map.mapboxgl-map'
        );

        if (document.querySelector(".map-container")) {
            document.querySelector(".map-container").classList.remove("hidden");
        }

        if (document.querySelector('.populate-marker')) {
            this.generateLocations();
        }
    }

    generateLocations() {
        let location = document.querySelector('.populate-marker');
        let lat = location.getAttribute('data-latitude');
        let lng = location.getAttribute('data-longitude');
        let address = location.getAttribute('data-address');

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
                    address: address
                }
            }]
        };

        this.resetMapMarkers(markers);
    }

    resetMapMarkers(markers) {
        this.markers = this.markers || [];
        this.markers.forEach(marker => marker.remove());
        if (markers && markers.features.length > 0) {
            this.markers = AddMarkers(markers, this.map);
        }
        this.map.resize();
    }

    addEventListeners() {
        const events = ['click', 'keydown'];
        const shareModalBtn = document.querySelector('#share-appointment-btn');
        const shareModal = document.querySelector('.share-appointment-modal');
        const shareFormModal = document.querySelector('.share-appointment-form-modal');
        const successModal = document.querySelector('.email-success-modal');
        const editModal = document.querySelector('.edit-appointment-modal');
        const closeBtns = document.querySelectorAll('.close');
        const includeDateTimeOnlyBtn = document.querySelector('#date-and-time-only');
        const includeDetailsBtn = document.querySelector('#include-details-btn');
        const addToCalendarBtn = document.querySelector('#add-to-calendar-btn');
        const editBtn = document.querySelector('.edit');
        const shareAppointmentDetails = document.querySelector('#share-appointment-form-send');

        events.forEach(event => {
            shareModalBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    if (this.isVideoVisitFlow) {
                        if (!navigator.share) {
                            shareFormModal.classList.remove('hidden');
                            return;
                        } else {
                            navigator.share({
                                text: 'Video Visit',
                                url: 'https://www.dulyhealthandcare.com/',
                            })
                        }
                    } else {
                        shareModal.classList.remove('hidden');
                        // focus on modal while open
                        enableFocusTrap('.share-appointment-modal button, .share-appointment-modal span.material-icons.close');
                    }

                    // analytics
                    const event = new Event('shareAppointmentStart');
                    window.dispatchEvent(event);
                }
            })

            addToCalendarBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    this.addToCalendarClicked = true;

                    if (this.isVideoVisitFlow) {
                        this.addToCalendar();
                    } else {
                        shareModal.classList.remove('hidden');
                        // focus on modal while open
                        enableFocusTrap('.share-appointment-modal button, .share-appointment-modal span.material-icons.close');
                    }


                    // analytics
                    const event = new Event('addToCalendarStart');
                    window.dispatchEvent(event);
                }
            });

            includeDateTimeOnlyBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    this.shareDetails = 'date-time-only';
                    // focus on modal while open
                    enableFocusTrap('.share-appointment-form-modal button, .share-appointment-form-modal span.material-icons.close');

                    // create ICS file with updated details instead of showing the share appointment details form
                    if (this.addToCalendarClicked) {
                        this.addToCalendar();
                    } else if (navigator.share) {
                        // on mobile, form field should not show. only native share option
                        let shareAppointmentModal = document.querySelector('.share-appointment-modal');
                        let time = document.querySelector('.scheduled-appointment-time').innerHTML.trim();
                        let date = document.querySelector('.scheduled-appointment-date').innerHTML.trim();

                        let textField = '';

                        if (document.querySelector('.video-visit-only')) {
                            textField = `I have a video appointment with Duly Health and Care on ${date} and ${time}.`
                        } else if (document.querySelector('.physician-visit-only')) {
                            textField = `I have an appointment with Duly Health and Care on ${date} and ${time}.`
                        }

                        navigator.share({
                            text: textField,
                            url: 'https://www.dulyhealthandcare.com/',
                        })
                            .then(() => {
                                console.log('Successful share');
                                shareAppointmentModal.classList.add('hidden');

                                // analytics
                                const event = new Event('shareAppointmentComplete');
                                window.dispatchEvent(event);
                            })
                            .catch((error) =>
                                console.log('Error sharing', error)
                            );
                    } else {
                        shareModal.classList.add('hidden');
                        shareFormModal.classList.remove('hidden');
                    }
                }
            });

            includeDetailsBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    this.shareDetails = 'all-details';
                    // focus on modal while open
                    enableFocusTrap('.share-appointment-form-modal form button, .share-appointment-form-modal form input, .share-appointment-form-modal span.material-icons.close');
                    // create ICS file with updated details instead of showing the share appointment details form
                    if (this.addToCalendarClicked) {
                        this.addToCalendar();
                    } else if (navigator.share) {
                        // on mobile, form field should not show. only native share option
                        let shareAppointmentModal = document.querySelector('.share-appointment-modal');
                        let time = document.querySelector('.scheduled-appointment-time').innerHTML.trim();
                        let date = document.querySelector('.scheduled-appointment-date').innerHTML.trim();
                        let textField = '';
                        if (document.querySelector('.video-visit-share-field')) {
                            textField = `I have a video appointment with Duly Health and Care on ${date} and ${time}.`;
                        } else if (document.querySelector('.location-share-field')) {
                            let physician = document.querySelector('.physician-name').innerHTML.trim();
                            let location = document.querySelector('.location-share-field').innerHTML.replace('Location: ', '').trim();
                            textField = `I have an appointment with ${physician} at Duly Health and Care on ${date} and ${time} at ${location}.`;
                        }

                        navigator.share({
                            text: textField,
                            url: 'https://www.dulyhealthandcare.com/',
                        })
                            .then(() => {
                                console.log('Successful share');
                                shareAppointmentModal.classList.add('hidden');

                                // analytics
                                const event = new Event('shareAppointmentComplete');
                                window.dispatchEvent(event);
                            })
                            .catch((error) =>
                                console.log('Error sharing', error)
                            );
                    } else {
                        shareModal.classList.add('hidden');
                        shareFormModal.classList.remove('hidden');
                    }
                }
            });

            closeBtns.forEach(el => {
                el.addEventListener(event, (e) => {
                    if (this.keydownOrClick(event, e)) {
                        this.addToCalendarClicked = false;
                        shareModal.classList.add('hidden');
                        shareFormModal.classList.add('hidden');
                        editModal.classList.add('hidden');
                        successModal.classList.add('hidden');
                    }
                })
            })

            editBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    editModal.classList.remove('hidden');
                    // focus on modal while open
                    enableFocusTrap('.edit-appointment-modal button, .edit-appointment-modal span.material-icons.close');
                };
            });

            shareAppointmentDetails.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    e.preventDefault();
                    e.stopPropagation();
                    this.queueEmail();
                }
            })
        })
    }

    validateShareForm() {
        const shareAppointmentForm = document.getElementById('share-appointment-form');
        const submitButton = document.querySelector('#share-appointment-form-send');
        shareAppointmentForm.addEventListener('input', () => {
            submitButton.disabled = !shareAppointmentForm.checkValidity();
        })
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }

    resetFormAnimations(modal) {
        const fields = modal.querySelectorAll('.mdc-text-field');

        for (let i = 0; i < fields.length; i++) {
            let label = fields[i].querySelector('span.mdc-floating-label');

            fields[i].classList.remove('mdc-ripple-upgraded');
            fields[i].classList.remove('mdc-text-field-success');
            label.classList.remove('mdc-floating-label--float-above');
        }

        const shareFormSubmitButton = document.querySelector('#share-appointment-form-send');
        if (shareFormSubmitButton) {
            shareFormSubmitButton.disabled = true;
        }
    }

    queueEmail() {
        clearTimeout(this._timeout);
        this._timeout = setTimeout((e) => {
            this.sendEmail();
        }, 500);
    };

    addToCalendar() {
        let shareAppointmentModal = document.querySelector('.share-appointment-modal');
        let date = document.querySelector('.scheduled-appointment-date').innerHTML;
        let time = document.querySelector('.scheduled-appointment-time').innerHTML;
        let timeString = date + "\t" + time;
        let addressOne = document.querySelector('.address-line').innerHTML;
        let zipcode = document.querySelector('.city-state-zip').innerHTML;
        let location = addressOne + ", " + zipcode;
        let physicianDescription = document.querySelector('.physician-share-field') ? document.querySelector('.physician-share-field').innerHTML : '';
        let limitedPhysicianDescription = document.querySelector('.physician-visit-only').innerHTML;
        let myChartLink = document.querySelector('.my-chart-link').href;

        let properties = {};
        const searchParams = new URLSearchParams();

        if (this.shareDetails == 'all-details') {
            properties = {
                'description': physicianDescription,
                'startTime': timeString,
                'location': location,
            }
        } else {
            properties = {
                'description': limitedPhysicianDescription,
                'startTime': timeString,
            }
        }

        // video visits don't have a location
        if (document.querySelector('.video-visit-only')) {
            let limitedVideoDescription = document.querySelector('.video-visit-only').innerHTML;
            let videoDescription = document.querySelector('.video-visit-share-field').innerHTML;

            if (this.shareDetails == 'all-details') {
                properties['description'] = videoDescription;
            } else {
                properties['description'] = limitedVideoDescription;
            }

            properties['location'] = myChartLink;
        }

        if (this.isVideoVisitFlow) {
            properties['description'] = 'Video Visit';
            properties['location'] = myChartLink;
        }

        Object.keys(properties).forEach(key => searchParams.append(key, properties[key]));

        let url = '/schedule/share/calendar?' + searchParams.toString();

        fetch(url, {
            method: 'GET',
            mode: 'same-origin',
            cache: 'no-cache',
            follow: true,
            async: true,
            credentials: 'same-origin',
            headers: {
                'Accept': 'text/html',
                'x-isAjax': true
            }
        }).then(async (response) => {
            const text = await response.text();
            return response.ok ? text : Promise.reject(response);
        }).then(async (data) => {
            // analytics
            const event = new Event('addToCalendarComplete');
            window.dispatchEvent(event);

            this.addToCalendarClicked = false;
            shareAppointmentModal.classList.add('hidden');

            // this is needed otherwise native iOS will block the download
            let blob = new Blob([data], { type: 'text/calendar' })
            let link = document.createElement('a')
            link.href = window.URL.createObjectURL(blob)
            link.download = 'Appointment Reminder.ics'
            link.click()
        }).catch((response) => {
            if (document.querySelector('.calendar-error')) {
                let error = document.querySelector('.calendar-error');
                error.classList.remove('hidden');
                error.textContent = "Sorry, we weren’t able to proceed. Please try again."
            }
        });
    }

    sendEmail() {
        let physicianName = document.querySelector('.physician-name').innerHTML;
        let time = `${document.querySelector('.scheduled-appointment-date').innerHTML} at ${document.querySelector('.scheduled-appointment-time').innerHTML}`;
        let addressOne = document.querySelector('.address-line').innerHTML;
        let zipcode = document.querySelector('.city-state-zip').innerHTML;
        let location = addressOne + ", " + zipcode;
        let sendToEmail = document.querySelector('#send-to-email-address');
        let senderName = document.querySelector('#sender-name');
        let senderEmail = document.querySelector('#sender-email');
        const searchParams = new URLSearchParams();
        const successModal = document.querySelector('.email-success-modal');
        const shareFormModal = document.querySelector('.share-appointment-form-modal');
        const form = shareFormModal.querySelector('form');
        let myChartLink = document.querySelector('.my-chart-link').href;

        if (document.querySelector('.video-visit-only') || this.isVideoVisitFlow) {
            this.visitType = 'video';
        }

        let search = {
            'share_details': this.shareDetails,
            'physician': physicianName,
            'time': time,
            'location': location,
            'send_to': sendToEmail.value,
            'sender_name': senderName.value,
            'sender_email': senderEmail.value,
            'visit_type': this.visitType
        };

        if (this.isVideoVisitFlow) {
            search['location'] = myChartLink;
        }

        Object.keys(search).forEach(key => searchParams.append(key, search[key]));

        let url = '/schedule/share/email?' + searchParams.toString();

        fetch(url, {
            method: 'GET',
            mode: 'same-origin',
            cache: 'no-cache',
            follow: true,
            async: true,
            credentials: 'same-origin',
            headers: {
                'Accept': 'text/html',
                'x-isAjax': true
            }
        }).then(async (response) => {
            const json = await response.json();
            return response.ok ? json : Promise.reject(response);
        }).then(async (data) => {
            successModal.classList.remove('hidden');
            shareFormModal.classList.add('hidden');

            enableFocusTrap('.email-success-modal span.material-icons.close');

            form.reset();
            this.resetFormAnimations(shareFormModal);

            // analytics
            const event = new Event('shareAppointmentComplete');
            window.dispatchEvent(event);
        }).catch((response) => {
            if (document.querySelector('.email-error')) {
                let error = document.querySelector('.email-error');
                error.classList.remove('hidden');
                error.textContent = "Sorry, we weren’t able to proceed. Please try again."
            }
        });
    }
}