'use strict';

import { InitializeMap, AddMarkers } from './mapbox';
import { Search } from './search';
import { breakpoints } from './variables';
import { FilterHelper } from './filters/filter-helper';
import { Analytics } from "./analytics.js";

export class LocationsList {
    constructor() {
        const locationsListPage = document.querySelector('.locations-list-page');
        const selectSchedulingLocationsPage = document.querySelector('.select-scheduling-locations-page');
        if (!locationsListPage && !selectSchedulingLocationsPage) {
            return;
        }
    
        this.analytics = new Analytics(false);
        
        this.locationPermissions(this.triggerNearMeSort.bind(this));

        window.addEventListener('resize', _ => {
            this.resetMapMarkers();
        });

        this.setupFilters();
        this.setupNearMeIcon();
        this.toggleServiceDropdown();
        this.toggleCareLabels();

        this.filters = {
            cities: [],
            careTypes: [],
            services: [],
        };

        this.filterHelper = new FilterHelper(_ => { });
        this.disableFiltersSync = false;

        // enable only mobile or desktop filters depending on current viewport size
        this.isMobile = window.innerWidth < parseInt(breakpoints["bs-large"]) ? true : false;

        this.selector = this.isMobile ? '.event-filter-modal' : '.location-search-container';

        document.querySelector(".filter-btn-mobile").addEventListener("click", _ => {
            document.querySelector(".event-filter-modal").classList.remove("hidden");
        });

        document.querySelectorAll(".event-filter-modal .material-icons.close, .view-results").forEach(elem => elem.addEventListener("click", _ => {
            document.querySelector(".event-filter-modal").classList.add("hidden");
        }));

        this.search_service_attribute = new Search(
            "#search-location-services",
            ".search-content form.location-service",
            location.origin + "/locations/auto-suggestions",
            "search_service_attribute",
            "#html-content-template-recent-searches",
            "#html-content-template-auto-complete"
        );

        this.search_location = new Search(
            "#search-locations",
            ".search-content form.physical-location",
            location.origin + "/locations/auto-suggestions",
            "search_location",
            "#html-content-template-recent-searches",
            "#html-content-template-auto-complete"
        );

        document.querySelector(".location-search-btn").addEventListener("click", this.submitSearch.bind(this));

        document.querySelectorAll(".location-search-container form").forEach(form => form.addEventListener("submit", e => {
            e.preventDefault();
            e.stopPropagation();
            this.submitSearch();
            return false;
        }), true);

        this.restoreFiltersFromURI();

        window.addEventListener('popstate', (e) => {
            this.restoreFiltersFromURI(e.state);
        });
    }

    restoreFiltersFromURI(state = false) {
        this.href = new URL(location);

        if (state) {
            // set URL params from previous state
            let searchParams = new URLSearchParams(state);
            for (const [key, value] of searchParams) {
                if (searchParams.get(key)) {
                    this.href.searchParams.append(key, value)
                } else {
                    this.href.searchParams.set(key, value)
                }
            }
        } 

        // UI still shows previous checkboxes as checked
        this.filterHelper.resetCheckboxes();

        // restore page filter
        this.page = this.href.searchParams.get('page');

        // restore per-page filter
        this.perPage = this.href.searchParams.get('per-page');

        // restore search_physician_attribute filter
        this.filters.search_service_attribute = this.href.searchParams.get('search_service_attribute');

        if (!this.href.searchParams.get('search_service_attribute')) {
            this.filterHelper.resetSearchField('#search-location-services');
        } else {
            let searchField = document.querySelector('#search-location-services');

            const parent= searchField.parentNode;
            const label = parent.querySelector('label');
    
            parent.classList.add('mdc-text-field-success');
            label.classList.add('mdc-floating-label--float-above');
            searchField.value = this.href.searchParams.get('search_service_attribute');
        }

        // restore address filter
        this.filters.search_location = this.href.searchParams.get('search_location');
        
        if (!this.href.searchParams.get('search_location')) {
            this.filterHelper.resetSearchField('#search-locations');
        }

        // restore laboratory services filter
        this.filters.laboratory_services = this.href.searchParams.get('laboratory_services');
        const laboratoryServicesContainers = document.querySelectorAll('.laboratory-services-container');

        if (this.filters.laboratory_services) {
            document.querySelectorAll('.filter-entry.laboratory-services-container .label-text').forEach(label => label.click());
        } else {
            laboratoryServicesContainers.forEach(container => {
                container.classList.remove('active');
            });
        }

        // restore open_now filter
        this.filters.open_now = this.href.searchParams.get('open_now');
        const openNowContainers = document.querySelectorAll('.open-now-container');

        if (this.filters.open_now) {
            document.querySelectorAll('.filter-entry.open-now-container .label-text').forEach(label => label.click());
        } else {
            openNowContainers.forEach(container => {
                container.classList.remove('active');
            });
        }

        // restore service filters
        this.filterHelper.restoreListFilterHelper('.location-search-container', '.service-checkbox', 'service[]', this.href.searchParams);

        // restore care filters
        this.filterHelper.restoreListFilterHelper('.location-search-container', '.care-checkbox', 'care[]', this.href.searchParams);

        // restore city filters
        this.filterHelper.restoreListFilterHelper('.location-search-container', '.city-checkbox', 'city[]', this.href.searchParams);

        // restore distance filter
        this.filterHelper.restoreRadioFilterHelper('.location-search-container' ,'.distance-container', 'distance-id', 'distance', this.href.searchParams)

        // restore latitude filter
        this.latitude = this.href.searchParams.get('latitude');

        // restore longitude filter
        this.longitude = this.href.searchParams.get('longitude');

        this.queueUpdateHrefFilters(false);
    }

