'use strict';

/**
 * A self-contained search input helper.
 * It wraps itself around an input field and provides "recent history" and "auto suggestions" lists.
 *  
 */
export class Search {

    /**
     * constructor innitiates the class
     * 
     * @param {*} inputSelector - CSS selector for the primary input field
     * @param {*} inputContainerSelector  - CSS selector for a parent container that contains the input;
     *                                      used to handle "position: absolute" elements floating around the input field
     * @param {*} autoCompleteUrl - a url that will be used to find auto suggestions; it should return a JSON array of strings
     * @param {*} autoCompleteSearchParameter  - a parameter in the autoCompleteUrl that should be updated with user's input
     * @param {*} recentHistoryTemplateSelector  - CSS selector for the recent history html content template
     * @param {*} autoCompleteTemplateSelector  - CSS selector for the auto-complete html content template
     */
    constructor(
        inputSelector,
        inputContainerSelector,
        autoCompleteUrl,
        autoCompleteSearchParameter,
        recentHistoryTemplateSelector,
        autoCompleteTemplateSelector
    ) {
        this.inputSelector = inputSelector;
        this.autoCompleteUrl = autoCompleteUrl;
        this.recentHistoryTemplate = document.querySelector(recentHistoryTemplateSelector);
        this.autoCompleteTemplate = document.querySelector(autoCompleteTemplateSelector);
        this.autoCompleteSearchParameter = autoCompleteSearchParameter;
        this.inputContainerSelector = inputContainerSelector;
        this.inputElement = document.querySelector(this.inputSelector);
        this.inputContainerElement = document.querySelector(inputContainerSelector);

        if (this.inputElement) {
            this.setup();
        }
    }

    /**
     * storeRecentSearch stores a provided query so it can be shown in "recent history"
     * 
     * @param {*} query
     */
    storeRecentSearch(query) {
        // do not store empty strings
        if (query && query.length > 0) {
            // store in localStorage
            let list = JSON.parse(localStorage.getItem(`search-recent-history-${this.inputSelector}`));
            list = list ? list : [];

            // remove duplicates
            list = list.filter(v => v !== query);

            // push query on top of the list
            list.push(query);

            // update the new list in localStorage
            localStorage.setItem(`search-recent-history-${this.inputSelector}`, JSON.stringify(list));

            this.repopulateHistory();
        }
    }

    /**
     * setup setups listeners asnd other controls
     */
    setup() {
        // create the search history container
        this.createSearchHistoryContainer();
        this.createAutocompleteContainer();
        this.repopulateHistory();
        this.setOnClearListener();
        this.setFocusAndBlurListeners();
        this.setAutoCompleteListeners();
        this.setArrowKeysListeners();
    }

    /**
     * createSearchHistoryContainer creates a floating container that will be used to display the list of recent searches
     */
    createSearchHistoryContainer() {
        let searchHistory = this.inputContainerElement.querySelector(".search-history");
        if (searchHistory) {
            searchHistory.parentNode.removeChild(searchHistory);
        }
        searchHistory = this.recentHistoryTemplate.content.cloneNode(true);
        this.inputContainerElement.appendChild(searchHistory);
        this.searchHistory = this.inputContainerElement.querySelector(".search-history");
    }

    /**
     * createAutocompleteContainer creates a floating container that will be used to display the list of auto suggestions
     */
    createAutocompleteContainer() {
        let autoComplete = this.inputContainerElement.querySelector(".auto-complete");
        if (autoComplete) {
            autoComplete.parentNode.removeChild(autoComplete);
        }
        autoComplete = this.autoCompleteTemplate.content.cloneNode(true);
        this.inputContainerElement.appendChild(autoComplete);
        this.autoComplete = this.inputContainerElement.querySelector(".auto-complete");
    }

    /**
     * repopulateHistory checks the most recent list in localStorage and refreshes the list of recent searches
     */
    repopulateHistory() {
        let list = JSON.parse(localStorage.getItem(`search-recent-history-${this.inputSelector}`));
        list = list ? list.splice(list.length - 5, list.length) : [];
        this.inputContainerElement.querySelector(".list-of-recent-searches").dataset.count = list.length;
        list = list.reduce((accumulator, currentValue) => `<div class="item">${currentValue}</div>` + accumulator, "");
        this.inputContainerElement.querySelector(".list-of-recent-searches").innerHTML = list;

        this.inputContainerElement.querySelector(".list-of-recent-searches").querySelectorAll(".item").forEach(item => {
            item.addEventListener("click", ({ target }) => {
                this.inputElement.value = target.innerText;
                this.searchHistory.classList.remove("show-recent-history");
                this.inputElement.focus();
                this.autoComplete.classList.remove("show-auto-complete");
                this.showingAutoSuggestions = false;
                this.showingHistory = true;
            });
        });
    }

    /**
     * 
     * @param {*} callback - fn that be called when the ajax call is over
     */
    throttlePopulateAutoComplete(callback) {
        clearTimeout(this._timeout);
        this._timeout = setTimeout((e) => {
            this.populateAutoComplete(callback, this._timeout);
        }, 500);
    }

