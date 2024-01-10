'use strict';

import { breakpoints } from './variables';

/**
 * Manages the products list page
 */
export class Products {
    constructor() {
        if (!document.querySelector('.product-list-page')) {
            return;
        }

        this.setupBrandFilterListeners();
        this.setupConditionFilterListeners();
        this.setupMobileFilterAndSearch();
        this.setupFilterToggles();
        this.setupSearch();
    }

    setupFilterToggles() {
        const filterSectionTitles = document.querySelectorAll('.filterSectionTitle');
        let callOutCount = window.innerWidth > breakpoints['bs-large'] ? document.querySelector('#filters-desktop .filterSectionTitle .brandCount') : document.querySelector('.products-list-container .mobile-filters-modal .filter-container .filterSectionTitle .brandCount');
        let categoryCallOutCount = window.innerWidth > breakpoints['bs-large'] ? document.querySelector('#categoryCount') : document.querySelector('.products-list-container .mobile-filters-modal .filter-container .filterSectionTitle #categoryCount');
        filterSectionTitles.forEach((title) => {
            const icon = title.querySelector('.material-icons');

            if (icon.textContent === 'add') {
                callOutCount.hidden = true;
                categoryCallOutCount.hidden = true;
            }

            title.addEventListener('click', (e) => {
                if (e.target.nextElementSibling.classList.contains('hidden')) {
                    e.target.nextElementSibling.classList.remove('hidden');
                    icon.innerHTML = 'remove';
                    if (e.target.nextElementSibling.classList.contains('brands')) {
                        callOutCount.hidden = true;
                        categoryCallOutCount.hidden = false;
                    } else if (e.target.nextElementSibling.classList.contains('conditions')) {
                        categoryCallOutCount.hidden = true;
                        callOutCount.hidden = false;
                    }
                } else {
                    e.target.nextElementSibling.classList.add('hidden');
                    icon.innerHTML = 'add';
                    if (e.target.nextElementSibling.classList.contains('brands')) {
                        callOutCount.hidden = false;
                    } else if (e.target.nextElementSibling.classList.contains('conditions')) {
                        categoryCallOutCount.hidden = false;
                    }
                }
            })
        })
    }

    setupSearch() {
        const form = document.getElementById('searchProductsForm');
        const searchInput = document.getElementById('searchProducts');
        if(!form || !searchInput) {
            return;
        }
        form.addEventListener('submit', (e) => {
            e.preventDefault();
            this.query = searchInput.value;
            document.querySelector(".products-list-container .loading-spinner").classList.remove('hidden');
            document.querySelector(".products-list-container .product-list").classList.add('hidden');
            this.throttleUpdateProductList();
        })
    }

    /**
     * Sets up the listeners for the brand filters
     */
    setupBrandFilterListeners() {
        let brandCount = 0;
        if (window.innerWidth > breakpoints['bs-large']) {
            this.brandFiltersContainer = document.querySelector('#filters-desktop .brands');
            this.brandSelector = document.querySelector('#filters-desktop .filterSectionTitle .brandCount');
        } else {
            this.brandFiltersContainer = document.querySelector('.mobile-filters-modal .brands');
            this.brandSelector = document.querySelector('.products-list-container .mobile-filters-modal .filter-container .filterSectionTitle .brandCount');
        }
        this.activeBrandFilters = [];
        this.brandURLQueryParameterName = 'brand';

        if (this.brandFiltersContainer) {
            this.brandFilters = this.brandFiltersContainer.querySelectorAll('input[type="checkbox"].brand-filter');
            const syncFilter = (filter) => {
                if (filter.checked) {
                    brandCount += 1;
                    this.brandSelector.textContent = '('+ brandCount +')';
                    this.activeBrandFilters.push(filter.value);
                } else {
                    if (this.activeBrandFilters.includes(filter.value)) {
                        this.activeBrandFilters.splice(this.activeBrandFilters.indexOf(filter.value), 1);
                        brandCount -= 1;
                        this.brandSelector.textContent = brandCount === 0 ? null : '('+ brandCount +')';
                    }
                }
            }
            this.brandFilters.forEach((filter) => {
                syncFilter(filter);
                filter.addEventListener('change', (e) => {
                    syncFilter(e.target);
                    this.throttleUpdateProductList();
                })
            });
        }

        // clear mobile count for brands filter
        const mobileFiltersModal = document.querySelector('.mobile-filters-modal');
        if (!mobileFiltersModal) {
            return;
        }
        const clearAll = mobileFiltersModal.querySelector('.clear-all');
        clearAll.addEventListener('click', () => {
            brandCount = 0;
            this.brandSelector.textContent = null;
            console.log()
        });
    }