    queueUpdateHrefFilters(pushState = true) {
        if (!this.disableFiltersSync) {
            clearTimeout(this._timeout);
            this._timeout = setTimeout((e) => {
                this.updateHrefFilters(pushState);
            }, 500);
        }
    };

    triggerNearMeSort() {
        const inputField = document.querySelector('#search-locations');
        const text = document.querySelector('.near-me-clicked').innerHTML;
        const parent = inputField.parentNode;
        const label = parent.querySelector('span.mdc-floating-label');

        // if "near me" is clicked, clear any previous sorts from specific location
        document.querySelector('#search-locations').value = "";
        this.queueUpdateHrefFilters();
        document.querySelector(".location-search-btn").click();

        // update placeholder text
        parent.classList.add('mdc-text-field-success');
        label.classList.add('mdc-floating-label--float-above');
        inputField.value = text;
    }

    setupNearMeIcon() {
        ['click', 'keydown'].forEach(event => {
            document.querySelector(".location-search-container span.material-icons.near_me").addEventListener(event, e => {
                if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
                    this.locationPermissions(this.triggerNearMeSort.bind(this));
                }
            });
        });
    }

    submitSearch() {
        if (document.querySelector("#search-location-services")) {
            let search_service_attribute_value = document.querySelector("#search-location-services").value;
            if (this.filters.search_service_attribute || search_service_attribute_value.length > 0) {
                this.search_service_attribute.storeRecentSearch(search_service_attribute_value);
                this.filters.search_service_attribute = search_service_attribute_value;
            }
        }

        let search_location_value = document.querySelector("#search-locations").value;
        if (this.filters.search_location || search_location_value.length > 0) {
            this.search_location.storeRecentSearch(search_location_value);
            this.filters.search_location = search_location_value;
        }

        this.queueUpdateHrefFilters();
    }

    clearAllFilters() {
        const container = document.querySelector('.event-filter-modal');
        const checkboxes = container.querySelectorAll('.checkbox-container input');
        const counts = container.querySelectorAll('.count');
        const radioBtns = container.querySelectorAll('.radio input');
        const laboratoryServiceContainers = document.querySelectorAll('.laboratory-services-container');
        const openNowContainers = document.querySelectorAll('.open-now-container');

        for (let i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = false;
        }

        for (let x = 0; x < counts.length; x++) {
            counts[x].innerHTML = " ";
        }

        for (let y = 0; y < radioBtns.length; y++) {
            if (radioBtns[y].id.includes('any')) {
                radioBtns[y].click();
                radioBtns[y].closest('.filter-entry').classList.remove('active');
                radioBtns[y].closest('.filter-entry').classList.remove('selected');
            }
        }

        for (let l = 0; l < laboratoryServiceContainers.length; l++) {
            laboratoryServiceContainers[l].classList.remove('active');
        }

        for (let z = 0; z < openNowContainers.length; z++) {
            openNowContainers[z].classList.remove('active');
        }

        this.filters.laboratory_services = null;
        this.filters.open_now = null;
        this.queueUpdateHrefFilters();
    }

    locationPermissions(callback = null) {
        const nearMeElement = document.querySelector('.location-search-container .material-icons.near_me');
        const loadingSpinnerElement = document.querySelector('.location-search-container .loading-spinner');

        if (navigator.geolocation) {
            nearMeElement.classList.add('hidden');
            loadingSpinnerElement.classList.remove('hidden');
            navigator.geolocation.getCurrentPosition(position => {
                this.latitude = position.coords.latitude;
                this.longitude = position.coords.longitude;
                nearMeElement.classList.remove('hidden');
                loadingSpinnerElement.classList.add('hidden');

                if (callback) {
                    callback();
                }

                this.showPosition(position);
            }, _ => {
                this.determineDistanceFilterLogic();
                this.showSortedByTitleLocations();
            });
        } else {
            console.log("Geolocation is not supported by this browser.");
            this.showSortedByTitleLocations();
        }
    }

    determineDistanceFilterLogic() {
        if (document.querySelector('.distance-container')) {
            // separate containers for desktop and mobile
            let containers = document.querySelectorAll('.distance-container');

            if (!this.filters.search_location) {
                containers.forEach(container => {
                    container.classList.add('disabled');
                })
            } else {
                containers.forEach(container => {
                    container.classList.remove('disabled');
                })
            }
        }
    }

    showDistances() {
        const distances = document.querySelectorAll('.distance');

        for(let i = 0; i < distances.length; i++) {
            distances[i].classList.add('show');
        }
    }
    
    showPosition(position) {
        this.latitude = position.coords.latitude;
        this.longitude = position.coords.longitude;
        this.calculateDistance();
    }

    showError(error) {
        this.showSortedByTitleLocations();
    }

    showSortedByTitleLocations() {
        let container = document.querySelector('.left-container .container');
        container.classList.remove('hidden');
        document.querySelector('.loading-spinner').classList.add('hidden');
    }

    toggleMap(mapBtn) {
        const map = document.querySelector('.mapboxgl-map');
        const table = document.querySelector('.left-container');

        if (map.classList.contains('show-map')) {
            map.classList.remove('show-map');
            table.classList.remove('hide-table');
            mapBtn.innerHTML = "Map";
        } else {
            map.classList.add('show-map');
            table.classList.add('hide-table');
            mapBtn.innerHTML = "List";
        }

    }

    toggleServiceDropdown() {
        const clickEvents = ['click', 'keydown', 'change'];
        const dropDowns = document.querySelectorAll('.dropdown-container');

        clickEvents.forEach((event) => {
            dropDowns.forEach((element) => {
                element.addEventListener(event, (e) => {
                    element.classList.toggle('expand');
                });
            })
        });
    }

    removeOtherToolTips(tips) {
        for (let i = 0; i < tips.length; i++) {
            tips[i].classList.remove('show-tip');
        }
    }

    toggleCareLabels() {
        const clickEvents = ['click', 'keydown'];
        const toolTips = document.querySelectorAll('.tool-tip');

        clickEvents.forEach((event) => {
            toolTips.forEach((element) => {
                let label = element.querySelector('.label p');
                let close = element.querySelector('.close');

                label.addEventListener(event, (e) => {
                    this.removeOtherToolTips(toolTips);
                    element.classList.add('show-tip');
                })

                // close button only shows on mobile
                close.addEventListener(event, (e) => {
                    this.removeOtherToolTips(toolTips);
                })
            });
        });
    }

    setupFilters() {
        const clickEvents = ['click', 'keydown', 'change'];
        const labels = document.querySelectorAll('.filter-entry');
        const mapBtn = document.querySelector('#map-button');
        const checkboxes = document.querySelectorAll('.filter-entry input[type=checkbox]');
        const distanceRadioBtns = document.querySelectorAll('.distance-container input[type=radio]');
        const laboratoryServiceContainers = document.querySelectorAll('.laboratory-services-container');
        const labServMobileCheckbox = document.getElementById('labServCheckbox');
        const openNowContainers = document.querySelectorAll('.open-now-container');
        const openNowMobileCheckbox = document.getElementById('openNowCheckbox');
        const clearAll = document.querySelector('.clear-all');

        labServMobileCheckbox.addEventListener('change', () => {
            laboratoryServiceContainers.forEach((container) => {
                if (container.classList.contains('active')) {
                    container.classList.remove('active');
                    this.filters.laboratory_services = false;
                } else {
                    container.classList.add('active');
                    this.filters.laboratory_services = true;
                }
                this.queueUpdateHrefFilters();
            })
        });

        openNowMobileCheckbox.addEventListener('change', () => {
            openNowContainers.forEach((container) => {
                if (container.classList.contains('active')) {
                    container.classList.remove('active');
                    this.filters.open_now = false;
                } else {
                    container.classList.add('active');
                    this.filters.open_now = true;
                }
                this.queueUpdateHrefFilters();
            })
        })        

        clickEvents.forEach((event) => {
            laboratoryServiceContainers.forEach(container => {
                container.addEventListener(event, (e) => {
                    if (container.classList.contains('active')) {
                        container.classList.remove('active');
                        labServMobileCheckbox.checked = false;
                        this.filters.laboratory_services = false;
                    } else {
                        container.classList.add('active');
                        labServMobileCheckbox.checked = true;
                        this.filters.laboratory_services = true;
                    }
                    this.queueUpdateHrefFilters();
                });
            });

            clearAll.addEventListener(event, (e) => {
                if (this.filterHelper.keydownOrClick(event, e)) { 
                    this.clearAllFilters();    
                }
            });

            openNowContainers.forEach(container => {
                container.addEventListener(event, (e) => {
                    if (container.classList.contains('active')) {
                        container.classList.remove('active');
                        openNowMobileCheckbox.checked = false;
                        this.filters.open_now = false;
                    } else {
                        container.classList.add('active');
                        openNowMobileCheckbox.checked = true;
                        this.filters.open_now = true;
                    }
                    this.queueUpdateHrefFilters();
                });
            });

            mapBtn.addEventListener(event, (e) => {
                this.toggleMap(mapBtn);
            })

            checkboxes.forEach((checkbox) => {
                checkbox.addEventListener(event, (e) => {
                    let container = checkbox.closest('.checkbox-container');
                    if(!container) return;
                    if (container.classList.contains('parent-service')) {
                        let id = container.getAttribute('data-service-id');
                        let inputValue = container.querySelector('input').checked;
                        this.checkChildrenServices(id, inputValue);
                    }

                    if (container.classList.contains('subservice')) {
                        let inputValue = container.querySelector('input').checked;
                        let id = container.getAttribute('data-service-id');
                        
                        if (!inputValue) {
                            let parent = document.querySelector(`${this.selector} .parent-service[data-service-id="${id}"] input`);

                            if (parent) {
                                parent.checked = false;
                            }
                        }
                    }

                    this.queueUpdateHrefFilters();
                });
            });

            distanceRadioBtns.forEach((btn) => {
                btn.addEventListener(event, (e) => {
                    if (e.target.id.includes('id-any')) {
                        this.filters.distance = null;
                    }
                    this.queueUpdateHrefFilters();
                });
            });

            labels.forEach((element) => {
                element.addEventListener(event, (e) => {
                    if (this.filterHelper.keydownOrClick(event, e)) {
                        if ((e.target.classList.contains('label-text') || e.target.classList.contains('keyboard_arrow_down')) && element.classList.contains('selected')) {
                            element.classList.remove('selected');
                        } else {
                            this.filterHelper.removeOtherSelectedFilters(labels);
                            if (!e.target.classList.contains('close')) {
                                element.classList.add('selected');
                                this.filterHelper.filtersAppliedCount();
                            }
                        }

                        if (e.target.type == 'radio' && !this.isMobile) {
                            element.classList.remove('selected');
                        }

                        if (e.target.classList.contains('close')) {
                            this.queueUpdateHrefFilters();
                        }
                    }
                });
            });
        });

        document.addEventListener('click', (e) => {
            // clicking outside filter container should close filter only on desktop
            const desktopFilterContainer = document.querySelector('.location-search-container');
            const visibleToolTip = document.querySelector('.show-tip');
            const toolTips = document.querySelectorAll('.tool-tip');
            const container = desktopFilterContainer.getElementsByClassName('selected')[0];

            if (container) {
                if (container !== e.target && !container.contains(e.target) && !e.target.classList.contains('filter-entry')) {
                    container.classList.remove('selected');
                }
            }

            if (visibleToolTip) {
                if (visibleToolTip !== e.target && !visibleToolTip.contains(e.target)) {
                    this.removeOtherToolTips(toolTips);
                }
            }
        });

        this.map = InitializeMap(
            document.querySelector(".map"),
            [-88.0084115, 41.8293935],
            (e) => {
            },
            '#map.mapboxgl-map'
        );

        this.resetMapMarkers();
    }

    checkChildrenServices(id, checkedValue) {
        let children = document.querySelectorAll(`${this.selector} .subservice[data-service-id="${ id }"] input`);

        children.forEach(child => {
            child.checked = checkedValue;
        });
    }

    getLocations() {
        let locations = [];

        document.querySelectorAll(".single-location").forEach(location => {
            locations.push({
                type: 'Feature',
                geometry: {
                    type: 'Point',
                    coordinates: [location.dataset.locationLng, location.dataset.locationLat]
                },
                properties: {
                    name: location.dataset.locationCity,
                    address: location.dataset.locationAddress,
                }
            })
        });

        return {
            type: 'FeatureCollection',
            features: locations
        };
    }

    updateHrefFilters(pushState) {
        this.syncServicesFilters();
        this.syncCareTypeFilters();
        this.syncCityFilters();
        this.syncDistanceFilter();
        this.determineShowOpenNowFilter();

        this.href = new URL(location.href);

        if (this.filters.search_service_attribute != undefined) {
            this.href.searchParams.set('search_service_attribute', this.filters.search_service_attribute);
        }

        this.href.searchParams.delete('search_location');
        if (this.filters.search_location != undefined && this.filters.search_location != '') {
            //TODO: Remove . " il" once Duly expands out
            this.href.searchParams.set('search_location', this.filters.search_location + ', il');
        }

        this.href.searchParams.delete('city[]');
        this.filters.cities.forEach(city => this.href.searchParams.append('city[]', city));

        this.href.searchParams.delete('service[]');
        this.filters.services.forEach(service => this.href.searchParams.append('service[]', service));

        this.href.searchParams.delete('care[]');
        this.filters.careTypes.forEach(type => this.href.searchParams.append('care[]', type));

        this.href.searchParams.delete('laboratory_services');
        if (this.filters.laboratory_services) {
            this.href.searchParams.set('laboratory_services', this.filters.laboratory_services);
        }

        if (this.page) {
            this.href.searchParams.set('page', this.page);
        }

        this.href.searchParams.delete('open_now');
        if (this.filters.open_now) {
            this.href.searchParams.set('open_now', this.filters.open_now);
        }

        this.href.searchParams.delete('distance');
        this.href.searchParams.delete('latitude');
        this.href.searchParams.delete('longitude');

        this.href.searchParams.delete('distance');
        if (this.filters.distance){
            this.href.searchParams.set('distance', this.filters.distance);
        }

        if (this.latitude) {
            this.href.searchParams.set('latitude', this.latitude);
            this.href.searchParams.set('longitude', this.longitude);
        }

        if (pushState) {
            history.pushState(this.href.search, "", this.href.href);
        }

        this.fireAjaxEvent();
    }

    syncCityFilters() {
        var checkboxes = document.querySelectorAll('.city-checkbox');
        var selectedCities = [];

        for (var i = 0; i < checkboxes.length; i++) {
            var checkbox = checkboxes[i];
            var input = checkbox.querySelector('input');

            if (input.checked) {
                var label = checkbox.querySelector('label').innerHTML;
                selectedCities.push(label);
            }
        }

        this.filters.cities = [...new Set(selectedCities)];
    }

    syncCareTypeFilters() {
        var checkboxes = document.querySelectorAll('.care-checkbox');
        var selectedCareTypes = [];

        for (var i = 0; i < checkboxes.length; i++) {
            var checkbox = checkboxes[i];
            var input = checkbox.querySelector('input');
            if (input.checked) {
                var label = checkbox.querySelector('label').innerHTML;
                selectedCareTypes.push(label);
            }
        }

        this.filters.careTypes = [...new Set(selectedCareTypes)];
    }

    determineShowOpenNowFilter() {
        let openNowFilters = document.querySelectorAll('.open-now-container');
        let divider = document.querySelector('.open-now-divider');
        let showFilter = (this.filters.careTypes.length || this.filters.laboratory_services == true);

        openNowFilters.forEach(container => {
            if (showFilter) {
                container.classList.remove('hidden');
                divider.classList.remove('hidden');
            } else {
                container.classList.add('hidden');
                divider.classList.add('hidden');
                container.classList.remove('active');
            }
        });

        if (!showFilter) {
            this.filters.open_now = false;
        }
    }

    syncServicesFilters() {
        var checkboxes = document.querySelectorAll('.service-checkbox');
        var selectedServices = [];

        for (var i = 0; i < checkboxes.length; i++) {
            var checkbox = checkboxes[i];
            var input = checkbox.querySelector('input');

            if (input.checked) {
                var label = checkbox.querySelector('label').textContent;
                selectedServices.push(label);
            }
        }
        
        this.filters.services = [...new Set(selectedServices)];
    }

    syncDistanceFilter() {
        var radios = document.querySelectorAll('.distance-container .radio');
        var selectedDistance = null;

        for (var i = 0; i < radios.length; i++) {
            var radio = radios[i];
            var input = radio.querySelector('input');

            if (input.checked) {
                var label = radio.querySelector('label').innerHTML;
                selectedDistance = label.replace(/\D+/g, '');
            }
        }

        this.filters.distance = selectedDistance;
    }

    calculateDistance() {
        this.href = new URL(location.href);
        this.href.searchParams.set('latitude', this.latitude);
        this.href.searchParams.set('longitude', this.longitude);
        // need to send location coordinates to controller
        return this.fireAjaxEvent();
    }

    resetMapMarkers() {
        this.markers = this.markers || [];
        this.markers.forEach(marker => marker.remove());
        let locations = this.getLocations();
        if (locations.features.length > 0) {
            this.markers = AddMarkers(locations, this.map);
        }
        this.map.resize();
    }

    /**
     * Update the view with new locations data using current filters.
     */
    async fireAjaxEvent() {
        let pagination = document.querySelector('.locations-list-page .pagination');
        let container = document.querySelector('.left-container .container');
        const loadingSpinner = document.querySelector('.content .left-container .loading-spinner');

        container.classList.add('hidden');
        loadingSpinner.classList.remove('hidden');
        pagination && pagination.classList.add('hidden');

        if (this.latitude && this.longitude) {
            this.href.searchParams.set('latitude', this.latitude);
            this.href.searchParams.set('longitude', this.longitude);
        }

        let qd = {};
        (new URL(this.href)).search.replace("?", "").split("&").forEach(function (item) {
            var s = item.split("="),
                k = s[0].replace("%5B", "").replace("%5D", ""),
                v = s[1] && decodeURIComponent(s[1]);
            (qd[k] = qd[k] || []).push(v);
        });

        delete qd.search_location;
        delete qd.search_service_attribute;

        this.analytics.LocationSearch(qd);

        fetch(this.href, {
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
            let container = document.querySelector('.left-container .container');
            container.innerHTML = data;
            container.classList.remove('hidden');
            loadingSpinner.classList.add('hidden');
            pagination && pagination.classList.remove('hidden');

            this.resetMapMarkers();
            this.toggleServiceDropdown();
            this.toggleCareLabels();

            if (this.latitude && this.longitude) {
                this.showDistances();
            }

            container.querySelector('.container').classList.remove('hidden');
        }).catch((response) => {
            loadingSpinner.classList.add('hidden');
            document.querySelector('.display-error').classList.remove('hidden');
        });
    }
}