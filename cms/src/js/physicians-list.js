'use strict';

import { InitializeMap, AddMarkers } from './mapbox';
import { Search } from './search';
import { Analytics } from "./analytics.js";
import { breakpoints } from './variables';
import { FilterHelper } from './filters/filter-helper';
import noUiSlider from 'nouislider';
export class PhysiciansList {
    constructor() {
        if (!document.querySelector('.physicians-list-page')) {
            return;
        }
    
        this.analytics = new Analytics(false);

        // enable only mobile or desktop filters depending on current viewport size
        this.isMobile = window.innerWidth < parseInt(breakpoints["bs-large"]) ? true : false;

        this.selector = this.isMobile ? '.event-filter-modal' : '.physician-search-container';

        window.addEventListener('resize', _ => {
            this.resetMapMarkers();
        });

        this.setupFilters();
        this.setupNearMeIcon();

        this.filters = {
            cities: [],
            services: [],
            languages: [],
            genders: [],
            hospitalAffiliations: []
        };

        this.filterHelper = new FilterHelper(_ => { });

        document.querySelector(".filter-btn-mobile").addEventListener("click", _ => {
            document.querySelector(".event-filter-modal").classList.remove("hidden");
        });

        document.querySelectorAll(".event-filter-modal .material-icons.close, .view-results").forEach(elem => elem.addEventListener("click", _ => {
            document.querySelector(".event-filter-modal").classList.add("hidden");
        }));

        this.search_physician_attribute = new Search(
            "#search-physicians-by-attributes",
            ".search-content form.physician-attribute",
            location.origin + "/physicians/auto-suggestions",
            "search_physician_attribute",
            "#html-content-template-recent-searches",
            "#html-content-template-auto-complete"
        );

        this.address = new Search(
            "#search-physicians-by-location",
            ".search-content form.physician-location",
            location.origin + "/physicians/auto-suggestions",
            "address",
            "#html-content-template-recent-searches",
            "#html-content-template-auto-complete"
        );

        document.querySelector(".physician-search-btn").addEventListener("click", this.submitSearch.bind(this));

        document.querySelectorAll(".physician-search-container form").forEach(form => form.addEventListener("submit", e => {
            e.preventDefault();
            e.stopPropagation();
            this.submitSearch();
            return false;
        }), true);

        this.locationPermissions();

        this.setupPaginationListeners();

        this.setupAgeSlider();

        this.restoreFiltersFromURI();

        window.addEventListener('popstate', (e) => {
            this.restoreFiltersFromURI(e.state);
        });

        document.querySelector(".clear-all").addEventListener("click", _ => {
            this.disableFiltersSync = true;
            document.querySelectorAll(".event-filter-modal input[type='checkbox']:checked").forEach(checkbox => checkbox.checked = false);
            
            document.querySelectorAll("input[type='radio']").forEach(filterEntry => {
                if (filterEntry.id.includes('any')) {
                    filterEntry.click();
                }
            });

            document.querySelectorAll(".event-filter-modal .filter-entry").forEach(filterEntry => {
                filterEntry.classList.remove("selected");
                filterEntry.classList.remove("active");
            });
            
            this.filters.lgbtqia_resource = null;
            this.filters.custom_min_age = null;
            this.filters.custom_max_age = null;
            this.disableFiltersSync = false;
            this.filterHelper.filtersAppliedCount();
            this.determineSliderLogic(false);

            let servicesCount = document.querySelector('.event-filter-modal .services-container .count');

            if (servicesCount) {
                servicesCount.innerHTML = '';
            }

            this.queueUpdateHrefFilters();
        });
    }

    urlHasNoParams() {
        // reset filters
        this.filters = {
            cities: [],
            services: [],
            languages: [],
            genders: [],
            hospitalAffiliations: []
        };

        // remove any search field material designs and animations
        this.filterHelper.resetSearchField('#search-physicians-by-attributes');
        this.filterHelper.resetSearchField('#search-physicians-by-location');

        // filters can still show a count and state that is not accurate
        // needs to be reset
        this.filterHelper.resetFilterEntry();
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
        } else {
            this.urlHasNoParams();
        }

        // UI still shows previous checkboxes as checked
        this.filterHelper.resetCheckboxes();

        // do not fire off ajax requests while the filters are being restored in bulk
        this.disableFiltersSync = true;

        // restore page filter
        this.page = this.href.searchParams.get('page');