    /**
     * Sets up the listeners for the condition filters
     */
    setupConditionFilterListeners() {
        let categoryCount = 0;
        if (window.innerWidth > breakpoints['bs-large']) {
            this.conditionFiltersContainer = document.querySelector('#filters-desktop .conditions');
            this.categorySelector = document.querySelector('#categoryCount');
        } else {
            this.conditionFiltersContainer = document.querySelector('.mobile-filters-modal .conditions');
            this.categorySelector = document.querySelector('.products-list-container .mobile-filters-modal .filter-container .filterSectionTitle #categoryCount');
        }
        this.activeConditionFilters = [];
        this.conditionURLQueryParameterName = 'conditionId';

        if (this.conditionFiltersContainer) {
            this.conditionFilters = this.conditionFiltersContainer.querySelectorAll('input[type="checkbox"].condition-filter');
            const syncFilter = (filter) => {
                if (filter.checked) {
                    categoryCount += 1;
                    this.categorySelector.textContent = '(' + categoryCount + ')';
                    this.activeConditionFilters.push(filter.value);
                } else {
                    if (this.activeConditionFilters.includes(filter.value)) {
                        this.activeConditionFilters.splice(this.activeConditionFilters.indexOf(filter.value), 1);
                        categoryCount -= 1;
                        this.categorySelector.textContent = categoryCount === 0 ? null : '(' + categoryCount + ')';
                    }
                }
            }
            this.conditionFilters.forEach((filter) => {
                syncFilter(filter);
                filter.addEventListener('change', (e) => {
                    syncFilter(e.target);
                    this.throttleUpdateProductList();
                });
            });
        }

        // clear mobile count for category filter
        const mobileFiltersModal = document.querySelector('.mobile-filters-modal');
        if (!mobileFiltersModal) {
            return;
        }
        const clearAll = mobileFiltersModal.querySelector('.clear-all');
        clearAll.addEventListener('click', () => {
            categoryCount = 0;
            this.categorySelector.textContent = null;
            console.log()
        });
    }

    /**
     * Throttles the product list update. This is helpful for cases when a user interacts with the multiple filters
     * quickly. We don't want to queue up AJAX requests for every interaction with a filter, but rather after a short delay.
     */
    throttleUpdateProductList() {
        clearTimeout(this._timeout);
        this._timeout = setTimeout(_ => {
            document.querySelector(".products-list-container .loading-spinner").classList.remove('hidden');
            document.querySelector(".products-list-container .product-list").classList.add('hidden');
            this.updateProductList(this._timeout);
        }, 500);
    }

    /**
     * Use a provided autoCompleteUrl url to get a list of auto complete strings
     * 
     * @param {*} timeout - setTimeout id; used to control overlapping ajax cals
     */
    updateProductList(timeout) {
        // set the url to be the origin+pathname only, i.e. without any current query params
        let href = new URL(window.location.origin + window.location.pathname);

        // update the URL query params
        this.activeBrandFilters.forEach(brand => {
            href.searchParams.append('brand[]', brand);
        });
        this.activeConditionFilters.forEach(conditionId => {
            href.searchParams.append('conditionId[]', conditionId);
        });

        if (this.query) {
            href.searchParams.append('query', this.query);
        }

        history.replaceState({}, document.title, href);

        fetch(href, {
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
        }).then(response => {
            return response.text();
        }).then(products => {
            if (this._timeout !== timeout) {
                return;
            }

            document.querySelector(".product-list").innerHTML = products;

            document.querySelector(".products-list-container .loading-spinner").classList.add('hidden');
            document.querySelector(".product-list").classList.remove('hidden');
        });
    }

    setupMobileFilterAndSearch() {
        const mobileFiltersModal = document.querySelector('.mobile-filters-modal');
        if (!mobileFiltersModal) {
            return;
        }
        const closeModal = mobileFiltersModal.querySelector('.close-modal');
        if (closeModal) {
            closeModal.addEventListener('click', () => {
                mobileFiltersModal.classList.add('hidden');
            })
        }
        const clearAll = mobileFiltersModal.querySelector('.clear-all');
        clearAll.addEventListener('click', () => {
            const brandFilters = mobileFiltersModal.querySelectorAll('.brand-filter');
            if (!brandFilters) {
                return;
            }
            brandFilters.forEach((filter) => {
                if (filter.checked) {
                    if (this.activeBrandFilters.includes(filter.value)) {
                        this.activeBrandFilters.splice(this.activeBrandFilters.indexOf(filter.value), 1);
                    }
                    filter.checked = false;
                }
            })
            const conditionFilters = mobileFiltersModal.querySelectorAll('.condition-filter');
            if (!conditionFilters) {
                return;
            }
            conditionFilters.forEach((filter) => {
                if (filter.checked) {
                    if (this.activeConditionFilters.includes(filter.value)) {
                        this.activeConditionFilters.splice(this.activeConditionFilters.indexOf(filter.value), 1);
                    }
                    filter.checked = false;
                }
            });
            this.throttleUpdateProductList();
        })
        document.querySelector('.filter-btn-mobile').addEventListener('click', _ => {
            mobileFiltersModal.classList.remove('hidden');
        });

        document.querySelector('.view-results').addEventListener('click', _ => {
            mobileFiltersModal.classList.add('hidden');
        });
    }
}