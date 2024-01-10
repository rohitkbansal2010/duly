'use strict';

import { InitializeMap, AddMarkers } from '../mapbox';
import { Search } from '../search';
import { breakpoints } from '../variables';
import { FilterHelper } from '../filters/filter-helper';
import noUiSlider from 'nouislider';
import { Analytics } from "../analytics";
import { SelectAppointment } from './_select-appointment/select-appointment';

export class PhysicianAppointment {
    constructor() {
        if (!document.querySelector('.select-physician-page')) {
            return;
        }
    
        this.analytics = new Analytics(false);

        this.map = InitializeMap(
            document.querySelector(".map"),
            [-88.0084115, 41.8293935],
            // this.onGeolocateCallback.bind(this),
            '#map.mapboxgl-map'
        );
        
        this.filters = {
            cities: [],
            languages: [],
            genders: [],
            hospitalAffiliations: [],
            order_by: 'availability'
        };

        this.filterHelper = new FilterHelper(_ => { });

        // enable only mobile or desktop filters depending on current viewport size
        this.isMobile = window.innerWidth < parseInt(breakpoints["bs-large"]) ? true : false;

        this.selector = this.isMobile ? '.event-filter-modal' : '.schedule-search-physicians-container';

        this.restoreFiltersFromURI();
        this.setupFilters();
        this.mapToggleListener();
        this.resetMapMarkers();
        this.updateHrefFilters();
        this.setupAgeSlider();

        this.search_physician_attribute = new Search(
            "#search-physician-appointments",
            ".search-content .search-appointment-physicians",
            location.origin + "/schedule/physicians/auto-suggestions",
            "search_physician_attribute",
            "#html-content-template-recent-searches",
            "#html-content-template-auto-complete"
        );
        
        this.addEventListeners();
        this.setupPaginationListeners();
        
        window.addEventListener('popstate', (e) => {
            this.restoreFiltersFromURI(e.state);
        });
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
            this.filters.custom_min_age = values[0];
            this.filters.custom_max_age = values[1];
            this.queueUpdateHrefFilters();
        });

        // add slider event listeners
        this.slider.noUiSlider.on('slide', (values, handle) => {
            this.updateSliderText(values[0], values[1]);
        });
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
        this.filters.search_physician_attribute = this.href.searchParams.get('search_physician_attribute');

        if (!this.href.searchParams.get('search_physician_attribute')) {
            this.filterHelper.resetSearchField('#search-physician-appointments');
        }

        // restore address filter
        this.filters.address = this.href.searchParams.get('address');

        if (!this.href.searchParams.get('address')) {
            this.filterHelper.resetSearchField('#search-zip-code');
        }

        // restore lgbtqia_resource filter
        this.filters.lgbtqia_resource = this.href.searchParams.get('lgbtqia_resource');
        const containers = document.querySelectorAll('.lgbtqia-container');

        if (this.filters.lgbtqia_resource) {
            document.querySelectorAll('.filter-entry.lgbtqia-container .label-text').forEach(label => label.click());
        } else {
            containers.forEach(container => {
                container.classList.remove('active');
            });
        }

        // restore city filters
        this.filterHelper.restoreListFilterHelper('.schedule-search-physicians-container', '.city-checkbox', 'city[]', this.href.searchParams);

        // restore service filters
        this.filterHelper.restoreListFilterHelper('.schedule-search-physicians-container', '.service-checkbox', 'service[]', this.href.searchParams);

        // restore language filters
        this.filterHelper.restoreListFilterHelper('.schedule-search-physicians-container', '.language-checkbox', 'language[]', this.href.searchParams);

        // restore gender filters
        this.filterHelper.restoreListFilterHelper('.schedule-search-physicians-container', '.gender-checkbox', 'gender[]', this.href.searchParams);

        // restore ages filters
        this.filterHelper.restoreListFilterHelper('.physician-search-container','.age-checkbox', 'age[]', this.href.searchParams);

        // restore city filters
        this.filterHelper.restoreListFilterHelper('.schedule-search-physicians-container','.hospital-affiliation-checkbox', 'hospital[]', this.href.searchParams);

        // restore availability filter
        this.filterHelper.restoreRadioFilterHelper('.schedule-search-physicians-container', '.availability-container', 'availability-id', 'availability', this.href.searchParams);

        // restore custom filters
        let activateSlider = false;
        this.filters.custom_min_age = this.href.searchParams.get('min_age');
        this.filters.custom_max_age = this.href.searchParams.get('max_age');

        if (this.filters.custom_min_age != null && this.filters.custom_max_age != null) {
            activateSlider = true;
            let values = [this.filters.custom_min_age,  this.filters.custom_max_age];
            this.determineSliderLogic(true, values);
        }

        if (this.href.searchParams.get('rating')) {
            // restore ratings filter - applies to only this page
            this.restoreRatingsFilter();
        } else {
            this.removeRatingsFilter();
        }

        // restore latitude filter
        this.latitude = this.href.searchParams.get('latitude');

        // restore longitude filter
        this.longitude = this.href.searchParams.get('longitude');

        this.queueUpdateHrefFilters(false);
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

    restoreRatingsFilter() {
        let containers = document.querySelectorAll('.rating-container');
        const ratingParam = this.href.searchParams.get('rating');

        containers.forEach(container => {
            let count = container.querySelector('.count');
            count.innerHTML = ratingParam;
        })

        const starRatings = document.querySelectorAll('.star-rate');
        this.removePreviousRating(starRatings);

        starRatings.forEach(rating => {
            if (rating.getAttribute('data-rating') == ratingParam) {
                rating.classList.add('selected-star');
            }
        });

        this.starRatingEffect(starRatings);
    }

    removeRatingsFilter() {
        let containers = document.querySelectorAll('.rating-container');
        this.filters.rating = 0;

        containers.forEach(container => {
            container.classList.remove('active');
            container.classList.remove('selected');
            container.classList.remove('show-stars');
    
            let count = container.querySelector('.count');
            let close = container.querySelector('.close');
            let stars = container.querySelectorAll('.star-rate');
            count.innerHTML = "";
            close.classList.add('hidden');
    
            for (let i = 0; i < stars.length; i++)  {
                stars[i].classList.remove('selected-star');
                stars[i].classList.remove('fill');
            }
        })
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

    determineSliderLogic(show = true, values = null) {
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
        const tileContainer = document.querySelector('.tile-container');
        const map = document.querySelector('.map-container');

        ['click', 'keydown'].forEach(event => {
            showMapBtn.addEventListener(event, (e)=> { 
                if (this.keydownOrClick(event, e)) {
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

                    if (this.keydownOrClick(event, e)) {
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
            const desktopFilterContainer = document.querySelector('.schedule-search-physicians-container');
            const container = desktopFilterContainer.getElementsByClassName('selected')[0];

            if (container) {
                if (container !== e.target && !container.contains(e.target) && !e.target.classList.contains('filter-entry')) {
                    container.classList.remove('selected');
                }
            }
        });
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

    resetMapMarkers() {
        this.markers = this.markers || [];
        this.markers.forEach(marker => marker.remove());
        let locations = this.getLocations();
        if (locations.features.length > 0) {
            this.markers = AddMarkers(locations, this.map);
        }
        this.map.resize();
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }

    zipcodeSearch() {
        let value = document.querySelector('#search-zip-code').value;
        this.filters.address = value;
        this.filters.order_by = "proximity";
        this.href.searchParams.delete('order_by');
        this.href.searchParams.set('order_by', 'proximity');
        this.page = 1;
        this.href.searchParams.set('page', this.page);
        this.queueUpdateHrefFilters();
    }

    addEventListeners() {
        const events = ['click', 'keydown'];
        const filterOnMobile = document.querySelector('.filter_list');
        const modal = document.querySelector('.event-filter-modal');
        const closeStarRating = document.querySelectorAll('.rating-container .close');
        const closeModal = document.querySelector('.close-modal');
        const clearAll = document.querySelector('.clear-all');
        const viewAll = document.querySelector('.view-results');
        const starRatings = document.querySelectorAll('.star-rate');
        const types = document.querySelectorAll('.type');
        const searchBtn = document.querySelector(".search-results-btn");
        const searchForm = document.querySelector('.search-appointment-physicians');
        const zipCodeSearch = document.querySelector('#search-zip-code');
        const zipCodeSearchBtn = document.querySelector(".search-results-btn-zip-code");
        const nearMe = document.querySelector('.near_me');

        searchForm.addEventListener('keydown', (e) => {
            if (e.keyCode === 13) {
                this.submitSearch();
            }
        })

        zipCodeSearch.addEventListener('keydown', (e) => {
            if (e.keyCode === 13) {
                this.zipcodeSearch();
            }
        })

        events.forEach((event) => {
            searchBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    this.submitSearch();
                }
            });

            nearMe.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    this.nearMeLogic();
                }
            });

            zipCodeSearchBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    this.zipcodeSearch();
                }
            });

            types.forEach((element) => {
                element.addEventListener(event, (e)=> {
                    if (this.keydownOrClick(event, e)) {
                        for (let i = 0; i < types.length; i++) {
                            types[i].classList.remove('active');
                        }
                        this.showNearYou(element);
                    }
                });
            });

            closeStarRating.forEach((element) => {
                element.addEventListener(event, (e)=> {
                    if (this.keydownOrClick(event, e)) {
                        this.filters.rating = 0;
                        this.queueUpdateHrefFilters();
                    }
                });
            });
            
            filterOnMobile.addEventListener(event, (e)=> {
                if (this.keydownOrClick(event, e)) {
                    modal.classList.remove('hidden');
                }
            });

            closeModal.addEventListener(event, (e)=> {
                if (this.keydownOrClick(event, e)) {
                    modal.classList.add('hidden');
                }
            });

            clearAll.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) { 
                    this.clearAllFilters();    
                }
            });

            viewAll.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) { 
                    this.fireAjaxEvent();
                    modal.classList.add('hidden');
                }
            });

            starRatings.forEach((element) => {
                element.addEventListener(event, (e)=> {
                    if (this.keydownOrClick(event, e)) {
                        this.removePreviousRating(starRatings);
                        element.classList.add('selected-star');
                        this.starRatingEffect(starRatings);
                    }
                });
            });
        });
    }

    nearMeLogic() {
        const inputField = document.querySelector('#search-zip-code');
        const text = document.querySelector('.near-me-clicked').innerHTML;
        const parent = inputField.parentNode;
        const label = parent.querySelector('label');
        const nearMeElement = document.querySelector('.material-icons.near_me');
        const loadingSpinnerElement = document.querySelector('.loading-spinner');

        if (navigator.geolocation) {
            nearMeElement.classList.add('hidden');
            loadingSpinnerElement.classList.remove('hidden');
            navigator.geolocation.getCurrentPosition(position => {
                this.filters.order_by = 'proximity';
                this.latitude = position.coords.latitude;
                this.longitude = position.coords.longitude;

                // update placeholder text
                parent.classList.add('mdc-text-field-success');
                label.classList.add('mdc-floating-label--float-above');
                inputField.value = text;
                
                loadingSpinnerElement.classList.add('hidden');
                nearMeElement.classList.remove('hidden');
                this.filters.address = null;
                this.queueUpdateHrefFilters();
            }, _ => {
                loadingSpinnerElement.classList.add('hidden');
            });
        } else {
            console.log("Geolocation is not supported by this browser.");
        }
    }

    showNearYou(element) {
        let tileContainer = document.querySelector('.tile-container');
        let zipContainer = document.querySelector('.zipcode-container');
        let helperText = document.querySelector('.helper-text');
        element.classList.add('active');
        
        if (this.latitude && this.longitude || this.filters.address) {
            if (element.id == 'near-you') {
                zipContainer.classList.remove('hidden');
                this.filters.order_by = 'proximity';
            } else {
                zipContainer.classList.add('hidden');
                helperText.classList.add('hidden');
                this.filters.order_by = 'availability';
            }
    
            return this.queueUpdateHrefFilters();
        } 

        if (!this.filters.address) {
            if (element.id == 'near-you') {
                zipContainer.classList.remove('hidden');
                helperText.classList.remove('hidden');
                tileContainer.classList.add('hidden');
            } else {
                zipContainer.classList.add('hidden');
                helperText.classList.add('hidden');
                tileContainer.classList.remove('hidden');
            }
        }
    }

    removePreviousRating(nodeList) {
        for (let i = 0; i < nodeList.length; i++) {
            nodeList[i].classList.remove('selected-star');
        }
    }

    starRatingEffect(nodeList) {
        if (document.querySelector('.selected-star')) {
            let selectedIndex = document.querySelector('.selected-star').getAttribute('data-rating');
            let plusSigns = document.querySelectorAll('.rating-container .plus');

            for (let i = 0; i < nodeList.length; i++) {
                if (nodeList[i].getAttribute('data-rating') <= selectedIndex) {
                    nodeList[i].classList.add('fill');
                } else {
                    nodeList[i].classList.remove('fill');
                }
            }

            // count text changes only if user is filtering by 5/5 stars
            if (selectedIndex == 5) {
                for (let i = 0; i < plusSigns.length; i++) {
                    plusSigns[i].innerHTML = "Only";
                }
            } else {
                for (let i = 0; i < plusSigns.length; i++) {
                    plusSigns[i].innerHTML = "+";
                }
            }

            this.filters.rating = selectedIndex;
            this.queueUpdateHrefFilters();
        }
    }

    clearAllFilters() {
        const container = document.querySelector('.event-filter-modal');
        const checkboxes = container.querySelectorAll('.checkbox-container input');
        const counts = container.querySelectorAll('.count');
        const radioBtns = container.querySelectorAll('.radio input');
        const ratingContainers = container.querySelectorAll('.rating-container');
        const starRatings = document.querySelectorAll('.star-rate');
        const lgbtqiaContainers = document.querySelectorAll('.lgbtqia-container');

        for (let i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = false;
        }

        for (let x = 0; x < counts.length; x++) {
            counts[x].innerHTML = " ";
        }

        for (let y = 0; y < radioBtns.length; y++) {
            if (radioBtns[y].id.includes('any')) {
                radioBtns[y].click();
            }
        }

        for (let z = 0; z < starRatings.length; z++) {
            starRatings[z].classList.remove('fill');
            starRatings[z].classList.remove('selected-star');
        }

        for (let q = 0; q < ratingContainers.length; q++) {
            ratingContainers[q].classList.remove('show-stars');

            if (ratingContainers[q].querySelector('.count')) {
                ratingContainers[q].querySelector('.count').innerHTML = "";
            }
        }

        for (let l = 0; l < lgbtqiaContainers.length; l++) {
            lgbtqiaContainers[l].classList.remove('active');
        }

        this.filters.lgbtqia_resource = null;
        this.filters.custom_min_age = null;
        this.filters.custom_max_age = null;
        this.queueUpdateHrefFilters();
    }

    submitSearch() {
        let search_physician_attribute_value = document.querySelector("#search-physician-appointments").value;
        if (this.filters.search_physician_attribute || search_physician_attribute_value.length > 0) {
            this.search_physician_attribute.storeRecentSearch(search_physician_attribute_value);
            this.filters.search_physician_attribute = search_physician_attribute_value;

            // a new search term gathers new results and the pagination needs to be reset
            this.page = 1;
            this.href.searchParams.set('page', this.page);
        }
        
        this.queueUpdateHrefFilters();
    }

    updateHrefFilters(pushHistory) {
        this.syncLanguagesFilters();
        this.syncGendersFilters();
        this.syncAgesFilter();
        this.syncHospitalAffiliationsFilters();
        this.syncAvailabilityFilter();
        this.syncCityFilters();
        this.syncLgbtqiaFilter();

        this.href = new URL(location.origin + location.pathname);

        if (this.page) {
            this.href.searchParams.set('page', this.page);
        }

        if (this.perPage) {
            this.href.searchParams.set('per-page', this.perPage);
        }

        if (this.filters.beginDate) {
            this.href.searchParams.set('beginDate', moment(this.filters.beginDate, 'M-D-Y').format('M-D-Y'));
        }

        if (this.filters.search_physician_attribute != undefined) {
            this.href.searchParams.set('search_physician_attribute', this.filters.search_physician_attribute);
        }

        if (this.filters.address != undefined) {
            //TODO: Remove . " il" once Duly expands out
            this.href.searchParams.set('address', this.filters.address + ' il');
        }

        this.href.searchParams.delete('lgbtqia_resource');
        if (this.filters.lgbtqia_resource) {
            this.href.searchParams.set('lgbtqia_resource', this.filters.lgbtqia_resource);
        }

        this.href.searchParams.delete('city[]');
        this.filters.cities.forEach(city => this.href.searchParams.append('city[]', city));

        this.href.searchParams.delete('language[]');
        this.filters.languages.forEach(language => this.href.searchParams.append('language[]', language));

        this.href.searchParams.delete('gender[]');
        this.filters.genders.forEach(gender => this.href.searchParams.append('gender[]', gender));

        this.href.searchParams.delete('age[]');
        this.filters.ages.forEach(age => this.href.searchParams.append('age[]', age));

        this.href.searchParams.delete('hospital[]');
        this.filters.hospitalAffiliations.forEach(hospital => this.href.searchParams.append('hospital[]', hospital));

        this.href.searchParams.delete('rating');
        if (this.filters.rating) {
            this.href.searchParams.set('rating', this.filters.rating);
        };

        this.href.searchParams.delete('availability');
        if (this.filters.availability) {
            this.href.searchParams.set('availability', this.filters.availability);
        }

        this.href.searchParams.delete('custom_min_age');
        this.href.searchParams.delete('custom_max_age');

        if (this.filters.custom_min_age != null && this.filters.custom_max_age != null) {
            this.href.searchParams.set('min_age', this.filters.custom_min_age);
            this.href.searchParams.set('max_age', this.filters.custom_max_age);
        }

        this.href.searchParams.delete('order_by');
        if (this.filters.order_by) {
            this.href.searchParams.set('order_by', this.filters.order_by);

            this.href.searchParams.delete('latitude');
            if (this.latitude) {
                this.href.searchParams.set('latitude', this.latitude);
            }

            this.href.searchParams.delete('longitude');
            if (this.longitude) {
                this.href.searchParams.set('longitude', this.longitude);
            }
        }

        if (pushHistory) {
            history.pushState(this.href.search, "", this.href.href);
        }

        this.fireAjaxEvent();
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

        this.filters.cities = selectedCities;
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

        this.filters.languages = selectedLanguages;
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

        this.filters.genders = selectedGenders;
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

        this.filters.hospitalAffiliations = selectedHospitalAffiliations;
    }

    queueUpdateHrefFilters(pushState = true) {
        clearTimeout(this._timeout);
        this._timeout = setTimeout((e) => {
            this.updateHrefFilters(pushState);
        }, 500);
    };

    /**
     * Update the view with search results
     */
    async fireAjaxEvent() {
        let spinner = document.querySelector('.main.loading-spinner');
        let container = document.querySelector('.tile-container');
        let helperText = document.querySelector('.helper-text');

        // hide content
        spinner.classList.remove('hidden');
        container.classList.add('hidden');
        helperText.classList.add('hidden');

        let qd = {};
        (new URL(this.href)).search.replace("?", "").split("&").forEach(function (item) {
            var s = item.split("="),
                k = s[0].replace("%5B", "").replace("%5D", ""),
                v = s[1] && decodeURIComponent(s[1]);
            (qd[k] = qd[k] || []).push(v);
        });

        delete qd.address;
        delete qd.search_physician_attribute;
        delete qd.order_by;

        this.analytics.PhysicianSearch(qd, "Select Physician Search");

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
                return Promise.reject(response)
            }
        }).then(async (data) => {
            document.querySelector('.tile-container').innerHTML = data;
            let resultCount = document.querySelector('.result-count-total');
            let ajaxCount = document.querySelector('.total-count-hidden');

            if (ajaxCount) {
                resultCount.innerHTML = ajaxCount.innerHTML;
            } else {
                resultCount.innerHTML = "0";
            }
            
            spinner.classList.add('hidden');
            container.classList.remove('hidden');
            this.addEventListeners();
            this.resetMapMarkers();
            this.setupPaginationListeners();

            if (parseInt(resultCount.innerHTML) == 1) {
                const singlePhysicianTile = document.querySelector('.select-physician-page .physician-content .tile-container.one-result input[name="appointment_physician_id"]');
                if (singlePhysicianTile) {
                    const recommendedProvidersHack = new SelectAppointment(singlePhysicianTile.value, null, null, null, null, null, null, null, true);
                }
            }
        }).catch((response) => {
            console.log(response)
            if (document.querySelector('.fetch-error')) {
                let error = document.querySelector('.fetch-error');
                error.classList.remove('hidden');
                error.textContent = "Sorry, we weren’t able to proceed. Please try again."
            }
        });
    }
}