        // restore per-page filter
        this.perPage = this.href.searchParams.get('per-page');

        // restore search_physician_attribute filter
        this.filters.search_physician_attribute = this.href.searchParams.get('search_physician_attribute');

        if (!this.href.searchParams.get('search_physician_attribute')) {
            this.filterHelper.resetSearchField('#search-physicians-by-attributes');
        }

        // restore address filter
        this.filters.address = this.href.searchParams.get('address');
        
        if (!this.href.searchParams.get('address')) {
            this.filterHelper.resetSearchField('#search-physicians-by-location');
        }

        // restore lgbtqia_resource filter
        this.filters.lgbtqia_resource = this.href.searchParams.get('lgbtqia_resource');
        if (this.filters.lgbtqia_resource) {
            document.querySelectorAll('.filter-entry.lgbtqia-container .label-text').forEach(label => label.click());
        }

        // restore custom filters
        let activateSlider = false;
        this.filters.custom_min_age = this.href.searchParams.get('min_age');
        this.filters.custom_max_age = this.href.searchParams.get('max_age');

        if (this.filters.custom_min_age != null && this.filters.custom_max_age != null) {
            activateSlider = true;
            let values = [this.filters.custom_min_age,  this.filters.custom_max_age];
            this.determineSliderLogic(true, values);
        }

        // restore city filters
        this.filterHelper.restoreListFilterHelper('.physician-search-container', '.city-checkbox', 'city[]', this.href.searchParams);

        // restore service filters
        this.filterHelper.restoreListFilterHelper('.physician-search-container', '.service-checkbox', 'service[]', this.href.searchParams);

        // restore language filters
        this.filterHelper.restoreListFilterHelper('.physician-search-container', '.language-checkbox', 'language[]', this.href.searchParams);

        // restore gender filters
        this.filterHelper.restoreListFilterHelper('.physician-search-container', '.gender-checkbox', 'gender[]', this.href.searchParams);

        // restore city filters
        this.filterHelper.restoreListFilterHelper('.physician-search-container','.hospital-affiliation-checkbox', 'hospital[]', this.href.searchParams);

        // restore availability filter
        this.filterHelper.restoreRadioFilterHelper('.physician-search-container', '.availability-container', 'availability-id', 'availability', this.href.searchParams);

        // restore ages filters
        this.filterHelper.restoreListFilterHelper('.physician-search-container','.age-checkbox', 'age[]', this.href.searchParams);

        // restore sort filter
        this.filterHelper.restoreRadioFilterHelper('.physician-search-container', '.sort-container', 'sort-id', 'order_by', this.href.searchParams)

        // restore latitude filter
        this.latitude = this.href.searchParams.get('latitude');

        // restore longitude filter
        this.longitude = this.href.searchParams.get('longitude');

        if (activateSlider) {
            let container = document.querySelector(`${this.selector} .ages-container`);
            container.click();
            container.classList.remove('selected');
        } else {
            if (this.slider) {
                this.slider.classList.add('hidden');
            }
            let slider = document.querySelector('#age-slider');
            slider.checked = false;
        }

        // re-enable ajax requests
        this.disableFiltersSync = false;

