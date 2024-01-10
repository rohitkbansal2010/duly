import { InitializeMap, AddMarkers } from './mapbox';
import { SpecialtySelector } from './specialty-selector';

export class LocationPage {
    constructor() {
        this.container = document.querySelector("section.single-location-entry");
        if (!this.container) {
            // abort if single location entry selector element not found
            return;
        }

        this.specialtySelector = new SpecialtySelector(this.onSpecialtySelectedCallback, this.onSuccessfullAjaxCallback);
        this.latitude = parseFloat(this.container.querySelector(".location-details-container .address").dataset.latitude);
        this.longitude = parseFloat(this.container.querySelector(".location-details-container .address").dataset.longitude);

        this.checkServiceParams();

        this.map = InitializeMap(
            document.querySelector("#map-container"),
            [
                this.longitude,
                this.latitude
            ],
            null,
            null
        );

        this.locationPermissions(_ => {
            this.fireAjaxEvent(this.latitude, this.longitude);
        });

        AddMarkers(this.getLocations(), this.map);
    }

    checkServiceParams() {
        const urlParams = new URLSearchParams(window.location.search);
        const service = urlParams.get('referrer');

        if (service) {
            const serviceSelect = document.querySelector(`.main-name[data-attr-services='${service}']`);
            const subspecialty = document.querySelector(`.subspecialty[data-attr-services='${service}']`);
            
            if (serviceSelect) {
                serviceSelect.click();
            }

            if (subspecialty) {
                subspecialty.click();
            }
        }
    }

    locationPermissions(callback = null) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(position => {
                this.latitude = position.coords.latitude;
                this.longitude = position.coords.longitude;
                if (callback) {
                    callback();
                }
            });
        }
    }

    onSuccessfullAjaxCallback() {
        if (document.querySelector('.choose-patient-prompt')) {     
            const scheduleBtn = document.querySelectorAll('.servicesForm');
            const modal = document.querySelector('.choose-patient-prompt');

            scheduleBtn.forEach(btn => btn.addEventListener('submit', (e)=> {
                e.preventDefault();
                modal.classList.remove('hidden');
            }));
        }
    }

    onSpecialtySelectedCallback(selectedSpecialty) {
        const suiteServices = document.querySelectorAll(".service-offered");
        const dsc = document.querySelector(".location-details-container .description");
        const walkin = document.querySelector(".location-details-container .walk-in-services");

        if (selectedSpecialty.id && selectedSpecialty.id == 'show-all-services') {
            suiteServices.forEach((el) => {
                el.classList.add("hidden");
            });

            dsc.classList.remove("hidden");

            // not all locations will have walk in services so this container may not exist
            if (walkin) {
                walkin.classList.remove('hidden');
            }
        } else {
            dsc.classList.add("hidden");
            if (walkin) {
                walkin.classList.add('hidden');
            }

            let selectedIds = selectedSpecialty.getAttribute('data-attr-services');
            let visibleNodes = document.querySelectorAll(".service-offered[data-attr-service-id~=\"" + selectedIds + "\"");

            suiteServices.forEach((el) => {
                el.classList.add("hidden");
            });

            visibleNodes.forEach((el) => {
                el.classList.remove("hidden");
            });
        }
    }

    getLocations() {
        const address = document.querySelector('.address');
        const addressName = address.querySelector('.street-address').innerHTML;
        const lat = address.dataset.latitude;
        const lng = address.dataset.longitude;

        return {
            type: 'FeatureCollection',
            features: [{
                type: 'Feature',
                geometry: {
                    type: 'Point',
                    coordinates: [lng, lat]
                },
                properties: {
                    name: addressName,
                    address: ''
                }
            }]
        };
    }

     /**
     * Update the distance
     */
    async fireAjaxEvent(latitude, longitude) {
        const locationId = document.querySelector('.location-details-container').id;
        const url = '/locations/distance?latitude=' + latitude + "&longitude=" + longitude + "&locationId=" + locationId;        

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
            if (response.ok) {
                return text;
            } else {
                return Promise.reject(text)
            }
        }).then(async (data) => {
            document.querySelector('.distance-to-user').innerHTML = data;
            document.querySelector('.distance').classList.remove('hidden');
        }).catch((response) => {});
    }
}