    /**
     * Use a provided autoCompleteUrl url to get a list of auto complete strings
     * 
     * @param {*} callback - fn that be called when the ajax call is over
     * @param {*} timeout - setTimeout id; used to control overlapping ajax cals
     */
    populateAutoComplete(callback, timeout) {
        let href = new URL(this.autoCompleteUrl);
        href.searchParams.set(this.autoCompleteSearchParameter, this.inputElement.value);
        this.autoCompleteUrl = href;

        fetch(this.autoCompleteUrl, {
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
            return response.json();
        }).then(list => {
            if (this._timeout !== timeout) {
                return;
            }

            list = list ? list.splice(0, 5) : [];
            this.autoComplete.dataset.count = list.length;

            list = list.reduce((accumulator, currentValue) => accumulator + `<div class="item">${currentValue}</div>`, "");

            if (list.length == 0) {
                this.autoComplete.classList.remove("show-auto-complete");
                return;
            }

            this.inputContainerElement.querySelector(".list-of-suggestions").innerHTML = list;

            this.inputContainerElement.querySelector(".list-of-suggestions").querySelectorAll(".item").forEach(item => {
                item.addEventListener("click", ({ target }) => {
                    this.inputElement.value = target.innerText;
                    this.autoComplete.classList.remove("show-auto-complete");
                    this.searchHistory.classList.remove("show-recent-history");
                    this.inputElement.focus();
                });
            });

            callback && callback();
        });
    }

    /**
     * setOnClearListener sets up the "clear" button listener
     */
    setOnClearListener() {
        this.inputContainerElement.querySelector(".search-history .clear").addEventListener("click", _ => {
            localStorage.removeItem(`search-recent-history-${this.inputSelector}`);
            this.repopulateHistory();
        });
    }

    /**
     * setFocusAndBlurListeners sets up listeners around the input field
     */
    setFocusAndBlurListeners() {
        this.inputElement.addEventListener("click", e => {
            if (this.inputElement.value.length > 0) {
                this.searchHistory.classList.remove("show-recent-history");
                this.autoComplete.classList.add("show-auto-complete");
            } else {
                this.searchHistory.classList.add("show-recent-history");
                this.autoComplete.classList.remove("show-auto-complete");
            }
        });

        document.querySelector("body").addEventListener("click", e => {
            if (e.target.closest(this.inputContainerSelector)) {
                return;
            }
            this.searchHistory.classList.remove("show-recent-history");
            this.autoComplete.classList.remove("show-auto-complete");
        });
    }

    /**
     * setAutoCompleteListeners sets up listeners to trigger auto suggestions
     */
    setAutoCompleteListeners() {
        this.inputElement.addEventListener("keyup", event => {
            this.lastKey = event.keyCode;
            if (event.target.value.length > 0) {
                if (event.keyCode !== 38 && event.keyCode !== 40 && event.keyCode !== 13) {
                    this.throttlePopulateAutoComplete(_ => {
                        if (this.lastKey !== 38 && this.lastKey !== 40 && this.lastKey !== 13) {
                            this.searchHistory.classList.remove("show-recent-history");
                            this.autoComplete.classList.add("show-auto-complete");
                        }
                    });
                }
            } else {
                this.searchHistory.classList.add("show-recent-history");
                this.autoComplete.classList.remove("show-auto-complete");
            }
        });
    }

    /**
     * Enables navigating through the dropdown lists using arrow keys
     */
    setArrowKeysListeners() {
        this.currentListSelectionIndex = null;
        this.inputContainerElement.addEventListener("keyup", event => {
            if (event.keyCode == 38) {
                // up key
                if (this.currentListSelectionIndex == null) {
                    this.currentListSelectionIndex = 0;
                } else {
                    this.currentListSelectionIndex--;
                    this.currentListSelectionIndex = this.currentListSelectionIndex < 0 ? 0 : this.currentListSelectionIndex;
                }
                this.markCurrentItemAsSelected();
            } else if (event.keyCode == 40) {
                // down key
                if (this.currentListSelectionIndex == null) {
                    this.currentListSelectionIndex = 0;
                } else {
                    this.currentListSelectionIndex++;
                    if (this.searchHistory.classList.contains("show-recent-history")) {
                        let currentCount = this.inputContainerElement.querySelector(".list-of-recent-searches").dataset.count;
                        this.currentListSelectionIndex = this.currentListSelectionIndex + 1 <= currentCount ? this.currentListSelectionIndex : currentCount - 1;
                    } else if (this.autoComplete.classList.contains("show-auto-complete")) {
                        let currentCount = this.autoComplete.dataset.count;
                        this.currentListSelectionIndex = this.currentListSelectionIndex + 1 <= currentCount ? this.currentListSelectionIndex : currentCount - 1;
                    }
                }
                this.markCurrentItemAsSelected();
            } else if (event.keyCode == 13) {
                this.searchHistory.classList.remove("show-recent-history");
                this.autoComplete.classList.remove("show-auto-complete");
            }
        });
    }

    /**
     * Once this.currentListSelectionIndex is set, this function adds a "selected" class to the selected item.
     * It also fills out the input field with the selected value.
     */
    markCurrentItemAsSelected() {
        let selectedItem = null;
        if (this.searchHistory.classList.contains("show-recent-history")) {
            this.inputContainerElement.querySelectorAll(".list-of-recent-searches .item").forEach((item, index) => {
                if (index === this.currentListSelectionIndex) {
                    item.classList.add("selected");
                    selectedItem = item;
                } else {
                    item.classList.remove("selected");
                }
            });
        } else if (this.autoComplete.classList.contains("show-auto-complete")) {
            this.autoComplete.querySelectorAll(".item").forEach((item, index) => {
                if (index === this.currentListSelectionIndex) {
                    item.classList.add("selected");
                    selectedItem = item;
                } else {
                    item.classList.remove("selected");
                }
            });
        }

        if (selectedItem) {
            this.inputElement.value = selectedItem.innerText;
        }
    }
}