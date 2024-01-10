'use strict';

import { FilterHelper } from './filters/filter-helper';
import { Search } from './search';
import { breakpoints } from './variables';
import moment from 'moment';
import Pikaday from "pikaday";
import { Analytics } from "./analytics.js";

export class Events {
    constructor() {
        if (!document.querySelector('.event-listings')) {
            return;
        }

        this.analytics = new Analytics(false);

        this.filters = {
            services: [],
            categories: [],
            cities: []
        };

        // enable only mobile or desktop filters depending on current viewport size
        this.selector = window.innerWidth < parseInt(breakpoints["bs-large"]) ? '.filters-mobile-container' : '.event-search-container';

        this.setupFilters();
        this.initPikaDay();

        this.filterHelper = new FilterHelper(_ => { });

        this.search_event_attribute = new Search(
            "#search-events",
            ".search-content form.event-attribute",
            location.origin + "/events/auto-suggestions",
            "search_event_attribute",
            "#html-content-template-recent-searches",
            "#html-content-template-auto-complete"
        );

        document.querySelector(".event-search-btn").addEventListener("click", this.submitSearch.bind(this));

        document.querySelectorAll(".event-search-container form").forEach(form => form.addEventListener("submit", e => {
            e.preventDefault();
            e.stopPropagation();
            this.submitSearch();
            return false;
        }), true);

        this.setupPaginationListeners();

        this.restoreFiltersFromURI();

        window.addEventListener('popstate', (e) => {
            this.restoreFiltersFromURI(e.state);
        });
    }

    closePickersOnMobile() {
        let calendars = document.querySelectorAll('.display-calendar');

        if (calendars) {
            calendars.forEach(calendar => {
                calendar.classList.remove('display-calendar');
            });
        }
    }

    clearAllFilters() {
        const container = document.querySelector('.event-filter-modal');
        const checkboxes = container.querySelectorAll('.checkbox-container input');
        const counts = container.querySelectorAll('.count');
        const type = container.querySelector('.type');
        const radioBtns = container.querySelectorAll('.radio input');
        const defaultRadio = container.querySelector('#mobile-all-dates');
        const datepickerContainer = container.querySelector('.date-picker-container');
        const datePickerStart = container.querySelector('#mobile-date-picker-start');
        const datePickerEnd = container.querySelector('#mobile-date-picker-end');

        for (let i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = false;
        }

        for (let x = 0; x < counts.length; x++) {
            counts[x].innerHTML = " ";
        }

        for (let y = 0; y < radioBtns.length; y++) {
            if (radioBtns[y].id.includes('all')) {
                radioBtns[y].click();
            }
        }

        // default back to all dates when clear all
        datepickerContainer.classList.add('hidden');
        defaultRadio.checked = true;
        datePickerStart.value = "";
        datePickerEnd.value = "";
        type.innerHTML = "";
        this.filters.rangeStart = null;
        this.filters.rangeEnd = null;
        this.filters.startsBeforeOrAt = null;
        this.closePickersOnMobile();
        this.queueUpdateHrefFilters();
    }


    setupFilters() {
        const clickEvents = ['click', 'keydown', 'change'];
        const mobileFilterContainer = document.querySelector('.filters-mobile-container');
        const labels = document.querySelectorAll('.filter-entry');
        const checkboxes = document.querySelectorAll('.filter-entry input[type=checkbox]');
        const radiobtns = document.querySelectorAll('.filter-entry input[type=radio]');
        // mobile events
        const mobileFilterBtn = document.querySelector('.filter-btn-mobile');
        const close = mobileFilterContainer.querySelector('.close-modal');
        const clearAll = mobileFilterContainer.querySelector('.clear-all');
        const viewAll = mobileFilterContainer.querySelector('.view-results');
        const pickers = mobileFilterContainer.querySelectorAll('.date-picker');
        
        clickEvents.forEach((event) => {
            mobileFilterBtn.addEventListener(event, (e) => {
                if (this.filterHelper.keydownOrClick(event, e)) {
                    mobileFilterContainer.classList.remove('hide');
                    document.querySelector('body').classList.add('fixed-for-modal');
                }
            });

            pickers.forEach((element) => {
                element.addEventListener(event, (e)=> {
                    if (this.filterHelper.keydownOrClick(event, e)) {
                        // only one picker should be open on mobile
                        this.closePickersOnMobile();
                        element.classList.add('display-calendar');
                    }
                });
            })

            clearAll.addEventListener(event, (e) => {
                if (this.filterHelper.keydownOrClick(event, e)) { 
                    this.clearAllFilters();    
                }
            });

            close.addEventListener(event, (e) => {
                if (this.filterHelper.keydownOrClick(event, e)) {
                    mobileFilterContainer.classList.add('hide');
                    document.querySelector('body').classList.remove('fixed-for-modal');
                }
            });

            viewAll.addEventListener(event, (e) => {
                if (this.filterHelper.keydownOrClick(event, e)) {
                    mobileFilterContainer.classList.add('hide');
                    document.querySelector('body').classList.remove('fixed-for-modal');
                }
            });

            checkboxes.forEach((checkbox) => {
                checkbox.addEventListener(event, (e) => {
                    this.page = 1;
                    this.queueUpdateHrefFilters();
                });
            });

            radiobtns.forEach((button) => {
                button.addEventListener(event, (e) => {
                    this.page = 1;
                    this.determineDatePicker();
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

                        if (e.target.classList.contains('close')) {
                            this.queueUpdateHrefFilters();
                        }

                        this.page = 1;
                    }
                });
            });
        });

