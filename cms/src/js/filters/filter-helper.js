'use strict';
import { breakpoints } from './../variables';

export class FilterHelper {    
    resetCheckboxes() {
        const checkboxes = document.querySelectorAll('.filter-entry input[type=checkbox]');
        
        for (let i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = false;
        }
    }

    resetSearchField(selector) {
        const searchField = document.querySelector(selector);
        if (!searchField) {
            return;
        }
        const parent= searchField.parentNode;

        parent.classList.remove('mdc-text-field-success');
        searchField.value = '';
    }

    removeStateAndCount(selector, radio = false) {
        selector.classList.remove('active');
        selector.classList.remove('selected');

        let count = selector.querySelector('.count');
        let close = selector.querySelector('.close');
        count.innerHTML = "";
        close.classList.add('hidden');

        if (radio) {
            let btns = selector.querySelectorAll('input[type=radio]');

            for (let i = 0; i < btns.length; i++) {
                btns[i].checked = false;
            }
        }
    }

    resetFilterEntry() {
        const entries = document.querySelectorAll('.filter-entry');
        
        for (let i = 0; i < entries.length; i++) {
            this.removeStateAndCount(entries[i]);
        }
    }

    restoreFilter(filter) {
        filter.querySelector('input').click();
                
        // clicking a filter will open the drowndown
        // we don't want it shown, so let's hide it
        filter.closest('.filter-entry').classList.remove('selected');
    }

    removeFilter(filter) {
        let filterContainer = filter.closest('.filter-entry');
 
        if (filterContainer && !filterContainer.querySelector('input[type=checkbox]:checked')) {
            this.removeStateAndCount(filterContainer);
        }
    }

    restoreListFilterHelper(containerSelector, radioCSSSelector, uriQueryParamName, searchParams) {
        // enable only mobile or desktop filters depending on current viewport size
        const selector = window.innerWidth < parseInt(breakpoints["bs-large"]) ? `.event-filter-modal ${radioCSSSelector}` : `${containerSelector} ${radioCSSSelector}`;

        for (const filter of document.querySelectorAll(selector)) {
            let params = searchParams.getAll(uriQueryParamName);
            let label = filter.querySelector('label').innerHTML;

            // age params don't match labels
            // param = Children; label = Children (5-12)
            if (filter.classList.contains('age-checkbox')) {
                params.forEach(param => {
                    if (label.includes(param)) {
                        this.restoreFilter(filter);
                    } else {
                        this.removeFilter(filter);
                    }
                })
            }

            if (params.includes(label)) {
                this.restoreFilter(filter);
            } else {
                this.removeFilter(filter);
            }
        }
    }

    restoreRadioFilterHelper(containerSelector, radioCSSSelector, idSelector, uriQueryParamName, searchParams) {
        // enable only mobile or desktop filters depending on current viewport size
        const selector = window.innerWidth < parseInt(breakpoints["bs-large"]) ? `.event-filter-modal ${radioCSSSelector} .radio` : `${containerSelector} ${radioCSSSelector} .radio`;

        for (const filter of document.querySelectorAll(selector)) {
            if (searchParams.getAll(uriQueryParamName).includes(filter.querySelector('input').id.replace(`${idSelector}-`, ''))) {
                filter.querySelector('input').click();
                
                // clicking a filter will open the drowndown
                // we don't want it shown, so let's hide it
                filter.closest('.filter-entry').classList.remove('selected');
            } else {
                let filterContainer = filter.closest('.filter-entry');

                if (filterContainer && !searchParams.get(uriQueryParamName)) {
                    this.removeStateAndCount(filterContainer, true);
                }
            }
        }
    }

    filtersAppliedCount() {
        const filterEvent = document.querySelector('.selected:not(.dates-container):not(.laboratory-services-container):not(.open-now-container)');

        if (filterEvent) {
            const checkboxes = filterEvent.querySelectorAll('.checkbox-container input[type=checkbox], .distance-container input[type=radio], .availability-container input[type=radio], .visit-type-container input[type=checkbox], .sort-container input[type=radio], .ages-container input[type=checkbox], .ages-container input[type=radio]');
            const count = filterEvent.querySelector('.count');
            const close = filterEvent.querySelector('.close');
            const filterBy = [];
            this.clearFilterEvent('.selected');

            // grab all checkboxes that are selected
            for (let i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    filterBy.push(checkboxes[i].id);
                }
            }

            // special treatment for star ratings since they are not checkboxes
            if (filterEvent.classList.contains('rating-container')) {
                if (document.querySelector('.selected-star')) {
                    filterEvent.classList.add('active');
                    let rating = document.querySelector('.selected-star').getAttribute('data-rating');
                    count.innerHTML = rating;
                    close.classList.remove('hidden');
                    filterEvent.classList.add('show-stars');
                } else {
                    filterEvent.classList.remove('active');
                }
            } else {
                if (filterBy.length) {
                    filterEvent.classList.add('active');
                    count.innerHTML = "(" + filterBy.length + ")";
                    close.classList.remove('hidden');
                } else {
                    filterEvent.classList.remove('active');
        
                    if (count) {
                        count.innerHTML = "";
                    }
        
                    close.classList.add('hidden');
                }
            }
        }
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }

    clearFilterEvent(containerClass) {
        let clickEvents = ['click', 'keydown'];
        let filter = document.querySelector(containerClass);
        let count = filter.querySelector('.count');
        let close = filter.querySelector('.close');
        let checkboxes = filter.querySelectorAll('.checkbox-container input, .distance-container input, .availability-container input, .sort-container input');

        clickEvents.forEach(event => {
            close.addEventListener(event, (e)=> {
                count.innerHTML = "";
                close.classList.add('hidden');
                filter.classList.remove('active');

                for (let i = 0; i < checkboxes.length; i++) {
                    checkboxes[i].checked = false;
                }

                if (filter.classList.contains('rating-container')) {
                    const stars = document.querySelectorAll('.star-rate');
                    filter.classList.remove('show-stars');

                    for (let i = 0; i < stars.length; i++) {
                        stars[i].classList.remove('selected-star');
                        stars[i].classList.remove('fill');
                    }
                }
            });
        })
    }

    removeOtherSelectedFilters(nodeList) {
        for (let i = 0; i < nodeList.length; i++) {
            nodeList[i].classList.remove('selected');
        }
    }
}
    