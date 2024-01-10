'use strict';
import { Search } from './search';
import { Analytics } from "./analytics.js";

export class SearchListings {
    constructor() {
        if (!document.querySelector('.search-page')) {
            return;
        }

        this.query = new Search(
            "#search-results",
            ".search-content form.site-wide-search",
            location.origin + "/search/auto-suggestions",
            "query",
            "#html-content-template-recent-searches",
            "#html-content-template-auto-complete"
        );

        document.querySelectorAll(".search-results-container form").forEach(form => form.addEventListener("submit", e => {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }), true);

        this.addFormEventListeners();
        this.addFiltersEventListeners();
        this.generateUrl();
        
        window.addEventListener('popstate', (e) => {
            this.generateUrl(e.state);
        });

        this.analytics = new Analytics(false);
    }

    generateUrl(state = false) {
        this.href = new URL(location);
        this.searchTerm = this.href.searchParams.get('query');

        const container = document.querySelector('.site-wide-search');
        const searchBar = document.querySelector('#search-results');
        const label = container.querySelector('span.mdc-floating-label');

        if (state) {
            // set URL params from previous state
            let searchParams = new URLSearchParams(state);
            for (const [key, value] of searchParams) {
                this.href.searchParams.set(key, value)
            }
        } else {
            this.urlHasNoParams(label);
        }
        
        if (this.searchTerm) {
            label.classList.add('mdc-floating-label--float-above');
            searchBar.value = this.searchTerm;
            this.submitSearch(false);
        }
    }
    
    urlHasNoParams(label) {
        document.querySelector("#search-results").value = '';
        document.querySelector('.dynamic-results').innerHTML = '';
        label.classList.remove('mdc-floating-label--float-above');
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }

    submitSearch(pushState = true) {
        this.searchTerm = document.querySelector("#search-results").value;
        this.href.searchParams.set('query', this.searchTerm);

        // only store the value if the value is not empty
        if (this.searchTerm.length > 0) {
            this.query.storeRecentSearch(this.searchTerm);
        }
        
        if (pushState) {
            history.pushState(this.href.search, "", this.href.href);
        }

        this.fireAjaxEvent();
    }

    addFormEventListeners() {
        document.querySelector('#search-results').addEventListener('keydown', (e)=> {
            if (e.keyCode === 13) {
                this.submitSearch();
            }
        });

        ['click', 'keydown'].forEach((event) => {
            document.querySelector('.search-results-btn').addEventListener(event, (e)=> {
                if (this.keydownOrClick(event, e)) {
                    this.submitSearch();
                }
            });
        });
    }

    addFiltersEventListeners() {
        const filters = document.querySelectorAll('.filter');
        const events = ['click', 'keydown'];

        events.forEach((event) => {
            filters.forEach((element) => {
                element.addEventListener(event, (e)=> {
                    if (this.keydownOrClick(event, e)) {
                        this.href.searchParams.delete('section');
                        this.href.searchParams.set('section', element.id);
                        this.href.searchParams.set('page', 1);
                        this.submitSearch();

                        this.analytics._sendEvent("Search", "Site Search", element.id);
                    }
                });
            });
        });
    }

    /**
     * Update the view with search results
     */
    async fireAjaxEvent() {
        let spinner = document.querySelector('.loading-spinner');
        let container = document.querySelector('.dynamic-results');

        // hide content
        spinner.classList.remove('hidden');
        container.classList.add('hidden');
        
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
            this.addFiltersEventListeners();
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
