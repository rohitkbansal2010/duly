'use strict';
import { Search } from '../search';
import { enableFocusTrap } from '../utils';

export class ServiceSearch {
    constructor() {
        if (!document.querySelector('.schedule-services-page')) {
            return;
        }

        this.query = new Search(
            "#search-services",
            ".search-content form.search-appointment-services",
            location.origin + "/schedule/services/auto-suggestions",
            "query",
            "#html-content-template-recent-searches",
            "#html-content-template-auto-complete"
        );

        document.querySelectorAll(".search-results-container form").forEach(form => form.addEventListener("submit", e => {
            e.preventDefault();
            e.stopPropagation();
            this.submitSearch();
            return false;
        }), true);

        this.addEventListeners();

        this.href = new URL(location.href);
        this.searchTerm = this.href.searchParams.get('query');
        let container = document.querySelector('.results-container');
        
        if (this.searchTerm) {
            this.prepopulateResults();
        } 

        if (document.querySelector('.no-schedule-for-service')) {
            this.showAlertModals();
        }

        container.classList.remove('hidden');
    }

    showAlertModals() {
        const events = ['click', 'keydown'];
        let modals = document.querySelectorAll('.no-schedule-for-service');
        let dismissBtns = document.querySelectorAll('.dismiss-modal');
        let closeBtns = document.querySelectorAll('.close');

        events.forEach(event => {
            modals.forEach(modal => {
                modal.addEventListener(event, (e)=> {
                    if (this.keydownOrClick(event, e)) {
                        // focus on modal while open
                        enableFocusTrap('.no-appointment-types-modal button, .no-appointment-types-modal span.material-icons.close');
                        
                        let id = modal.id.split('service-')[1];

                        if (document.querySelector('#alert-' + id) && id) {
                            document.querySelector('#alert-' + id).classList.remove('hidden');
                        }
                    }
                })
            })

            dismissBtns.forEach(btn => {
                btn.addEventListener(event, (e) => {
                    if (this.keydownOrClick(event, e)) {
                        this.hideAllModals();
                    }
                });
            });

            closeBtns.forEach(btn => {
                btn.addEventListener(event, (e) => {
                    if (this.keydownOrClick(event, e)) {
                        this.hideAllModals();
                    }
                });
            });
        });
    }

    hideAllModals() {
        let modals = document.querySelectorAll('.no-appointment-types-modal');

        for (let i = 0; i < modals.length; i++) {
            modals[i].classList.add('hidden');
        }
    }

    prepopulateResults() {
        let container = document.querySelector('.search-appointment-services');
        let searchBar = document.querySelector('#search-services');
        let label = container.querySelector('span.mdc-floating-label');
        
        if (label && searchBar) {
            label.classList.add('mdc-floating-label--float-above');
            searchBar.value = this.searchTerm;
            this.fireAjaxEvent();
        }
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }

    submitSearch() {
        let query_value = document.querySelector("#search-services").value;
        if (query_value.length > 0) {
            this.query.storeRecentSearch(query_value);
        }
    }

    addEventListeners() {
        const btn = document.querySelector('.search-results-btn');
        const events = ['click', 'keydown'];
        const form = document.querySelector('#search-services');

        form.addEventListener('keydown', (e)=> {
            if (e.keyCode === 13) {
                this.fireAjaxEvent();
            }
        })

        events.forEach((event) => {
            btn.addEventListener(event, (e)=> {
                if (this.keydownOrClick(event, e)) {
                    this.fireAjaxEvent();
                }
            })
        });
    }

    /**
     * Update the view with search results
     */
    async fireAjaxEvent() {
        let spinner = document.querySelector('.loading-spinner');
        let container = document.querySelector('.dynamic-results');
        let searchBar = document.querySelector('#search-services');

        // hide content
        spinner.classList.remove('hidden');
        container.classList.add('hidden');

        this.href.searchParams.delete('query');
        this.href.searchParams.set('query', searchBar.value);

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
            document.querySelector('.dynamic-results').innerHTML = data;
            spinner.classList.add('hidden');
            container.classList.remove('hidden');
            this.addEventListeners();
            this.showAlertModals();
        }).catch((response) => {
            if (document.querySelector('.fetch-error')) {
                let error = document.querySelector('.fetch-error');
                error.classList.remove('hidden');
                error.textContent = "Sorry, we weren’t able to proceed. Please try again."
            }
        });
    }
}