        document.addEventListener('click', (e) => {
            // clicking outside filter container should close filter only on desktop
            const desktopFilterContainer = document.querySelector('.event-search-container');
            const container = desktopFilterContainer.getElementsByClassName('selected')[0];

            if (container) {
                if (container !== e.target && !container.contains(e.target) && !e.target.classList.contains('filter-entry')) {
                    container.classList.remove('selected');
                }
            }
        });
    }

    initPikaDay() {
        let containerStart =  document.querySelector(`${this.selector} .date-picker-start`);
        let containerEnd = document.querySelector(`${this.selector} .date-picker-end`);
        let inputFieldStart = containerStart.querySelector('input');
        let inputFieldEnd =  containerEnd.querySelector('input');

        let startOptions = {
            field: inputFieldStart,
            toString(date, format) {
                return moment(date).format('MM/DD/YYYY');
            },
            onSelect: (e) => {
                this.filters.rangeStart = this.formatDate(e);
                this.queueUpdateHrefFilters();
            }
        }
        let endOptions = {
            field: inputFieldEnd,
            toString(date, format) {
                return moment(date).format('MM/DD/YYYY');
            },
            onSelect: (e) => {
                this.filters.rangeEnd = this.formatDate(e);
                this.queueUpdateHrefFilters();
            }
        }

        // initialize with additional properties for mobile
        if (this.selector.includes('filters-mobile-container')) {
            startOptions.container = containerStart;
            startOptions.bound = false;
            endOptions.container = containerEnd;
            endOptions.bound = false;
        }

        // for start date
        new Pikaday(startOptions);

        inputFieldStart.addEventListener('blur', (e) => {
            let date = moment(new Date(inputFieldStart.value)).toISOString();

            if (moment(date).isValid()) {
                this.filters.rangeStart = this.formatDate(inputFieldStart.value);
                this.queueUpdateHrefFilters();
            } else {
                inputFieldStart.parentNode.classList.remove('mdc-text-field-success');
                inputFieldStart.parentNode.classList.add("mdc-text-field--invalid", "mdc-text-field-error");
            }
        })

        // for end date
        new Pikaday(endOptions);

        inputFieldEnd.addEventListener('blur', (e) => {
            let date = moment(inputFieldEnd.value).toISOString();

            if (moment(date).isValid()) {
                this.filters.rangeEnd = this.formatDate(inputFieldEnd.value);
                this.queueUpdateHrefFilters();
            } else {
                inputFieldEnd.parentNode.classList.remove('mdc-text-field-success');
                inputFieldEnd.parentNode.classList.add("mdc-text-field--invalid", "mdc-text-field-error");
            }
        })
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

    formatDateForPreset(date) {
        let d = new Date(date);

        let options = {
            month: 'short',
            day: 'numeric',
            year: 'numeric',
            timeZone: 'UTC'
        };

        return d.toLocaleDateString('en-US', options);
    }

    formatDate(date) {
        let d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [year, month, day].join('-');
    }

    deleteRangeFilters(types = []) {
        types.forEach(type => {
            this.href.searchParams.delete(type);
            this.filters[type] = null;
        })
    }

    determineDatePicker() {
        let container =  document.querySelector(`${this.selector} .dates-container`);
        let datesContainer = container.querySelector('.date-picker-container');
        let radioBtnSelected = container.querySelector('input[type=radio]:checked');
        let radioValue = "";

        if (radioBtnSelected) {
            radioValue = radioBtnSelected.id;
        }

        if (radioValue.includes("date-range")) {
            this.deleteRangeFilters(['startsBeforeOrAt']);
            container.classList.add('active');
            datesContainer.classList.remove('hidden');
        } else if (radioValue.includes("next-seven-days")) {
            this.deleteRangeFilters(['rangeEnd', 'rangeStart']);

            let now = new Date();
            // set a week out from today
            now.setDate(now.getDate() + 1 * 7);
            let nowFormatted = this.formatDate(now);
            this.filters.startsBeforeOrAt = nowFormatted;

            container.classList.add('active');
            datesContainer.classList.add('hidden');
            this.closePickersOnMobile();
            this.queueUpdateHrefFilters();
        } else {
            this.deleteRangeFilters(['rangeEnd', 'rangeStart', 'startsBeforeOrAt']);
            container.classList.remove('active');
            this.closePickersOnMobile();
            this.queueUpdateHrefFilters();
        }
    }

    submitSearch() {
        let search_event_attribute_value = document.querySelector("#search-events").value;
        if (this.filters.search_event_attribute || search_event_attribute_value.length > 0) {
            this.search_event_attribute.storeRecentSearch(search_event_attribute_value);
            this.filters.search_event_attribute = search_event_attribute_value;
            
            // a new search term gathers new results and the pagination needs to be reset
            this.page = 1;
            this.href.searchParams.set('page', this.page);
        }

        this.queueUpdateHrefFilters();
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
        
        // do not fire off ajax requests while the filters are being restored in bulk
        this.disableFiltersSync = true;

        // restore page filter
        this.page = this.href.searchParams.get('page');

        // restore per-page filter
        this.perPage = this.href.searchParams.get('per-page');

        // restore search_physician_attribute filter
        this.filters.search_event_attribute = this.href.searchParams.get('search_event_attribute');

        if (!this.href.searchParams.get('search_event_attribute')) {
            this.filterHelper.resetSearchField('#search-events');
        }

        // restore city filters
        this.filterHelper.restoreListFilterHelper('.event-search-container', '.city-checkbox', 'city[]', this.href.searchParams);

        // restore service filters
        this.filterHelper.restoreListFilterHelper('.event-search-container', '.service-checkbox', 'service[]', this.href.searchParams);

        // restore categories filters
        this.filterHelper.restoreListFilterHelper('.event-search-container', '.category-checkbox', 'category[]', this.href.searchParams);

        if (!this.href.searchParams.get('rangeStart')) {
            this.filters.rangeStart = null;
        }

        if (!this.href.searchParams.get('rangeEnd')) {
            this.filters.rangeEnd = null;
        }

        if (!this.href.searchParams.get('startsBeforeOrAt')) {
            this.filters.startsBeforeOrAt = null;
        }
        // restore rangeStart and rangeEnd
        this.restoreDatePickerValues();

        // re-enable ajax requests
        this.disableFiltersSync = false;

        this.queueUpdateHrefFilters(false);
    }

    resetInputField(container) {
        ['.date-picker-start input', '.date-picker-end input'].forEach(type => {
            let input = container.querySelector(type);
            let parent = input.parentNode;
            let label = parent.querySelector('label');

            input.value = "";
            parent.classList.remove('mdc-text-field-success');
            label.classList.remove('mdc-floating-label--float-above');
        });
    }

    setRange(searchParam) {
        let date = this.href.searchParams.get(searchParam);
        let datesContainer =  document.querySelector(`${this.selector} .dates-container`);

        if (datesContainer) {
            if (date == null) {
                datesContainer.querySelector('.all-dates input').checked = true;
            } else {
                if (searchParam == "startsBeforeOrAt") {
                    datesContainer.classList.add('active');
                    datesContainer.querySelector('.next-seven-days input').checked = true;
                    this.resetInputField(datesContainer);
                } else if (searchParam == "all-dates"){ 
                    datesContainer.querySelector('.all-dates input').checked = true;
                    this.resetInputField(datesContainer);
                } else {    
                    datesContainer.classList.add('active');
                    datesContainer.querySelector('.date-range input').checked = true;

                    if (searchParam == "rangeStart" || searchParam == "rangeEnd") {
                        let parent = searchParam == "rangeStart" ? datesContainer.querySelector('.date-picker-start input').parentNode : datesContainer.querySelector('.date-picker-end input').parentNode
                        let label = parent.querySelector('label');
                        parent.classList.add('mdc-text-field-success');
                        label.classList.add('mdc-floating-label--float-above');
                        parent.querySelector('input').value = this.formatDateForPreset(date);
                    } 
                }
            }
            this.determineDatePicker();
        }
    }

    restoreDatePickerValues() {
        const rangeStart = this.href.searchParams.get('rangeStart');
        const rangeEnd = this.href.searchParams.get('rangeEnd');
        const startsBeforeOrAt = this.href.searchParams.get('startsBeforeOrAt');

        if (rangeStart) {
            this.setRange('rangeStart');
        }

        if (rangeEnd) {
            this.setRange('rangeEnd')
        }

        if (startsBeforeOrAt) {
            this.setRange('startsBeforeOrAt')
        }

        if (!rangeStart && !rangeEnd && !startsBeforeOrAt) {
            this.setRange('all-dates');
        }
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

    syncCategoriesFilters() {
        var checkboxes = document.querySelectorAll('.category-checkbox');
        var selectedCategories = [];

        for (var i = 0; i < checkboxes.length; i++) {
            var checkbox = checkboxes[i];
            var input = checkbox.querySelector('input');

            if (input.checked) {
                var label = checkbox.querySelector('label').innerHTML;
                selectedCategories.push(label);
            }
        }

        this.filters.categories = [...new Set(selectedCategories)];
    }

    updateHrefFilters(pushHistory) {
        if (this.disableFiltersSync) {
            return;
        }

        this.syncCityFilters();
        this.syncServicesFilters();
        this.syncCategoriesFilters();

        this.href = new URL(location.origin + location.pathname);

        if (this.page) {
            this.href.searchParams.set('page', this.page);
        }

        if (this.perPage) {
            this.href.searchParams.set('per-page', this.perPage);
        }

        if (this.filters.search_event_attribute != undefined) {
            this.href.searchParams.set('search_event_attribute', this.filters.search_event_attribute);
        }

        this.href.searchParams.delete('city[]');
        this.filters.cities.forEach(city => this.href.searchParams.append('city[]', city));

        this.href.searchParams.delete('service[]');
        this.filters.services.forEach(service => this.href.searchParams.append('service[]', service));

        this.href.searchParams.delete('category[]');
        this.filters.categories.forEach(category => this.href.searchParams.append('category[]', category));

        this.href.searchParams.delete('rangeStart');
        if (this.filters.rangeStart) {
            this.href.searchParams.set('rangeStart', this.filters.rangeStart);
        }

        this.href.searchParams.delete('rangeEnd');
        if (this.filters.rangeEnd) {
            this.href.searchParams.set('rangeEnd', this.filters.rangeEnd);
        }

        this.href.searchParams.delete('startsBeforeOrAt');
        if (this.filters.startsBeforeOrAt) {
            this.href.searchParams.set('startsBeforeOrAt', this.filters.startsBeforeOrAt);
        }

        if (pushHistory) {
            history.pushState(this.href.search, "", this.href.href);
        }

        this.fireAjaxEvent();
    }

    queueUpdateHrefFilters(pushState = true) {
        clearTimeout(this._timeout);
        this._timeout = setTimeout((e) => {
            this.updateHrefFilters(pushState);
        }, 500);
    };

    /**
     * Update the view with new events data using current filters.
     */
    async fireAjaxEvent() {
        // hide content
        const listings = document.querySelector('.listings');
        const loadingSpinner = document.querySelector('.loading-spinner');
        listings.classList.add('hidden');
        loadingSpinner.classList.remove('hidden');

        let qd = {};
        (new URL(this.href)).search.replace("?", "").split("&").forEach(function (item) {
            var s = item.split("="),
                k = s[0].replace("%5B", "").replace("%5D", ""),
                v = s[1] && decodeURIComponent(s[1]);
            (qd[k] = qd[k] || []).push(v);
        });

        delete qd.search_event_attribute;

        this.analytics.EventsSearch(qd);

        fetch(this.href, {
            method: 'GET',
            mode: 'same-origin',
            cache: 'no-cache',
            follow: true,
            async: true,
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'text/html',
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
            loadingSpinner.classList.add('hidden');
            listings.classList.remove('hidden');
            listings.innerHTML = data
            
            this.setupPaginationListeners();
        }).catch(async (e) => {
            const error = document.querySelector('.display-error');
            error.classList.remove('hidden');
            loadingSpinner.classList.add('hidden');
        });
    };
}