        this.queueUpdateHrefFilters(false);
    }

    setupAgeSlider(){
        this.slider = document.querySelector(`${this.selector} .slider-container`);
        let min = 0;
        let max = 80;

        if (this.filters.custom_min_age) {
            min = this.filters.custom_min_age;
        }

        if (this.filters.custom_max_age) {
            max = this.filters.custom_max_age;
        }

        // initialize slider
        noUiSlider.create(this.slider, {
            start: [min, max],
            connect: true,
            tooltips: true,
            format: {
                to: (value) => {
                    return parseInt(value);
                },
                from: (value) => {
                    return parseInt(value);
                }
            },
            range: {
                'min': 0,
                'max': 100
            }
        });

        this.updateSliderText(min, max);

        // add slider event listeners
        this.slider.noUiSlider.on('end', (values, handle) => {
            this.fireSliderEvent(values);
        });

        // add slider event listeners
        this.slider.noUiSlider.on('slide', (values, handle) => {
            this.updateSliderText(values[0], values[1]);
        });
    }

    updateSliderText(min, max) {
        let minText = document.querySelector(`${this.selector} .indicator .min`);
        let maxText = document.querySelector(`${this.selector} .indicator .max`);

        if (minText) {
            minText.innerHTML = min;
        }

        if (maxText) {
            maxText.innerHTML = max;
        }
    }

    fireSliderEvent(values) {
        this.filters.custom_min_age = values[0];
        this.filters.custom_max_age = values[1];
        this.queueUpdateHrefFilters();
    }

    setupPaginationListeners() {
        document.querySelectorAll(".pagination-list a").forEach(link => link.addEventListener("click", e => {
            e.preventDefault();
            let href = new URL(link);
            let searchParams = new URLSearchParams(href.search);

            if (searchParams.get('page')) {
                this.page = searchParams.get('page');
                this.href.searchParams.set('page', this.page);
            }

            if (searchParams.get('per-page')) {
                this.perPage = searchParams.get('per-page');
                this.href.searchParams.set('per-page', this.perPage);
            }

            this.queueUpdateHrefFilters();
            window.scrollTo(0, 0);
        }));
    }

    triggerNearMeSort() {
        const inputField = document.querySelector('#search-physicians-by-location');
        const text = document.querySelector('.near-me-clicked').innerHTML;
        const parent = inputField.parentNode;
        const label = parent.querySelector('span.mdc-floating-label');
        const nearMeElement = document.querySelector('.physician-location .material-icons.near_me');
        const loadingSpinnerElement = document.querySelector('.physician-location .loading-spinner');

        // if "near me" is clicked, clear any previous sorts from specific location
        document.querySelector('#search-physicians-by-location').value = "";
        if (window.innerWidth <= breakpoints['bs-large']) {
            document.querySelector("[for='mobile-sort-id-1']").click();
        } else {
            document.querySelector("[for='sort-id-1']").click();
        }
        document.querySelector(".physician-search-btn").click();

        // update placeholder text
        parent.classList.add('mdc-text-field-success');
        label.classList.add('mdc-floating-label--float-above');
        inputField.value = text;
        nearMeElement.classList.remove('hidden');
        loadingSpinnerElement.classList.add('hidden');
    }

    setupNearMeIcon() {
        ['click', 'keydown'].forEach(event => {
            document.querySelector(".physician-search-container span.material-icons.near_me").addEventListener(event, e => {
                if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
                    const nearMeElement = document.querySelector('.physician-location .material-icons.near_me');
                    const loadingSpinnerElement = document.querySelector('.physician-location .loading-spinner');

                    nearMeElement.classList.add('hidden');
                    loadingSpinnerElement.classList.remove('hidden');
                    this.locationPermissions(this.triggerNearMeSort.bind(this));
                }
            });
        });
    }

    submitSearch() {
        let search_physician_attribute_value = document.querySelector("#search-physicians-by-attributes").value;
        if (this.filters.search_physician_attribute || search_physician_attribute_value.length > 0) {
            this.search_physician_attribute.storeRecentSearch(search_physician_attribute_value);
            this.filters.search_physician_attribute = search_physician_attribute_value;

            // a new search term gathers new results and the pagination needs to be reset
            this.page = 1;
            this.href.searchParams.set('page', this.page);
        }
        let address_value = document.querySelector("#search-physicians-by-location").value;
        if (this.filters.address || address_value.length > 0) {
            this.address.storeRecentSearch(address_value);
            //TODO: Remove . " il" once Duly expands out
            this.filters.address = address_value + " il";
        }

        this.queueUpdateHrefFilters();
    }

    locationPermissions(callback = null) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(position => {
                this.latitude = position.coords.latitude;
                this.longitude = position.coords.longitude;
                if (callback) {
                    callback();
                }
            }, _ => {});
        }
    }

    queueUpdateHrefFilters(pushState = true) {
        clearTimeout(this._timeout);
        this._timeout = setTimeout((e) => {
            this.updateHrefFilters(pushState);
        }, 500);
    };

    setupFilters() {
        const clickEvents = ['click', 'keydown', 'change'];
        const labels = document.querySelectorAll('.filter-entry:not(.lgbtqia-container)');
        const checkboxes = document.querySelectorAll('.filter-entry input[type=checkbox]');
        const radiobtns = document.querySelectorAll('.filter-entry input[type=radio]');
        const lgbtqiaContainer = document.querySelector(`${this.selector} .lgbtqia-container`);
        const lgbtqiaCheckbox = document.getElementById('lgbtqiaCheckbox');

        lgbtqiaCheckbox.addEventListener('change', (e) => {
            lgbtqiaContainer.classList.toggle('active');
            this.queueUpdateHrefFilters();
        })

        clickEvents.forEach((event) => {
            lgbtqiaContainer.addEventListener(event, (e) => {
                lgbtqiaContainer.classList.toggle('active');
                this.queueUpdateHrefFilters();
            });

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

            radiobtns.forEach((button) => {
                button.addEventListener(event, (e) => {
                    if (!e.target.id.includes('age-slider')) {
                        this.queueUpdateHrefFilters();
                    } else {
                        this.uncheckAgeBoxes();
                        this.determineSliderLogic(true);
                    }
                });
            });

            labels.forEach((element) => {
                element.addEventListener(event, (e) => {
                    let isAgeFilter = e.target.id.includes('age-slider');

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

                        if (e.target.type == 'radio' && !isAgeFilter && !this.isMobile) {
                            element.classList.remove('selected');
                        }

                        if (e.target.classList.contains('close')) {
                            if (element.classList.contains('ages-container')) {
                                this.determineSliderLogic(false);
                            }
                            this.queueUpdateHrefFilters();
                        }

                        this.page = 1;
                    }
                });
            });
        });

        document.addEventListener('click', (e) => {
            // clicking outside filter container should close filter only on desktop
            const desktopFilterContainer = document.querySelector('.physician-search-container');
            const container = desktopFilterContainer.getElementsByClassName('selected')[0];

            if (container) {
                if (container !== e.target && !container.contains(e.target) && !e.target.classList.contains('filter-entry')) {
                    container.classList.remove('selected');
                }
            }
        });

        this.map = InitializeMap(
            document.querySelector(".map"),
            [-88.0084115, 41.8293935],
            // this.onGeolocateCallback.bind(this)
        );

        this.resetMapMarkers();
        this.mapToggleListener();
    }

    checkChildrenServices(id, checkedValue) {
        let children = document.querySelectorAll(`${this.selector} .subservice[data-service-id="${ id }"] input`);

        children.forEach(child => {
            child.checked = checkedValue;
        });
    }

    uncheckAgeBoxes() {
        const ageCheckBoxes = document.querySelectorAll('.age-checkbox input');
        let updateHrefs = false;

        ageCheckBoxes.forEach(box => {
            if (box.checked) {
                updateHrefs = true;
            }

            box.checked = false;
        });

        if (updateHrefs) {
            this.queueUpdateHrefFilters();
        }
    }

    determineSliderLogic(show = true,  values = null) {
        let slider = document.querySelector(`${this.selector} .slider-container`);
        let indicator = document.querySelector(`${this.selector} .indicator`);
        let input = document.querySelector(`${this.selector} .slider input`);

        if (show) {
            slider.classList.remove('hidden');
            indicator.classList.remove('hidden');
            input.checked = true;

            if (this.slider) {
                if (values) {
                    this.slider.noUiSlider.set(values);
                    this.updateSliderText(values[0], values[1]);
                } else {
                    // default age range
                    this.slider.noUiSlider.set([0, 80]);
                    this.updateSliderText(0, 80);
                    this.fireSliderEvent([0, 80]);
                }
            }
        } else {
            slider.classList.add('hidden');
            indicator.classList.add('hidden');
            input.checked = false;
            this.filters.custom_min_age = null;
            this.filters.custom_max_age = null;
        }

        this.filterHelper.filtersAppliedCount();
    }

    mapToggleListener() {
        const showMapBtn = document.querySelector('.map-list-switcher');
        const tileContainer = document.querySelector('.left-container');
        const map = document.querySelector('.map-container');

        ['click', 'keydown'].forEach(event => {
            showMapBtn.addEventListener(event, (e) => {
                if (this.filterHelper.keydownOrClick(event, e)) {
                    map.classList.toggle('show-map');

                    if (map.classList.contains('show-map')) {
                        tileContainer.classList.add('hidden');
                        showMapBtn.innerHTML = "List";
                    } else {
                        tileContainer.classList.remove('hidden');
                        showMapBtn.innerHTML = "Map";
                    }
                }
            });
        })
    }

    getLocations() {
        let locations = [];

        document.querySelectorAll(".map-marker-point").forEach(location => {
            let lat = location.getAttribute('data-latitude');
            let lng = location.getAttribute('data-longitude');
            let address = location.getAttribute('data-address');
            let city = location.getAttribute('data-city');
            let state = location.getAttribute('data-state');
            let postcode = location.getAttribute('data-postcode');

            locations.push({
                type: 'Feature',
                geometry: {
                    type: 'Point',
                    coordinates: [lng, lat]
                },
                properties: {
                    name: city,
                    address: address + ", " + city + ", " + state + " " + postcode
                }
            })
        });

        return {
            type: 'FeatureCollection',
            features: locations
        };
    }

    updateHrefFilters(pushHistory) {
        if (this.disableFiltersSync) {
            return;
        }

        this.syncCityFilters();
        this.syncLgbtqiaFilter();
        this.syncServicesFilters();
        this.syncLanguagesFilters();
        this.syncGendersFilters();
        this.syncAgesFilter();
        this.syncHospitalAffiliationsFilters();
        this.syncDistanceFilter();
        this.syncAvailabilityFilter();
        this.syncSortFilter();

        this.href = new URL(location.origin + location.pathname);

        if (this.href.searchParams.get('latitude')) {
            this.href.searchParams.delete('latitude');
        }

        if (this.href.searchParams.get('longitude')) {
            this.href.searchParams.delete('longitude');
        }

        if (this.page) {
            this.href.searchParams.set('page', this.page);
        }

        if (this.perPage) {
            this.href.searchParams.set('per-page', this.perPage);
        }

        if (this.filters.search_physician_attribute != undefined) {
            this.href.searchParams.set('search_physician_attribute', this.filters.search_physician_attribute);
        }

        if (this.filters.address != undefined) {
            this.href.searchParams.set('address', this.filters.address);
        }

        this.href.searchParams.delete('lgbtqia_resource');
        if (this.filters.lgbtqia_resource) {
            this.href.searchParams.set('lgbtqia_resource', this.filters.lgbtqia_resource);
        }

        this.href.searchParams.delete('city[]');
        this.filters.cities.forEach(city => this.href.searchParams.append('city[]', city));

        this.href.searchParams.delete('service[]');
        this.filters.services.forEach(service => this.href.searchParams.append('service[]', service));

        this.href.searchParams.delete('language[]');
        this.filters.languages.forEach(language => this.href.searchParams.append('language[]', language));

        this.href.searchParams.delete('gender[]');
        this.filters.genders.forEach(gender => this.href.searchParams.append('gender[]', gender));

        this.href.searchParams.delete('age[]');
        this.filters.ages.forEach(age => this.href.searchParams.append('age[]', age));

        this.href.searchParams.delete('custom_min_age');
        this.href.searchParams.delete('custom_max_age');

        if (this.filters.custom_min_age != null && this.filters.custom_max_age != null) {
            this.href.searchParams.set('min_age', this.filters.custom_min_age);
            this.href.searchParams.set('max_age', this.filters.custom_max_age);
        }

        this.href.searchParams.delete('hospital[]');
        this.filters.hospitalAffiliations.forEach(hospital => this.href.searchParams.append('hospital[]', hospital));

        this.href.searchParams.delete('availability');
        if (this.filters.availability) {
            this.href.searchParams.set('availability', this.filters.availability);
        }

        this.href.searchParams.delete('order_by');
        if (this.filters.order_by) {
            this.href.searchParams.set('order_by', this.filters.order_by);
        }

        if (this.filters.order_by) {
            if (this.latitude) {
                this.href.searchParams.set('latitude', this.latitude);
            }

            if (this.longitude) {
                this.href.searchParams.set('longitude', this.longitude);
            }
        }

        if (pushHistory) {
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

    syncLgbtqiaFilter() {
        const lgbtqiaContainer = document.querySelector(`${this.selector} .lgbtqia-container`);
        const lgbtqiaCheckbox = document.getElementById('lgbtqiaCheckbox');
        if (lgbtqiaContainer.classList.contains('active')) {
            lgbtqiaCheckbox.checked = true;
            this.filters.lgbtqia_resource = true;
        } else {
            lgbtqiaCheckbox.checked = false;
            this.filters.lgbtqia_resource = null;
        }
    }

    syncServicesFilters() {
        var checkboxes = document.querySelectorAll('.service-checkbox');
        var selectedServices = [];

        for (var i = 0; i < checkboxes.length; i++) {
            var checkbox = checkboxes[i];
            var input = checkbox.querySelector('input');

            if (input.checked) {
                var label = checkbox.querySelector('label').innerHTML;
                selectedServices.push(label);
            }
        }

        this.filters.services = [...new Set(selectedServices)];
    }

    syncLanguagesFilters() {
        var checkboxes = document.querySelectorAll('.language-checkbox');
        var selectedLanguages = [];

        for (var i = 0; i < checkboxes.length; i++) {
            var checkbox = checkboxes[i];
            var input = checkbox.querySelector('input');

            if (input.checked) {
                var label = checkbox.querySelector('label').innerHTML;
                selectedLanguages.push(label);
            }
        }

        this.filters.languages = [...new Set(selectedLanguages)];
    }

    syncGendersFilters() {
        var checkboxes = document.querySelectorAll('.gender-checkbox');
        var selectedGenders = [];

        for (var i = 0; i < checkboxes.length; i++) {
            var checkbox = checkboxes[i];
            var input = checkbox.querySelector('input');

            if (input.checked) {
                var label = checkbox.querySelector('label').innerHTML;
                selectedGenders.push(label);
            }
        }

        this.filters.genders = [...new Set(selectedGenders)];
    }

    syncAgesFilter() {
        var checkboxes = document.querySelectorAll('.age-checkbox');
        var selectedAges = [];

        for (var i = 0; i < checkboxes.length; i++) {
            var checkbox = checkboxes[i];
            var input = checkbox.querySelector('input');

            if (input.checked) {
                var label = checkbox.querySelector('label').innerHTML;
                var sanatizedLabel = label.match(/[a-zA-Z]+/g);
                selectedAges.push(sanatizedLabel);
            }
        }

        if (selectedAges.length) {
            this.determineSliderLogic(false);
        }

        this.filters.ages = [...new Set(selectedAges)];
    }

    syncHospitalAffiliationsFilters() {
        var checkboxes = document.querySelectorAll('.hospital-affiliation-checkbox');
        var selectedHospitalAffiliations = [];

        for (var i = 0; i < checkboxes.length; i++) {
            var checkbox = checkboxes[i];
            var input = checkbox.querySelector('input');

            if (input.checked) {
                var label = checkbox.querySelector('label').innerHTML;
                selectedHospitalAffiliations.push(label);
            }
        }

        this.filters.hospitalAffiliations = [...new Set(selectedHospitalAffiliations)];
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

    syncSortFilter() {
        var radios = document.querySelectorAll('.sort-container .radio');
        var selectedSort = null;

        for (var i = 0; i < radios.length; i++) {
            var radio = radios[i];
            var input = radio.querySelector('input');
            var id = input.id;

            if (input.checked && !id.includes('id-any')) {
                selectedSort = input.id.replace(/\D+/g, '');
            }
        }

        this.filters.order_by = selectedSort;
    }

    syncAvailabilityFilter() {
        var radios = document.querySelectorAll('.availability-container .radio');
        var selectedAvailability = null;

        for (var i = 0; i < radios.length; i++) {
            var radio = radios[i];
            var input = radio.querySelector('input');
            var id = input.id;

            if (input.checked && !id.includes('id-any')) {
                selectedAvailability = radio.querySelector('label').innerHTML;
            }
        }

        this.filters.availability = selectedAvailability;
    }

    /**
     * Update the view with new physicians data using current filters.
     */
    async fireAjaxEvent() {
        let table = document.querySelector('.body-container .content .tile-container');
        let spinner = document.querySelector('.body-container .content .loading-spinner');

        // hide content
        table.classList.add('hidden');
        spinner.classList.remove('hidden');

        let qd = {};
        (new URL(this.href)).search.replace("?", "").split("&").forEach(function (item) {
            var s = item.split("="),
                k = s[0].replace("%5B", "").replace("%5D", ""),
                v = s[1] && decodeURIComponent(s[1]);
            (qd[k] = qd[k] || []).push(v);
        });

        delete qd.address;
        delete qd.search_physician_attribute;

        this.analytics.PhysicianSearch(qd, "Physician Search");

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
            table.innerHTML = data;
            spinner.classList.add('hidden');
            table.classList.remove('hidden');

            this.resetMapMarkers();

            this.setupPaginationListeners();
        });
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

    setCookie(name, value, days = 365, path = '/') {
        const expires = new Date(Date.now() + days * 864e5).toUTCString()
        document.cookie = name + '=' + encodeURIComponent(value) + '; expires=' + expires + '; path=' + path + '; SameSite=Lax' + '; secure';
    }

    getCookie(name) {
        return document.cookie.split('; ').reduce((r, v) => {
            const parts = v.split('=')
            return parts[0] === name ? decodeURIComponent(parts[1]) : r
        }, '')
    }
}