'use strict';

import { InitializeMap, AddMarkers } from './mapbox';
import { ClosestCareLocation } from './closest-care-location';
import { MDCSelect } from '@material/select';

export class HomePage {
    constructor() {
        this.container = document.querySelector("section.homepage-container");
        if (!this.container) {
            return;
        }

        /* const iamLookingSelect = new MDCSelect(document.querySelector("#iam-looking"));
        const iamTypeSelect = new MDCSelect(document.querySelector("#iam-type"));

        const foundation = iamLookingSelect.getDefaultFoundation();
        const adapter = foundation.adapter_;

        iamTypeSelect.listen("MDCSelect:change", () => {
            const li = document.querySelectorAll("#iam-looking .mdc-list li");
            let selectedElementIndex = -1;
            let firstElementIndex = -1;

            let currentElement = adapter.getSelectedMenuItem();

            li.forEach((el, index) => {
                el.classList.remove("mdc-list-item--selected");
                if (el.getAttribute('data-parent-id') == iamTypeSelect.value) {
                    el.classList.remove("hidden");

                    if (firstElementIndex == -1) {
                        firstElementIndex = index;
                    }

                    if (currentElement.textContent === el.textContent) {
                        selectedElementIndex = index;
                    }
                } else {
                    el.classList.add("hidden");
                }
            });

            const desiredElementIndex = selectedElementIndex == -1 ? firstElementIndex : selectedElementIndex;

            foundation.setSelectedIndex(desiredElementIndex);
            adapter.focusMenuItemAtIndex(desiredElementIndex);

            currentElement = adapter.getSelectedMenuItem();
            currentElement.click();
        });

        const btn = document.querySelector("#iam-button-action");
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            const el = adapter.getSelectedMenuItem();
            const link = el.getAttribute("data-url");
            const newWindow = el.getAttribute("data-newwindow");

            if (newWindow == "") {
                window.location = link;
            } else {
                window.open(link);
            }
        }); */

        this.ajaxCalled = false;

        this.map = InitializeMap(
            document.querySelector("#map"),
            [
                -88.014710,
                41.831440
            ],
            this.onGeolocateCallback.bind(this),
            ".information-container .information"
        );

        this.gatherMarkers();
    }

    onGeolocateCallback(geoCoords) {
        if (this.ajaxCalled == false) {
            this.ajaxCalled = true;

            if (geoCoords) {
                new ClosestCareLocation(geoCoords, this.map);
            }
        }
    }

    /**
     * Update the map with dynamic markers showing all immediate care and express care locations
     */
    async gatherMarkers() {
        const url = '/locations?care[]=Immediate Care&care[]=Express Care';

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
                return Promise.reject(response)
            }
        }).then(async (data) => {
            document.querySelector('.dynamic-care-locations').innerHTML = data;
            const locations = document.querySelectorAll('.single-location');
            let locationMarkers = [];

            for (let i = 0; i < locations.length; i++) {
                let location = locations[i];
                let lat = location.getAttribute('data-location-lat');
                let lng = location.getAttribute('data-location-lng');
                let address = location.querySelector('.address').innerHTML;
                let cityLabel = location.querySelector('.city').innerHTML;

                // Only want to display the city, not Chicago, IL 60657;
                let city = cityLabel.split(',')[0];

                let marker = {
                    type: 'Feature',
                    geometry: {
                        type: 'Point',
                        coordinates: [lng, lat]
                    },
                    properties: {
                        name: city,
                        address: address
                    }
                }

                locationMarkers.push(marker);
            }

            if (locationMarkers.length) {
                let markers = {
                    type: 'FeatureCollection',
                    features: locationMarkers
                }

                AddMarkers(markers, this.map, false);
                this.map.resize();
            }
        }).catch((response) => {
            if (document.querySelector('.fetch-error')) {
                let error = document.querySelector('.fetch-error');
                error.classList.remove('hidden');
                error.textContent = "Sorry, we weren’t able to proceed. Please try again."
            }
        });
    }
}